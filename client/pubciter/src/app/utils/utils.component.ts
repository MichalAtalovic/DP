import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-utils',
  templateUrl: './utils.component.html',
  styleUrls: ['./utils.component.css']
})
export class UtilsComponent implements OnInit {

  public panelData = { header: 'Utils', iconPath: 'assets/utils_fade.png'}

  constructor() { }

  ngOnInit(): void {
  }

}
