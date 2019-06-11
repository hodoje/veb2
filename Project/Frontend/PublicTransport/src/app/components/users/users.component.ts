import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserConfirmationService } from 'src/app/services/user-confirmation.service';
import { User } from 'src/app/models/user.model';
import { UserHttpService } from 'src/app/services/user-http.service';
import { jsonpCallbackContext } from '@angular/common/http/src/module';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this.userConfirmationService.disconnect();
  }

  unregisteredUsers: User[] = [];
  isConnected: Boolean;

  constructor(private userConfirmationService: UserConfirmationService, private userHttpService: UserHttpService) { }

  ngOnInit() {
    this.checkConnection();
    this.initialize();

    this.userHttpService.getAll().subscribe(users => {
      this.unregisteredUsers = users;
    });

    this.userConfirmationService.registerForInitialUsers();
  }

  private checkConnection() {
    this.userConfirmationService.startConnection().subscribe(connestionStatus => this.isConnected = connestionStatus);
  }

  private initialize() {
    this.userConfirmationService.addNewUnregisteredUser().subscribe(e => this.addUser(e));
  }

  private addUser(user: User) {
    console.log("NEW UNREGISTERD USER" + JSON.stringify(user));
    this.unregisteredUsers.push(user);
  }

  onClick() {

  }
}
