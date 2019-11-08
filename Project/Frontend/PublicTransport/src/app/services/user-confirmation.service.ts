import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

// We declare this global variable so we can use jquery inside our Angular components
declare var $;

@Injectable()
export class UserConfirmationService {
    // This is the name of our Hub defined on the server as
    // [HubName("userConfirmation")]
    // public class UserProfileConfirmationHub : Hub
    // {
    //  ...
    // }   
    private proxyName: string = 'userConfirmation';
    // This is the proxy object used to for invoking server side methods
    // and for defining actions executed when server invokes client methods  
    private proxy: any;  
    // Object that holds the connection
    // used for creating the proxy, starting and stopping connections (and some other stuff)
    private connection: any;  
    private events: string[] = [];
    public connectionExists: Boolean; 
    
    // Events to which the component that uses this service will subscribe to
    // these events will fire when the server hub calls a client methodf
    public addUserNotificationEventEmitter: EventEmitter<User>;
    public userConfirmedNotificationEventEmitter: EventEmitter<string>;
    public userDeclinedNotificationEventEmitter: EventEmitter<string>;
    public userBannedNotificationEventEmitter: EventEmitter<string>;
    public userUnbanNotificationEventEmitter: EventEmitter<string>;

    constructor() {
        this.addUserNotificationEventEmitter = new EventEmitter<User>();
        this.userConfirmedNotificationEventEmitter = new EventEmitter<string>();
        this.userDeclinedNotificationEventEmitter = new EventEmitter<string>();
        this.userBannedNotificationEventEmitter = new EventEmitter<string>();
        this.userUnbanNotificationEventEmitter = new EventEmitter<string>();

        this.connectionExists = false;
        // create a hub connection  
        this.connection = $.hubConnection("http://localhost:52296/");
        // Setting the query string parameters
        this.connection.qs = { "token" : "Bearer " + localStorage.getItem("jwt") };
        // create new proxy with the given name 
        this.proxy = this.connection.createHubProxy(this.proxyName); 
    }

    public startConnection(): Observable<Boolean> { 
        return Observable.create((observer) => {
            this.connection.start()
            .done((data: any) => {
                this.connectionExists = true;
                // Next methods is used to do anything with the data and that will be the return value of this whole return expression
                observer.next(true);
                // Complete method is used to stop observing the data
                observer.complete();
            })
            .fail((error: any) => {
                this.connectionExists = false;
                observer.next(false);
                observer.complete(); 
            });  
          });
    }

    public disconnect(): void {
        this.connection.stop();
    }

    public closeConnection() {
        this.connection.stop();
    }

    // Called by user
    // Called by admin
    public resetEmitters() {
        this.events.forEach(event => this.proxy.off(event));
        this.events = [];
    }

    // Called by admin
    public registerForNewUsers(): void {
        let eventName = 'newUser';
        this.events.push(eventName);
        this.proxy.on(eventName, (data: User) => {
            this.addUserNotificationEventEmitter.emit(data);
        }); 
    }

    // Called by user
    public registerForUserConfirmation(): Observable<string> {
        let eventName = 'confirmUser';
        this.events.push(eventName);
        return Observable.create((observer) => {
            this.proxy.on(eventName, (data: string) => {  
                console.log('received time: ' + data);  
                observer.next(data);
                observer.complete();
            });  
        });
    }

    // Called by user
    // Called by admin
    public registerForUserDeclining(): void {
        let eventName = 'declineUser';
        this.events.push(eventName);
        return Observable.create((observer) => {
            this.proxy.on(eventName, (data: string) => {
                this.userDeclinedNotificationEventEmitter.emit(data);
                observer.complete();
            });
        })
    }

    // Called by user
    public AwaitRegistration(){
        this.proxy.invoke("AwaitRegistration");
    }
    
    // Called by admin
    public registerForUserBan(): void {
        let eventName = 'banUser';
        this.events.push(eventName);
        this.proxy.on(eventName, (data: string) => {
            this.userBannedNotificationEventEmitter.emit(data);
        });
    }

    // Called by admin
    public registerForUserUnban(): void {
        let eventName = 'unbanUser';
        this.events.push(eventName)
        this.proxy.on(eventName, (data: string) => {
            this.userUnbanNotificationEventEmitter.emit(data);
        });
    }
}