import { BaseHttpService } from './base-http.service';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';

export class UserHttpService extends BaseHttpService<User> {
    specifiedUrl = "Account"

    getAllRegisteredUsers(): Observable<User[]> {
        return this.httpClient.get<User[]>(this.base_url + this.specifiedUrl + "/AllRegisteredUsers")
    }

    getAllUnConfirmedUsers(): Observable<User[]> {
        return this.httpClient.get<User[]>(this.base_url + this.specifiedUrl + "/PendingUsers");
    }

    confirmUser(email: string): Observable<any> {
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/ConfirmRegistration", {email});
    }

    declineUser(email: string): Observable<any> {
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/DeclineUser", {email});
    }

    banUser(email: string): Observable<any> {
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/BanUser", {email});
    }

    unbanUser(email: string): Observable<any> {
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/UnbanUser", {email});
    }
}