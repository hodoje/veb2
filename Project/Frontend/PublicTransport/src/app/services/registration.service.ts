import { BaseHttpService } from './base-http.service';
    
import { Injectable } from '@angular/core';
import { Registration } from '../models/registration.model';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService extends BaseHttpService<any>{

  constructor(protected httpClient: HttpClient) {
    super(httpClient);
    this.specifiedUrl = "account/register";
  }

  register(registration: Registration, callback){
    return this.httpClient.post(this.specifiedUrl, registration).subscribe(
      confirm => {
        console.log("Successful registration.");
        callback(true);
      },
      err => {
        console.log(err.status);
        callback(false);
      }
    );
  }
}