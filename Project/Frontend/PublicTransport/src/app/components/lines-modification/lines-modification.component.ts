import { Component, OnInit } from '@angular/core';
import { Polyline } from 'src/app/models/map-models/polyline.model';
import { TransportationLinesHttpService } from 'src/app/services/transportation-lines-http.service';
import { TransportationLinePlan } from 'src/app/models/transportation-route-plan.model';
import { GeoLocation } from 'src/app/models/map-models/geolocation';
import { StationHttpService } from 'src/app/services/station-http.service';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';

@Component({
  selector: 'app-lines-modification',
  templateUrl: './lines-modification.component.html',
  styleUrls: ['./lines-modification.component.scss']
})
export class LinesModificationComponent implements OnInit {

  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543;

  plans: TransportationLinePlan[] = [];
  currentPlan: TransportationLinePlan;
  currentStations = [];
  allStations = [];  
  // https://coryrylan.com/blog/creating-a-dynamic-checkbox-list-in-angular
  updateLineStationsForm: FormGroup;

  constructor(private transportationLineService: TransportationLinesHttpService,
    private stationsService: StationHttpService,
    private formBuilder: FormBuilder) {
      this.updateLineStationsForm = this.formBuilder.group({
        stations: new FormArray([])
      });
  }

  addStationCheckboxes(){
    this.allStations.forEach((station, i) => {
      var determineIfItNeedsToBeCheckedByCheckingInCurrentLinesStations = true;
      const control = new FormControl(determineIfItNeedsToBeCheckedByCheckingInCurrentLinesStations);
      (this.updateLineStationsForm.controls.stations as FormArray).push(control);
    })
  }

  ngOnInit() { 
    this.getAllStations();
  }

  getAllPlans(){
    this.transportationLineService.getAllTransportationLinePlans().subscribe(
      (data) => {
        this.plans = data;
        console.log(data);
        this.currentPlan = this.plans[0];
        this.currentStations = this.currentPlan.routes.map(r => {
          return r.station;
        });
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getAllStations(){
    this.stationsService.getAll().subscribe(
      (data) => {
        this.allStations = data;
        this.addStationCheckboxes(); // Can add checkboxes only after data arrives
        console.log(this.allStations);
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getAllPlans();
      }
    );
  }

  updateLineStations(){
    const selectedOrderIds = this.updateLineStationsForm.value.orders
    .map((v, i) => v ? this.allStations[i].id : null)
    .filter(v => v !== null);
    console.log(selectedOrderIds);
  }

  addStationToPlan(sId: number){
    this.transportationLineService.addStationToPlan(this.currentPlan.lineNumber, sId).subscribe(
      (data) => {
        this.getAllPlans();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  removeStationFromPlan(sId: number){
    this.transportationLineService.removeStationFromPlan(this.currentPlan.lineNumber, sId).subscribe(
      (data) => {
        this.getAllPlans();
      },
      (err) => {
        console.log(err);
      }
    );
  }
  
  changePlan(){
    console.log(this.currentPlan.lineNumber);
    this.currentStations = this.currentPlan.routes.map(r => {
      return r.station;
    });
  }

  checkIfExist(stationName){
    return this.currentStations.find(s => s.name === stationName);
  }

  createPolyline(plan: TransportationLinePlan, color: string): Polyline {
    let geoLocations: GeoLocation[] = [];

    plan.routes.forEach(route => {
        geoLocations.push(new GeoLocation(route.station.latitude, route.station.longitude));
    });

    return new Polyline(geoLocations, color, '', plan.lineNumber);
  }

  placeMarker($event){
    console.log($event.coords.lat + " " + $event.coords.lng);
  }

  onClick(stationName) {
    console.log(stationName);
  }
}
