import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css']
})
export class SyncComponent implements OnInit {

  public panelData = { header: 'SYnc', iconPath: 'assets/sync_fade.png'}

  constructor() { }

  ngOnInit(): void {
  }

}
