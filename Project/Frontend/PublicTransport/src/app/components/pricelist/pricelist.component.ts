import { Component, OnInit } from '@angular/core';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { Subscription, timer, pipe } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';

@Component({
  selector: 'app-pricelist',
  templateUrl: './pricelist.component.html',
  styleUrls: ['./pricelist.component.scss']
})
export class PricelistComponent implements OnInit {

  subscritpion: Subscription;
  ticketTypePriceLists: PriceListTicketType[] = [];
  error: string;

  constructor(private plttService: PLTTService) { }

  ngOnInit() {
    // PERIODIC POLL
    // this.subscritpion = timer(0, 5000).pipe(
    //   switchMap(() => this.plttService.getAll()))
    //   .subscribe((pltts) => this.plttService,
    //              (error) => this.error = error);

    this.plttService.getAll().subscribe(
      (pltts) => {
        this.ticketTypePriceLists = pltts
        console.log(pltts);
      },
      (error) => this.error = error);
  }

}
