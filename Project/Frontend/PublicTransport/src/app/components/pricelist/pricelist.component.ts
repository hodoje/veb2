import { Component, OnInit } from '@angular/core';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { Subscription, timer, pipe } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';
import { TicketsComponent } from '../tickets/tickets.component';

@Component({
  selector: 'app-pricelist',
  templateUrl: './pricelist.component.html',
  styleUrls: ['./pricelist.component.scss']
})
export class PricelistComponent implements OnInit {

  subscritpion: Subscription;
  ticketTypePriceLists: {};
  error: string;

  constructor(private plttService: PLTTService) {
    this.ticketTypePriceLists = new Object();
   }

  ngOnInit() {
    this.plttService.getAll().subscribe(
      (pltts: PriceListTicketType[]) => {
        
        pltts.forEach(ticket =>{

          if (this.ticketTypePriceLists[ticket.userType] === undefined){
            this.ticketTypePriceLists[ticket.userType] = [];
          }

          this.ticketTypePriceLists[ticket.userType].push(ticket);
        })
      },
      (error) => this.error = error);
  }

}
