import wave
import os
import re

from pathlib import Path

base = "./logs/"
folders = []
groups = {}

for folder in os.walk(base):
	p = folder[0]
	
	if re.match(r"^\d{4}-\d{2}-\d{2}$",  p.replace(base, "")):
		folders.append(p)

for folder in folders:
	Path(folder + "/stitched").mkdir(parents=True, exist_ok=True)

	for fname in os.listdir(folder):
		path = folder + "/" + fname
		
		if os.path.isfile(path):
			date = folder.replace(base, "")
			tag = date + "/" + fname.split(']')[1][1:-4]
			
			if tag not in groups.keys():
				groups[tag] = []
			
			groups[tag].append(path)
		
for g in groups.keys():

	print("Stitching {}...".format(g))

	files = groups[g]

	date = g.split('/')[0]
	folder = files[0].split('[')[0] + "stitched"
	outfile = "{}/[{}] {}.wav".format(folder, date, g.split('/')[1])

	data = []
	for f in files:
		w = wave.open(f, 'rb')
		data.append([w.getparams(), w.readframes(w.getnframes())])
		w.close()
	
	output = wave.open(outfile, 'wb')
	output.setparams(data[0][0])
	for i in range(len(data)):
		output.writeframes(data[i][1])
	output.close()