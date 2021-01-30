import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DataService } from '../services/data.service';
import { QuarantineService } from '../services/quarantine.service';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  @Input() public header: any;
  @Input() public iconPath: any;
  @Input() public collapsed = true;

  @Output() public widthEmitter = new EventEmitter<any>();
  @Output() public action = new EventEmitter<any>();

  constructor(
    private dataService: DataService,
    private quarantineService: QuarantineService
  ) { }

  ngOnInit(): void {
  }

  collapse(value: boolean) {
    this.collapsed = value;
    this.widthEmitter.emit(value);
    this.dataService.sendData({action: 'ACTION PANEL RESIZE', data: value});
  }

  panelActionExecute(args: any) {
    switch (args?.action) {
      case 'CLEAR QUARANTINE':
        this.quarantineService.clearQuarantine();
        this.action.emit(args);
        break;
    }
  }

}
