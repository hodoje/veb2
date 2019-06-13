import { Component, OnInit } from '@angular/core';
import { TransportationLinesHttpService } from 'src/app/services/transportation-lines-http.service';
import { Polyline } from 'src/app/models/map-models/polyline.model';
import { GeoLocation } from 'src/app/models/map-models/geolocation';
import { TransporationLinePlan } from 'src/app/models/transporation-route-plan.model';


@Component({
  selector: 'app-lines-grid',
  templateUrl: './lines-grid.component.html',
  styleUrls: ['./lines-grid.component.scss']
})
export class LinesGridComponent implements OnInit {

  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543;

  lineNumbers: number[] = [];

  shownRoutes: Polyline[] = [];

  constructor(private transportationLineHttp: TransportationLinesHttpService) { }

  ngOnInit() {
    this.transportationLineHttp.getAll().subscribe(
      (transporationLines) => {
        this.lineNumbers = transporationLines.map(function(line){
          return line.lineNum;
        });
      });
  }

  onLineBtnClick(lineNumber: number) {
    if (this.shownRoutes.find(route => route.lineNumber === lineNumber)) {
      // delete polyline

      this.shownRoutes = this.shownRoutes.filter(route => route.lineNumber !== lineNumber);
      console.log("DELETE: " + lineNumber);
    }
    else {
      this.transportationLineHttp.getTransporationLinePlan(lineNumber).subscribe(
        (plan) => this.addPlanToMap(plan)
      );
      
      this.shownRoutes.push(new Polyline(null, 'red', 'any', lineNumber));
      console.log("ADD: " + lineNumber);
    }
  }

  addPlanToMap(plan: TransporationLinePlan){
    this.shownRoutes.push(this.createPolyline(plan));
  }

  createPolyline(plan: TransporationLinePlan): Polyline {
    let geoLocations: GeoLocation[] = [];

    plan.routes.forEach(route => {
        geoLocations.push(new GeoLocation(route.station.latitude, route.station.longitude));
    });

    console.log(geoLocations);

    return new Polyline(geoLocations, 'red', '', plan.lineNumber);
  }

  placeMarker($event){
    console.log($event.coords.lat + " " + $event.coords.lng);
  }
}
