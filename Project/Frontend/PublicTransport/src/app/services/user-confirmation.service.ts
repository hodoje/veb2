import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

declare var $;

@Injectable()
export class UserConfirmationService {

    private proxy: any;  
    private proxyName: string = 'userConfirmation';  
    private connection: any;  
    public connectionExists: Boolean; 

    public notificationReceived: EventEmitter<User>;  

    constructor() {
        this.notificationReceived = new EventEmitter<User>();
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

    public registerForNewUsers(): void {
        this.proxy.on('newUser', (data: User) => {  
            this.notificationReceived.emit(data);
        }); 
    }


    //temp
    public addNewUnregisteredUser() {
        return Observable.create((observer) => {
            this.proxy.on('getUsers', (data: User[]) => {
                console.log("initial users: " + data);
                observer.next(data);
            })
        });
    }

    public closeConnection() {
        this.connection.stop();
    }
}