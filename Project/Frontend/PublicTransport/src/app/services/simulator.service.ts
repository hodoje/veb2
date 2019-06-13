import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

declare var $;

@Injectable()
export class SimulationService {

    private proxy: any;  
    private proxyName: string = 'simulator';  
    private connection: any;  
    private events: string[] = [];

    public connectionExists: Boolean; 
    
    public vehicleNotification: EventEmitter<User>;

    constructor() {
        this.vehicleNotification = new EventEmitter<User>();

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

    public registerForVehiclePositions(): void {
        let eventName = 'vehicleChangePosition';
        this.events.push(eventName);

        this.proxy.on(eventName, (data: User) => {
            this.vehicleNotification.emit(data);
        }); 
    }

    public closeConnection() {
        this.connection.stop();
    }
}