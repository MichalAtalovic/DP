import re
import sys
import getopt

def parse_ris(data):

    lines = data.split('\n')
    entries = dict()
    entries['author'] = list()
    entries['author'].append('')
    for TempLine in lines:
        if re.match('TY', TempLine):
            entries['Type'] = TempLine[6:]
        elif re.match('PB', TempLine):
            entries['publisher'] = TempLine[6:]
        elif re.match('ID', TempLine):
            entries['doi'] = TempLine[6:]
        elif re.match('TI', TempLine) or re.match('T1', TempLine) or re.match('T2', TempLine):
            entries['title'] = TempLine[6:]
            entries['journal'] = TempLine[6:]
        elif re.match('PY', TempLine):
            entries['year'] = TempLine[6:10]
        elif re.match('UR', TempLine):
            entries['url'] = TempLine[6:]
        elif re.match('Y1', TempLine):
            entries['year'] = TempLine[6:10]
        elif re.match('JO', TempLine) or re.match('J1', TempLine) or re.match('JF', TempLine) or re.match('JA', TempLine):
            entries['journal'] = TempLine[6:]
        elif re.match('A1', TempLine):
            entries['author'][0].append(TempLine[6:])
        elif re.match("AU", TempLine):
            entries['author'].append(TempLine[6:])
        elif re.match('VL', TempLine):
            entries['volume'] = TempLine[6:]
        elif re.match('IS', TempLine):
            entries['issue'] = TempLine[6:]
        elif re.match('SP', TempLine):
            entries['startpage'] = TempLine[6:]
        elif re.match('EP', TempLine):
            entries['endpage'] = TempLine[6:]
        elif re.match('AB', TempLine):
            entries['abstract'] = TempLine[6:]

    if entries['author'][0] == '':
        entries['author'].pop(0)
    return entries


def ris2bib(entries):

    bib_file_name = name_bib(entries)

    # clean previosly created file
    bib = open("output.bib", 'w')
    bib.write("")
    bib.close()

    bib = open("output.bib", 'w+')
    bib.write('@article{' + bib_file_name[0:-4] + ',')
    bib.write('\n\ttitle = \"' + entries['title']+'\",')
    if ('author' in entries):
        bib.write('\n\tauthor = \"' + entries['author'][0])
        for entry in entries['author']:
            bib.write(' and ' + entry)
        bib.write("\",")
    if ('journal' in entries):
        bib.write('\n\tjournal = \"' + entries['journal']+'\",')
    if ('volume' in entries):
        bib.write('\n\tvolume = \"' + entries['volume']+'\",')
    if ('issue' in entries):
        bib.write('\n\tissue = \"' + entries['issue']+'\",')
    if ('startpage' in entries and 'endpage' in entries):
        bib.write('\n\tpages = \"' + entries['startpage'] + ':'
              + entries['endpage'] + '\",')
    if ('year' in entries):
        bib.write('\n\tyear = \"' + entries['year']+'\",')
    if ('publisher' in entries):
        bib.write('\n\tpublisher = \"' + entries['publisher']+'\",')
    if ('doi' in entries):
        bib.write('\n\tdoi = \"' + entries['doi']+'\",')
    if ('url' in entries):
        bib.write('\n\turl = \"' + entries['url']+'\",')
    if ('abstract' in entries):
        bib.write('\n\tabstract = \"' + str(entries['abstract'].encode("ascii", "ignore")).lstrip('b\'').rstrip('\'') + '\",')
    bib.write('\n}')
    bib.close()

    bib = open("output.bib", 'r')
    out = bib.read()
    bib.close()

    return out

# create file name
def name_bib(entries):
    start_pos = entries['author'][0].index(',')+2
    if ('.' in entries['author'][0][start_pos:len(entries['author'][0])]):
        FirstAuthorLastName = entries['author'][0][0:start_pos-2]
    else:
        end_pos = len(entries['author'][0])
        FirstAuthorLastName = entries['author'][0][start_pos:end_pos]
    return FirstAuthorLastName +\
        entries['year'] + '.bib'


# main method
def main(argv):
    try:
        opts, args = getopt.getopt(
            argv, "h:data:", ["data="])
    except getopt.GetoptError:
        sys.exit(
            'Usage: $> ris2bib.py [--h] --data <input string>')

    if (len(opts) != 1):
        sys.exit(
            'ERROR: Usage: $> ris2bib.py [--h] --data <input string>')

    data = ''

    for opt, arg in opts:
        if opt == '--h' or opt == '-h':
            sys.exit(
                'Usage: $> ris2bib.py [--h] --data <input string>')
        elif opt in ("-data", "--data"):
            data = arg
    
    result = ''
            
    for record in data.split('||'):
        result = result + ris2bib(parse_ris(data.replace('|', '\n'))) + '\n\n'
    print(result)

main(sys.argv[1:])
