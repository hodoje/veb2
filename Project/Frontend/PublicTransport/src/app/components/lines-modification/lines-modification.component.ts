import { Component, OnInit } from '@angular/core';
import { Polyline } from 'src/app/models/map-models/polyline.model';
import { TransportationLinesHttpService } from 'src/app/services/transportation-lines-http.service';
import { TransportationLinePlan } from 'src/app/models/transportation-route-plan.model';
import { GeoLocation } from 'src/app/models/map-models/geolocation';
import { StationHttpService } from 'src/app/services/station-http.service';
import { FormGroup, FormBuilder, FormArray, FormControl } from '@angular/forms';
import { TransportationLine } from 'src/app/models/transportationline.model';
import { RoutePoint } from 'src/app/models/route-point.model';

@Component({
  selector: 'app-lines-modification',
  templateUrl: './lines-modification.component.html',
  styleUrls: ['./lines-modification.component.scss']
})
export class LinesModificationComponent implements OnInit {

  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543;

  allLines: TransportationLine[];
  currentLine: TransportationLine;
  currentPlan: TransportationLinePlan;
  allStations = [];  
  // https://coryrylan.com/blog/creating-a-dynamic-checkbox-list-in-angular
  updateLineStationsForm: FormGroup;
  isUpdateButtonDisabled = true;

  constructor(private transportationLineService: TransportationLinesHttpService,
    private stationsService: StationHttpService,
    private formBuilder: FormBuilder) {
      this.updateLineStationsForm = this.formBuilder.group({
        stations: new FormArray([])
      });
  }

  ngOnInit() { 
    this.getAllLines();
    this.getAllStations();
  }
  
  getAllLines() {
    this.transportationLineService.getAll().subscribe(
      (data: TransportationLine[]) => {
        this.allLines = data;
        this.currentLine = this.allLines[0];
      },
      (err) => {
        console.log(err);
      },
      () => {
        this.getPlanForCurrentLine();
      }
    );
  }
    
  getAllStations() {
    this.stationsService.getAll().subscribe(
      (data) => {
        this.allStations = data;
        this.addStationCheckboxes();        
      },
      (err) => {
        console.log(err);
      }
    );
  }

  getPlanForCurrentLine(){
    this.transportationLineService.getTransportationLinePlan(this.currentLine.lineNum).subscribe(
      (data: TransportationLinePlan) => {
        this.currentPlan = data;
        this.determineCheckboxStates();
      },
      (err) =>{
        console.log(err);
      }
    );
  }

  addStationCheckboxes() {
    this.allStations.forEach((station, i) => {
      const control = new FormControl();
      (this.updateLineStationsForm.controls.stations as FormArray).push(control);
    });
  }

  determineCheckboxStates(){
    let updateLineStationsFormValue = [];
    this.allStations.forEach(station => {
      let isStationChecked = this.currentPlan.routePoints.find(rp => rp.station.id == station.id) ? true : false;
      updateLineStationsFormValue.push(isStationChecked);
    });
    (this.updateLineStationsForm.controls.stations as FormArray).setValue(updateLineStationsFormValue);
  }

  changeLine(){
    this.getPlanForCurrentLine();
  }

  updatePlanPathForCurrentPlan(stationId: number, isChecked: boolean) {
    this.isUpdateButtonDisabled = false;
    if(isChecked){
      let newRoutePoint = new RoutePoint();
      let nextSequenceNo = Math.max(...this.currentPlan.routePoints.map(r => r.sequenceNumber)) + 1;
      newRoutePoint.sequenceNumber = nextSequenceNo;
      newRoutePoint.station = this.allStations.find(s => s.id == stationId);
      this.currentPlan.routePoints.push(newRoutePoint);
    }
    else{
      let routePointToBeRemoved = this.currentPlan.routePoints.find(rp => rp.station.id == stationId) as RoutePoint;
      let routePointToBeRemovedSequenceNo = routePointToBeRemoved.sequenceNumber;
      this.currentPlan.routePoints = this.currentPlan.routePoints.filter(rp => rp.station.id != stationId);
      let routePointsToBeUpdated = this.currentPlan.routePoints.filter(rp => rp.sequenceNumber > routePointToBeRemovedSequenceNo) as RoutePoint[];
      this.currentPlan.routePoints.forEach(rp => {
        if(routePointsToBeUpdated.indexOf(rp) > -1){
          --rp.sequenceNumber;
        }
      });
    }
  }

  updateLinePlan(){
    this.transportationLineService.updateTransportationLinePlan(this.currentPlan).subscribe(
      () => {
        this.isUpdateButtonDisabled = true;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  createPolyline(plan: TransportationLinePlan, color: string): Polyline {
    let geoLocations: GeoLocation[] = [];

    plan.routePoints.forEach(route => {
        geoLocations.push(new GeoLocation(route.station.latitude, route.station.longitude));
    });

    return new Polyline(geoLocations, color, '', plan.lineNumber);
  }
}
