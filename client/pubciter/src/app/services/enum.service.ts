import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseRestService } from './base-rest-service/base-rest.service';

@Injectable({
  providedIn: 'root'
})
export class EnumService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  async getPublicationCategories() {
    return this.get('enum/publicationCategory');
  }

  async getCitationCategories() {
    return this.get('enum/citationCategory');
  }

  async getExportFormats() {
    return this.get('enum/exportFormat');
  }

  async insertUpdateExportFormat(req: any) {
    return this.put('enum/exportFormat', req);
  }

  async deleteExportFormat(id: number) {
    return this.delete(`enum/exportFormat/${id}`);
  }

}
