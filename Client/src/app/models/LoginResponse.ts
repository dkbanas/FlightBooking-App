export interface LoginResponse {
  token_type:string;
  access_token: string;
  refresh_token:string;
  token_expiration: number;
  refresh_token_expiration: number;
}
