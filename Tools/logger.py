import re
import time
import math
import sys

from datetime import datetime
from uniden import userial

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

# Keep looping
while True:
	# Get screen buffer
	buff = str(userial.command(ser, "STS"))
	buff = buff.split(",")

	# If "busy" == 0 and reception is going we close it
	if buff[14] == "0" and inreception:
		inreception = False
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

			# Print first log part
			string = "[{}] - \"{}\" - {} Mhz [{}] ({}/5) : {} => {}".format(start.strftime("%Y-%m-%d %H:%M:%S"), alphatag, frequency, mode, power, channel, bank)
			print(string, end="", flush=True)
			logfile.write(string)
	
	time.sleep(0.1)
