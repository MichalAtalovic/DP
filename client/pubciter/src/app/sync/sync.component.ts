import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css']
})
export class SyncComponent implements OnInit {

  public margin: any;

  constructor() { }

  ngOnInit(): void {
  }

  panelResized(widthInPx: any) {
    console.log(widthInPx);
    this.margin = widthInPx;
  }

}
