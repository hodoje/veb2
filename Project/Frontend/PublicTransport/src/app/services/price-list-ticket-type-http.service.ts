import { BaseHttpService } from './base-http.service';
import { PriceListTicketType } from '../models/pricelisttickettype.model';

export class PLTTService extends BaseHttpService<PriceListTicketType>{
    specifiedUrl = "ticketTypePricelists"
}