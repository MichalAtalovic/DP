import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { interval } from 'rxjs';
import { SyncDialogComponent } from '../dialogs/sync-dialog/sync-dialog.component';
import { PublicationService } from '../services/publication.service';

@Component({
  selector: 'app-sync',
  templateUrl: './sync.component.html',
  styleUrls: ['./sync.component.css']
})
export class SyncComponent implements OnInit {

  public panelData = { header: 'Sync', iconPath: 'assets/sync_fade.png' }
  public syncStatus: any;
  public subscription: any;
  public isIdle: boolean = true;

  constructor(
    public publicationService: PublicationService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.subscription = interval(1000).subscribe((x: any) => {
      this.publicationService.getSyncStatus().then((data: any) => {
        this.syncStatus = [
          {
            name: 'Google Scholar',
            status: data.googleScholar
          },
          {
            name: 'Semantics Scholar',
            status: data.semanticsScholar
          },
          {
            name: 'www.OpenCitations.net',
            status: data.openCitations
          }
        ];

        this.isIdle = data.googleScholar === 'Idle' && data.semanticsScholar === 'Idle' && data.openCitations === 'Idle';
      });
    });
  }

  sync() {
    console.log('Synchronization process started');
    this.publicationService.sync().then(() => {
      this.subscription.unsubscribe();
      this.isIdle = true;
    });
  }

  openSyncDialog(): void {
    const dialogRef = this.dialog.open(SyncDialogComponent);

    dialogRef.afterClosed().subscribe(result => {
      if (result === 'start') {
        this.sync();
      }
    });
  }

}
