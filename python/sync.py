from scholarlylib.scholarly import scholarly
import json
import codecs
import sys, getopt

# Synchronization method
def sync(author):

   search_query = scholarly.search_author(author)
   author = scholarly.fill(next(search_query))
   publications = []

   for pub in author['publications']:
      pub = scholarly.fill(pub)
      pub_struct = {
            'bib': pub['bib'],
            'citations': [citation['bib'] for citation in scholarly.citedby(pub)],
            'num_citations': pub['num_citations'] if 'num_citations' in pub.keys() else 0,
            'pub_url': pub['pub_url'] if 'pub_url' in pub.keys() else '',
            'eprint_url': pub['eprint_url'] if 'eprint_url' in pub.keys() else '',
            'cites_per_year': pub['cites_per_year']
      }

      publications.append(pub_struct)

   data = {
      'author': {
         'scholar_id': author['scholar_id'] if 'scholar_id' in author.keys() else '',
         'url_picture': author['url_picture'] if 'url_picture' in author.keys() else '',
         'affiliation': author['affiliation'] if 'affiliation' in author.keys() else '',
         'total_cites': author['citedby'] if 'citedby' in author.keys() else 0
      },
      'publications': publications
   }

   # export json to file
   with codecs.open('output/output.json', 'w', 'utf-8') as f:
      json.dump(data, f, ensure_ascii = False)

def main(argv):
   try:
      opts, args = getopt.getopt(argv,"h:author:", ["author="])
   except getopt.GetoptError:
      print('ERROR> Usage: $> sync.py [--h] --author <author name>')
      sys.exit(2)

   if (len(args) == 1):
      print('ERROR> Usage: $> sync.py [--h] --author <author name>')

   for opt, arg in opts:
      if opt == '--h' or opt == '-h':
         print('Usage: $> sync.py [--h] --author <author name>')
         sys.exit()
      elif opt in ("-author", "--author"):
         sync(arg)

main(sys.argv[1:])
