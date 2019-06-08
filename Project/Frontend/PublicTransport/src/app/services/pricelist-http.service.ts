import { BaseHttpService } from './base-http.service';

export class PricelistHttpService extends BaseHttpService<any>{
  specifiedUrl = "pricelists"

  getActivePricelist(){
    return this.httpClient.get(this.base_url + this.specifiedUrl + "/getActivePricelist");
  }
}