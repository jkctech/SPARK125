from uniden import userial

# Get serial connection & Open
ser = userial.get()
ser.open()

# Enable Programming Mode
userial.command(ser, "PRG")

# Dump all memory channels
for i in range(1,501):
	print(userial.command(ser, "CIN," + str(i)))

# Disable Programming Mode
userial.command(ser, "EPG")

# Close serial
ser.close()