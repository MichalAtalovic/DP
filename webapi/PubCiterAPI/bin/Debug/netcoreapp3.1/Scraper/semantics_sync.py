import semanticscholar as sch
import sys
import getopt

# Synchronization method
def sync(doi) -> object:
    paper = sch.paper(doi, timeout=5)

    citations = []

    for citation in paper['citations']:
        citations.append({
            'title': citation['title'] if citation['title'] != None else '',
            'pub_url': citation['url'] if citation['url'] != None else '',
            'journal': citation['venue'] if citation['venue'] != None else '',
            'pub_year': citation['year'] if citation['year'] != None else '',
            'doi': citation['doi'] if citation['doi'] != None else ''
        })

    print(citations)

# main method
def main(argv):
    try:
        opts, args = getopt.getopt(argv, "h:doi:", ["doi="])
    except getopt.GetoptError:
        print(
            'ERROR> Usage: $> semantics_sync.py [--h] --doi <publication DOI>')
        sys.exit(2)

    if (len(args) == 1):
        print(
            'ERROR> Usage: $> semantics_sync.py [--h] --doi <publication DOI>')

    for opt, arg in opts:
        if opt == '--h' or opt == '-h':
            print('Usage: $> semantics_sync.py [--h] --doi <publication DOI>')
            sys.exit()
        elif opt in ("-doi", "--doi"):
            sync(arg)
            sys.exit()


main(sys.argv[1:])
