import { QuarantineDialogComponent } from '../../dialogs//quarantine-dialog/quarantine-dialog.component';
import { Component, EventEmitter, Input, OnInit, Output, SecurityContext, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog } from '@angular/material/dialog';
import { QuarantineService } from 'src/app/services/quarantine.service';
import { BibliographyParser } from 'src/utils/bibliography-parser';
import * as astrocite from 'astrocite';
import { AuthorService } from 'src/app/services/author.service';
import { PublicationService } from 'src/app/services/publication.service';
import { MatMenuTrigger } from '@angular/material/menu';
import { RemoveCitationDialogComponent } from 'src/app/dialogs/remove-citation-dialog/remove-citation-dialog.component';
import { EditCitationDialogComponent } from 'src/app/dialogs/edit-citation-dialog/edit-citation-dialog.component';
import { DomSanitizer } from '@angular/platform-browser'
import { PipeTransform, Pipe } from "@angular/core";

@Pipe({ name: 'safeHtml'})
export class SafeHtmlPipe implements PipeTransform  {
  constructor(private sanitized: DomSanitizer) {}
  transform(value: any) {
    return this.sanitized.bypassSecurityTrustHtml(value);
  }
}

@Pipe({ name: 'highlight'})
export class HighlightPipe implements PipeTransform  {
  constructor() {}
  transform(value: any, searchedText: string): string {
    return value?.replace(new RegExp(searchedText, 'g'), '<b><span style="background-color: yellow; color: darkslategrey; border-radius:3px;">' + searchedText + '</span></b>');
  }
}

@Component({
  selector: 'app-publication-card',
  templateUrl: './publication-card.component.html',
  styleUrls: ['./publication-card.component.css']
})
export class PublicationCardComponent implements OnInit {

  @Input() public publication: any;
  @Input() public template: any;
  @Input() public searchedText: any;

  @Output() public publicationMoved: EventEmitter<any> = new EventEmitter<any>();
  @Output() public onRemovePublication: EventEmitter<any> = new EventEmitter<any>();
  @Output() public onEditPublication: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatMenuTrigger) matMenu!: MatMenuTrigger;

  public displayedColumns: string[] = ['position', 'title', 'authors', 'journal', 'publicationYear', 'opts'];
  public dataSource: MatTableDataSource<any> = new MatTableDataSource();
  public paginatedData: MatTableDataSource<any> = new MatTableDataSource();
  public showCitationsState = false;
  public publicationId: any;

  constructor(
    public dialog: MatDialog,
    public quarantineService: QuarantineService,
    public authorService: AuthorService,
    public publicationService: PublicationService,
    private sanitizer: DomSanitizer
  ) { }

  openQuarantineDialog(args: any): void {
    const dialogRef = this.dialog.open(QuarantineDialogComponent, {
      data: { title: this.publication.title, args }
    });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'add':
          this.quarantineService.addToQuarantine(result?.publicationId);
          this.publicationMoved.emit(result?.publicationId)
          break;
        case 'remove':
          this.quarantineService.removeFromQuarantine(result?.publicationId);
          this.publicationMoved.emit(result?.publicationId)
          break;
        default:
          break;
      }
    });
  }

  ngOnInit(): void { }

  ngAfterViewInit() {
    // append position
    if (this.publication.publicationCitationList) {
      this.publication.publicationCitationList.forEach((value: any, index: number) => {
        value.citation.position = index + 1;
      })
    }

    this.dataSource = new MatTableDataSource(this.publication.publicationCitationList);
  }

  showCitations() {
    this.showCitationsState = !this.showCitationsState;
    if (this.showCitationsState) {
      setTimeout(() => this.dataSource.paginator = this.paginator);
    }
  }

  readDocument() {
    if (this.publication.eprintUrl) {
      window.open(this.publication.eprintUrl)
    }
  }

  onClickImport(event: any) {
    document.getElementById('fileInput_' + this.publication.publicationId.toString())?.click();
  }

  handleImportedFile(data: any) {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      switch (data.target.files[0].name.split('.')[data.target.files[0].name.split('.').length - 1]?.toLowerCase()) {
        case 'bib':
          console.log('BIB file');
          new BibliographyParser(this.authorService).preparePublications(astrocite.bibtex.parse(fileReader.result as string) as any).then(result => {
            result.forEach(x => {
              console.log('INSERTING CITATION');
              console.log(x);
              console.log('FOR PUBLICATION: ' + this.publication.publicationId);
              this.publicationService.insertCitation(x, this.publication.publicationId);
              this.publication.publicationCitationList.push({
                citation: x
              });
            });

            this.ngAfterViewInit();
          });

          break;
        case 'ris':
          console.log('RIS file');
          new BibliographyParser(this.authorService).preparePublications(astrocite.ris.parse(fileReader.result as string) as any).then(result => {
            result.forEach(x => {
              console.log('INSERTING CITATION');
              console.log(x);
              this.publicationService.insertCitation(x, this.publication.publicationId);
              this.publication.publicationCitationList.push({
                citation: x
              });
            });

            this.ngAfterViewInit();
          });

          break;
        default:
          console.log('unsupported format');
          break;
      }
    }

    fileReader.readAsText(data.target.files[0]);
  }

  removeCitation(citation: any) {
    const dialogRef = this.dialog.open(RemoveCitationDialogComponent, {
      data: { citation }
    });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'remove':
          this.publicationService.removeCitation(this.publication.publicationId, result?.data?.citationId).then(() => {
            this.publication.publicationCitationList = this.publication.publicationCitationList.filter((x: any) => x.citation.citationId !== result?.data?.citationId);
            this.ngAfterViewInit();
          });

          break;
        default:
          break;
      }
    });
  }

  editCitation(citation: any) {
    const dialogRef = this.dialog.open(EditCitationDialogComponent, {
      data: { citation }
    });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'update':
          this.publicationService.updateCitation(result.data, this.publication.publicationId).then(() => {
            this.ngAfterViewInit();
          });

          break;
        default:
          break;
      }
    });
  }

  removePublication(publication: any) {
    this.onRemovePublication.emit(publication);
  }

  editPublication(publication: any) {
    this.onEditPublication.emit(publication);
  }

}
