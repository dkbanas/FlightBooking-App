import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable, tap} from 'rxjs';
import {FlightRequest} from '../models/FlightRequest';
import {FlightResponse} from '../models/FlightResponse';
import {PageableFlightResponse} from '../models/PageableFlightResponse';
import {SortOrder} from '../models/SortOrder';
import {PageableAirportResponse} from '../models/PageableAirportResponse';

@Injectable({
  providedIn: 'root'
})
export class FlightService {
  private apiUrl = 'http://localhost:5111/Flight'

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  createFlight(flight: FlightRequest): Observable<FlightResponse> {
    return this.http.post<FlightResponse>(this.apiUrl, flight, { headers: this.getAuthHeaders() })
      .pipe(
        tap(response => console.log('Flight created:', response))
      );
  }

  getFlights(): Observable<FlightResponse[]> {
    return this.http.get<FlightResponse[]>(this.apiUrl)
      .pipe(
        tap(airports => console.log('Fetched flights:', airports))
      );
  }

  getPaginatedAndSortedFlights(
    page: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: SortOrder
  ): Observable<PageableFlightResponse> {
    const params = new HttpParams({
      fromObject: {
        Page: page.toString(),
        PageSize: pageSize.toString(),
        SortColumn: sortColumn,
        SortOrder: sortOrder.toString(),
      }
    });

    // Log the full URL and parameters to check if everything is correct
    // console.log('Making request with URL:', `${this.apiUrl}/paginated-sorted`);
    // console.log('Query Parameters:', params.toString());

    return this.http.get<PageableFlightResponse>(`${this.apiUrl}/paginated-sorted`, { params });
  }

  deleteFlight(code: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${code}`, { headers: this.getAuthHeaders() })
      .pipe(
        tap(() => console.log(`Deleted flight with code: ${code}`))
      );
  }

  updateFlight(code: string, flight: FlightRequest): Observable<FlightResponse> {
    return this.http.put<FlightResponse>(`${this.apiUrl}/${code}`, flight, { headers: this.getAuthHeaders() })
      .pipe(
        tap(response => console.log('Flight updated:', response))
      );
  }

  getFlightByCode(code: string): Observable<FlightResponse> {
    return this.http.get<FlightResponse>(`${this.apiUrl}/${code}`)
      .pipe(
        tap(airport => console.log('Fetched flight:', airport))
      );
  }

  searchFlights(
    departureLocationId: number,
    arrivalLocationId: number,
    departureDate: string,
    returnDate: string,
    numberOfPassengers: number,
    roundTrip: boolean
  ): Observable<any[]> {
    const params = new HttpParams()
      .set('departureLocationId', departureLocationId.toString())
      .set('arrivalLocationId', arrivalLocationId.toString())
      .set('departureDate', departureDate)
      .set('returnDate', roundTrip ? returnDate : '')
      .set('numberOfPassengers', numberOfPassengers.toString())
      .set('roundTrip', roundTrip.toString());

    // console.log('HTTP Request Params:', params.toString());

    return this.http.get<any[]>(`${this.apiUrl}/search`, { params }).pipe(
      tap(flights => console.log('Fetched flights:', flights))
    );
  }

  getTop5CheapestFlights(): Observable<any> {
    return this.http.get(`${this.apiUrl}/top5-cheapest`);
  }
}
