from uniden import userial

# Get serial connection & Open
ser = userial.get()
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