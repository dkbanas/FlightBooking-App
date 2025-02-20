export interface ReservationResponse {
  id: number;
  flightNumber: string;
  userEmail: string;
  seatNumbers: string[];
  reservationDate: string;
}
