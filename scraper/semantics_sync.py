import semanticscholar as sch
import codecs
import json
import sys, getopt

# Synchronization method
def sync(doi):
    paper = sch.paper(doi, timeout=5)

    citations = []

    for citation in paper['citations']:
        citations.append({
            'title': citation['title'],
            'pub_url': citation['url'],
            'journal': citation['venue'],
            'pub_year': citation['year'],
            'doi': citation['doi']
        })
        
    # export json to file
    with codecs.open('output/semantics_output.json', 'w', 'utf-8') as f:
        json.dump(citations, f, ensure_ascii = False)

    sys.exit(0)

# main method
def main(argv):
   try:
      opts, args = getopt.getopt(argv,"h:doi:", ["doi="])
   except getopt.GetoptError:
      print('ERROR> Usage: $> semantics_sync.py [--h] --doi <publication DOI>')
      sys.exit(2)

   if (len(args) == 1):
      print('ERROR> Usage: $> semantics_sync.py [--h] --doi <publication DOI>')

   for opt, arg in opts:
      if opt == '--h' or opt == '-h':
         print('Usage: $> semantics_sync.py [--h] --doi <publication DOI>')
         sys.exit()
      elif opt in ("-doi", "--doi"):
         sync(arg)

main(sys.argv[1:])