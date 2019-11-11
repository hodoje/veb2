import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { AdminHubService } from 'src/app/services/admin-hub.service';
import { User } from 'src/app/models/user.model';
import { UserHttpService } from 'src/app/services/user-http.service';
import { Subscription } from 'rxjs';
import { ImageHttpService } from 'src/app/services/image-http.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, OnDestroy {

  unconfirmedUsers: User[];
  confirmedUsers: User[];
  isConnected: Boolean;
  subscriptions: Subscription[] = [];

  ngOnDestroy(): void {
    this.stopAdminHubServiceConnection();
  }

  constructor(
    private adminHubService: AdminHubService, 
    private userHttpService: UserHttpService, 
    private ngZone: NgZone,
    private imageService: ImageHttpService,) { }

  ngOnInit() {
    this.startAdminHubServiceConnection();
    this.checkConnection();
    this.subscribeForRegisteredUsers();
    this.subscribeForConfirmedUsers();
    this.subscribeForDeclinedUsers();
    this.subscribeForBannedUsers();
    this.subscribeForUnbannedUsers();

    this.userHttpService.getAllUnConfirmedUsers().subscribe(users => {
      this.unconfirmedUsers = users;
      this.unconfirmedUsers.forEach(user => {
        this.getUserImage(user);
      });
    });

    this.userHttpService.getAllRegisteredUsers().subscribe(users => {
      this.confirmedUsers = users;
    })
  }

  private startAdminHubServiceConnection() {
    this.adminHubService.startConnection();
  }

  private stopAdminHubServiceConnection() {
    this.adminHubService.stopConnection();
  }

  private checkConnection() {
    this.adminHubService.connectionEstablishedEventEmmiter.subscribe(connestionStatus => this.isConnected = connestionStatus);
  }

  // Subscribe for actions taken by other admins
  private subscribeForRegisteredUsers() {
    this.adminHubService.userRegisteredNotificationEventEmitter.subscribe(
      (e) => {
        this.onUserRegistered(e);
      }
    );
  }

  public onUserRegistered(user: User) {
    this.ngZone.run(() => {
      this.unconfirmedUsers.push(user);
      this.getUserImage(user);
    });
  }

  private subscribeForConfirmedUsers() {
    this.adminHubService.userConfirmedNotificationEventEmitter.subscribe(
      (e) => {
        this.onUserConfirmed(e);
      }
    );
  }

  public onUserConfirmed(userEmail: string) {
    this.ngZone.run(() => {
      let confirmedUser = this.unconfirmedUsers.find(user => user.email === userEmail);
      this.unconfirmedUsers = this.unconfirmedUsers.filter(user => user.email !== userEmail);
      this.confirmedUsers.push(confirmedUser);
    });
  }

  private subscribeForDeclinedUsers() {
    this.adminHubService.userDeclinedNotificationEventEmitter.subscribe(
      (e) => {
        this.onUserDeclined(e);
      }
    );
  }

  public onUserDeclined(userEmail: string){
    this.ngZone.run(() => {
      this.unconfirmedUsers = this.unconfirmedUsers.filter(user => user.email !== userEmail);
    });
  }

  private subscribeForBannedUsers() {
    this.adminHubService.userBannedNotificationEventEmitter.subscribe(
      (e) => {
        this.onUserBanned(e);
      }
    );
  }

  public onUserBanned(userEmail: string) {
    this.ngZone.run(() => {
      let userToBan = this.confirmedUsers.find(user => user.email === userEmail);
      userToBan.banned = true;
    });
  }

  private subscribeForUnbannedUsers() {
    this.adminHubService.userBannedNotificationEventEmitter.subscribe(
      (e) => {
        this.onUserUnbanned(e);
      }
    );
  }

  public onUserUnbanned(userEmail: string) {
    this.ngZone.run(() => {
      let userToUnban = this.confirmedUsers.find(user => user.email === userEmail);
      userToUnban.banned = false;
    });
  }

  // This admin's actions
  public acceptUser(email: string) {
    this.userHttpService.confirmUser(email).subscribe(
      (confirm) => {
        console.log("confirmed");
      },
      (error) => {
        console.log("error");
      }
    );
  }

  public declineUser(email: string) {
    this.userHttpService.declineUser(email).subscribe(
      (decline) => {
        console.log("declined");
      },
      (error) => {
        console.log("error");
      }
    );
  }

  public banUser(email: string) {
    this.userHttpService.banUser(email).subscribe(
      () => {
        let bannedUser = this.confirmedUsers.find(user => user.email === email);
        bannedUser.banned = true;
      }
    );
  }

  public unbanUser(email: string) {
    this.userHttpService.unbanUser(email).subscribe(
      () => {
        let unbannedUser = this.confirmedUsers.find(user => user.email === email);
        unbannedUser.banned = false;
      }
    );
  }

  public getUserImage(user: User){
    this.imageService.getImageForUsername(user.email).subscribe(
      data => { 
        this.createImageFromBlob(user, data);
      }, 
      error => {
        console.log(error);
    });
  }

  private createImageFromBlob(user: User, image: Blob){
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      user.document = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
}
