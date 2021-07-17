import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-remove-publication-dialog',
  templateUrl: './remove-publication-dialog.component.html',
  styleUrls: ['./remove-publication-dialog.component.css']
})
export class RemovePublicationDialogComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<RemovePublicationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  ngOnInit(): void { }

  onYesClick() {
    this.dialogRef.close({ operation: 'remove', data: this.data.citation });
  }

}
