import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
//import * as citationjs from 'citation-js';
declare var require: any;

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
    const Cite = require('citation-js');

    // create an instance of Cite
    const Cite1 = new Cite();

    // add the first bibtext entry
    Cite1.add('@ARTICLE{54902, author={M. J. {Wiener}}, journal={IEEE Transactions on Information Theory}, title={Cryptanalysis of short RSA secret exponents}, year={1990}, volume={36},number={3},pages={553-558},doi={10.1109/18.54902}}');

    // create a bibliography
    const formatted1 = Cite1.format('bibliography', {
      format: 'html',
      template: 'apa'
    });

    // everything is great!!!
    console.log(Cite1); // output a Cite object with data from bibtext1
    console.log(formatted1);// output a html block with data from bibtext1



    // Same process here but with the second entry
    const Cite2 = new Cite();

    // this successfully add the second entry to the second Cite object
    Cite2.add('@article{misut2015measuring,   title={Measuring of Quality in the Context of e-Learning},   author={Misut, Martin and Pribilova, Katarina},   journal={Procedia-Social and Behavioral Sciences},   volume={177},   pages={312--319},   year={2015},   publisher={Elsevier} }');

    // create a second bibliography
    const formatted2 = Cite2.format('bibliography', {
      format: 'html',
      template: 'apa'
    });

    console.log(Cite2);   // this output a Cite object with data from bibtext2 which is great,

    console.log(formatted2);  // But this output a html block with the data from bibtext1
    //even though I used Cite2 which was a new instance of Cite with a different bibtext entry, why????
  }

}
