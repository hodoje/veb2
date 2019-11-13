import { checkIfContainsAtLeastOneUpperCaseLetterValidator, checkIfContainsAtLeastOneLowerCaseLetterValidator, checkIfContainsAtLeastOneNumberValidator, checkIfContainsAtLeastOnePunctuationMarkValidator, checkIfPasswordsEqualValidator, checkOldNewPasswordValidator } from '../../common/reactiveFormsValidators/password-validators.directive';
import { ChangePasswordModel } from './../../models/change-password.model';
import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { UserType } from 'src/app/models/user-type.model';
import { UserTypeHttpService } from 'src/app/services/user-type-http.service';
import { User } from 'src/app/models/user.model';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ImageHttpService } from 'src/app/services/image-http.service';
import { UserHubService } from 'src/app/services/user-hub.service';
import { Subscription } from 'rxjs';

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
  isConnected: Boolean;  
  // Used in registration process
  isRegistrationCompleted: Boolean;
  registrationStatus: string;
  registrationMessageStatus: string;
  logoutCounter: number;

  ngOnDestroy(): void {
    this.stopUserHubServiceConnection();
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
    private userHubService: UserHubService,
    private ngZone: NgZone) {
      this.uploadedFile = null;
      this.registrationSuccessful = false;
      this.currentUserType = null;
      this.userTypes = [];
      this.myData = new User();
      this.isImageLoaded = false;
      this.registrationStatus = this.RegistrationStatuses.Processing;
      this.registrationMessageStatus = this.RegistrationStatuses.Processing;
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

  private startUserHubServiceConnection() {
    console.log("started user hub connection");
    this.userHubService.startConnection();
  }

  private stopUserHubServiceConnection() {
    console.log("stopped user hub connection");
    this.userHubService.stopConnection();
  }
  
  private checkConnection() {
    this.userHubService.connectionEstablishedEventEmmiter.subscribe(connectionStatus => this.isConnected = connectionStatus);
  }

  private subscribeForConfirmedRegistration() {
    this.userHubService.userConfirmedNotificationEventEmitter.subscribe(
      () => {
        this.onUserConfirmed();
      }
    );
  }

  public onUserConfirmed() {
    this.ngZone.run(() => {
      this.isRegistrationCompleted = true;
      this.registrationMessageStatus = this.RegistrationStatuses.Accepted;
      this.startLogoutTimer();
    });
  }

  private subscribeForDeclinedRegistration() {
    this.userHubService.userDeclinedNotificationEventEmitter.subscribe(
      () => {
        this.onUserDeclined();
      }
    );
  }

  public onUserDeclined(){
    this.ngZone.run(() => {
      this.isRegistrationCompleted = true;
      this.registrationMessageStatus = this.RegistrationStatuses.Rejected;
      this.startLogoutTimer();
    });
  }

  initConnectionToRegistrationService(){
    if(this.myData.registrationStatus === this.RegistrationStatuses.Processing){
      this.startUserHubServiceConnection();
      this.checkConnection();
      this.subscribeForConfirmedRegistration();
      this.subscribeForDeclinedRegistration();
    }
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
        if(data.registrationStatus === this.RegistrationStatuses.Processing){
          this.isRegistrationCompleted = false;
        }
        else{
          this.isRegistrationCompleted = true;
          this.registrationStatus = null;
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
      window.location.replace('/login');
    });
  }
}
