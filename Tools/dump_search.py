from uniden import userial
import sys

# Check args
if len(sys.argv) != 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

# Enable Programming Mode
userial.command(ser, "PRG")

# Dump Custom Search Banks
for i in range(1,11):
	print(userial.command(ser, "CSP," + str(i)))

# Disable Programming Mode
userial.command(ser, "EPG")

# Close serial
ser.close()