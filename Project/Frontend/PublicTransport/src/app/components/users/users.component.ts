import { Component, OnInit, OnDestroy, NgZone, ChangeDetectionStrategy } from '@angular/core';
import { UserConfirmationService } from 'src/app/services/user-confirmation.service';
import { User } from 'src/app/models/user.model';
import { UserHttpService } from 'src/app/services/user-http.service';
import { jsonpCallbackContext } from '@angular/common/http/src/module';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators'
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this.userConfirmationService.disconnect();
  }

  unregisteredUsers: User[];
  isConnected: Boolean;

  constructor(private userConfirmationService: UserConfirmationService, private userHttpService: UserHttpService, private ngZone: NgZone) { }

  ngOnInit() {
    this.checkConnection();
    this.initialize();

    this.userHttpService.getAllUnConfirmedUsers().subscribe(users => {
      this.unregisteredUsers = users;
    });

    this.userConfirmationService.registerForNewUsers();
  }

  private checkConnection() {
    this.userConfirmationService.startConnection().subscribe(connestionStatus => this.isConnected = connestionStatus);
  }

  private initialize() {
    this.userConfirmationService.notificationReceived.subscribe(e => this.addUser(e));
  }

  private addUser(user: any) {
    this.ngZone.run(() => {
      this.unregisteredUsers.push(user);
    }); 
  }
}
