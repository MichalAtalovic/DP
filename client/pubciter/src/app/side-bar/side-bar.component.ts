import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-side-bar',
  templateUrl: './side-bar.component.html',
  styleUrls: ['./side-bar.component.css']
})
export class SideBarComponent implements OnInit {

  @Input() public header: any;
  @Input() public iconPath: any;

  @Output() public widthEmitter = new EventEmitter<any>();

  public collapsed = false;

  constructor() { }

  ngOnInit(): void {
  }

  collapse(value: boolean) {
    this.collapsed = value;
    if (value) {
      this.widthEmitter.emit(90);
    }
    else {
      this.widthEmitter.emit(300);
    }
  }

}
