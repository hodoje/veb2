import { Component, OnInit, Input } from '@angular/core';
import { LoginToNavbarService } from 'src/app/services/login-to-navbar.service';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { Router } from '@angular/router';

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
    private accountService: AccountHttpService, 
    private router: Router) {
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
    this.accountService.logOut((isLoggedOut) => {
      if(isLoggedOut){
        this.isLoggedIn = false;
        this.userRole = undefined;
        this.router.navigate(['/home']);
      }
    });
  }
}
