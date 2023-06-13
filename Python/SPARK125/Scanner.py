from .Logger import Logger
from .ScannerSerial import ScannerSerial

class Scanner:
	_serial = None

	# Init scanner
	def __init__(self, port):
		self.connect(port)
	
	# Init serial connection manually
	def connect(self, port):
		self._serial = ScannerSerial(port)
	
	# Send raw command
	def cmd(self, command):
		if self._serial != None:
			return self._serial.cmd(str(command))
		else:
			Logger.error("Serial port not initialized.")

	# Get scanner model
	def model(self):
		return self.cmd("MDL")
	
	# Get scanner firmware version
	def firmware(self):
		return self.cmd("VER")

	# Set scanner volume [0-15]
	def volume(self, volume = None):
		if volume == None:
			return self.cmd("VOL")

		try:
			vol = int(volume)
		except ValueError:
			Logger.error("Volume '{}' is invalid. [0-15]")

		if vol < 0 or vol > 15:
			Logger.error("Volume '{}' out of range. [0-15]")

		return self.cmd("VOL," + str(volume)) == "OK"
	
	# Set scanner squelch [0-15]
	def squelch(self, squelch =  None):
		if squelch == None:
			return self.cmd("SQL")

		try:
			vol = int(squelch)
		except ValueError:
			Logger.error("Squelch '{}' is invalid. [0-15]")

		if vol < 0 or vol > 15:
			Logger.error("Squelch '{}' out of range. [0-15]")

		return self.cmd("SQL," + str(squelch)) == "OK"

	# Close connection to scanner
	def close(self):
		try:
			self._serial.close()
			self._serial = None
		except Exception:
			pass
