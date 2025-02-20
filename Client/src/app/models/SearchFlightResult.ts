import {FlightResponse} from './FlightResponse';

export interface SearchFlightResult {
  outboundFlight: {
    flightNumber: string;
    airline: string;
    departureLocation: { city: string; code: string; };
    arrivalLocation: { city: string; code: string; };
    departureTime: string;
    arrivalTime: string;
    duration: string;
    priceEconomy: number;
    availableSeatsEconomy: number;
    totalSeatsEconomy: number;
  };
  returnFlight?: {
    flightNumber: string;
    airline: string;
    departureLocation: { city: string; code: string; };
    arrivalLocation: { city: string; code: string; };
    departureTime: string;
    arrivalTime: string;
    duration: string;
    priceEconomy: number;
    availableSeatsEconomy: number;
    totalSeatsEconomy: number;
  } | null;
}
