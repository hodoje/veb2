import { Component, OnInit } from '@angular/core';
import { AuthHttpService } from 'src/app/services/auth-http.service';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private http: AuthHttpService, private router: Router) { }

  ngOnInit() {
  }

  login(user: User, form: NgForm){
    this.http.logIn(user, () => {});
    form.reset();
  }
}
