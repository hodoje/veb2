import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { TicketHttpService } from 'src/app/services/ticket-http.service';
import { NgxSpinnerService } from 'ngx-spinner';


@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

  ticketTypes: PriceListTicketType[];
  currentTicket: PriceListTicketType;

  isTicketBought: Boolean = false;
  buttonPressed: Boolean = false;
  isProccessing: Boolean = false;

  constructor(private plttService: PLTTService, private ticketService: TicketHttpService,
              private ngxSpinner: NgxSpinnerService) {
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

  buyTicket(email: string) {
    this.isProccessing = true;
    this.showSpinner();
    
    this.ticketService.buyTicket(this.currentTicket.ticketId, email).subscribe(
      data =>{
        this.isTicketBought = true;
      },
      error =>{
        this.isTicketBought = false;
      },
      () => {
        this.hideSpinner();
        this.buttonPressed = true;
        this.isProccessing = false;
      }
    );
  }
  
  showSpinner() {
    this.ngxSpinner.show();
  }

  hideSpinner() {
    this.ngxSpinner.hide();
  }

  showEmailInput(): Boolean {
    let role = localStorage.getItem("role");
    return role !== "Admin" && role !== "AppUser" && role !== "Controller";
  }

  buttonEnabled(): Boolean {
    return this.showEmailInput();
  }
}
