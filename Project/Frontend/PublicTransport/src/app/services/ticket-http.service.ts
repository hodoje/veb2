import { BaseHttpService } from './base-http.service';
import { TicketDto } from '../models/ticket.model';
import { Observable } from 'rxjs';

export class TicketHttpService extends BaseHttpService<TicketDto>{
    specifiedUrl = "Tickets";

    buyTicket(id: number): Observable<TicketDto> {
        let ticketTdo = new TicketDto();
        ticketTdo.ticketTypeId = id;
        return this.httpClient.post<TicketDto>(this.base_url + this.specifiedUrl + '/BuyTicket',
        ticketTdo);
    }
}