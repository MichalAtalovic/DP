import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-enums-action-panel',
  templateUrl: './enums-action-panel.component.html',
  styleUrls: ['./enums-action-panel.component.css']
})
export class EnumsActionPanelComponent implements OnInit {

  @Output() action: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit(): void { }

  openPublicationCategoryEnum() {
    this.action.emit({ action: 'OPEN_PUBLICATION_CATEGORY_ENUM' });
  }

  openCitationCategoryEnum() {
    this.action.emit({ action: 'OPEN_CITATION_CATEGORY_ENUM' });
  }

  openExportFormatEnum() {
    this.action.emit({ action: 'OPEN_EXPORT_FORMAT_ENUM' });
  }

}
