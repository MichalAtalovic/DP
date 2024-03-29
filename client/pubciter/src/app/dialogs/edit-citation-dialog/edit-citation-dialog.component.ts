import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EnumService } from 'src/app/services/enum.service';

@Component({
  selector: 'app-edit-citation-dialog',
  templateUrl: './edit-citation-dialog.component.html',
  styleUrls: ['./edit-citation-dialog.component.css']
})
export class EditCitationDialogComponent implements OnInit {

  public form: FormGroup;
  public citationCategories: any;

  constructor(
    public dialogRef: MatDialogRef<EditCitationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public enumService: EnumService
  ) {
    this.form = new FormGroup({
      title: new FormControl(null),
      authors: new FormControl(null),
      journal: new FormControl(null),
      journalVolume: new FormControl(null),
      year: new FormControl(null),
      doi: new FormControl(null),
      citationCategory: new FormControl(null),
      abstract: new FormControl(null)
    });
  }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.enumService.getCitationCategories().then(result => {
      this.citationCategories = result as any;
    });
  }

  onUpdateClick() {
    this.form.markAllAsTouched();
    if (this.form?.valid) {
      this.dialogRef.close({ operation: 'update', data: this.data.citation });
    }
  }

}
