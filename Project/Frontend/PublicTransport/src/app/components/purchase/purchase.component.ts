import { Component, OnInit } from '@angular/core';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { TicketHttpService } from 'src/app/services/ticket-http.service';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

  ticketTypes: PriceListTicketType[];
  currentTicket: PriceListTicketType;
  feedback: string;

  constructor(private plttService: PLTTService, private ticketService: TicketHttpService) {
    this.ticketTypes = [];
   }

  ngOnInit() {
    this.plttService.getUserPrices().subscribe(
      tickets =>{
        this.ticketTypes = tickets
        this.currentTicket = this.ticketTypes.length > 0 ? this.ticketTypes[0] : undefined;
      }
    );
  }

  buyTicket(email: string){
    console.log(email);
    this.ticketService.buyTicket(this.currentTicket.ticketId, email).subscribe(
      data =>{
        // TODO
      },
      error => this.feedback = error
    );
  }

  showEmailInput(): Boolean{
    let role = localStorage.getItem("role");
    return role !== "Admin" && role !== "AppUser" && role !== "Controller";
  }

  buttonEnabled(): Boolean{
    return this.showEmailInput();
  }
}
