import { AuthorService } from './../services/author.service';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { QuarantineDialogComponent } from '../dialogs/quarantine-dialog/quarantine-dialog.component';
import { DataService } from '../services/data.service';
import { QuarantineService } from '../services/quarantine.service';
import * as astrocite from 'astrocite';
import { PublicationService } from '../services/publication.service';
import { BibliographyParser } from 'src/utils/bibliography-parser';

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
    private authorService: AuthorService
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
}
