import { Component, OnInit, Input } from '@angular/core';
import { LoginToNavbarService } from 'src/app/services/login-to-navbar.service';
import { AuthHttpService } from 'src/app/services/auth-http.service';
import { Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  isLoggedIn = false;
  userRole: string;

  constructor(
    private loginToNavbarService: LoginToNavbarService, 
    private authService: AuthHttpService, 
    private router: Router,
    private spinner: NgxSpinnerService) {
    if(localStorage.jwt !== undefined){
      this.isLoggedIn = true;
      this.userRole = localStorage.role;
    }
  }

  ngOnInit() {
    this.loginToNavbarService.change.subscribe(
      isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
        this.userRole = localStorage.role;
      }
    );
  }

  logout(){
    this.spinner.show();
    this.authService.logOut((isLoggedOut) => {
      if(isLoggedOut){
        this.isLoggedIn = false;
        this.userRole = undefined;
        this.spinner.hide();
        this.router.navigate(['/home']);
      }
    });
  }
}
