import re
import time
import math
import argparse
import sys
import queue

from pathlib import Path
from datetime import datetime
from uniden import userial

# Argument parser
parser = argparse.ArgumentParser(description='Uniden Toolkit Spark125')

parser.add_argument('-l', '--list-devices', action='store_true', help='show list of audio devices and exit')
args, remaining = parser.parse_known_args()
if args.list_devices:
	import sounddevice as sd
	print(sd.query_devices())
	parser.exit(0)

parser.add_argument('-p', '--port', help='Serial port to interface with the scanner.', required=True)
parser.add_argument('-r', '--record', action='store_true', help='Record receptions using PyAudio.')
parser.add_argument('-d', '--device', type=int)
parser.add_argument('-c', '--channels', type=int, default=1)

args = parser.parse_args(remaining)

# Only import and define recording class if needed
if args.record:
	import sounddevice as sd
	import soundfile as sf
	import threading

	class Recorder():
		filename = ""
		isrecording = False
		buffersize = 20

		device = 0
		channels = 2
		rate = 44100

		mic_queue = queue.Queue(maxsize=buffersize)

		def __init__(self, deviceindex, channels = 2, rate = 44100):
			self.device = deviceindex
			self.channels = channels
			self.rate = rate
		
		def callback(self, indata, frames, time, status):
			if status:
				print(status, file=sys.stderr)
			self.mic_queue.put(indata.copy())

		def _record(self):
			with sf.SoundFile(self.filename, mode='x', samplerate=self.rate, channels=self.channels, subtype=None) as file:
				with sd.InputStream(samplerate=self.rate, device=self.device, channels=self.channels, callback=self.callback):
					while self.isrecording:
						file.write(self.mic_queue.get())

		def Record(self, filename):
			self.isrecording = True
			self.filename = filename

			t = threading.Thread(target=self._record)
			t.start()

		def Stop(self):
			self.isrecording = False
	
	# Init recorder
	rec = Recorder(args.device, channels=args.channels)
	recdir = "logs"
	Path(recdir).mkdir(parents=True, exist_ok=True)

# Get serial connection & Open
ser = userial.get(args.port)
ser.open()

start = 0
inreception = False

logfile = open("log.log", "a", buffering=1)

# Keep looping
try:
	while True:
		# Get screen buffer
		buff = str(userial.command(ser, "STS"))
		buff = buff.split(",")

		# If "busy" == 0 and reception is going we close it
		if buff[14] == "0" and inreception:
			inreception = False
			if args.record:
				rec.Stop()
			stop = datetime.now().replace(microsecond=0)
			string = " <{}>".format(stop - start)
			print(string)
			logfile.write(string + "\n")
			logfile.flush()

		# If not in reception, but marked as busy, start reception mark
		if inreception == False and buff[14] == "1":
			frequency = buff[6][7:-4]

			# Somtetimes you get glitched output, this confirms
			if re.match(r'^\d+\.\d+$', frequency):
				# Mark as reception and start timer
				inreception = True
				start = datetime.now().replace(microsecond=0)

				# Strip information
				alphatag = buff[4].strip()
				channel = int(buff[6].split(' ')[0][2:])
				bank = math.floor(channel / 50) + 1
				mode = "AM" if buff[8][1:].strip() == "\\x98\\x99\\x9a" else "FM"
				power = int(buff[20])

				f_stamp = "{}".format(start.strftime("%Y-%m-%d %H:%M:%S"))

				# Start recording
				if args.record:
					
					name = alphatag
					for c in "/\\|:*?\"<>":
						name = name.replace(c, "_")
					
					folder = f_stamp.split(' ')[0]
					Path(recdir + "/" + folder).mkdir(parents=True, exist_ok=True)

					fname = "[{}] {} - {}.wav".format(f_stamp.replace(':', '-'), name, frequency)
					rec.Record("{}/{}/{}".format(recdir, folder, fname))

				# Print first log part
				string = "[{}] - \"{}\" - {} Mhz [{}] ({}/5) : {} => {}".format(f_stamp, alphatag, frequency, mode, power, channel, bank)
				print(string, end="", flush=True)
				logfile.write(string)

		time.sleep(0.1)

except KeyboardInterrupt:
	exit(0)
