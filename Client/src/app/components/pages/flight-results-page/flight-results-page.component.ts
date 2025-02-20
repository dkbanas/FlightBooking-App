import {Component, EventEmitter, Input, Output} from '@angular/core';
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from '@angular/common';
import {ActivatedRoute, Router} from '@angular/router';


@Component({
  selector: 'app-flight-results-page',
  standalone: true,
  imports: [
    DatePipe,
    CurrencyPipe,
    NgForOf,
    NgIf
  ],
  templateUrl: './flight-results-page.component.html',
  styleUrl: './flight-results-page.component.scss'
})
export class FlightResultsPageComponent {
  flights: any[] = []; // Array to store flights data

  constructor(private route: ActivatedRoute, private router: Router) {}

  ngOnInit(): void {
    // Retrieve flight data passed from the previous page (router state)
    const state = history.state;
    if (state && state.flights) {
      this.flights = state.flights;
      // Optionally log flights data for debugging purposes
      // console.log('Flights received:', this.flights);
    } else {
      // Alert the user if no flight data is available
      alert('No flights data found.');
    }
  }

  // Navigate to the flight details page, passing selected flight and passenger data
  viewDetails(flight: any): void {
    const numberOfPassengers = history.state.numberOfPassengers;
    this.router.navigate(['/flight-details-page'], { state: { flight, numberOfPassengers } });
  }
}
