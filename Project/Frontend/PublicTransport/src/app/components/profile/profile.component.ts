import { checkIfContainsAtLeastOneUpperCaseLetterValidator, checkIfContainsAtLeastOneLowerCaseLetterValidator, checkIfContainsAtLeastOneNumberValidator, checkIfContainsAtLeastOnePunctuationMarkValidator, checkIfPasswordsEqualValidator, checkOldNewPasswordValidator } from '../../common/reactiveFormsValidators/password-validators.directive';
import { ChangePasswordModel } from './../../models/change-password.model';
import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { UserType } from 'src/app/models/user-type.model';
import { RegistrationHttpService } from 'src/app/services/registration-http.service';
import { UserTypeHttpService } from 'src/app/services/user-type-http.service';
import { User } from 'src/app/models/user.model';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImageHttpService } from 'src/app/services/image-http.service';
import { UserConfirmationService } from 'src/app/services/user-confirmation.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit, OnDestroy {
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
  isConnected: Boolean;
  subscriptions: Subscription[] = [];
  // Used in registration process
  isRegistrationCompleted: Boolean;
  registrationStatus: string;
  logoutCounter: number;

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
    this.subscriptions.length = 0;

    this.userConfirmationService.disconnect();
  }

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
    requestedUserType: new FormControl(
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
    private imageService: ImageHttpService,
    private userConfirmationService: UserConfirmationService,
    private router: Router) {
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

  initConnectionToRegistrationService(){
    if(this.myData.registrationStatus === this.RegistrationStatuses.Processing){
      this.checkConnection();
      this.userConfirmationService.registerForUserDeclining();
      this.isRegistrationCompleted = false;
    }
  }

  private checkConnection(){
    this.userConfirmationService.startConnection().subscribe(
      (connectionStatus) => {
        this.isConnected = connectionStatus;
        if(this.isConnected){
          this.userConfirmationService.resetEmitters();
          this.userConfirmationService.AwaitRegistration();
          this.subscribeForConfirmedRegistration();
          this.subscribeForDeclinedRegistration();
        }
      },
      (error) => console.log(error));
  }

  private subscribeForConfirmedRegistration(){
    // console.log("subscribe to UserConfirmationEvent(1)");
    this.subscriptions.push(this.userConfirmationService.registerForUserConfirmation().subscribe(
      (e) => {
        this.isRegistrationCompleted = true;
        this.registrationStatus = this.RegistrationStatuses.Accepted;
        this.startLogoutTimer();
        // this.userRegistrationStatus = this.RegistrationStatuses.Accepted;
        // var successfulRegistrationAnimation = setInterval(() => {
        //   this.userSuccessfullyRegistered = true;
        //   clearInterval(successfulRegistrationAnimation);
        // }, 3000);
    },
    (err) => console.log(err)));
    // console.log("subscribe to UserConfirmationEvent(2)");
  }

  private subscribeForDeclinedRegistration(){
    this.subscriptions.push(this.userConfirmationService.userDeclinedNotification.subscribe((e) => {
      this.isRegistrationCompleted = true;
      this.registrationStatus = this.RegistrationStatuses.Rejected;
      this.startLogoutTimer();
      // this.userRegistrationStatus = this.RegistrationStatuses.Accepted;
      // var successfulRegistrationAnimation = setInterval(() => {
      //   this.userSuccessfullyRegistered = true;
      //   clearInterval(successfulRegistrationAnimation);
      // }, 3000);  
    },
    (err) => console.log(err)));
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
          requestedUserType: this.currentUserType
        });
        //this.userRegistrationStatus = data.registrationStatus;
        if(data.registrationStatus !== this.RegistrationStatuses.Accepted){
          this.isRegistrationCompleted = false;
        }
        else{
          this.isRegistrationCompleted = true;
        }
        this.personalDataForm.markAsPristine();
        this.getUserImage();
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.initConnectionToRegistrationService();
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
    console.log(personalDataFormValue);
    personalDataFormValue.requestedUserType = personalDataFormValue.requestedUserType.name;
    this.accountService.changeUserData(personalDataFormValue).subscribe(
      (data) => {
        this.getMyData();
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
        if(data){
          this.getUserImage();
          this.documentForm.markAsPristine();
        }
      },
      (err) => {
        console.log(err);
      }
    );
  }

  changePassword(passwordFormValue: ChangePasswordModel){
    this.accountService.changePassword(passwordFormValue).subscribe(
      (confirm) => {
        this.changePasswordForm.reset();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  startLogoutTimer(){
    this.logoutCounter = 30;
    let logoutInterval = setInterval(() => {
      if(this.logoutCounter !== 0){
        this.logoutCounter--;
      }
      else{
        this.logoutUserAfterRegistration();
        clearInterval(logoutInterval);
      }
    }, 1000);
  }

  logoutUserAfterRegistration(){
    this.accountService.logOut(() => {
      // this.router.navigate(['/login']);
      // window.location.href = `${window.location.hostname}/login`;
      window.location.replace('/login');
    });
  }
}
