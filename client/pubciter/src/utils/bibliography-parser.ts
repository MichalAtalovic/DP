import { AuthorService } from "src/app/services/author.service";
import { Publication } from "src/model/publication";

export class BibliographyParser {

  constructor(
    private authorService: AuthorService
  ) { }

  public async preparePublications(jsonArray: any[]): Promise<Publication[]> {
    const publications: Publication[] = [];

    const authors = await this.authorService.getAuthors();
    let authorId = (authors as any[])[0].authorId;

    if (jsonArray) {
      jsonArray.forEach(x => {
        let authors = '';
        (x.author as any[])?.forEach((author, index) => {
          let item = author.given + ' ' + author.family;
          if (index < ((x.author as any[]).length - 1)) {
            item += ', ';
          }

          authors += item;
        });

        publications.push({
          title: x.title,
          doi: x.DOI ?? '',
          authors: authors,
          publicationYear: x.issued['date-parts'][0][0] ?? '',
          pages: x.page,
          journal: x['container-title'] ?? '',
          journalVolume: x.volume ?? '',
          authorId: Number(authorId as any)
        } as Publication);
      });
    }

    return await publications;
  }
}
