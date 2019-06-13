import { PricelistHttpService } from './services/pricelist-http.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { NgxSpinnerModule } from 'ngx-spinner';
import { AgmCoreModule } from '@agm/core';

import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from 'src/app/interceptors/token.interceptor';
import { LoginComponent } from './components/login/login.component';
import { AccountHttpService } from './services/account-http.service';
import { PLTTService } from './services/price-list-ticket-type-http.service';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RegisterComponent } from './components/register/register.component';
import { PassengerComponent } from './components/passenger/passenger.component';
import { ControllerComponent } from './components/controller/controller.component';
import { AdminComponent } from './components/admin/admin.component';
import { LoginToNavbarService } from './services/login-to-navbar.service';
import { TicketsComponent } from './components/tickets/tickets.component';
import { PricelistComponent } from './components/pricelist/pricelist.component';
import { PurchaseComponent } from './components/purchase/purchase.component';
import { SchedulesHttpService } from './services/schedules-http.service';
import { ScheduleComponent } from './components/schedule/schedule.component';
import { TransportationLinesHttpService } from './services/transportation-lines-http.service';
import { TransportationLineTypesHttpService } from './services/transportation-line-types-http.service';
import { RegistrationHttpService } from './services/registration-http.service';
import { TicketHttpService } from './services/ticket-http.service';
import { UserTypeHttpService } from './services/user-type-http.service';
import { DayOfTheWeekHttpService } from './services/day-of-the-week-http.service';
import { LinesGridComponent } from './components/lines-grid/lines-grid.component';
import { VehiclesMapComponent } from './components/vehicles-map/vehicles-map.component';
import { PricelistModificationComponent } from './components/pricelist-modification/pricelist-modification.component';
import { LinesModificationComponent } from './components/lines-modification/lines-modification.component';
import { StationsModificationComponent } from './components/stations-modification/stations-modification.component';
import { SchedulesModificationComponent } from './components/schedules-modification/schedules-modification.component';
import { ProfileComponent } from './components/profile/profile.component';
import { UsersComponent } from './components/users/users.component';
import { PasswordEqualValidatorDirective } from './common/directives/password-equal-validator.directive';
import { PasswordPatternValidatorDirective } from './common/directives/password-pattern-validator.directive';
import { ImageHttpService } from './services/image-http.service';
import { UserConfirmationService } from './services/user-confirmation.service';
import { UserHttpService } from './services/user-http.service';
import { UserComponent } from './components/user/user.component';
import { EmailExistsValidatorDirective } from './common/directives/email-exists-validator.directive';
import { SimulationService } from './services/simulator.service';

@NgModule({
  declarations:[
    AppComponent,
    LoginComponent,
    HomeComponent,
    NavbarComponent,
    RegisterComponent,
    PassengerComponent,
    ControllerComponent,
    AdminComponent,    
    TicketsComponent,
    PricelistComponent,
    PurchaseComponent,
    ScheduleComponent,
    LinesGridComponent,
    VehiclesMapComponent,
    PricelistModificationComponent,
    LinesModificationComponent,
    StationsModificationComponent,
    SchedulesModificationComponent,
    ProfileComponent,
    UsersComponent,
    PasswordEqualValidatorDirective,
    PasswordPatternValidatorDirective,
    UserComponent,
    EmailExistsValidatorDirective
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MDBBootstrapModule.forRoot(),
    NgxSpinnerModule,
    AgmCoreModule.forRoot({apiKey: 'AIzaSyDnihJyw_34z5S1KZXp90pfTGAqhFszNJk'})
  ],
  providers: 
  [
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},
    AccountHttpService,
    PLTTService,
    LoginToNavbarService,
    SchedulesHttpService,
    TransportationLinesHttpService,
    TransportationLineTypesHttpService,
    TicketHttpService,
    DayOfTheWeekHttpService, 
    RegistrationHttpService,
    UserTypeHttpService,
    PricelistHttpService,
    ImageHttpService,
    UserConfirmationService,
    UserHttpService,
    SimulationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
