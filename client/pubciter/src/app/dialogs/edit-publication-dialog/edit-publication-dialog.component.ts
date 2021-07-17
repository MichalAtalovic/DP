import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EnumService } from 'src/app/services/enum.service';

@Component({
  selector: 'app-edit-publication-dialog',
  templateUrl: './edit-publication-dialog.component.html',
  styleUrls: ['./edit-publication-dialog.component.css']
})
export class EditPublicationDialogComponent implements OnInit {

  public form: FormGroup;
  public publicationCategories: any;

  constructor(
    public dialogRef: MatDialogRef<EditPublicationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public enumService: EnumService
  ) {
    this.form = new FormGroup({
      title: new FormControl(null),
      authors: new FormControl(null),
      publisher: new FormControl(null),
      journal: new FormControl(null),
      journalVolume: new FormControl(null),
      year: new FormControl(null),
      doi: new FormControl(null),
      publicationCategory: new FormControl(null),
      abstract: new FormControl(null),
      publicationUrl: new FormControl(null)
    });
   }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.enumService.getPublicationCategories().then(result => {
      this.publicationCategories = result as any;
    });
  }

  onUpdateClick() {
    this.form.markAllAsTouched();
    if (this.form?.valid) {
      this.dialogRef.close({ operation: 'update', data: this.data.publication });
    }
  }

}
