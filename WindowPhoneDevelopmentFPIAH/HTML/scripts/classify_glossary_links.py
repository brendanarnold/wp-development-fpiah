
import os
from bs4 import BeautifulSoup

for fn in os.listdir("../"):
    if not fn.endswith(".html"):
        continue

    fh = open('..\\' + fn)

    soup = BeautifulSoup(fh)

    for a in soup.find_all('a'):
        if a.get('href') is None:
            continue
        if not a.get('href').startswith('glossary.html'):
            continue
        a["class"] = "glossaryterm"
    out_fh = open('dump\\' + fn, 'w')
    out_fh.write(soup.prettify())
        


# import elementtree.ElementTree as ET
# import os



#     fp = '..\\' + fn
#     fh = open(fp)
#     tree = ET.parse(fh)
#     for a in tree.findall(".//a"):
#         href = a.attrib.get("href")
#         if href is None:
#             continue
#         if href.startswith("glossary.html"):
#             a.attrib["class"] = "glossaryterm"
#     out_fh = open("dump\\" + fn, "w")
#     tree.write(out_fh)
#     
# 
