import { Component, OnInit } from '@angular/core';
import { TicketHttpService } from 'src/app/services/ticket-http.service';

@Component({
  selector: 'app-ticket-validation',
  templateUrl: './ticket-validation.component.html',
  styleUrls: ['./ticket-validation.component.scss']
})
export class TicketValidationComponent implements OnInit {

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
