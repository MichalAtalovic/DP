import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-my-library-action-panel',
  templateUrl: './my-library-action-panel.component.html',
  styleUrls: ['./my-library-action-panel.component.css']
})
export class MyLibraryActionPanelComponent implements OnInit {

  @Output() action: EventEmitter<any> = new EventEmitter<any>();

  constructor(
    public httpClient: HttpClient
  ) { }

  ngOnInit(): void {
  }

  onClickImport() {
    document.getElementById('fileInput')?.click();
  }

  handleImportedFile(data: any) {
    let fileReader = new FileReader();
    fileReader.onload = (e) => {
      this.action.emit({ action: 'IMPORT PUBLICATION', format: data.target.files[0].name.split('.')[data.target.files[0].name.split('.').length - 1], raw: fileReader.result });
    }
    fileReader.readAsText(data.target.files[0]);
  }

  onClickExport() {
    this.action.emit({ action: 'EXPORT' });
  }

}
