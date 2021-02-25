import sys
import getopt
import os
import codecs
import json

# convert method
def convert(input_format, output_format, data):
    input_file = open("input." + input_format, "w")
    input_file.write(data.replace("    ", "|").replace("||||", "\n                ").replace("|||", "\n            ").replace("||", "\n        ").replace("|", "\n    "))
    input_file.close()

    os.system('pybtex-convert input.' + input_format +
              ' output.' + output_format)

    output_file = open("output." + output_format, "r")
    output = output_file.read()
    output_file.close()

    return output

# main method
def main(argv):
    try:
        opts, args = getopt.getopt(
            argv, "h:input-format:output-format:data:", ["input-format=", "output-format=", "data="])
    except getopt.GetoptError:
        sys.exit(
            'Usage: $> pybtex_convert.py [--h] --input-format <format> --output-format <format> --data <input string>')

    if (len(opts) != 3):
        sys.exit(
            'ERROR: Usage: $> pybtex_convert.py [--h] --input-format <format> --output-format <format> --data <input string>')

    input_format = ''
    output_format = ''
    data = ''

    for opt, arg in opts:
        if opt == '--h' or opt == '-h':
            sys.exit(
                'Usage: $> pybtex_convert.py [--h] --input-format <format> --output-format <format> --data <input string>')
        elif opt in ("-input-format", "--input-format"):
            input_format = arg
        elif opt in ("-output-format", "--output-format"):
            output_format = arg
        elif opt in ("-data", "--data"):
            data = arg

    print(convert(input_format, output_format, data))

main(sys.argv[1:])
