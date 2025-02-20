import {Component, OnInit} from '@angular/core';
import {AirportService} from '../../../services/airport.service';
import {FormsModule} from '@angular/forms';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {NgbPagination} from '@ng-bootstrap/ng-bootstrap';
import {PageableAirportResponse} from '../../../models/PageableAirportResponse';
import {SortOrder} from '../../../models/SortOrder';

@Component({
  selector: 'app-airport-page',
  standalone: true,
  imports: [
    FormsModule,
    NgForOf,
    NgIf,
    NgbPagination
  ],
  templateUrl: './airport-page.component.html',
  styleUrl: './airport-page.component.scss'
})
export class AirportPageComponent implements OnInit {
  airports: any[] = [];
  currentPage = 1;
  pageSize = 6;
  totalPages = 0;
  totalElements = 0;

  sortColumn: string = 'name';
  sortOrder: SortOrder = SortOrder.Ascending;

  pageSizeOptions = [6, 12, 24, 48];

  continents: string[] = ['World', 'North America', 'South America', 'Europe', 'Asia', 'Australia', 'Africa'];
  selectedContinent: string = 'World';

  constructor(private airportService: AirportService) {}

  ngOnInit(): void {
    this.loadAirports();
  }

  get currentStartIndex(): number {
    return (this.currentPage - 1) * this.pageSize + 1;
  }

  get currentEndIndex(): number {
    const end = this.currentPage * this.pageSize;
    return end > this.totalElements ? this.totalElements : end;
  }

  onCategoryChange(continent: string): void {
    this.selectedContinent = continent;
    this.currentPage = 1;
    this.loadAirports();
  }

  loadAirports(): void {
    const continentParam = this.selectedContinent === 'World' ? '' : this.selectedContinent;


    // Use sortColumn and sortOrder instead of sortBy
    this.airportService.getPageableAirports(
      this.currentPage,
      this.pageSize,
      this.sortColumn,
      this.sortOrder,
      continentParam
    ).subscribe({
      next: (response: PageableAirportResponse) => {
        this.airports = response.items;
        this.totalPages = Math.ceil(response.totalCount / this.pageSize);
        this.totalElements = response.totalCount;
      },
      error: (error) => console.error('Error loading airports:', error)
    });
  }

  onPageSizeChange(): void {
    this.currentPage = 1;
    this.loadAirports();
  }

  onSortChange(): void {
    // Split the sortColumn value into the column and order parts
    const [column, order] = this.sortColumn.split(',');

    // Set the sort column and order based on the selected value
    this.sortColumn = column;
    this.sortOrder = order === 'Descending' ? SortOrder.Descending : SortOrder.Ascending;

    // Reset to the first page when sorting changes
    this.currentPage = 1;

    // Reload the airports with the updated sorting parameters
    this.loadAirports();
  }

  onPageChange(newPage: number): void {
    this.currentPage = newPage;
    this.loadAirports();
  }
}
