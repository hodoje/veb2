import { Component, OnInit } from '@angular/core';
import { TicketHttpService } from 'src/app/services/ticket-http.service';

@Component({
  selector: 'app-controller',
  templateUrl: './controller.component.html',
  styleUrls: ['./controller.component.scss']
})
export class ControllerComponent implements OnInit {

  responseStatus: String;
  isButtonPressed: Boolean = false;
  isServiceProcessing: Boolean = false;

  constructor(private ticketService: TicketHttpService) { }

  ngOnInit() {
  }

  onValidateTicket(id: string){
    this.isServiceProcessing = true;

    this.ticketService.validateTicket(id).subscribe(
      data => {
        this.finally();
        this.responseStatus = data.isValid;
        console.log(this.responseStatus);
      },
      error =>{
        this.finally();
        this.responseStatus = "internalError"
        console.log(this.responseStatus);
      },
    );
  }

  private finally(){
    this.isServiceProcessing = false;
    this.isButtonPressed = true;
  }

  clearFeedback(){
    this.isButtonPressed = false;
    console.log(this.isButtonPressed);
  }

  getResponseStatus() {
    return this.responseStatus;
  }

}
