import { BaseHttpService } from './base-http.service';
import { TicketDto } from '../models/ticket.model';
import { Observable } from 'rxjs';

export class TicketHttpService extends BaseHttpService<TicketDto>{
    specifiedUrl = "Tickets";

    buyTicket(id: number, email: string): Observable<TicketDto> {
        let ticketTdo = new TicketDto(id, email);
        return this.httpClient.post<TicketDto>(this.base_url + this.specifiedUrl + '/BuyTicket',
        ticketTdo);
    }
}