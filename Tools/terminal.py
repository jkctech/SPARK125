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
	cmd = input(">> ").upper()

	# Custom commands
	# Clear
	if (cmd == "CLEAR" or cmd == "CLS"):
		if os.name == 'posix':
			os.system('clear')
		else:
			os.system('cls')
		continue

	# Exit
	elif (cmd == "EXIT"):
		userial.command(ser, "EPG")
		ser.close()
		break

	# Execute command & Print result
	print(userial.command(ser, cmd))
