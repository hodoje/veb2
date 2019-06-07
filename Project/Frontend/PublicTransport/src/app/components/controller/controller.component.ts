import { Component, OnInit } from '@angular/core';
import { TicketHttpService } from 'src/app/services/ticket-http.service';

@Component({
  selector: 'app-controller',
  templateUrl: './controller.component.html',
  styleUrls: ['./controller.component.scss']
})
export class ControllerComponent implements OnInit {

  isTicketValid: Boolean;
  constructor(private ticketService: TicketHttpService) { }

  ngOnInit() {
  }

  validateTicket(id: string){
    this.ticketService.validateTicket(id).subscribe(
      isValid => this.isTicketValid = isValid,
      error => this.isTicketValid = false
    );
  }

}
