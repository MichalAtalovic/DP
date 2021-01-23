import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.css']
})
export class QuarantineComponent implements OnInit {

  public margin: any;

  constructor() { }

  ngOnInit(): void {
  }

  panelResized(widthInPx: any) {
    console.log(widthInPx);
    this.margin = widthInPx;
  }

}
