import { BaseHttpService } from './base-http.service';
import { UserType } from '../models/user-type.model';

export class UserTypeHttpService extends BaseHttpService<UserType>{
    specifiedUrl = "userTypes";
}