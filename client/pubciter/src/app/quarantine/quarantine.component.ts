import { QuarantineService } from './../services/quarantine.service';
import { Component, OnInit } from '@angular/core';
import { DataService } from '../services/data.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.css']
})
export class QuarantineComponent implements OnInit {

  public panelData = { header: 'Quarantine', iconPath: 'assets/quarantine_fade.png'}
  public publications: any;
  public subscription: Subscription;

  constructor(
    public quarantineService: QuarantineService,
    public dataService: DataService
  ) {
    this.subscription = this.dataService.getData().subscribe(args => {
      if (args?.action === 'REFRESH QUARANTINE') {
        setTimeout(() => {
          this.getData();
        }, 1000);
      }
    });
  }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.getData();
  }

  onPublicationMoved(publicationId: any) {
    this.publications = this.publications.filter((x: any) => x.quarantinedPublicationId !== publicationId);
  }

  getData() {
    this.quarantineService.getQuarantinedPublications().then(response => {
      this.publications = response as any;
    });
  }

  public trackItem (index: number, item: any) {
    return item.trackId;
  }

}
