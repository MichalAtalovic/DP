import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { AuthorService } from '../services/author.service';
import { DataService } from '../services/data.service';
import { PublicationService } from '../services/publication.service';

@Component({
  selector: 'app-my-library',
  templateUrl: './my-library.component.html',
  styleUrls: ['./my-library.component.css']
})
export class MyLibraryComponent implements OnInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  public displayedColumns: string[] = ['position', 'title', 'authors', 'journal', 'publicationYear'];
  public dataSource: MatTableDataSource<any> = new MatTableDataSource();
  public publications: any;
  public panelData = { header: 'My library', iconPath: 'assets/library_fade.png' }
  public viewType = 'cards';
  public subscription: Subscription;

  constructor(
    public publicationService: PublicationService,
    public authorService: AuthorService,
    public dataService: DataService
  ) {
    this.subscription = this.dataService.getData().subscribe(args => {
      if (args.action === 'INSERTED PUBLICATION') {
        if (!this.publications) {
          this.publications = [];
        }

        this.publications.push(args.data);
      }
    });
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    Promise.all([
      this.publicationService.getPublications(),
      this.authorService.getAuthors()
    ]).then(values => {
      this.publications = values[0] as any;

      // append position
      if (this.publications) {
        this.publications.forEach((value: any, index: number) => {
          value.position = index + 1;
        });

        this.dataSource = new MatTableDataSource(this.publications);
      }

      const author = (values[1] as any[])[0];
      this.viewType = author?.settings?.libraryTableView ? 'table' : 'cards';
    });
  }

  onPublicationMoved(publicationId: any) {
    this.publications = this.publications.filter((x: any) => x.publicationId !== publicationId);
  }

  changeViewType(args: any) {
    this.viewType = args.value;
    if (this.viewType === 'table') {
      setTimeout(() => this.dataSource.paginator = this.paginator);
    }
  }

  search(searchedText: string) {
    this.publicationService.getPublications(searchedText).then(response => {
      this.publications = response as any;

      // append position
      if (this.publications) {
        this.publications.forEach((value: any, index: number) => {
          value.position = index + 1;
        });

        this.dataSource = new MatTableDataSource(this.publications);
        if (this.viewType === 'table') {
          setTimeout(() => this.dataSource.paginator = this.paginator);
        }
      }
    });
  }

  trackByFn(index: any) {
    return index;
  }

}
