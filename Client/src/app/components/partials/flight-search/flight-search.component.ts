import {Component, ViewChild} from '@angular/core';
import {NgClass, NgForOf, NgIf} from '@angular/common';
import {NgbPopover} from '@ng-bootstrap/ng-bootstrap';
import {FormsModule} from '@angular/forms';
import {debounceTime, Subject, switchMap} from 'rxjs';
import {AirportService} from '../../../services/airport.service';
import {FlightService} from '../../../services/flight.service';
import {HttpClient} from '@angular/common/http';
import {Router} from '@angular/router';

@Component({
  selector: 'app-flight-search',
  standalone: true,
  imports: [
    NgIf,
    NgbPopover,
    NgClass,
    FormsModule,
    NgForOf
  ],
  templateUrl: './flight-search.component.html',
  styleUrl: './flight-search.component.scss'
})
export class FlightSearchComponent {
  roundTrip = true; // Indicates if the user is searching for a round trip
  passengersVisible = false; // Toggles the visibility of the passenger selection UI
  adults = 1; // Default number of adults
  children = 0; // Default number of children

  departureAirportInput: string = '';  // User input for departure airport
  arrivalAirportInput: string = '';    // User input for arrival airport
  departureDate: string = '';          // Selected departure date
  returnDate: string = '';             // Selected return date (if round trip)

  filteredFromAirports: any[] = [];    // Stores the list of filtered airports for departure
  filteredToAirports: any[] = [];      // Stores the list of filtered airports for arrival

  private departureAirportId: number | null = null;  // ID of the selected departure airport
  private arrivalAirportId: number | null = null;    // ID of the selected arrival airport

  private fromSubject = new Subject<string>(); // Subject to debounce the departure airport search
  private toSubject = new Subject<string>();   // Subject to debounce the arrival airport search

  constructor(
    private airportService: AirportService, // Service to search airports
    private flightService: FlightService,   // Service to search flights
    private router: Router,                 // Router for navigation
    private http: HttpClient                // HTTP Client for API requests
  ) {
    // Set up debounced search for departure airport
    this.fromSubject.pipe(
      debounceTime(300), // Wait for 300ms of inactivity before triggering the search
      switchMap(value => this.airportService.searchAirports(value)) // Fetch airports based on input
    ).subscribe(airports => this.filteredFromAirports = airports);

    // Set up debounced search for arrival airport
    this.toSubject.pipe(
      debounceTime(300),
      switchMap(value => this.airportService.searchAirports(value))
    ).subscribe(airports => this.filteredToAirports = airports);
  }

  /**
   * Returns a formatted summary of the number of passengers
   * @returns {string} - Summary of passengers in a readable format
   */
  get passengerSummary(): string {
    return `${this.adults} Adult${this.adults !== 1 ? 's' : ''}, ${this.children} Child${this.children !== 1 ? 'ren' : ''}`;
  }

  /**
   * Toggles the visibility of the passengers selection UI
   */
  togglePassengers() {
    this.passengersVisible = !this.passengersVisible;
  }

  /**
   * Increments the number of passengers for a given type (adults or children)
   * @param type - 'adults' or 'children' to specify which type to increment
   */
  increment(type: 'adults' | 'children') {
    if (type === 'adults') this.adults++;
    else this.children++;
  }

  /**
   * Decrements the number of passengers for a given type (adults or children)
   * @param type - 'adults' or 'children' to specify which type to decrement
   */
  decrement(type: 'adults' | 'children') {
    if (type === 'adults' && this.adults > 1) this.adults--;
    else if (type === 'children' && this.children > 0) this.children--;
  }

  /**
   * Handles changes in the departure airport input field, triggers search if necessary
   */
  onFromInputChange() {
    this.handleInputChange(this.departureAirportInput, 'from');
  }

  /**
   * Handles changes in the arrival airport input field, triggers search if necessary
   */
  onToInputChange() {
    this.handleInputChange(this.arrivalAirportInput, 'to');
  }

  /**
   * Handles changes in airport input fields, triggers search or clears filters
   * @param input - The input string entered by the user
   * @param inputType - 'from' or 'to' to specify which airport field was modified
   */
  private handleInputChange(input: string, inputType: 'from' | 'to') {
    if (input.length >= 3) {
      if (inputType === 'from') {
        this.fromSubject.next(input);
      } else {
        this.toSubject.next(input);
      }
    } else {
      // Clear the relevant airport list and ID if input length is less than 3
      if (inputType === 'from') {
        this.filteredFromAirports = [];
        this.departureAirportId = null;
      } else {
        this.filteredToAirports = [];
        this.arrivalAirportId = null;
      }
    }
  }

  /**
   * Handles airport selection from the filtered list and updates the corresponding input field
   * @param airport - The selected airport object
   * @param inputType - 'from' or 'to' to specify which airport is being selected
   */
  selectAirport(airport: any, inputType: 'from' | 'to') {
    if (airport && airport.id) {
      if (inputType === 'from') {
        this.departureAirportId = Number(airport.id);
        this.departureAirportInput = `${airport.name} - ${airport.city} - ${airport.country}`;
      } else {
        this.arrivalAirportId = Number(airport.id);
        this.arrivalAirportInput = `${airport.name} - ${airport.city} - ${airport.country}`;
      }
    } else {
      alert('Please select a valid airport from the suggestions.');
    }
    // Clear airport filters after selection
    this.filteredFromAirports = [];
    this.filteredToAirports = [];
  }

  /**
   * Validates and initiates the flight search
   */
  searchFlights() {
    // Validation for required fields before performing the search
    if (!this.departureAirportId || !this.arrivalAirportId) {
      alert('Please select valid departure and destination airports.');
      return;
    }
    if (!this.departureDate) {
      alert('Please select a valid departure date.');
      return;
    }

    const departureLocationId = this.departureAirportId;
    const arrivalLocationId = this.arrivalAirportId;
    const numberOfPassengers = this.adults + this.children;
    const roundTrip = this.roundTrip;
    const returnDate = roundTrip ? this.returnDate : '';

    // Validate return date for round trips
    if (roundTrip && !this.returnDate) {
      alert('Please select a valid return date for round trip.');
      return;
    }

    // Ensure return date is not set for one-way trips
    if (!roundTrip && this.returnDate) {
      alert('Return date should be empty for one-way trips.');
      return;
    }

    const formattedDepartureDate = new Date(this.departureDate).toISOString();
    const formattedReturnDate = roundTrip && this.returnDate ? new Date(this.returnDate).toISOString() : '';

    // Initiate flight search
    this.flightService.searchFlights(
      departureLocationId,
      arrivalLocationId,
      formattedDepartureDate,
      formattedReturnDate,
      numberOfPassengers,
      roundTrip
    ).subscribe({
      next: (flights) => {
        if (flights.length === 0) {
          alert('No flights found for the selected criteria. Please choose another date.');
        } else {
          this.router.navigate(['/flight-results-page'], { state: { flights, numberOfPassengers } });
        }
      },
      error: (error) => {
        console.error('Error fetching flight data', error);
        alert('There was an error fetching flight data. Please try again later.');
      }
    });
  }
}
