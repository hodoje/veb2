import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class BaseHttpService<T> {

  baseUrl = "http://localhost:52295";
  specifiedUrl = "";

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.baseUrl + this.specifiedUrl);
  }

  getById(id: number): Observable<T> {
    return this.httpClient.get<T>(this.baseUrl + this.specifiedUrl + `/${id}`);
  }

  put(id: number, inputData: T) {
    return this.httpClient.put(this.baseUrl + this.specifiedUrl + `/${id}`, inputData);
  }

  post(inputData: any): Observable<T> {
    return this.httpClient.post<T>(this.baseUrl + this.specifiedUrl, inputData);
  }

  delete(id: number) {
    return this.httpClient.delete(this.baseUrl + this.specifiedUrl + `/${id}`);
  }
}
