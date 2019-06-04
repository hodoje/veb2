import { BaseHttpService } from './base-http.service';
import { Registration } from '../models/registration.model';

export class RegistrationService extends BaseHttpService<any>{
  specifiedUrl = "account/register";

  register(registration: Registration, callback){
    return this.post(registration).subscribe(
      confirm => {
        callback(true);
      },
      err => {
        callback(false);
      }
    );
  }
}