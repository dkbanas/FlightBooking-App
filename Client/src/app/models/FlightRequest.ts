
export interface FlightRequest {
  flightNumber: string;
  departureLocationId: number;
  arrivalLocationId: number;
  departureTime: Date;
  arrivalTime: Date;
  price: number;
  totalSeats: number;
  airline: string;
}
