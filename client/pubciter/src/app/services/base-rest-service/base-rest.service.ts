import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseRestService {

  protected options: any;
  protected headers: HttpHeaders;
  protected baseUrl: string = `http://${window.location.href.split('/')[2].split(':')[0]}:${environment.webapiPort}`;

  constructor(protected httpClient: HttpClient) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      Accept: 'q=0.8;application/json;q=0.9',
      'Access-Control-Allow-Origin': '*',
    });
  }

  protected get(url: string) {
    return this.httpClient.get(`${this.baseUrl}/${url}`, { headers: this.headers }).toPromise();
  }

  protected post(url: string, data: any) {
    return this.httpClient.post(`${this.baseUrl}/${url}`, JSON.stringify(data), { headers: this.headers }).toPromise();
  }

  protected put(url: string, data: any) {
    return this.httpClient.put(`${this.baseUrl}/${url}`, JSON.stringify(data), { headers: this.headers }).toPromise();
  }

  protected delete(url: string) {
    return this.httpClient.delete(`${this.baseUrl}/${url}`, { headers: this.headers }).toPromise();
  }
}
