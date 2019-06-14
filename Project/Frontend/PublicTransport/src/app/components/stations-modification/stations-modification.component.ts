import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { LongLatValidators } from 'src/app/common/reactiveFormsValidators/long-lat-validators.directive';
import { MarkerInfo } from 'src/app/models/map-models/marker-info.model';
import { Station } from 'src/app/models/station.model';
import { StationHttpService } from 'src/app/services/station-http.service';
import { GeoLocation } from 'src/app/models/map-models/geolocation';

declare var google;

@Component({
  selector: 'app-stations-modification',
  templateUrl: './stations-modification.component.html',
  styleUrls: ['./stations-modification.component.scss']
})
export class StationsModificationComponent implements OnInit {

  stations: Station[];
  currentStation: Station;
  currentSelectedStation: string;
  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543;
  mapInfo: MarkerInfo;

  stationDataForm = new FormGroup({
    name: new FormControl(
      null,
      [Validators.required, Validators.minLength(3), Validators.maxLength(30), Validators.pattern('[a-zA-Z0-9]*')]
    ),
    longitude: new FormControl(
      null,
      [Validators.required, LongLatValidators.checklongitudeInterval]
    ),
    latitude: new FormControl(
      null,
      [Validators.required, LongLatValidators.checklatitudeInterval]
    )
  });

  get sdForm(){
    return this.stationDataForm.controls;
  }

  constructor(private stationService: StationHttpService) { 
    this.mapInfo = new MarkerInfo(
      new GeoLocation(this.currentLatitude, this.currentLongitude),
      "title",
      "icon",
      "label");
  }

  ngOnInit() {
    this.getAllStation();
  }

  getAllStation(){
    this.stationService.getAll().subscribe(
      (data) => {
        this.stations = data;
        this.currentStation = this.stations[0];
        this.currentSelectedStation = this.stations[0].name;
        this.stationDataForm.patchValue({
          name: this.currentStation.name,
          longitude: this.currentStation.longitude,
          latitude: this.currentStation.latitude
        });
        this.mapInfo.title = this.currentStation.name;
        this.mapInfo.location.latitude = this.currentStation.latitude;
        this.mapInfo.location.longitude = this.currentStation.longitude;
        this.stationDataForm.markAsUntouched();
      },
      (err) => {
        console.log(err);
      }
    );
  }

  sdFormPlaceMarker(event, form: FormGroup){
    this.mapInfo.location.latitude = event.coords.lat;
    this.mapInfo.location.longitude = event.coords.lng;

    let resultLocationData: any;
    let geocoder = new google.maps.Geocoder();
    let mylatLng = new google.maps.LatLng(this.mapInfo.location.latitude, this.mapInfo.location.longitude);
    let geocoderRequest = {latLng: mylatLng};
    let that = this;
    geocoder.geocode(geocoderRequest, function(result, status){
      let station = new Station();
      resultLocationData = result[0] as Array<string>;

      station.latitude = event.coords.lat;
      station.longitude = event.coords.lng;

      form.patchValue({
        longitude: station.longitude,
        latitude: station.latitude
      });
      form.markAsDirty();
      that.currentStation.longitude = station.longitude;
      that.currentStation.latitude = station.latitude;
    });
  }

  currentStationChanged(){
    this.currentStation = this.stations.find(s => s.name === this.currentSelectedStation);
    this.mapInfo.location.latitude = this.currentStation.latitude;
    this.mapInfo.location.longitude = this.currentStation.longitude;
    this.stationDataForm.patchValue({
      name: this.currentStation.name,
      longitude: this.currentStation.longitude,
      latitude: this.currentStation.latitude
    });
  }

  addStation(){
    if(this.stationDataForm.value){
      let newStation = new Station();
      newStation = this.stationDataForm.value;
      this.stationService.post(newStation).subscribe(
        (confirm) => {
          this.getAllStation();
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }

  updateStation(){
    if(this.stationDataForm.value){
      let updatedStation = new Station();
      updatedStation = this.stationDataForm.value;
      updatedStation.id = this.currentStation.id;
      this.stationService.put(updatedStation.id, updatedStation).subscribe(
        (confirm) => {
          this.getAllStation();
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }

  removeStation(){
    if(this.currentStation.id){
      this.stationService.delete(this.currentStation.id).subscribe(
        (confirm) => {
          this.getAllStation();
        },
        (err) => {
          console.log(err);
        }
      );
    }
  }

}
