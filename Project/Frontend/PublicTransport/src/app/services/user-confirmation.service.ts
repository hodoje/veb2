import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

declare var $;

@Injectable()
export class UserConfirmationService {

    private proxy: any;  
    private proxyName: string = 'userConfirmation';  
    private connection: any;  
    private isEmitterInitialized = true

    public connectionExists: Boolean; 
    
    public addUserNotification: EventEmitter<User>;
    public userConfirmedNotification: EventEmitter<string>;
    public userDeclinedNotification: EventEmitter<string>;

    constructor() {
        this.addUserNotification = new EventEmitter<User>();
        this.userConfirmedNotification = new EventEmitter<string>();
        this.userDeclinedNotification = new EventEmitter<string>();
        this.connectionExists = false;
        // create a hub connection  
        this.connection = $.hubConnection("http://localhost:52296/");
        console.log(localStorage.getItem("jwt"));
        this.connection.qs = { "token" : "Bearer " + localStorage.getItem("jwt") };
        // create new proxy with the given name 
        this.proxy = this.connection.createHubProxy(this.proxyName); 
    }

    public disconnect(): void {
        this.connection.stop();
    }

    public startConnection(): Observable<Boolean> { 
      
        return Observable.create((observer) => {
           
            this.connection.start()
            .done((data: any) => {
                this.connectionExists = true;
    
                observer.next(true);
                observer.complete();
            })
            .fail((error: any) => {
                this.connectionExists = false;
    
                observer.next(false);
                observer.complete(); 
            });  
          });
    }

    public resetEmitters() {
        this.proxy.off('newUser');
        this.proxy.off('confirmUser');
        this.proxy.off('declineUser');
    }

    public registerForNewUsers(): void {
        this.proxy.on('newUser', (data: User) => {
            this.addUserNotification.emit(data);
        }); 
    }

    public registerForUserConfirmation(): void {
        this.proxy.on('confirmUser', (data: string) => {
            this.userConfirmedNotification.emit(data);
        });
    }

    public registerForUserDeclining(): void {
        this.proxy.on('declineUser', (data: string) => {
            this.userDeclinedNotification.emit(data);
        });
    }

    public closeConnection() {
        this.connection.stop();
    }
}