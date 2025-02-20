import {Component, OnInit} from '@angular/core';
import {ReservationService} from '../../../services/reservation.service';
import {AuthService} from '../../../services/auth.service';
import {CurrencyPipe, DatePipe, NgForOf, NgIf} from '@angular/common';
import {
  NgbAccordionBody,
  NgbAccordionButton, NgbAccordionCollapse,
  NgbAccordionDirective,
  NgbAccordionHeader,
  NgbAccordionItem
} from '@ng-bootstrap/ng-bootstrap';
import {
  CancelReservationModalComponent
} from '../../modals/cancel-reservation-modal/cancel-reservation-modal.component';

@Component({
  selector: 'app-my-reservation-page',
  standalone: true,
  imports: [
    DatePipe,
    NgIf,
    NgForOf,
    NgbAccordionDirective,
    NgbAccordionItem,
    NgbAccordionHeader,
    NgbAccordionButton,
    NgbAccordionCollapse,
    NgbAccordionBody,
    CancelReservationModalComponent,
    CurrencyPipe
  ],
  templateUrl: './my-reservation-page.component.html',
  styleUrl: './my-reservation-page.component.scss'
})
export class MyReservationPageComponent implements OnInit {
  reservations: any[] = [];
  userId!: number;

  constructor(
    private reservationService: ReservationService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    const userId = this.authService.getUserIdFromToken();
    if (!userId) {
      console.error('User is not logged in or token is invalid');
      alert('Please log in again.');
      return;
    }
    this.userId = userId;
    this.fetchReservations();
  }

  getTotalPrice(reservation: any): number {
    return reservation.seatNumbers.length * reservation.price;
  }


  fetchReservations(): void {
    this.reservationService.getReservationsByUser(this.userId).subscribe({
      next: (data) => {
        this.reservations = data;
      },
      error: (err) => {
        console.error('Error fetching reservations:', err);
      }
    });
  }

}
