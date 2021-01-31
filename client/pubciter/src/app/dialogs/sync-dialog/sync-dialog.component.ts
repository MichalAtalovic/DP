import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-sync-dialog',
  templateUrl: './sync-dialog.component.html',
  styleUrls: ['./sync-dialog.component.css']
})
export class SyncDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<SyncDialogComponent>
  ) { }

  ngOnInit(): void {
  }

  onYesClick() {
    this.dialogRef.close('start');
  }

}
