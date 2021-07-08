from uniden import userial
import sys
import os

# Check args
if len(sys.argv) != 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

# Loop
while True:
	cmds = input(">> ").upper().split(';')
	
	for cmd in cmds:
	
		cmd = cmd.strip()

		# Custom commands
		# Clear
		if (cmd == "CLEAR" or cmd == "CLS"):
			if os.name == 'posix':
				os.system('clear')
			else:
				os.system('cls')
			continue
		
		elif (len(cmd) > 3 and cmd[0:5] == "KEYS,"):
			chars = cmd[5:]

			for c in chars:
				userial.command(ser, "KEY,{},P".format(c))
			continue

		# Exit
		elif (cmd == "EXIT"):
			try:
				userial.command(ser, "EPG")
				ser.close()
			except Exception:
				pass
			exit(0)

		# Execute command & Print result
		print(userial.command(ser, cmd))
