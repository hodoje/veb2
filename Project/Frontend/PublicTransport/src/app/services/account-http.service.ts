import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { BaseHttpService } from './base-http.service';
import { LoginModel } from '../models/login.model';
import { Registration } from '../models/registration.model';
import { NgForm } from '@angular/forms';

@Injectable()
export class AccountHttpService{
    base_url = "http://localhost:52296/";
    loginUrl = "oauth/token"
    logoutUrl = "api/account/logout"
    getUserDataUrl = "api/account/getUserPersonalData"
    changePasswordUrl = "api/account/changePassword"

    constructor(private http: HttpClient){
    }

    logIn(user: LoginModel, callback: any){
        let data = `username=${user.username}&password=${user.password}&grant_type=password`;
        let httpOptions = {
            headers: {
                "Content-type": "application/x-www-form-urlencoded"
            }
        }

        this.http.post<any>(this.base_url + this.loginUrl, data, httpOptions)
        .subscribe(data => {
              
          let jwt = data.access_token;

          let jwtData = jwt.split('.')[1]
          let decodedJwtJsonData = window.atob(jwtData)
          let decodedJwtData = JSON.parse(decodedJwtJsonData)

          let role = decodedJwtData.role

          console.log('jwtData: ' + jwtData)
          console.log('decodedJwtJsonData: ' + decodedJwtJsonData)
          console.log('decodedJwtData: ' + JSON.stringify(decodedJwtData))
          console.log('Role ' + role)

          localStorage.setItem('jwt', jwt)
          localStorage.setItem('role', role);
          callback(true);
          
        },
        err => {
            callback(false, err.status);
        });
    }

    logOut(callback){
        localStorage.removeItem("jwt");
        localStorage.removeItem("role");
        callback(true);
        // this.http.post(this.base_url + this.logoutUrl, null).subscribe(
        //     (confirm) => {
        //         localStorage.removeItem("jwt");
        //         localStorage.removeItem("role");
        //         callback(true);
        //     },
        //     (err) =>{
        //         localStorage.removeItem("jwt");
        //         localStorage.removeItem("role");
        //         callback(true);
        //     }
        // );
    }

    getUserPersonalData(){
        return this.http.get(this.base_url + this.getUserDataUrl);
    }
}