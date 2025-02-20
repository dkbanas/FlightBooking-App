import {Component, EventEmitter, Output, TemplateRef, ViewChild} from '@angular/core';
import {NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ReservationService} from '../../../services/reservation.service';

@Component({
  selector: 'app-cancel-reservation-modal',
  standalone: true,
  imports: [],
  templateUrl: './cancel-reservation-modal.component.html',
  styleUrl: './cancel-reservation-modal.component.scss'
})
export class CancelReservationModalComponent {
  @ViewChild('cancelReservationModal') cancelReservationModal!: TemplateRef<any>;
  @Output() reservationCanceled = new EventEmitter<void>();
  reservationId: number | null = null;

  constructor(private modalService: NgbModal, private reservationService: ReservationService) {}


  openModal(reservationId: number): void {
    this.reservationId = reservationId;
    const modalRef = this.modalService.open(this.cancelReservationModal, { ariaLabelledBy: 'cancelReservationModalLabel' });
    modalRef.dismissed.subscribe(() => {
      this.reservationId = null;
    });
  }


  confirmCancel(): void {
    if (this.reservationId) {
      this.reservationService.cancelReservation(this.reservationId).subscribe({
        next: () => {
          this.modalService.dismissAll();
          this.reservationCanceled.emit(); // Emituje zdarzenie po anulowaniu
        },
        error: (err:any) => {
          console.error('Error canceling reservation:', err);
          alert('An error occurred while canceling the reservation. Please try again.');
        },
      });
    }
  }
}
