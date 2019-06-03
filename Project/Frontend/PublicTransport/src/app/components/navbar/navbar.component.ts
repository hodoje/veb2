import { Component, OnInit, Input } from '@angular/core';
import { LoginToNavbarService } from 'src/app/services/login-to-navbar.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  isLoggedIn = false;

  constructor(private loginToNavbarService: LoginToNavbarService) { }

  ngOnInit() {
  }

}
