import { BaseHttpService } from './base-http.service';
import { TicketDto } from '../models/ticket.model';
import { Observable } from 'rxjs';

export class TicketHttpService extends BaseHttpService<TicketDto>{
    specifiedUrl = "Tickets";

    buyTicket(id: number): Observable<TicketDto> {
        console.log(id);
        console.log(this.base_url + this.specifiedUrl + '/BuyTicket');
        return this.httpClient.post<TicketDto>(this.base_url + this.specifiedUrl + '/BuyTicket',
        {ticketTypeId: id});
    }
}