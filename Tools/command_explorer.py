from uniden import userial
import sys

def char_range(c1, c2):
	for c in range(ord(c1), ord(c2) + 1):
		yield chr(c)

# Skipping list
skip = [
	"POF",
	"PRG"
]

# Check args
if len(sys.argv) != 2:
	print("Please provide a serial port to connect to.")
	exit(1)

# Get serial connection & Open
ser = userial.get(sys.argv[1])
ser.open()

# Generate all possible commands
for a in char_range('A', 'Z'):
	for b in char_range('A', 'Z'):
		for c in char_range('A', 'Z'):
			cmd = a + b + c

			if cmd in skip:
				continue
			else:
				output = userial.command(ser, cmd)

			if output != False:
				print(cmd, end="")
				print(" >> '", output, "'", sep="", end="")
				print()

ser.close()