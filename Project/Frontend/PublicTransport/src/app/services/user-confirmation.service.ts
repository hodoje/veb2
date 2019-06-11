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

    public notificationReceived: EventEmitter < User[] >;  

    constructor() {
        this.notificationReceived = new EventEmitter<User[]>();
        this.connectionExists = false;
        // create a hub connection  
        this.connection = $.hubConnection("http://localhost:52296/");
        console.log(localStorage.getItem("jwt"));
        this.connection.qs = { "token" : `Bearer ${localStorage.getItem("jwt")}` };
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
                console.log('Now connected ' + data.transport.name + ', connection ID= ' + data.id)
                this.connectionExists = true;
    
                observer.next(true);
                observer.complete();
            })
            .fail((error: any) => {  
                console.log('Could not connect ' + error);
                this.connectionExists = false;
    
                observer.next(false);
                observer.complete(); 
            });  
          });
    }

    public registerForInitialUsers(): void {
        this.proxy.on('getUsers', (data: User[]) => {  
            console.log('received notification: ' + data);  
            this.notificationReceived.emit(data);
        }); 
    }

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