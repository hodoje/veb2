import { Component, OnInit, CUSTOM_ELEMENTS_SCHEMA, AfterViewChecked } from '@angular/core';
import { PLTTService } from 'src/app/services/price-list-ticket-type-http.service';
import { PriceListTicketType } from 'src/app/models/pricelisttickettype.model';
import { TicketHttpService } from 'src/app/services/ticket-http.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { AccountHttpService } from 'src/app/services/account-http.service';
import { User } from 'src/app/models/user.model';
// import * as braintree from 'braintree-web';

declare var initPaypalButton: any;
declare var closePaypalButton: any;

@Component({
  selector: 'app-purchase',
  templateUrl: './purchase.component.html',
  styleUrls: ['./purchase.component.scss']
})
export class PurchaseComponent implements OnInit {

  // Braintree HostedFields
  // hostedFieldsInstance: braintree.HostedFields;
  // cardholdersName: string;
  /////////////////////////

  ticketTypes: PriceListTicketType[];
  currentTicket: PriceListTicketType;

  isPurchasing: Boolean = false;

  showPaypal: Boolean = false;

  userEmail: string;
  // showCreditCard: Boolean = false;

  constructor(private plttService: PLTTService, 
      private ticketService: TicketHttpService,
         private ngxSpinner: NgxSpinnerService,
         private accountService: AccountHttpService) {
    this.ticketTypes = [];
    this.currentTicket = undefined;
   }

  ngOnInit() {
    this.plttService.getUserPrices().subscribe(
      tickets =>{
        this.ticketTypes = tickets
        this.currentTicket = this.ticketTypes.length > 0 ? this.ticketTypes[0] : null;
      }
    );

    // Braintree HostedFields
    // this.setBraintreeTokenizationKey()
    // this.createBraintreeUI();
    this.initPaypalData();
  }

  initPaypalData(){
    this.accountService.getUserPersonalData().subscribe(
      (data: User) => {
        if(data === null || data === undefined){
          this.userEmail = "";
        }
        else{
          this.userEmail = data.email;
        }
      },
      (err) => {
        console.log(err);
      },
      () => {
        initPaypalButton(this.userEmail);
      }
    );
  }

  buyTicket() {
    this.showPaypal = true;
    this.isPurchasing = true;
    // (<HTMLInputElement>document.getElementById("priceInput")).value = <any>this.currentTicket.price;
  }
  
  goBackToEdit(){
    this.showPaypal = false;
    this.isPurchasing = false;
  }
  
  showEmailInput(): Boolean {
    let role = localStorage.getItem("role");
    return role !== "Admin" && role !== "AppUser" && role !== "Controller";
  }
  
  buttonEnabled(): Boolean {
    return this.showEmailInput();
  }

  //#region Spinner toggling
  showSpinner() {
    this.ngxSpinner.show();
  }

  hideSpinner() {
    this.ngxSpinner.hide();
  }
  //#endregion

  //#region Payment toggling
  // togglePaymentMethod(event){
  //   let btnText = event.target.textContent;
  //   let paypal = "Paypal";
  //   let creditCard = "Credit Card";
  //   switch(btnText) {
  //     case paypal:
  //       this.checkPaypalToggle();
  //       break;
  //     case creditCard:
  //       this.checkCreditCardToggle();
  //       break;
  //     default:
  //       break;
  //   }
  // }

  // private checkPaypalToggle(){
  //   if(!this.showPaypal){
  //     this.showCreditCard = false;
  //   }
  //   this.showPaypal = !this.showPaypal;
  // }

  // private checkCreditCardToggle(){
  //   if(!this.showCreditCard){
  //     this.showPaypal = false;
  //   }
  //   this.showCreditCard = !this.showCreditCard;
  // }
  //#endregion

  //#region Braintree Hosted Fields
  // setBraintreeTokenizationKey(){
  //   localStorage.setItem("brainTreeTokenKey", "sandbox_8hdtgsnz_bwxn2ztm5rvfr6sm");
  // }

  // getBraintreeTokenizationKey(){
  //   return localStorage.getItem("brainTreeTokenKey");
  // }

  // createBraintreeUI() {
  //   braintree.client.create({
  //     authorization: this.getBraintreeTokenizationKey()
  //   }).then((clientInstance) => {
  //     braintree.hostedFields.create({
  //       client: clientInstance,
  //       styles: {
  //         // Override styles for the hosted fields
  //       },

  //       // The hosted fields that we will be using
  //       // NOTE : cardholder's name field is not available in the field options
  //       // and a separate input field has to be used incase you need it
  //       fields: {
  //         number: {
  //           selector: '#card-number',
  //           placeholder: '1111 1111 1111 1111'
  //         },
  //         cvv: {
  //           selector: '#cvv',
  //           placeholder: '111'
  //         },
  //         expirationDate: {
  //           selector: '#expiration-date',
  //           placeholder: 'MM/YY'
  //         }
  //       }
  //     }).then((hostedFieldsInstance) => {

  //       this.hostedFieldsInstance = hostedFieldsInstance;

  //       hostedFieldsInstance.on('focus', (event) => {
  //         const field = event.fields[event.emittedBy];
  //         const label = this.findLabel(field);
  //         label.classList.remove('filled'); // added and removed css classes
  //         // can add custom code for custom validations here
  //       });

  //       hostedFieldsInstance.on('blur', (event) => {
  //         const field = event.fields[event.emittedBy];
  //         const label = this.findLabel(field); // fetched label to apply custom validations
  //         // can add custom code for custom validations here
  //       });

  //       hostedFieldsInstance.on('empty', (event) => {
  //         const field = event.fields[event.emittedBy];
  //         // can add custom code for custom validations here
  //       });

  //       hostedFieldsInstance.on('validityChange', (event) => {
  //         const field = event.fields[event.emittedBy];
  //         const label = this.findLabel(field);
  //         if (field.isPotentiallyValid) { // applying custom css and validations
  //           label.classList.remove('invalid');
  //         } else {
  //           label.classList.add('invalid');
  //         }
  //         // can add custom code for custom validations here
  //       });
  //     });
  //   });
  // }

  // // Tokenize the collected details so that they can be sent to your server
  // // called from the html when the 'Pay' button is clicked
  // tokenizeUserDetails() {
  //   this.showSpinner();
  //   this.hostedFieldsInstance.tokenize({cardholderName: this.cardholdersName}).then((payload) => {
  //     console.log(payload);
  //     // submit payload.nonce to the server from here
  //     this.ticketService.createPurchase(payload.nonce).subscribe(
  //       purchaseConfirm => {
  //         console.log(purchaseConfirm);
  //         this.hideSpinner();
  //       },
  //       purchaseError => {
  //         console.log(purchaseError);
  //         this.hideSpinner();
  //       }
  //     )
  //   }).catch((error) => {
  //     console.log(`error: ${error}`);
  //     this.hideSpinner();
  //     // perform custom validation here or log errors
  //   });
  // }

  // // Fetches the label element for the corresponding field
  // findLabel(field: braintree.HostedFieldsHostedFieldsFieldData) {
  //   return document.querySelector('.hosted-field--label[for="' + field.container.id + '"]');
  // }
  //#endregion
}
