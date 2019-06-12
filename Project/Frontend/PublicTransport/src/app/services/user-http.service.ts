import { BaseHttpService } from './base-http.service';
import { User } from '../models/user.model';
import { Observable } from 'rxjs';

export class UserHttpService extends BaseHttpService<User> {
    specifiedUrl = "Account"

    getAllUnConfirmedUsers(): Observable<User[]> {
        return this.httpClient.get<User[]>(this.base_url + this.specifiedUrl + "/PendingUsers");
    }

    confirmUser(email: string) {
        this.httpClient.post(this.base_url + this.specifiedUrl + "/ConfirmUser", email);
    }

    declineUser(email: string) {

    }
}