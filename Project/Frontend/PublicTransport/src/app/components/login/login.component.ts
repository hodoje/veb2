import { Component, OnInit } from '@angular/core';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { Router } from '@angular/router';
import { LoginModel } from 'src/app/models/login.model';
import { NgForm } from '@angular/forms';
import { LoginToNavbarService } from 'src/app/services/login-to-navbar.service';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  isLoggedIn = false;
  isBadLoginParams: boolean;
  isOtherError: boolean;
  errorDescription: string;
  isError: boolean;

  constructor(
    private accountService: AccountHttpService, 
    private router: Router,
    private loginToNavbar: LoginToNavbarService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
  }

  login(user: LoginModel, form: NgForm){
    this.spinner.show();
    this.accountService.logIn(user, (isLoggedIn, errorStatus, errorDescription) => {
      if(isLoggedIn){
        this.isError = false;
        this.isLoggedIn = isLoggedIn;
        this.loginToNavbar.login();
        this.router.navigate(['/home']);
      }
      else{
        this.isError = true;
        this.errorDescription = errorDescription;
      }
      this.spinner.hide();
    });
    form.reset();
  }
}
