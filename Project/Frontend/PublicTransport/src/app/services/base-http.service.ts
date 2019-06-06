import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class BaseHttpService<T> {

  base_url = "http://localhost:52296/api/";
  specifiedUrl = "";

  constructor(protected httpClient: HttpClient) { }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.base_url + this.specifiedUrl);
  }

  getById(id: number): Observable<T> {
    return this.httpClient.get<T>(this.base_url + this.specifiedUrl + `/${id}`);
  }

  put(id: number, inputData: T) {
    return this.httpClient.put(this.base_url + this.specifiedUrl + `/${id}`, inputData);
  }

  post(inputData: any): Observable<T> {
    return this.httpClient.post<T>(this.base_url + this.specifiedUrl, inputData);
  }

  delete(id: number) {
    return this.httpClient.delete(this.base_url + this.specifiedUrl + `/${id}`);
  }
}
