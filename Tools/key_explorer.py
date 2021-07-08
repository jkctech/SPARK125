from uniden import userial
import sys

def char_range(c1, c2):
	for c in range(ord(c1), ord(c2) + 1):
		yield chr(c)

# Check args
if len(sys.argv) != 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

# Generate all possible commands
for c in char_range('A', 'Z'):
    output = userial.command(ser, "KEY," + c + ",P")
    print(c, output)

ser.close()