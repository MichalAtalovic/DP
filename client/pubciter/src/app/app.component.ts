import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  public offset: number = 90;
  public refresh: any = true;

  constructor(
    public ds: DataService,
    public router: Router
  ) {
    this.subscription = this.ds.getData().subscribe(args => {
      if (args.action === 'ACTION PANEL RESIZE') {
        this.offset = args.data ? 90 : 300;
      }
    });
  }

  ngOnInit() {

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

  panelActionExecute(args: any) {
    switch (args?.action) {
      case 'CLEAR QUARANTINE':
        this.ds.sendData({ action: 'REFRESH QUARANTINE'})
        break;
    }
  }
}
