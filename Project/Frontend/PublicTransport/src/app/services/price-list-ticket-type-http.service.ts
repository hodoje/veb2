import { BaseHttpService } from './base-http.service';
import { PriceListTicketType } from '../models/pricelisttickettype.model';
import { Observable } from 'rxjs';

export class PLTTService extends BaseHttpService<PriceListTicketType>{
    specifiedUrl = "ticketTypePricelists"

    getUserPrices(): Observable<PriceListTicketType[]>{
        return this.httpClient.get<PriceListTicketType[]>(this.base_url + "ticketTypePricelists/GetMyTicketPrices");
    }
}