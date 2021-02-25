import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseRestService } from './base-rest-service/base-rest.service';

@Injectable({
  providedIn: 'root'
})
export class UtilsService extends BaseRestService {

  constructor(protected client: HttpClient) {
    super(client);
  }

  public async convert(convertFrom: string, convertTo: string, data: string) {
    return this.post(`utils/convert?convertFrom=${convertFrom}&convertTo=${convertTo}`, data, { responseType: 'text' });
  }

  public async format(formatAs: string, data: string) {
    return this.post(`utils/format?formatAs=${formatAs}`, data, { responseType: 'text' });
  }

}
