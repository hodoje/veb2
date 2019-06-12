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

  constructor(
    private accountService: AccountHttpService, 
    private router: Router,
    private loginToNavbar: LoginToNavbarService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
  }

  login(user: LoginModel, form: NgForm){
    this.spinner.show();
    this.accountService.logIn(user, (isLoggedIn, errorStatus) => {
      if(isLoggedIn){
        this.isLoggedIn = isLoggedIn;
        this.loginToNavbar.login();
        this.router.navigate(['/home']);
      }
      else{
        if(errorStatus === 400){
          this.isBadLoginParams = true;
        }
        else{
          this.isOtherError = true;
        }
      }
      this.spinner.hide();
    });
    form.reset();
  }
}
