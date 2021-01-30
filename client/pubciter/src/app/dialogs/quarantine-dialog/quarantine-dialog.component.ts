import { Component, Inject, Input, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-quarantine-dialog',
  templateUrl: './quarantine-dialog.component.html',
  styleUrls: ['./quarantine-dialog.component.css']
})
export class QuarantineDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<QuarantineDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
  }

  onYesClick() {
    this.dialogRef.close(this.data.args);
  }

}
