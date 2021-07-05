import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { ExportFormatDialogComponent } from '../dialogs/export-format-dialog/export-format-dialog.component';
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
  public displayedColumnsCitationCategories: string[] = ['code', 'name'];
  public displayedColumnsPublicationCategories: string[] = ['group', 'code', 'name'];
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
      this.enumService.getExportFormats().then(x => {
        this.exportFormatData = new MatTableDataSource(x as any);
      });
    });
  }

  openExportFormatDialog(args: any, title: string): void {
    if (!args?.data) {
      args.data = {};
    }
    console.log('ARGS', args);
    const dialogRef = this.dialog.open(ExportFormatDialogComponent, { data: { title, args } });

    dialogRef.afterClosed().subscribe(result => {
      switch (result?.operation) {
        case 'update':
          this.enumService.insertUpdateExportFormat(result.data).then(obj => {
            this.enumService.getExportFormats().then(x => {
              this.exportFormatData = new MatTableDataSource(x as any);
            });
          });
          break;
        default:
          break;
      }
    });
  }

}
