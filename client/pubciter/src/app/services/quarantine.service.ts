import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseRestService } from './base-rest-service/base-rest.service';

@Injectable({
  providedIn: 'root'
})
export class QuarantineService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  async getQuarantinedPublications() {
    return this.get('quarantine?citations=true');
  }

  async addToQuarantine(publicationId: any) {
    return this.post(`quarantine/add/${publicationId}`, null);
  }

  async removeFromQuarantine(publicationId: any) {
    return this.post(`quarantine/remove/${publicationId}`, null);
  }

  async clearQuarantine() {
    return this.post('quarantine/clear', null);
  }
}
