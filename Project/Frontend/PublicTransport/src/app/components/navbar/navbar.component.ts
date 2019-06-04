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

  constructor(private loginToNavbarService: LoginToNavbarService, private authService: AuthHttpService, private router: Router) { }

  ngOnInit() {
    this.loginToNavbarService.change.subscribe(
      isLoggedIn => {
        this.isLoggedIn = isLoggedIn;
      }
    );
  }

  logout(){
    this.authService.logOut((isLoggedOut) => {
      if(isLoggedOut){
        this.isLoggedIn = false;
        this.router.navigate(['/home']);
      }
    });
  }
}
