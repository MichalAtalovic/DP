import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-publication-category-dialog',
  templateUrl: './publication-category-dialog.component.html',
  styleUrls: ['./publication-category-dialog.component.css']
})
export class PublicationCategoryDialogComponent {

  public form: FormGroup = new FormGroup({
    code: new FormControl(null),
    group: new FormControl(null),
    name: new FormControl(null)
  });

  constructor(
    public dialogRef: MatDialogRef<PublicationCategoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  save() {
    this.dialogRef.close({ operation: 'update', data: this.data.args.data });
  }

}
