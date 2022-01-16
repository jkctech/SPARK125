import pyaudio

p = pyaudio.PyAudio()

for i in range(0, p.get_host_api_info_by_index(0).get('deviceCount')):
		if (p.get_device_info_by_host_api_device_index(0, i).get('maxInputChannels')) > 0:
			print("Input Device id ", i, " - ", p.get_device_info_by_host_api_device_index(0, i).get('name'))