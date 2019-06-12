import { Component, OnInit, Input } from '@angular/core';
import { User } from 'src/app/models/user.model';
import { UserHttpService } from 'src/app/services/user-http.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  @Input() userDetails: User;

  constructor(private userConfirmationHttp: UserHttpService) { }

  ngOnInit() {

  }

  onAccept() {
    this.userConfirmationHttp.confirmUser(this.userDetails.email).subscribe(
      (yay) => console.log("confirmed"),
      (error) => console.log("error")
    );
  }

  onDecline() {
    this.userConfirmationHttp.declineUser(this.userDetails.email).subscribe(
      (naaay) => console.log("declined"),
      (error) => console.log("error")
    );
  }
}
