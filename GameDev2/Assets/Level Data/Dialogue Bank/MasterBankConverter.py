import csv
import json

tempReader = ''
masterBank = []
with open('MasterBank.tsv', 'r') as csvfile:
    tempReader = csv.reader(csvfile, delimiter='\t', quotechar="|")
    #print(tempReader)
    for row in tempReader:
        masterBank.append(row)
for a in masterBank[1:len(masterBank)]:
    for b in range(0,len(a)):
        if b == 0:
            jsonName = a[b]
        if b == 1:
            dialogueFile = a[b]
        if b == 2:
            firstSpeaker = a[b]
        if b == 3:
            secondSpeaker = a[b]
        if b == 4:
            f_SpkSpr = a[b]
        if b == 5:
            s_SpkSpr = a[b]
        if b == 6:
            allDialogue = []
            speakerSequence = []
            with open(dialogueFile + '.tsv', 'r') as csvfile:
                tempReader = csv.reader(csvfile, delimiter='\t', quotechar="|")
                for row in tempReader:
                    print(row)
                    allDialogue.append(row[1])
                    speakerSequence.append(row[0])
            with open(jsonName + '.json', 'w') as outfile:
                data = {}
                data['allDialogue'] = allDialogue
                data['speakerSeq'] = speakerSequence
                data['firstSpeaker'] = firstSpeaker
                data['secondSpeaker'] = secondSpeaker
                data['fsSprite'] = f_SpkSpr
                data['ssSprite'] = s_SpkSpr
                data['chatType'] = 'chat'
                json.dump(data, outfile)
