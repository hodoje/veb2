import { Component, OnInit } from '@angular/core';
import { UserType } from 'src/app/models/user-type.model';
import { RegistrationHttpService } from 'src/app/services/registration-http.service';
import { UserTypeHttpService } from 'src/app/services/user-type-http.service';
import { User } from 'src/app/models/user.model';
import { AuthHttpService } from 'src/app/services/auth-http.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  myData: User;
  uploadedFile: File;
  registrationSuccessful: boolean;
  currentUserType: UserType;
  userTypes: UserType[];

  //#region Forms
  personalDataForm = new FormGroup({
    email: new FormControl(
    ),
    name: new FormControl(
      null, 
      [Validators.required, Validators.minLength(2), Validators.maxLength(60), Validators.pattern('[a-zA-Z]*')]
    ),
    lastname: new FormControl(
      null, 
      [Validators.required, Validators.minLength(2), Validators.maxLength(60), Validators.pattern('[a-zA-Z]*')]
    ),
    address: new FormControl(
      null, 
      [Validators.required, Validators.minLength(3), Validators.maxLength(100), Validators.pattern('[ a-zA-Z0-9]*')]
    ),
    birthday: new FormControl(
      null,
      Validators.required
    ),
    userType: new FormControl(
      null, 
      [Validators.required]
    )
  });

  documentForm = new FormGroup({
    documentImage: new FormControl()
  });
  //#endregion

  constructor(
    private accountService: AuthHttpService, 
    private userTypeService: UserTypeHttpService) {
    this.uploadedFile = null;
    this.registrationSuccessful = false;
    this.currentUserType = null;
    this.userTypes = [];
    this.myData = new User();
  }

  ngOnInit() {
    this.userTypeService.getAll().subscribe(
      (data: UserType[]) => {
        this.userTypes = data;
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getMyData();
      }
    );
  }

  getMyData(){
    this.accountService.getUserPersonalData().subscribe(
      (data: User) => {
        this.myData = data;
        this.currentUserType = this.userTypes.find(ut => ut.name === this.myData.userType.name);
        this.personalDataForm.patchValue({
          email: this.myData.email,
          name: this.myData.name,
          lastname: this.myData.lastname,
          address: this.myData.address,
          birthday: this.myData.birthday,
          userType: this.currentUserType
        });
        this.personalDataForm.markAsPristine();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  handleFileInput(files: FileList) {
    this.uploadedFile = files.item(0);
  }

  parseDate(dateString: string): Date {
    if (dateString) {
        return new Date(dateString);
    }
    return null;
  }

  changeData(registration){
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
  }

  changeDocument(documentFormValue){
  }

  changePassword(passwordFormValue){
  }
}
