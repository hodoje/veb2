import { BaseHttpService } from './base-http.service';
import { TransportationLinePlan } from '../models/transportation-route-plan.model';
import { Observable } from 'rxjs';
import { TransportationLine } from '../models/transportationline.model';
import { HttpParams } from '@angular/common/http';

export class TransportationLinesHttpService extends BaseHttpService<TransportationLine>{
  specifiedUrl = "transportationLines"

  getTransportationLinePlan(lineNumber: number): Observable<TransportationLinePlan> {
    let params = new HttpParams().set('lineNumber', lineNumber.toString());
    return this.httpClient.get<any>(this.base_url + this.specifiedUrl + `/Plan`, {params: params});
  }

  getAllTransportationLinePlans(): Observable<TransportationLinePlan[]> {
    return this.httpClient.get<any>(this.base_url + this.specifiedUrl + "/Plans");
  }

  updateTransportationLinePlan(routePlan: TransportationLinePlan){
    return this.httpClient.post(this.base_url + this.specifiedUrl + "/UpdateTransportationLinePlan", routePlan);
  }

  addStationToPlan(lineNum: number, sId: number){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/addStationToPlan', { lineNumber: lineNum, stationId: sId});
  }

  removeStationFromPlan(lineNum: number, sId: number){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/removeStationFromPlan', { lineNumber: lineNum, stationId: sId});
  }

  addTransportationLine(newLine: TransportationLine){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/addNewLine', newLine);
  }

  removeTransportationLine(lineToDelete: TransportationLine){
    return this.httpClient.post(this.base_url + this.specifiedUrl + '/RemoveTransportationLine', lineToDelete);
  }
}