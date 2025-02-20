import { Component } from '@angular/core';
import {FlightResponse} from '../../../models/FlightResponse';
import {FlightService} from '../../../services/flight.service';
import {Router} from '@angular/router';
import {CurrencyPipe, DatePipe, NgClass, NgForOf, NgIf} from '@angular/common';
import {NgbCarousel, NgbSlide} from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-cheapest-flight',
  standalone: true,
  imports: [
    CurrencyPipe,
    NgForOf,
    DatePipe,
    NgbCarousel,
    NgbSlide,
    NgIf,
    NgClass
  ],
  templateUrl: './cheapest-flight.component.html',
  styleUrl: './cheapest-flight.component.scss'
})
export class CheapestFlightComponent {
  topFlights: any[] = [];

  constructor(private flightService: FlightService, private router: Router) {}

  ngOnInit(): void {
    this.flightService.getTop5CheapestFlights().subscribe((flights) => {
      this.topFlights = flights;
    });
  }

}
