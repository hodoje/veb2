import { BaseHttpService } from './base-http.service';
import { PriceListTicketType } from '../dtos/pricelisttickettype.model';

export class PLTTService extends BaseHttpService<PriceListTicketType>{
    specifiedUrl = "api/TicketTypePricelists"
}