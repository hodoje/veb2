import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()
export class ImageHttpService {
    base_url = "http://localhost:52296/api/";
    specifiedUrl = "account/getUserDocument";

    constructor(private httpClient: HttpClient){}

    getImage(){
        return this.httpClient.get(this.base_url + this.specifiedUrl, { responseType: "blob" });
    }
}