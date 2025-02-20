import {AirportResponse} from './AirportResponse';

export interface PageableAirportResponse {
  items: AirportResponse[];
  page: number;
  pageSize: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}
