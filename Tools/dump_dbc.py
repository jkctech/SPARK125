from uniden import userial

# Get serial connection & Open
ser = userial.get()
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