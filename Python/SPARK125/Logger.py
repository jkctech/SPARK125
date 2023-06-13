class Logger:
	def log(msg, severity):
		print("[SPARK125] - {}: {}".format(severity, msg))

	def error(msg):
		Logger.log(msg, "Error")
