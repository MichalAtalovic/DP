import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-my-library',
  templateUrl: './my-library.component.html',
  styleUrls: ['./my-library.component.css']
})
export class MyLibraryComponent implements OnInit {

  public margin: any;

  constructor() { }

  ngOnInit(): void {
  }

  panelResized(widthInPx: any) {
    console.log(widthInPx);
    this.margin = widthInPx;
  }

}
