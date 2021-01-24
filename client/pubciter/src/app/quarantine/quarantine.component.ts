import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.css']
})
export class QuarantineComponent implements OnInit {

  public panelData = { header: 'Quarantine', iconPath: 'assets/quarantine_fade.png'}

  constructor() { }

  ngOnInit(): void {
  }

}
