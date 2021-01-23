import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  public margin: any;

  constructor() { }

  ngOnInit(): void {
  }

  panelResized(widthInPx: any) {
    console.log(widthInPx);
    this.margin = widthInPx;
  }

}
