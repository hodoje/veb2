import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { TicketsComponent } from './components/tickets/tickets.component';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { PassengerComponent } from './components/passenger/passenger.component';
import { ControllerComponent } from './components/controller/controller.component';
import { AdminComponent } from './components/admin/admin.component';
import { LinesGridComponent } from './components/lines-grid/lines-grid.component';
import { VehiclesMapComponent } from './components/vehicles-map/vehicles-map.component';
import { LinesModificationComponent } from './components/lines-modification/lines-modification.component';
import { StationsModificationComponent } from './components/stations-modification/stations-modification.component';
import { SchedulesModificationComponent } from './components/schedules-modification/schedules-modification.component';
import { PricelistModificationComponent } from './components/pricelist-modification/pricelist-modification.component';
import { ProfileComponent } from './components/profile/profile.component';
import { UsersComponent } from './components/users/users.component';

const routes: Routes = [
  {
    path: "home",
    component: HomeComponent
  },
  {
    path: "login",
    component: LoginComponent
  },
  {
    path: "register",
    component: RegisterComponent
  },
  {
    path: "tickets",
    component: TicketsComponent
  },
  {
    path: "schedules",
    component: ScheduleComponent
  },
  {
    path: "linesGrid",
    component: LinesGridComponent
  },
  {
    path: "vehiclesMap",
    component: VehiclesMapComponent
  },
  {
    path: "passenger",
    component: PassengerComponent
  },
  {
    path: "controller",
    component: ControllerComponent
  },
  {
    path: "admin",
    component: AdminComponent,
    children: [
      {
        path: "lines",
        component: LinesModificationComponent
      },
      {
        path: "stations",
        component: StationsModificationComponent
      },
      {
        path: "schedules",
        component: SchedulesModificationComponent
      },
      {
        path: "pricelist",
        component: PricelistModificationComponent
      },
      {
        path: "profile",
        component: ProfileComponent
      },
      {
        path: "users",
        component: UsersComponent
      },
      {
        path: "**",
        component: LinesModificationComponent
      }
    ]
  },
  {
    path: "**",
    redirectTo: "home"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
