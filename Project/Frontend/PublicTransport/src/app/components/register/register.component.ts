import { Component, OnInit } from '@angular/core';
import { RegistrationService } from 'src/app/services/registration.service';
import { Registration } from 'src/app/models/registration.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  registrationSuccessful: boolean;

  constructor(private registrationService: RegistrationService) {
    this.registrationSuccessful = false;
  }

  ngOnInit() {
  }

  register(registration: Registration){
    this.registrationService.register(registration, (successfulRegistration) => {
      if(successfulRegistration){
        this.registrationSuccessful = true;
      }
      else{
        this.registrationSuccessful = false;
      }
    });
  }

}
