import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  public panelData = { header: 'Settings', iconPath: 'assets/settings_fade.png'}

  constructor() { }

  ngOnInit(): void {
  }

}
