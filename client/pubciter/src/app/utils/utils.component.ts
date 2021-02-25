import { UtilsService } from './../services/utils.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatTabGroup } from '@angular/material/tabs';
import { FileSystemFileEntry, NgxFileDropEntry } from 'ngx-file-drop';

@Component({
  selector: 'app-utils',
  templateUrl: './utils.component.html',
  styleUrls: ['./utils.component.css']
})
export class UtilsComponent implements OnInit {

  @ViewChild('conversionTab') conversionTab!: MatTabGroup;
  @ViewChild('formatTab') formatTab!: MatTabGroup;

  public panelData = { header: 'Utils', iconPath: 'assets/utils_fade.png' };
  public conversionPreviewText: any = '';
  public formatPreviewText: any = '';
  public conversionOutput: any;
  public formatOutput: any;
  public inputConversionFormat: any = 'bib';
  public outputConversionFormat: any = 'bib';
  public outputFormat: any = 'html';

  constructor(
    public utilsService: UtilsService
  ) { }

  ngOnInit(): void {
  }

  public files: NgxFileDropEntry[] = [];

  public droppedToConvert(files: NgxFileDropEntry[]) {
    if (files[0].fileEntry.isFile) {

      if (!files[0].relativePath.toLowerCase().endsWith('.bib') &&
        !files[0].relativePath.toLowerCase().endsWith('.ris') &&
        !files[0].relativePath.toLowerCase().endsWith('.yaml') &&
        !files[0].relativePath.toLowerCase().endsWith('.xml')) {
        return;
      }

      const split = files[0].relativePath.toLowerCase().split('.');
      this.inputConversionFormat = split[split.length - 1];

      const fileEntry = files[0].fileEntry as FileSystemFileEntry;
      fileEntry.file((file: File) => {
        file.text().then((data) => {
          this.conversionPreviewText = data;
          this.conversionTab.selectedIndex = 1;
        });
      });
    }
  }

  public droppedToFormat(files: NgxFileDropEntry[]) {
    if (files[0].fileEntry.isFile) {

      if (!files[0].relativePath.toLowerCase().endsWith('.bib') &&
        !files[0].relativePath.toLowerCase().endsWith('.ris') &&
        !files[0].relativePath.toLowerCase().endsWith('.yaml') &&
        !files[0].relativePath.toLowerCase().endsWith('.xml')) {
        return;
      }

      const fileEntry = files[0].fileEntry as FileSystemFileEntry;
      fileEntry.file((file: File) => {
        file.text().then((data) => {
          this.formatPreviewText = data;
          this.formatTab.selectedIndex = 1;
        });
      });
    }
  }

  public changeOutputConversionFormat(e: any) {
    this.outputConversionFormat = e.value;
  }

  public changeInputConversionFormat(e: any) {
    this.inputConversionFormat = e.value;
  }

  public changeOutputFormat(e: any) {
    this.outputFormat = e.value;
  }

  public convert() {
    if (this.conversionPreviewText === '') {
      return;
    }

    this.utilsService.convert(this.inputConversionFormat, this.outputConversionFormat, this.conversionPreviewText).then((result: any) => {
      this.conversionOutput = result as any;
    });
  }

  public format() {
    if (this.formatPreviewText === '') {
      return;
    }
  }

  public downloadConversion() {

    if (!this.conversionOutput || this.conversionOutput === '') {
      return;
    }
    console.log('downloading');

    var file = new Blob([this.conversionOutput], {type: this.outputConversionFormat});
    if (window.navigator.msSaveOrOpenBlob) {
      window.navigator.msSaveOrOpenBlob(file, "out." + this.outputConversionFormat);
    }
    else {
        var a = document.createElement("a"),
                url = URL.createObjectURL(file);
        a.href = url;
        a.download = "out." + this.outputConversionFormat;
        document.body.appendChild(a);
        a.click();
        setTimeout(function() {
            document.body.removeChild(a);
            window.URL.revokeObjectURL(url);
        }, 0);
    }
}

}
