import { QuarantineService } from './../services/quarantine.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-quarantine',
  templateUrl: './quarantine.component.html',
  styleUrls: ['./quarantine.component.css']
})
export class QuarantineComponent implements OnInit {

  public panelData = { header: 'Quarantine', iconPath: 'assets/quarantine_fade.png'}
  public publications: any;

  constructor(
    public quarantineService: QuarantineService
  ) { }

  ngOnInit(): void {
  }

  ngAfterViewInit() {
    this.quarantineService.getQuarantinedPublications().then(response => {
      this.publications = response as any;
      console.log('QUARANTINED PUBLICATIONS');
      console.log(this.publications);
    });
  }

}
