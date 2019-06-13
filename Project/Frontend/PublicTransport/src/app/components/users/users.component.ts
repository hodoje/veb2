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
  registeredUsers: User[];
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
    this.subscribeForUserBanned();
    this.subscribeForUserUnban();

    this.userHttpService.getAllUnConfirmedUsers().subscribe(users => {
      this.unregisteredUsers = users;
    });

    this.userHttpService.getAllRegisteredUsers().subscribe(users => {
      this.registeredUsers = users;
    })

    this.userConfirmationService.registerForNewUsers();
    this.userConfirmationService.registerForUserConfirmation();
    this.userConfirmationService.registerForUserDeclining();
    this.userConfirmationService.registerForUserBan();
    this.userConfirmationService.registerForUserUnban();
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

  private subscribeForUserBanned() {
    this.subscriptions.push(this.userConfirmationService.userBannedNotification.subscribe(e => this.banUser(e)));
  }

  private subscribeForUserUnban() {
    this.subscriptions.push(this.userConfirmationService.userUnbanNotification.subscribe(e => this.unbanUser(e)));
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
      this.registeredUsers.push(confirmedUser);
    });
  }

  private addUser(user: any) {
    this.ngZone.run(() => {
      // TODO ANIMACIJA ?
      this.unregisteredUsers.push(user);
    }); 
  }

  private banUser(email: string) {
    this.ngZone.run(() => {
      let bannedUser = this.registeredUsers.find(user => user.email === email);
      bannedUser.banned = true;
    });
  }

  private unbanUser(email: string) {
    this.ngZone.run(() => {
      let unbannedUser = this.registeredUsers.find(user => user.email === email);
      unbannedUser.banned = false;
    });
  }

  onAccept(email: string) {
    this.userHttpService.confirmUser(email).subscribe(
      (yay) => console.log("confirmed"),
      (error) => console.log("error")
    );
  }

  onDecline(email: string) {
    this.userHttpService.declineUser(email).subscribe(
      (naaay) => console.log("declined"),
      (error) => console.log("error")
    );
  }

  onBan(email: string) {
    this.userHttpService.banUser(email).subscribe();
  }

  onUnban(email: string) {
    this.userHttpService.unbanUser(email).subscribe();
  }
}
