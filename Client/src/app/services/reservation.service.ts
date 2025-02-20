import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {ReservationRequest} from '../models/ReservationRequest';
import {Observable, tap} from 'rxjs';
import {ReservationResponse} from '../models/ReservationResponse';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private apiUrl = 'http://localhost:5111/reservation';

  constructor(private http: HttpClient) {}

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem('auth_token');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  createReservation(reservation: { flightId: number; userId: number; seatNumbers: string[] }) {
    return this.http.post<ReservationResponse>(`${this.apiUrl}`, reservation, {
      headers: this.getAuthHeaders().set('Content-Type', 'application/json'),
    });
  }

  getReservationsByUser(userId: number) {
    return this.http.get<any[]>(`${this.apiUrl}/user/${userId}`);
  }

  cancelReservation(reservationId: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${reservationId}`);
  }
}
