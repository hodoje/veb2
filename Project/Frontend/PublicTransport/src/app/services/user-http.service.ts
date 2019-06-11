import { BaseHttpService } from './base-http.service';
import { User } from '../models/user.model';

export class UserHttpService extends BaseHttpService<User> {
    specifiedUrl = "/Account/PendingUsers"
}