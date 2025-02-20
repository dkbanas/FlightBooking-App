import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {AirportRequest} from '../models/AirportRequest';
import {AirportResponse} from '../models/AirportResponse';
import {catchError, Observable, tap, throwError} from 'rxjs';
import {PageableAirportResponse} from '../models/PageableAirportResponse';
import {SortOrder} from '../models/SortOrder';

@Injectable({
  providedIn: 'root'
})
export class AirportService {
  private apiUrl = 'http://localhost:5111/Airport';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('accessToken');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  createAirport(airport: AirportRequest): Observable<AirportResponse> {
    return this.http.post<AirportResponse>(this.apiUrl, airport, { headers: this.getAuthHeaders() });
  }

  uploadImage(file: File, airportCode: string): Observable<{ ImageUrl: string }> {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('airportCode',airportCode);


    return this.http.post<{ ImageUrl: string }>(`${this.apiUrl}/upload-image`, formData);
  }

  getAirports(): Observable<AirportResponse[]> {
    return this.http.get<AirportResponse[]>(this.apiUrl);
  }

  getPageableAirports(
    page: number,
    pageSize: number,
    sortColumn: string,
    sortOrder: SortOrder,
    continent: string
  ): Observable<PageableAirportResponse> {
    // Build query parameters object
    const params = new HttpParams({
      fromObject: {
        Page: page.toString(),
        PageSize: pageSize.toString(),
        SortColumn: sortColumn,
        SortOrder: sortOrder.toString(),
        Continent: continent,
      }
    });

    // Log the full URL and parameters to check if everything is correct
    // console.log('Making request with URL:', `${this.apiUrl}/paginated-sorted`);
    // console.log('Query Parameters:', params.toString());

    return this.http.get<PageableAirportResponse>(`${this.apiUrl}/paginated-sorted`, { params });
  }

  searchAirports(query: string): Observable<AirportResponse[]> {
    const params = new HttpParams().set('query', query);
    return this.http.get<AirportResponse[]>(`${this.apiUrl}/search`, { params });
  }

  deleteAirport(code: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${code}`, { headers: this.getAuthHeaders() })
      .pipe(
        tap(() => console.log(`Deleted airport with code: ${code}`))
      );
  }

  updateAirport(code: string, airport: AirportRequest): Observable<AirportResponse> {
    return this.http.put<AirportResponse>(`${this.apiUrl}/${code}`, airport, { headers: this.getAuthHeaders() })
      .pipe(
        tap(response => console.log('Airport updated:', response))
      );
  }

  getAirportByCode(code: string): Observable<AirportResponse> {
    return this.http.get<AirportResponse>(`${this.apiUrl}/${code}`)
      .pipe(
        tap(airport => console.log('Fetched airport:', airport))
      );
  }
}
