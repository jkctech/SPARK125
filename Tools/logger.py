import re
import time
import math
import sys
import pyaudio
import wave
import threading

from pathlib import Path
from datetime import datetime
from uniden import userial

class Recorder():
	filename = ""
	isrecording = False
	frames = []

	device = 0
	channels = 2
	rate = 44100
	sample_format = pyaudio.paInt16
	chunk = 1024

	def __init__(self, deviceindex, channels = 2, rate = 44100, sample_format = pyaudio.paInt16, chunk = 1024):
		self.device = deviceindex
		self.channels = channels
		self.rate = rate
		self.sample_format = sample_format
		self.chunk = chunk

	def Start(self, filename):
		self.p = pyaudio.PyAudio()
		self.isrecording = True
		self.filename = filename
		
		t = threading.Thread(target=self._record)
		t.start()

	def Stop(self):
		self.isrecording = False
		
		wf = wave.open(self.filename, 'wb')
		wf.setnchannels(self.channels)
		wf.setsampwidth(self.p.get_sample_size(self.sample_format))
		wf.setframerate(self.rate)
		wf.writeframes(b''.join(self.frames))
		wf.close()

		# self.p.terminate()

	def _record(self):
		self.frames = []

		self.stream = self.p.open(
			format=self.sample_format,
			channels=self.channels,
			rate=self.rate,
			input=True,
			input_device_index=self.device,
			frames_per_buffer=self.chunk
		)

		while self.isrecording:
			data = self.stream.read(self.chunk)
			self.frames.append(data)

# Check args
if len(sys.argv) < 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

start = 0
inreception = False

logfile = open("log.log", "a", buffering=1)

dorecord = True

if dorecord:
	rec = Recorder(4, channels=1)
	recdir = "logs"
	Path(recdir).mkdir(parents=True, exist_ok=True)

# Keep looping
while True:
	try:
		# Get screen buffer
		buff = str(userial.command(ser, "STS"))
		buff = buff.split(",")

		# If "busy" == 0 and reception is going we close it
		if buff[14] == "0" and inreception:
			inreception = False
			if dorecord:
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
				if dorecord:
					
					name = alphatag
					for c in "/\\|:*?\"<>":
						name = name.replace(c, "_")
					
					folder = f_stamp.split(' ')[0]
					Path(recdir + "/" + folder).mkdir(parents=True, exist_ok=True)

					fname = "[{}] {} - {}.wav".format(f_stamp.replace(':', '-'), name, frequency)
					rec.Start("{}/{}/{}".format(recdir, folder, fname))

				# Print first log part
				string = "[{}] - \"{}\" - {} Mhz [{}] ({}/5) : {} => {}".format(f_stamp, alphatag, frequency, mode, power, channel, bank)
				print(string, end="", flush=True)
				logfile.write(string)
	
		time.sleep(0.1)
	
	except KeyboardInterrupt:
		exit(0)
	except Exception:
		pass
