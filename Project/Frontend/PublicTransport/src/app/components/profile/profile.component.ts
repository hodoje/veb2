import { checkIfContainsAtLeastOneUpperCaseLetterValidator, checkIfContainsAtLeastOneLowerCaseLetterValidator, checkIfContainsAtLeastOneNumberValidator, checkIfContainsAtLeastOnePunctuationMarkValidator, checkIfPasswordsEqualValidator, checkOldNewPasswordValidator } from '../../common/reactiveFormsValidators/password-validators.directive';
import { ChangePasswordModel } from './../../models/change-password.model';
import { Component, OnInit } from '@angular/core';
import { UserType } from 'src/app/models/user-type.model';
import { RegistrationHttpService } from 'src/app/services/registration-http.service';
import { UserTypeHttpService } from 'src/app/services/user-type-http.service';
import { User } from 'src/app/models/user.model';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImageHttpService } from 'src/app/services/image-http.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  RegistrationStatuses = {
    Processing: "Processing",
    Rejected: "Rejected",
    Accepted: "Accepted"
  };
  myData: User;
  uploadedFile: File;
  registrationSuccessful: boolean;
  currentUserType: UserType;
  userTypes: UserType[];
  imageToShow: any;
  isImageLoaded: boolean;
  fileLabelText = "Choose file";
  userRegistrationStatus: string;

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

  changePasswordForm = new FormGroup({
    oldPassword: new FormControl(
      "",
      [
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(30),
        checkIfContainsAtLeastOneUpperCaseLetterValidator,
        checkIfContainsAtLeastOneLowerCaseLetterValidator,
        checkIfContainsAtLeastOneNumberValidator,
        checkIfContainsAtLeastOnePunctuationMarkValidator,
        checkOldNewPasswordValidator("newPassword")
      ]
    ),
    newPassword: new FormControl(
      "",
      [
        Validators.required, 
        Validators.minLength(3), 
        Validators.maxLength(30),
        checkIfContainsAtLeastOneUpperCaseLetterValidator,
        checkIfContainsAtLeastOneLowerCaseLetterValidator,
        checkIfContainsAtLeastOneNumberValidator,
        checkIfContainsAtLeastOnePunctuationMarkValidator,
        checkIfPasswordsEqualValidator("confirmPassword"),
        checkOldNewPasswordValidator("oldPassword")
      ]
    ),
    confirmPassword: new FormControl(
      "",
      [
        Validators.required,
        checkIfPasswordsEqualValidator("newPassword")
      ]
    )
  });

  get cpForm(){
    return this.changePasswordForm.controls;
  }

  get pdForm(){
    return this.personalDataForm.controls;
  }
  //#endregion

  constructor(
    private accountService: AccountHttpService, 
    private userTypeService: UserTypeHttpService,
    private imageService: ImageHttpService) {
    this.uploadedFile = null;
    this.registrationSuccessful = false;
    this.currentUserType = null;
    this.userTypes = [];
    this.myData = new User();
    this.isImageLoaded = false;
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
        this.userRegistrationStatus = data.registrationStatus;
        this.personalDataForm.markAsPristine();
        this.getUserImage();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getUserImage(){
    this.imageService.getImage().subscribe(
      data => { 
        this.createImageFromBlob(data);
        this.isImageLoaded = true;
      }, 
      error => {
        this.isImageLoaded = false;
        console.log(error);
    });
  }

  createImageFromBlob(image: Blob){
    let reader = new FileReader();
    reader.addEventListener("load", () => {
        this.imageToShow = reader.result;
    }, false);

    if (image) {
        reader.readAsDataURL(image);
    }
  }

  handleFileInput(files: FileList) {
    this.uploadedFile = files.item(0);
    this.fileLabelText = this.uploadedFile.name;
    this.createImageFromBlob(this.uploadedFile);
    this.isImageLoaded = true;
  }

  parseDate(dateString: string): Date {
    if (dateString) {
        return new Date(dateString);
    }
    return null;
  }

  changeData(personalDataFormValue){
    personalDataFormValue.requestedUserType = this.currentUserType.name;
    this.accountService.changeUserData(personalDataFormValue).subscribe(
      (data) => {
        this.personalDataForm.markAsPristine();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  changeDocument(){
    let formData = new FormData();
    formData.append("documentImage", this.uploadedFile);
    this.accountService.changeUserDocument(formData).subscribe(
      (data) => {
        this.getUserImage();
        this.documentForm.markAsPristine();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  changePassword(passwordFormValue: ChangePasswordModel){
    this.accountService.changePassword(passwordFormValue).subscribe(
      (confirm) => {
        console.log(confirm);
        this.changePasswordForm.reset();
      },
      (err) => {
        console.log(err);
      }
    );
  }
}
