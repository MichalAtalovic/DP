import { AuthorService } from './../services/author.service';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { QuarantineDialogComponent } from '../dialogs/quarantine-dialog/quarantine-dialog.component';
import { DataService } from '../services/data.service';
import { QuarantineService } from '../services/quarantine.service';
import * as astrocite from 'astrocite';
import { PublicationService } from '../services/publication.service';
import { BibliographyParser } from 'src/utils/bibliography-parser';
import { Router } from '@angular/router';
import { ExportDialogComponent } from '../dialogs/export-dialog/export-dialog.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  @Input() public header: any;
  @Input() public iconPath: any;
  @Input() public collapsed = true;

  @Output() public widthEmitter = new EventEmitter<any>();
  @Output() public action = new EventEmitter<any>();

  constructor(
    private dataService: DataService,
    private quarantineService: QuarantineService,
    private dialog: MatDialog,
    private publicationService: PublicationService,
    private authorService: AuthorService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  collapse(value: boolean) {
    this.collapsed = value;
    this.widthEmitter.emit(value);
    this.dataService.sendData({ action: 'ACTION PANEL RESIZE', data: value });
  }

  panelActionExecute(args: any) {
    switch (args?.action) {
      case 'CLEAR QUARANTINE':
        this.openQuarantineDialog({ operation: 'remove all' });
        break;
      case 'IMPORT PUBLICATION':
        switch (args.format?.toLowerCase()) {
          case 'bib':
            console.log('BIB file');
            new BibliographyParser(this.authorService).preparePublications(astrocite.bibtex.parse(args.raw) as any).then(result => {
              result.forEach(x => {
                console.log('INSERTING');
                console.log(x);
                this.publicationService.insertPublication(x).then(() => {
                  this.dataService.sendData({ action: 'INSERTED PUBLICATION', data: x });
                });
              })
            });

            break;
          case 'ris':
            console.log('RIS file');
            new BibliographyParser(this.authorService).preparePublications(astrocite.ris.parse(args.raw) as any).then(result => {
              result.forEach(x => {
                console.log('INSERTING');
                console.log(x);
                this.publicationService.insertPublication(x).then(() => {
                  this.dataService.sendData({ action: 'INSERTED PUBLICATION', data: x });
                });
              })
            });

            break;
          default:
            console.log('unsupported format');
            break;
        }
        break;
      case 'OPEN_PUBLICATION_CATEGORY_ENUM':
        this.router.navigateByUrl('/enums?view=publicationCategory');
        break;
      case 'OPEN_CITATION_CATEGORY_ENUM':
        this.router.navigateByUrl('/enums?view=citationCategory');
        break;
      case 'OPEN_EXPORT_FORMAT_ENUM':
        this.router.navigateByUrl('/enums?view=exportFormat');
        break;
      case 'EXPORT':
        this.openExportDialog();
        break;
    }
  }

  openQuarantineDialog(args: any): void {
    const dialogRef = this.dialog.open(QuarantineDialogComponent, {
      data: { args }
    });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'remove all':
          this.quarantineService.clearQuarantine();
          this.action.emit({ action: 'CLEAR QUARANTINE' });
          break;
        default:
          break;
      }
    });
  }

  openExportDialog(): void {
    const dialogRef = this.dialog.open(ExportDialogComponent, {
      data: { title: 'Export data' }
    });

    dialogRef.afterClosed().subscribe(result => {
      const exportFormat = result?.exportFormat;
      switch (result?.operation) {
        case 'export':
          this.publicationService.getCitations(result.data.yearFrom, result.data.yearTo, result.data.publicationCategories, result.data.citationCategories).then((result: any) => {
            let grouped = _.groupBy(result, function (result) {
              return result.key.publicationId;
            });

            let htmlString = '';

            for (let key in grouped) {
              let citations = grouped[key];
              if (citations && (citations as any)?.length > 0) {
                (citations as any)?.forEach((citation: any) => {
                  let template = exportFormat?.replace(/\$|,/g, '@');
                  Object.keys(citation.value)?.forEach(prop => {
                    template = template.replace(`@{${prop}}`, citation.value[prop] ?? '');
                  });

                  htmlString += template + '</br>-------------------------------------------</br></br>'
                });
              }
            }

            this.buildFile(htmlString);
          });

          break;
        default:
          break;
      }
    });
  }

  public buildFile(content: string) {

    var file = new Blob([content], { type: 'html' });
    if (window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(file, `export_${new Date().getMonth()}_${new Date().getDay()}_${new Date().getFullYear()}_.html`);
    }
    else {
      var a = document.createElement("a"),
        url = URL.createObjectURL(file);
      a.href = url;
      a.download = `export_${new Date().getMonth()}_${new Date().getDay()}_${new Date().getFullYear()}_.html`;
      document.body.appendChild(a);
      a.click();
      setTimeout(function () {
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
      }, 0);
    }
  }

}
