import { EnumService } from './../../services/enum.service';
import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-export-dialog',
  templateUrl: './export-dialog.component.html',
  styleUrls: ['./export-dialog.component.css']
})
export class ExportDialogComponent implements OnInit {

  public form: FormGroup;
  public years: number[] = [];
  public availablePublicationCategories: any;
  public pickedPublicationCategories: any = [];
  public availableCitationCategories: any;
  public pickedCitationCategories: any = [];
  public availableExportFormats: any;

  constructor(
    public dialogRef: MatDialogRef<ExportDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private enumService: EnumService
  ) {
    this.form = new FormGroup({
      yearFrom: new FormControl(null),
      yearTo: new FormControl(null),
      publicationCategory: new FormControl(null),
      publicationCategories: new FormControl(null),
      citationCategory: new FormControl(null),
      citationCategories: new FormControl(null),
      exportFormat: new FormControl(null),
      ext: new FormControl(null)
    });
  }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    const currentYear = new Date().getFullYear();
    for (let year = currentYear; year >= 1930; year--) {
      this.years.push(year);
    }

    this.data.ext = 'HTML';

    Promise.all([
      this.enumService.getPublicationCategories(),
      this.enumService.getCitationCategories(),
      this.enumService.getExportFormats()
    ]).then(values => {
      this.availablePublicationCategories = values[0] as any;
      this.availableCitationCategories = values[1] as any;
      this.availableExportFormats = values[2] as any;
    });

  }

  publicationCategoryPicked(publicationCategory: any) {
    const picked = this.availablePublicationCategories.find((x: any) => x.id === publicationCategory.id);
    this.pickedPublicationCategories.push({ ...picked });
    this.availablePublicationCategories = this.availablePublicationCategories.filter((x: any) => x.id !== publicationCategory.id);
  }

  publicationCategoryRemoved(publicationCategory: any) {
    const removed = this.pickedPublicationCategories.find((x: any) => x.id === publicationCategory.id);
    this.availablePublicationCategories.push({ ...removed });
    this.pickedPublicationCategories = this.pickedPublicationCategories.filter((x: any) => x.id !== publicationCategory.id);
  }

  citationCategoryPicked(citationCategory: any) {
    const picked = this.availableCitationCategories.find((x: any) => x.id === citationCategory.id);
    this.pickedCitationCategories.push({ ...picked });
    this.availableCitationCategories = this.availableCitationCategories.filter((x: any) => x.id !== citationCategory.id);
  }

  citationCategoryRemoved(citationCategory: any) {
    const removed = this.pickedCitationCategories.find((x: any) => x.id === citationCategory.id);
    this.availableCitationCategories.push({ ...removed });
    this.pickedCitationCategories = this.pickedCitationCategories.filter((x: any) => x.id !== citationCategory.id);
  }

  export() {
    this.form.markAllAsTouched();
    console.log(this.form);
    if (this.form.valid) {
      this.dialogRef.close({
        operation: 'export',
        exportFormat: this.data.exportFormat,
        extension: this.data.ext,
        data: {
          yearFrom: this.data.yearFrom,
          yearTo: this.data.yearTo,
          publicationCategories: this.pickedPublicationCategories.map((x: any) => x.code),
          citationCategories: this.pickedCitationCategories.map((x: any) => x.code)
        }
      });
    }

  }

}
