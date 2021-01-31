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
}
