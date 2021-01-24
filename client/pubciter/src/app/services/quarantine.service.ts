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

  getQuarantinedPublications() {
    return this.get('quarantine?citations=true');
  }
}
