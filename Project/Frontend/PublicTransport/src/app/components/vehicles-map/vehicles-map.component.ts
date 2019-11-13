import { Component, OnInit, OnDestroy, NgZone } from '@angular/core';
import { MarkerInfo } from 'src/app/models/map-models/marker-info.model';
import { Subscription } from 'rxjs';
import { SimulatorHubService } from 'src/app/services/simulator-hub.service';

@Component({
  selector: 'app-vehicles-map',
  templateUrl: './vehicles-map.component.html',
  styleUrls: ['./vehicles-map.component.scss']
})
export class VehiclesMapComponent implements OnInit, OnDestroy {
  
  currentLatitude: number = 45.2520547;
  currentLongitude: number = 19.8326543; 
  currentPositions: MarkerInfo[] = [];
  isConnected: Boolean;

  ngOnDestroy(): void {
    this.stopSimulatorHubServiceConnection();
  }
  
  constructor(private simulatorHubService: SimulatorHubService, private ngZone: NgZone) { }

  ngOnInit() {
    this.startSimulatorHubServiceConnection();
    this.checkConnection();
    this.subscribeForNewVehiclePositions();
  }

  private startSimulatorHubServiceConnection() {
    this.simulatorHubService.startConnection();
  }

  private stopSimulatorHubServiceConnection() {
    this.simulatorHubService.stopConnection();
  }
  
  private checkConnection() {
    this.simulatorHubService.connectionEstablishedEventEmmiter.subscribe(
      (connectionStatus) => {
        this.isConnected = connectionStatus;
        this.simulatorHubService.startSimulation();
      }
    );
  }
  
  private subscribeForNewVehiclePositions(){
    this.simulatorHubService.newVehiclesPositionNotificationEventEmitter.subscribe(
      (e) => {
        this.onNewVehiclesPositions(e);
      }
    );
  }

  public onNewVehiclesPositions(newMarkers: MarkerInfo[]) {
    this.ngZone.run(() => {
      console.log(JSON.stringify(newMarkers));
      this.currentPositions = newMarkers;
    });
  }
}
