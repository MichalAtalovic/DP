import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-remove-citation-dialog',
  templateUrl: './remove-citation-dialog.component.html',
  styleUrls: ['./remove-citation-dialog.component.css']
})
export class RemoveCitationDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<RemoveCitationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void {
  }

  onYesClick() {
    this.dialogRef.close({ operation: 'remove', data: this.data.citation });
  }

}
