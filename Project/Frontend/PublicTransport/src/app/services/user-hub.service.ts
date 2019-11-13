import { Injectable, EventEmitter } from '@angular/core';

// We declare this global variable so we can use jquery inside our Angular components
declare var $;

@Injectable({
  providedIn: 'root'
})
export class UserHubService {
    // This is the name of our Hub defined on the server as
    // [HubName("accessHub")]
    // public class AccessHub : Hub
    // {
    //  ...
    // }   
    private proxyName: string = 'accessHub';
    // This is the proxy object used to for invoking server side methods
    // and for defining actions executed when server invokes client methods  
    private proxy: any;  
    // Object that holds the connection
    // used for creating the proxy, starting and stopping connections (and some other stuff)
    private connection: any;  
    public connectionExists: Boolean; 

    // Events to which the component that uses this service will subscribe to
    // these events will fire when the server hub calls a client method
    public connectionEstablishedEventEmmiter: EventEmitter <boolean>;    
    public userConfirmedNotificationEventEmitter: EventEmitter<string>;
    public userDeclinedNotificationEventEmitter: EventEmitter<string>;

  constructor() {
    this.connectionEstablishedEventEmmiter = new EventEmitter<boolean>();
    this.userConfirmedNotificationEventEmitter = new EventEmitter<string>();
    this.userDeclinedNotificationEventEmitter = new EventEmitter<string>();

    this.connectionExists = false;
    // create a hub connection  
    this.connection = $.hubConnection("http://localhost:52296/");
    // Setting the query string parameters
    this.connection.qs = { "token" : "Bearer " + localStorage.getItem("jwt") };
    // create new proxy with the given name 
    this.proxy = this.connection.createHubProxy(this.proxyName); 

    // register on server events 
    this.registerForUserConfirmed();
    this.registerForUserDeclined();

    // call the connecion start method to start the connection to send and receive events. 
    //this.startConnection();
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

  public registerForUserConfirmed() {
    let eventName = "confirmUser";
    this.proxy.on(eventName, (userEmail: string) => {
        this.userConfirmedNotificationEventEmitter.emit(userEmail);
    });
  }

  public registerForUserDeclined() {
    let eventName = "declineUser";
    this.proxy.on(eventName, (userEmail: string) => {
        this.userDeclinedNotificationEventEmitter.emit(userEmail);
    });
  }
}
