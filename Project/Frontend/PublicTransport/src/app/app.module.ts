import { ConfirmPasswordValidatorDirective } from './common/directives/confirm-password-validator.directive';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { MDBBootstrapModule } from 'angular-bootstrap-md';

import { AppComponent } from './app.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { TokenInterceptor } from 'src/app/interceptors/token.interceptor';
import { LoginComponent } from './components/login/login.component';
import { AuthHttpService } from './services/auth-http.service';
import { PLTTService } from './services/price-list-ticket-type-http.service';
import { HomeComponent } from './components/home/home.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { ContentComponent } from './components/content/content.component';
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
import { DayOfTheWeekHttpService } from './services/day-of-the-week-http.service';
import { UserTypeHttpService } from './services/user-type-http.service';
import { RegistrationHttpService } from './services/registration-http.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    HomeComponent,
    NavbarComponent,
    ContentComponent,
    RegisterComponent,
    PassengerComponent,
    ControllerComponent,
    AdminComponent,
    ConfirmPasswordValidatorDirective,
    TicketsComponent,
    PricelistComponent,
    PurchaseComponent,
    ScheduleComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MDBBootstrapModule.forRoot()
  ],
  providers: 
  [
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},
    AuthHttpService,
    PLTTService,
    LoginToNavbarService,
    SchedulesHttpService,
    TransportationLinesHttpService,
    TransportationLineTypesHttpService,
    DayOfTheWeekHttpService, 
    RegistrationHttpService,
    UserTypeHttpService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
