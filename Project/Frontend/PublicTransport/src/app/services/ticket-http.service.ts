import { BaseHttpService } from './base-http.service';
import { TicketDto } from '../models/ticket.model';
import { Observable } from 'rxjs';

export class TicketHttpService extends BaseHttpService<any>{
    specifiedUrl = "Tickets";

    buyTicket(id: number, email: string): Observable<TicketDto> {
        let ticketDto = new TicketDto(id, email);
        return this.httpClient.post<TicketDto>(this.base_url + this.specifiedUrl + '/BuyTicket',
        ticketDto);
    }

    validateTicket(id: string): Observable<any>{
        console.log(parseInt(id.substr(1)));
        let ticketDto = new TicketDto(parseInt(id.substr(1)), "");
        return this.httpClient.post<any>(this.base_url + this.specifiedUrl + "/ValidateTicket",
        ticketDto);
    }
}