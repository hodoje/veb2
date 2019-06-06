import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class RegistrationHttpService{
  base_url = "http://localhost:52296/api/";
  specifiedUrl = "account/register";

  constructor(private httpClient: HttpClient){
  }

  register(registration, callback){
    let httpOptions = {
      headers: new HttpHeaders().delete('Content-Type')
    }
    return this.httpClient.post(this.base_url + this.specifiedUrl, registration, httpOptions).subscribe(
      confirm => {
        callback(true);
      },
      err => {
        callback(false);
      }
    );
  }
}