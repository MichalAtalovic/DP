import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { PublicationService } from '../services/publication.service';

@Component({
  selector: 'app-my-library',
  templateUrl: './my-library.component.html',
  styleUrls: ['./my-library.component.css']
})
export class MyLibraryComponent implements OnInit {

  public publications: any;
  public panelData = { header: 'My library', iconPath: 'assets/library_fade.png'}

  constructor(
    public publicationService: PublicationService
  ) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.publicationService.getPublications().then(response => {
      this.publications = response as any;
      console.log('PUBLICATIONS');
      console.log(this.publications);
    });
  }

}
