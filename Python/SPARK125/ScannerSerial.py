import serial
import time

from .Logger import Logger

class ScannerSerial:
	_serial = None

	def __init__(self, port):
		try:
			ser = serial.Serial()
			ser.baudrate = 9600
			ser.port = port
			ser.open()
			self._serial = ser
		except serial.SerialException:
			Logger.error("Could not open serial port '{}'!".format(port))

	def cmd(self, command):
		try:
			self._serial.write((command + "\r").encode())
			time.sleep(0.015)
			while True:
				try:
					data = self._serial.read_all()
					data = data.decode().strip()
				except Exception:
					return data
				if len(data) > 0:
					if data == "ERR":
						return False
					else:
						return data[4:]
		except serial.SerialException as e:
			Logger.error("Could not send command: {}".format(str(e)))
