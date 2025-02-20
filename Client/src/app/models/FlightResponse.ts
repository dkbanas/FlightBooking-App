import {AirportResponse} from './AirportResponse';

export interface FlightResponse{
  id: number
  flightNumber: string
  departureLocation: AirportResponse
  arrivalLocation: AirportResponse
  departureTime: string
  arrivalTime: string
  price: number
  totalSeats: number
  duration: string
  airline: string
  availableSeats: number
  createdAt: string
}
