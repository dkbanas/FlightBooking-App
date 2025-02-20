import { Component } from '@angular/core';
import {AirportResponse} from '../../../models/AirportResponse';
import {AirportService} from '../../../services/airport.service';
import {FormsModule} from '@angular/forms';
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from '@angular/common';
import {NgbAccordionBody, NgbAccordionHeader, NgbPagination} from '@ng-bootstrap/ng-bootstrap';
import {AddAirportModalComponent} from '../../modals/add-airport-modal/add-airport-modal.component';
import {AddFlightModalComponent} from '../../modals/add-flight-modal/add-flight-modal.component';
import {FlightResponse} from '../../../models/FlightResponse';
import {FlightService} from '../../../services/flight.service';
import {PageableAirportResponse} from '../../../models/PageableAirportResponse';
import {PageableFlightResponse} from '../../../models/PageableFlightResponse';
import {SortOrder} from '../../../models/SortOrder';

@Component({
  selector: 'app-management-page',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    DatePipe,
    NgIf,
    AddAirportModalComponent,
    AddFlightModalComponent,
    CurrencyPipe,
    NgbPagination
  ],
  templateUrl: './management-page.component.html',
  styleUrl: './management-page.component.scss'
})
export class ManagementPageComponent {
  /////////////////////////////////
  airports: AirportResponse[] = [];
  currentPage = 1;
  pageSize = 6;
  totalPages = 0;
  totalElements = 0;
  airportSort: string = 'name,Ascending';
  pageSizeOptions = [6, 12, 24, 48];

  flights: FlightResponse[] = [];
  currentFlightPage = 1;
  flightPageSize = 6;
  totalFlightPages = 0;
  totalFlightElements = 0;
  flightPageSizeOptions = [6, 12, 24, 48];
  flightSort: string = 'DepartureTime,Ascending';

  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;
  /////////////////////////////////////
  constructor(private airportService: AirportService,private flightService:FlightService) {}

  ngOnInit(): void {
    this.loadAirports();
    this.loadFlights();
  }
  /////////////////////////////////////
  get currentStartIndex(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get currentEndIndex(): number {
    const endIndex = this.currentPage * this.pageSize;
    return Math.min(endIndex, this.totalElements);
  }

  get flightStartIndex(): number {
    return (this.currentFlightPage - 1) * this.flightPageSize + 1;
  }

  get flightEndIndex(): number {
    const endIndex = this.currentFlightPage * this.flightPageSize;
    return Math.min(endIndex, this.totalFlightElements);
  }
  /////////////////////////////////////
  getSortParams(sortValue: string): { sortBy: string, sortOrder: SortOrder } {
    const [sortBy, sortOrder] = sortValue.split(',');
    return { sortBy, sortOrder: sortOrder === 'Ascending' ? SortOrder.Ascending : SortOrder.Descending };
  }

  loadAirports(): void {
    const { sortBy, sortOrder } = this.getSortParams(this.airportSort);

    this.airportService.getPageableAirports(this.currentPage, this.pageSize, sortBy, sortOrder, '').subscribe({
      next: (response: PageableAirportResponse) => {
        this.airports = response.items;
        this.totalPages = Math.ceil(response.totalCount / this.pageSize);
        this.totalElements = response.totalCount;
        this.hasPreviousPage = response.hasPreviousPage;
        this.hasNextPage = response.hasNextPage;
      },
      error: (error) => console.error('Error loading airports:', error),
    });
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadAirports();
  }

  onSortChange(): void {
    this.currentPage = 1;
    this.loadAirports();
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.loadAirports();
  }
  /////////////////////////////////////
  loadFlights(): void {
    const { sortBy, sortOrder } = this.getSortParams(this.flightSort);

    this.flightService.getPaginatedAndSortedFlights(this.currentFlightPage, this.flightPageSize, sortBy, sortOrder).subscribe({
      next: (response: PageableFlightResponse) => {
        this.flights = response.items;
        this.totalFlightPages = Math.ceil(response.totalCount / this.flightPageSize);
        this.totalFlightElements = response.totalCount;
        this.hasPreviousPage = response.hasPreviousPage;
        this.hasNextPage = response.hasNextPage;
      },
      error: (error) => console.error('Error loading flights:', error),
    });
  }


  onFlightPageSizeChange(): void {
    this.currentFlightPage = 1;
    this.loadFlights();
  }

  onFlightSortChange(): void {
    this.currentFlightPage = 1;
    this.loadFlights();
  }

  onFlightPageChange(newPage: number): void {
    this.currentFlightPage = newPage;
    this.loadFlights();
  }
  /////////////////////////////////////

  deleteAirport(code: string): void {
    if (confirm('Are you sure you want to delete this airport?')) {
      this.airportService.deleteAirport(code).subscribe({
        next: () => {

          this.airports = this.airports.filter(airport => airport.code !== code);
          console.log('Airport deleted:', code);
          this.currentPage = 1;
          this.loadAirports();
        },
        error: (error) => {
          console.error('Error deleting airport:', error);
        }
      });
    }
  }

  deleteFlight(flightNumber: string): void {
    if (confirm('Are you sure you want to delete this flight?')) {
      this.flightService.deleteFlight(flightNumber).subscribe({
        next: () => {

          this.flights = this.flights.filter(flight => flight.flightNumber !== flightNumber);
          console.log('Flight deleted:', flightNumber);
          this.currentFlightPage = 1;
          this.loadFlights();
        },
        error: (error) => {
          console.error('Error deleting flight:', error);
        }
      });
    }
  }

  protected readonly SortOrder = SortOrder;
}
