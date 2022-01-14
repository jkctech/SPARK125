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

# Dump DBC areas 1 - 25
for i in range(1,26):
	print(userial.command(ser, "DBC," + str(i)))

# Disable Programming Mode
userial.command(ser, "EPG")

# Close serial
ser.close()