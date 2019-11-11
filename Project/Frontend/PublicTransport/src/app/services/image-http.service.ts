import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class ImageHttpService {
    base_url = "http://localhost:52296/api/";
    currentUserImage = "account/getUserDocument";
    selectedUserImage = "account/getUserDocumentForUserEmail";

    constructor(private httpClient: HttpClient){}

    getImage(){
        return this.httpClient.get(this.base_url + this.currentUserImage, { responseType: "blob" });
    }

    getImageForUsername(email: string){
        const headers = new HttpHeaders().set("Content-type", "text/plain; charset=utf-8");
        return this.httpClient.post(this.base_url + this.selectedUserImage, {"email": email}, {headers, responseType: "blob"});
    }
}