import { BaseHttpService } from './base-http.service';
import { TransporationLinePlan } from '../models/transporation-route-plan.model';
import { Observable } from 'rxjs';
import { TransportationLine } from '../models/transportationline.model';
import { HttpParams } from '@angular/common/http';

export class TransportationLinesHttpService extends BaseHttpService<TransportationLine>{
  specifiedUrl = "transportationLines"

  getTransporationLinePlan(lineNumber: number): Observable<TransporationLinePlan> {
    let params = new HttpParams().set('lineNumber', lineNumber.toString());
    return this.httpClient.get<any>(this.base_url + this.specifiedUrl + `/Plan`, {params: params});
  }

  getAllTransportationLinePlans(): Observable<TransporationLinePlan[]> {
    return this.httpClient.get<any>(this.base_url + this.specifiedUrl + "/Plans");
  }

  addStationToPlan(lineNum: number, sId: number){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/addStationToPlan', { lineNumber: lineNum, stationId: sId});
  }

  removeStationFromPlan(lineNum: number, sId: number){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/removeStationFromPlan', { lineNumber: lineNum, stationId: sId});
  }
}