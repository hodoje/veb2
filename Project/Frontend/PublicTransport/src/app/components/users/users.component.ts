import { Component, OnInit, OnDestroy, NgZone, ChangeDetectionStrategy } from '@angular/core';
import { UserConfirmationService } from 'src/app/services/user-confirmation.service';
import { User } from 'src/app/models/user.model';
import { UserHttpService } from 'src/app/services/user-http.service';
import { jsonpCallbackContext } from '@angular/common/http/src/module';
import { Observable, of, Subscription } from 'rxjs';
import { map } from 'rxjs/operators'
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, OnDestroy {

  unregisteredUsers: User[];
  isConnected: Boolean;
  subscriptions: Subscription[] = [];

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
    this.subscriptions.length = 0;

    this.userConfirmationService.disconnect();
  }

  constructor(private userConfirmationService: UserConfirmationService, private userHttpService: UserHttpService, private ngZone: NgZone) { }

  ngOnInit() {
    this.checkConnection();
    this.userConfirmationService.resetEmitters();
    this.subscribeForAddNewUser();
    this.subscribeForUserConfirmation();
    this.subscribeForUserDeclined();

    this.userHttpService.getAllUnConfirmedUsers().subscribe(users => {
      this.unregisteredUsers = users;
    });

    this.userConfirmationService.registerForNewUsers();
    this.userConfirmationService.registerForUserConfirmation();
    this.userConfirmationService.registerForUserDeclining();
  }

  private checkConnection() {
    this.userConfirmationService.startConnection().subscribe(connestionStatus => this.isConnected = connestionStatus);
  }

  private subscribeForAddNewUser() {
    this.subscriptions.push(this.userConfirmationService.addUserNotification.subscribe(e => this.addUser(e)));
  }

  private subscribeForUserConfirmation() {
    this.subscriptions.push(this.userConfirmationService.userConfirmedNotification.subscribe(e => this.confirmUser(e)));
  }

  private subscribeForUserDeclined() {
    this.subscriptions.push(this.userConfirmationService.userDeclinedNotification.subscribe(e => this.declineUser(e)));
  }

  private declineUser(email: string) {
    this.ngZone.run(() => {
      let confirmedUser = this.unregisteredUsers.find(user => user.email === email);
      // TODO ANIMACIJA ?
      this.unregisteredUsers = this.unregisteredUsers.filter(user => user.email !== email);
    });
  }

  private confirmUser(email: string) {
    this.ngZone.run(() => {
      let confirmedUser = this.unregisteredUsers.find(user => user.email === email);
      // TODO ANIMACIJA ?
      this.unregisteredUsers = this.unregisteredUsers.filter(user => user.email !== email);
    });
  }

  private addUser(user: any) {
    this.ngZone.run(() => {
      console.log("EVENT NEW USER");
      this.unregisteredUsers.push(user);
    }); 
  }
}
