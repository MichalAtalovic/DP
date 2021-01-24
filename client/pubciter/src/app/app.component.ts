import { Component, ElementRef, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs';
import { DataService } from './services/data.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  public title = 'PubCITER';
  public collapsed: boolean = true;
  public header: any;
  public iconPath: any;
  public subscription: Subscription;
  public offset: number = 110;

  constructor(public ds: DataService) {
    this.subscription = this.ds.getData().subscribe(x => {
      this.offset = x ? 110 : 320;
      console.log(this.offset);
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  panelResized(collapsed: boolean) {
    this.collapsed = collapsed;
  }

  activated(e: any) {
    this.header = e.panelData.header;
    this.iconPath = e.panelData.iconPath;
  }
}
