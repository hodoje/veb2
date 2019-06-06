import { Component, OnInit } from '@angular/core';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { TicketService } from 'src/app/services/ticket-http.service';

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

  ticketTypes: PriceListTicketType[];
  currentTicket: PriceListTicketType;
  feedback: string;

  constructor(private plttService: PLTTService, private ticketService: TicketService) {
    this.ticketTypes = [];
   }

  ngOnInit() {
    this.plttService.getUserPrices().subscribe(
      tickets => this.ticketTypes = tickets
    );
  }

  buyTicket(){
    this.ticketService.buyTicket(this.currentTicket.ticketId).subscribe(
      data =>{
        // TODO
      },
      error => this.feedback = error
    );
  }
}
