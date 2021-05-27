from uniden import userial

# Get serial connection & Open
ser = userial.get()
ser.open()

print(userial.command(ser, "STS"))

ser.close()