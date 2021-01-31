import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-hard-reset-dialog',
  templateUrl: './hard-reset-dialog.component.html',
  styleUrls: ['./hard-reset-dialog.component.css']
})
export class HardResetDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<HardResetDialogComponent>
  ) { }

  ngOnInit(): void {
  }

  onYesClick() {
    this.dialogRef.close('hard reset');
  }

}
