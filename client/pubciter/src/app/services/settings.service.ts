import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseRestService } from './base-rest-service/base-rest.service';

@Injectable({
  providedIn: 'root'
})
export class SettingsService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  async hardReset() {
    return this.post('hard-reset', null);
  }
}
