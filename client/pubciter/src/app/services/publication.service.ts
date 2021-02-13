import { BaseRestService } from './base-rest-service/base-rest.service';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PublicationService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  async getPublications(searchText: string = '') {
    return this.get(`publication?searchText=${searchText}`);
  }

  async sync() {
    return this.post('publication/citations/sync', null);
  }

  async getSyncStatus() {
    return this.get('publication/sync/status');
  }

  async insertPublication(publication: any) {
    return this.post('publication', publication);
  }

  async insertCitation(citation: any, publicationId: number) {
    return this.post(`publication/${publicationId}/citation`, citation);
  }

  async removeCitation(publicationId: number, citationId: number) {
    return this.delete(`publication/${publicationId}/${citationId}`);
  }
}
