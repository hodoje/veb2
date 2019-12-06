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
      },
      error =>{
        this.finally();
        this.responseStatus = "internalError"
      },
      () => {
        this.resetFeedback();
      }
    );
  }

  private finally(){
    this.isServiceProcessing = false;
    this.isButtonPressed = true;
  }

  clearFeedback(){
    this.isButtonPressed = false;
  }

  getResponseStatus() {
    return this.responseStatus;
  }

  resetFeedback(){
    let resetFeedbackCounter = 2;
    let logoutInterval = setInterval(() => {
      if(resetFeedbackCounter !== 0){
        resetFeedbackCounter--;
      }
      else{
        this.clearFeedback();
        clearInterval(logoutInterval);
      }
    }, 1000);
  }
}
