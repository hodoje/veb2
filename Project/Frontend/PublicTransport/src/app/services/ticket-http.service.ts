import { BaseHttpService } from './base-http.service';
import { TicketDto } from '../models/ticket.model';
import { Observable } from 'rxjs';
import { HttpHeaders } from '@angular/common/http';

export class TicketHttpService extends BaseHttpService<any>{
    specifiedUrl = "Tickets";

    // buyTicket(id: number, email: string): Observable<TicketDto> {
    //     let ticketDto = new TicketDto(id, email);
    //     return this.httpClient.post<TicketDto>(this.base_url + this.specifiedUrl + '/BuyTicket',
    //     ticketDto);
    // }

    validateTicket(id: string): Observable<any>{
        let ticketDto = new TicketDto(parseInt(id.substr(1)), "");
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/validateTicket",
        ticketDto);
    }

    // createPurchase(paymentMethodNonce: string){
    //     const headers = new HttpHeaders({'Content-Type':'application/json; charset=utf-8'});
    //     return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/createPurchase", {'paymentMethodNonce': paymentMethodNonce}, {headers});
    // }
}