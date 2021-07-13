import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { EnumService } from 'src/app/services/enum.service';

@Component({
  selector: 'app-edit-citation-dialog',
  templateUrl: './edit-citation-dialog.component.html',
  styleUrls: ['./edit-citation-dialog.component.css']
})
export class EditCitationDialogComponent implements OnInit {

  public citationCategories: any;

  constructor(
    public dialogRef: MatDialogRef<EditCitationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    public enumService: EnumService
  ) { }

  ngOnInit(): void { }

  ngAfterViewInit(): void {
    this.enumService.getCitationCategories().then(result => {
      this.citationCategories = result as any;
    });
  }

  onUpdateClick() {
    this.dialogRef.close({ operation: 'update', data: this.data.citation });
  }

}
