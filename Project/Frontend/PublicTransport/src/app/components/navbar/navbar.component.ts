import { Component, OnInit, Input } from '@angular/core';
import { LoginToNavbarService } from 'src/app/services/login-to-navbar.service';
import { AuthHttpService } from 'src/app/services/auth-http.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  isLoggedIn = false;
  userRole: string;

  constructor(private loginToNavbarService: LoginToNavbarService, private authService: AuthHttpService, private router: Router) {
    if(localStorage.jwt !== undefined){
      this.isLoggedIn = true;
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
    this.isLoggedIn = false;
    localStorage.jwt = undefined;
    localStorage.role = undefined;
  }
}
