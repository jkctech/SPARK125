from uniden import userial
import sys

# Check args
if len(sys.argv) != 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

print(userial.command(ser, "STS"))

ser.close()