import { Injectable, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { MarkerInfo } from '../models/map-models/marker-info.model';

declare var $;

@Injectable()
export class SimulatorHubService {
    
    private proxyName: string = 'simulatorHub';
    private proxy: any;  
    private connection: any;
    public connectionExists: Boolean; 
    
    public connectionEstablishedEventEmmiter: EventEmitter <boolean>;    
    public newVehiclesPositionNotificationEventEmitter: EventEmitter<any>;

    constructor() {
        this.connectionEstablishedEventEmmiter = new EventEmitter<boolean>();
        this.newVehiclesPositionNotificationEventEmitter = new EventEmitter<any>();

        this.connectionExists = false;
        // create a hub connection  
        this.connection = $.hubConnection("http://localhost:52296/");
        // Setting the query string parameters
        this.connection.qs = { "token" : "Bearer " + localStorage.getItem("jwt") };
        // create new proxy with the given name 
        this.proxy = this.connection.createHubProxy(this.proxyName); 

        // register on server events 
        this.registerForNewVehiclePositions();
    }

    public startConnection() { 
        this.connection.start().done((data: any) => {  
            console.log('Now connected ' + data.transport.name + ', connection ID= ' + data.id);  
            this.connectionEstablishedEventEmmiter.emit(true);  
            this.connectionExists = true;  
        }).fail((error: any) => {  
            console.log('Could not connect ' + error);  
            this.connectionEstablishedEventEmmiter.emit(false);  
        });
    }

    public stopConnection() {
        this.connection.stop();
    }

    public registerForNewVehiclePositions() {
        let eventName = 'vehiclesChangedPositions';        
        this.proxy.on(eventName, (data) => {
            this.newVehiclesPositionNotificationEventEmitter.emit(data);
        }); 
    }
    
    public startSimulation() {
        this.proxy.invoke('StartSimulation');
    }
}