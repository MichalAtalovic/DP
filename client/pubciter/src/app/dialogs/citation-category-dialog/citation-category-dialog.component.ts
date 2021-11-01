import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-citation-category-dialog',
  templateUrl: './citation-category-dialog.component.html',
  styleUrls: ['./citation-category-dialog.component.css']
})
export class CitationCategoryDialogComponent {

  public form: FormGroup = new FormGroup({
    code: new FormControl(null),
    name: new FormControl(null)
  });

  constructor(
    public dialogRef: MatDialogRef<CitationCategoryDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  save() {
    this.dialogRef.close({ operation: 'update', data: this.data.args.data });
  }

}
