import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { MarkerInfo } from 'src/app/models/map-models/marker-info.model';
import { Subscription } from 'rxjs';
import { SimulationService } from 'src/app/services/simulator.service';

@Component({
  selector: 'app-vehicles-map',
  templateUrl: './vehicles-map.component.html',
  styleUrls: ['./vehicles-map.component.scss']
})
export class VehiclesMapComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
    this.subscriptions = [];

    this.simulatorService.disconnect();
  }

  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543;

  currentPositions: MarkerInfo[] = [];

  subscriptions: Subscription[] = [];

  constructor(private simulatorService: SimulationService, private ngZone: NgZone) { }

  ngOnInit() {
    this.checkConnection();
    this.simulatorService.resetEmitters();
    this.subscribeForVehiclePositions();

    this.simulatorService.registerForVehiclePositions();
  }

  subscribeForVehiclePositions(){
    this.subscriptions.push(this.simulatorService.vehicleNotification.subscribe(e => this.moveVehicles(e)));
  }

  private checkConnection() {
    this.simulatorService.startConnection().subscribe(
      (connectionStatus) =>{
        if (connectionStatus) {
          this.simulatorService.createEvent();
          console.log("CONNECTED");
        }
      }
    );
  }

  moveVehicles(newMarkers: MarkerInfo[]) {
    this.ngZone.run(() => {
      console.log(JSON.stringify(newMarkers));
      this.currentPositions = newMarkers;
    });
  }
}
