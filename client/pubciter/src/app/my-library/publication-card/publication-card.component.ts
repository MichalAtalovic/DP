import { QuarantineDialogComponent } from '../../dialogs//quarantine-dialog/quarantine-dialog.component';
import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { QuarantineService } from 'src/app/services/quarantine.service';
@Component({
  selector: 'app-publication-card',
  templateUrl: './publication-card.component.html',
  styleUrls: ['./publication-card.component.css']
})
export class PublicationCardComponent implements OnInit {

  @Input() public publication: any;
  @Input() public template: any;

  @Output() public publicationMoved: EventEmitter<any> = new EventEmitter<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['position', 'title', 'author', 'journal', 'publicationYear'];
  public dataSource: MatTableDataSource<any> = new MatTableDataSource();
  public paginatedData: MatTableDataSource<any> = new MatTableDataSource();
  public showCitationsState = false;

  constructor(
    public dialog: MatDialog,
    public quarantineService: QuarantineService
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

}
