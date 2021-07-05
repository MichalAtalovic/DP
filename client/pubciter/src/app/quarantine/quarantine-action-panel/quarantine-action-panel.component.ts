import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-quarantine-action-panel',
  templateUrl: './quarantine-action-panel.component.html',
  styleUrls: ['./quarantine-action-panel.component.css']
})
export class QuarantineActionPanelComponent implements OnInit {

  @Output() action: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit(): void { }

  onClickClearList() {
    this.action.emit({ action: 'CLEAR QUARANTINE' });
  }

}
