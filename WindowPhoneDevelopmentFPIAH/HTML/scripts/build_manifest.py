
import os

TOP_DIR = '..'
MANIFEST_FN = '..\\MANIFEST.txt'

fh = open(MANIFEST_FN, 'w')

for path, dirs, fns in os.walk(TOP_DIR):
    if 'noupload' in path:
        continue
    if 'scripts' in path:
        continue
    for fn in fns:
        line = os.path.join(path, fn)[len(TOP_DIR)+1:]
        fh.write(line + '\n')
fh.close()


