import serial
import time

# Get serial object
def get():
	ser = serial.Serial()
	ser.baudrate = 9600
	ser.port = "COM19"
	return ser

# Command wrapper
def command(ser, command):
	ser.write((command + "\r").encode())
	time.sleep(0.015)
	while True:
		try:
			data = ser.read_all()
			data = data.decode().strip()
		except Exception:
			return data
		if len(data) > 0:
			if data == "ERR":
				return False
			else:
				return data