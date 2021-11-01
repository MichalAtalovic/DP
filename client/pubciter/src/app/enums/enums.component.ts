import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { CitationCategoryDialogComponent } from '../dialogs/citation-category-dialog/citation-category-dialog.component';
import { ExportFormatDialogComponent } from '../dialogs/export-format-dialog/export-format-dialog.component';
import { PublicationCategoryDialogComponent } from '../dialogs/publication-category-dialog/publication-category-dialog.component';
import { EnumService } from '../services/enum.service';

@Component({
  selector: 'app-enums',
  templateUrl: './enums.component.html',
  styleUrls: ['./enums.component.css']
})
export class EnumsComponent implements OnInit {

  @ViewChild(MatMenuTrigger) matMenu!: MatMenuTrigger;

  public panelData = { header: 'Enums', iconPath: 'assets/enum_fade.png' }
  public currentView = 'exportFormat';
  public displayedColumnsExportFormats: string[] = ['code', 'template', 'opts'];
  public displayedColumnsCitationCategories: string[] = ['code', 'name', 'opts'];
  public displayedColumnsPublicationCategories: string[] = ['group', 'code', 'name', 'opts'];
  public dataSource: MatTableDataSource<any> = new MatTableDataSource();

  public citationCategoryData: MatTableDataSource<any> = new MatTableDataSource();
  public publicationCategoryData: MatTableDataSource<any> = new MatTableDataSource();
  public exportFormatData: MatTableDataSource<any> = new MatTableDataSource();

  constructor(
    private activeRoute: ActivatedRoute,
    private enumService: EnumService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.activeRoute.queryParams?.subscribe(x => {
      if (x.view) {
        this.currentView = x.view;
      }
    });
  }

  ngAfterViewInit() {
    Promise.all([
      this.enumService.getExportFormats(),
      this.enumService.getCitationCategories(),
      this.enumService.getPublicationCategories()
    ]).then(values => {
      this.exportFormatData = new MatTableDataSource(values[0] as any);
      this.citationCategoryData = new MatTableDataSource(values[1] as any);
      this.publicationCategoryData = new MatTableDataSource(values[2] as any);
    })
  }

  resolveHeader() {
    switch (this.currentView) {
      case 'exportFormat':
        return 'Export formats';
      case 'publicationCategory':
        return 'Publication categories';
      case 'citationCategory':
        return 'Citation categories';
      default:
        return '';
    }
  }

  removeExportFormat(id: number) {
    this.enumService.deleteExportFormat(id).then(() => {
      this.enumService.getExportFormats().then((data: any) => {
        this.exportFormatData = new MatTableDataSource(data);
      });
    });
  }

  removePublicationCategory(id: number) {
    this.enumService.deletePublicationCategory(id).then(() => {
      this.enumService.getPublicationCategories().then((data: any) => {
        this.publicationCategoryData = new MatTableDataSource(data);
      });
    });
  }

  removeCitationCategory(id: number) {
    this.enumService.deleteCitationCategory(id).then(() => {
      this.enumService.getCitationCategories().then((data: any) => {
        this.citationCategoryData = new MatTableDataSource(data);
      });
    });
  }

  openExportFormatDialog(args: any, title: string): void {
    if (!args?.data) {
      args.data = {};
    }
    const dialogRef = this.dialog.open(ExportFormatDialogComponent, { data: { title, args } });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'update':
          this.enumService.insertUpdateExportFormat(result.data).then(obj => {
            this.enumService.getExportFormats().then(x => {
              this.publicationCategoryData = new MatTableDataSource(x as any);
            });
          });
          break;
        default:
          break;
      }
    });
  }

  openPublicationCategoryDialog(args: any, title: string): void {
    if (!args?.data) {
      args.data = {};
    }
    const dialogRef = this.dialog.open(PublicationCategoryDialogComponent, { data: { title, args } });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'update':
          this.enumService.insertUpdatePublicationCategory(result.data).then((obj: any) => {
            this.enumService.getPublicationCategories().then(x => {
              this.publicationCategoryData = new MatTableDataSource(x as any);
            });
          });
          break;
        default:
          break;
      }
    });
  }

  openCitationCategoryDialog(args: any, title: string): void {
    if (!args?.data) {
      args.data = {};
    }
    const dialogRef = this.dialog.open(CitationCategoryDialogComponent, { data: { title, args } });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'update':
          this.enumService.insertUpdateCitationCategory(result.data).then((obj: any) => {
            this.enumService.getCitationCategories().then(x => {
              this.citationCategoryData = new MatTableDataSource(x as any);
            });
          });
          break;
        default:
          break;
      }
    });
  }

}
