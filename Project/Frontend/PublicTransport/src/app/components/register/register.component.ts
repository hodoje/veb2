import { Component, OnInit } from '@angular/core';
import { RegistrationHttpService } from 'src/app/services/registration-http.service';
import { Registration } from 'src/app/models/registration.model';
import { FormGroup, FormControl, Validators, NgForm } from '@angular/forms';
import { UserTypeHttpService } from 'src/app/services/user-type-http.service';
import { UserType } from 'src/app/models/user-type.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  uploadedFile: File;
  registrationSuccessful: boolean;
  currentUserType: UserType;
  userTypes: UserType[];

  constructor(private registrationService: RegistrationHttpService, private userTypeService: UserTypeHttpService) {
    this.uploadedFile = null;
    this.registrationSuccessful = false;
    this.currentUserType = null;
    this.userTypes = [];
  }

  ngOnInit() {
    this.userTypeService.getAll().subscribe(
      (data: UserType[]) => {
        this.userTypes = data;
        this.currentUserType = this.userTypes[0];
      },
      (err) => {
        console.log(err);
      }
    );
  }

  handleFileInput(files: FileList) {
    this.uploadedFile = files.item(0);
  }

  register(registration, f: NgForm){
    let formData = new FormData();
    formData.append("email", registration.email);
    formData.append("password", registration.password);
    formData.append("confirmPassword", registration.confirmPassword);
    formData.append("name", registration.name);
    formData.append("lastname", registration.lastname);
    formData.append("address", registration.address);
    let birthdaystr = (new Date(registration.birthday)).toUTCString();
    formData.append("birthday", birthdaystr);
    formData.append("requestedUserType", this.currentUserType.name);
    formData.append("documentImage", this.uploadedFile);
    new Response(formData).text().then(console.log);
    this.registrationService.register(formData, (successfulRegistration) => {
      if(successfulRegistration){
        this.registrationSuccessful = true;
      }
      else{
        this.registrationSuccessful = false;
      }
    });
  }

}
