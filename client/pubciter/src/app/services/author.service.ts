import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseRestService } from './base-rest-service/base-rest.service';
import { SettingsService } from './settings.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  async getAuthors() {
    return this.get('author');
  }

  async updateAuthor(req: any) {
    return this.put('author', req);
  }
}
