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

# Dump all settings
print(userial.command(ser, "BLT"))
print(userial.command(ser, "BSV"))
print(userial.command(ser, "KBP"))
print(userial.command(ser, "PRI"))
print(userial.command(ser, "SCG"))
print(userial.command(ser, "SCO"))
print(userial.command(ser, "CLC"))
print(userial.command(ser, "SSG"))
print(userial.command(ser, "CSG"))
print(userial.command(ser, "WXS"))
print(userial.command(ser, "CNT"))
print(userial.command(ser, "VOL"))
print(userial.command(ser, "SQL"))

# Disable Programming Mode
userial.command(ser, "EPG")

# Close serial
ser.close()