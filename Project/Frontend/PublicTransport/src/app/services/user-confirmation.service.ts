import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

declare var $;

@Injectable()
export class UserConfirmationService {

    private proxy: any;  
    private proxyName: string = 'userConfirmation';  
    private connection: any;  
    private events: string[] = [];

    public connectionExists: Boolean; 
     
    public addUserNotification: EventEmitter<User>;
    public userConfirmedNotification: EventEmitter<string>;
    public userDeclinedNotification: EventEmitter<string>;
    public userBannedNotification: EventEmitter<string>;
    public userUnbanNotification: EventEmitter<string>;

    constructor() {
        this.addUserNotification = new EventEmitter<User>();
        this.userConfirmedNotification = new EventEmitter<string>();
        this.userDeclinedNotification = new EventEmitter<string>();
        this.userBannedNotification = new EventEmitter<string>();
        this.userUnbanNotification = new EventEmitter<string>();

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
        this.events.forEach(event => this.proxy.off(event));
    }

    public registerForNewUsers(): void {
        let eventName = 'newUser';
        this.events.push(eventName);
        
        this.proxy.on(eventName, (data: User) => {
            this.addUserNotification.emit(data);
        }); 
    }

    public registerForUserConfirmation(): Observable<string> {
        let eventName = 'confirmUser';
        this.events.push(eventName);

        // this.proxy.on(eventName, (data: string) => {
        //     this.userConfirmedNotification.emit(data);
        // });

        return Observable.create((observer) => {

            this.proxy.on('confirmUser', (data: string) => {  
                console.log('received time: ' + data);  
                observer.next(data);
            });  
        });
    }

    public registerForUserDeclining(): void {
        let eventName = 'declineUser';
        this.events.push(eventName);

        this.proxy.on(eventName, (data: string) => {
            this.userDeclinedNotification.emit(data);
        });
    }

    public AwaitRegistration(){
        this.proxy.invoke("AwaitRegistration");
    }
    
    public registerForUserBan(): void {
        let eventName = 'banUser';
        this.events.push(eventName);

        this.proxy.on(eventName, (data: string) => {
            this.userBannedNotification.emit(data);
        });
    }

    public registerForUserUnban(): void {
        let eventName = 'unbanUser';
        this.events.push(eventName)

        this.proxy.on(eventName, (data: string) => {
            this.userUnbanNotification.emit(data);
        });
    }

    public closeConnection() {
        this.connection.stop();
    }
}