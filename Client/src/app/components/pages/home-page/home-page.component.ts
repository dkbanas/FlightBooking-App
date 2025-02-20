import { Component } from '@angular/core';
import {NavbarComponent} from '../../partials/navbar/navbar.component';
import {FlightSearchComponent} from '../../partials/flight-search/flight-search.component';
import {CheapestFlightComponent} from '../../partials/cheapest-flight/cheapest-flight.component';
import {TipsComponent} from '../../partials/tips/tips.component';

@Component({
  selector: 'app-home-page',
  standalone: true,
  imports: [
    NavbarComponent,
    FlightSearchComponent,
    CheapestFlightComponent,
    TipsComponent
  ],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.scss'
})
export class HomePageComponent {

}
