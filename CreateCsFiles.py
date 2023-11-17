import os


with open('DayXXTemplate.cs') as f:
    lines = f.readlines()

for i in range(1, 26):
    newLines = []
    for l in lines:
        newLines += [l.replace("Day1", ("Day%d" % i)).replace("day1", ("day%d" % i))]
    with open ("Day%d.cs" % i, "w") as outf:
        outf.writelines(newLines)
