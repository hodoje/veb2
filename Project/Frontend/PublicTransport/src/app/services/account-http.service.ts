import { ChangePasswordModel } from './../models/change-password.model';
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
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
    changeUserDataUrl = "api/account/changeUserData"
    changePasswordUrl = "api/account/changePassword"
    changeUserDocumentUrl = "api/account/changeUserDocument"
    checkIfEmailExistsUrl = "api/account/checkIfEmailExists"

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

          localStorage.setItem('jwt', jwt)
          localStorage.setItem('role', role);
          callback(true);
          
        },
        err => {
            console.log(err);
            console.log(err.error.error_description);
            callback(false, err.status, err.error.error_description);
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

    changeUserData(changeUserData){
        return this.http.post(this.base_url + this.changeUserDataUrl, changeUserData);
    }

    changePassword(changedPasswordModel: ChangePasswordModel){
        return this.http.post(this.base_url + this.changePasswordUrl, changedPasswordModel);
    }

    changeUserDocument(changedUserDocumentData){
        let httpOptions = {
            headers: new HttpHeaders().delete('Content-Type')
        }
        return this.http.post(this.base_url + this.changeUserDocumentUrl, changedUserDocumentData, httpOptions);
    }

    checkIfEmailExists(emailToCheck: string){
        return this.http.post(this.base_url + this.checkIfEmailExistsUrl, { email: emailToCheck });
    }
}