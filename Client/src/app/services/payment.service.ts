import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'http://localhost:8080/payments/';

  constructor(private http: HttpClient) { }

  createOrder(orderDetails: any): Observable<{ approval_link: string }> {
    return this.http.post<{ approval_link: string }>(
      `${this.apiUrl}create-order`,
      orderDetails
    );
  }

  confirmPayment(token: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/confirm-payment?token=${token}`, null);
  }
}
