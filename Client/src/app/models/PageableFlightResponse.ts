
import {FlightResponse} from './FlightResponse';

export interface PageableFlightResponse {
  items: FlightResponse[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
