import { Routes } from '@angular/router';
import {HomePageComponent} from './components/pages/home-page/home-page.component';
import {ManagementPageComponent} from './components/pages/management-page/management-page.component';
import {AirportPageComponent} from './components/pages/airport-page/airport-page.component';
import {FlightResultsPageComponent} from './components/pages/flight-results-page/flight-results-page.component';
import {FlightDetailsPageComponent} from './components/pages/flight-details-page/flight-details-page.component';
import {MyReservationPageComponent} from './components/pages/my-reservation-page/my-reservation-page.component';
import {CompleteOrderPageComponent} from './components/pages/complete-order-page/complete-order-page.component';

export const routes: Routes = [
  {path:'',component:HomePageComponent},
  {path:'management',component:ManagementPageComponent},
  {path:'airport',component:AirportPageComponent},
  {path:'flight-results-page',component:FlightResultsPageComponent},
  {path:'flight-details-page',component:FlightDetailsPageComponent},
  {path:'reservation',component:MyReservationPageComponent},
  {path:'complete-order',component:CompleteOrderPageComponent}
];
