import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-export-format-dialog',
  templateUrl: './export-format-dialog.component.html',
  styleUrls: ['./export-format-dialog.component.css']
})
export class ExportFormatDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<ExportFormatDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) { }

  addTag(tag: string, element: any) {
    if (!this.data.args.data?.template) {
      this.data.args.data.template = '';
    }

    this.data.args.data.template = [this.data.args.data.template.slice(0, element.selectionStart), tag, this.data.args.data.template.slice(element.selectionStart)].join('');
  }

  save() {
    this.dialogRef.close({ operation: 'update', data: this.data.args.data });
  }

}
