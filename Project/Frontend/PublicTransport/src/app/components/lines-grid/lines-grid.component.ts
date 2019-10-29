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

  colors: string[] = ['#FF0000', '#00FF00', '#0000FF', '#FFFF00', '#00FFFF', '#FF00FF', '#C0C0C0', '#808080', '#800000', '#808000', '	#008000', '#800080', '#008080', '#000080',
                      '#FF4500', '#F0E68C', '	#00CED1'];

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
    let tempPolyline = this.shownRoutes.find(route => route.lineNumber === lineNumber);
    if (tempPolyline) {
      this.shownRoutes = this.shownRoutes.filter(route => route.lineNumber !== lineNumber);
      
      this.colors.push(tempPolyline.color);
    }
    else {
      this.transportationLineHttp.getTransporationLinePlan(lineNumber).subscribe(
        (plan) => this.addPlanToMap(plan)
      );
    }
  }

  addPlanToMap(plan: TransporationLinePlan){
    let color = this.colors.length > 1 ? this.colors.pop() : this.colors[0];

    this.shownRoutes.push(this.createPolyline(plan, color));
  }

  createPolyline(plan: TransporationLinePlan, color: string): Polyline {
    let geoLocations: GeoLocation[] = [];

    plan.routes.forEach(route => {
        geoLocations.push(new GeoLocation(route.station.latitude, route.station.longitude));
    });

    return new Polyline(geoLocations, color, '', plan.lineNumber);
  }

  placeMarker($event){
    console.log($event.coords.lat + " " + $event.coords.lng);
  }
}
