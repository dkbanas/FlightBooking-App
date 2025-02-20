import { Injectable } from '@angular/core';
import {BehaviorSubject, map, Observable} from 'rxjs';
import {HttpClient} from '@angular/common/http';
import {LoginResponse} from '../models/LoginResponse';
import {UserAuthRequest} from '../models/UserAuthRequest';
import {jwtDecode} from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5111/Auth';

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasValidToken());
  isLoggedIn$ = this.isLoggedInSubject.asObservable();

  private isAdminSubject = new BehaviorSubject<boolean>(this.isAdmin());
  isAdmin$ = this.isAdminSubject.asObservable();


  constructor(private http: HttpClient) {}

  register(request:UserAuthRequest): Observable<any>{
    return this.http.post(`${this.apiUrl}/register`,request);
  }

  login(request: UserAuthRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.apiUrl}/login`, request).pipe(map(response =>
      {
        localStorage.setItem('accessToken',response.access_token);
        document.cookie = `refreshToken=${response.refresh_token};`;
        this.isLoggedInSubject.next(true);
        this.isAdminSubject.next(this.isAdmin());
        return response;
      }));
  }

  refreshToken() : Observable<LoginResponse>{
    const refreshToken = this.getRefreshTokenFromCookie();
    return this.http.post<LoginResponse>(`${this.apiUrl}/refresh-token`,{refreshToken}).pipe(map(response =>
    {
      localStorage.setItem('accessToken',response.access_token);
      document.cookie = `refreshToken=${response.refresh_token};`;
      return response;
    }));
  }

  private getRefreshTokenFromCookie() : string | null {
    const cookieString = document.cookie;
    const cookieArray = cookieString.split('; ');
    for(const cookie of cookieArray) {
      const [name, value] = cookie.split('=');
      if(name == 'refreshToken') {
        return value;
      }
    }
    return null;
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    this.isLoggedInSubject.next(false);
    this.isAdminSubject.next(false);
  }

  isAdmin(): boolean {
    const token = localStorage.getItem('accessToken');
    if (!token) return false;

    try {
      const decoded: any = jwtDecode(token);
      return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] === 'Admin';
    } catch (error) {
      console.error('Error decoding token:', error);
      return false;
    }
  }

  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('accessToken');
    if (!token) return null;

    try {
      const decoded: any = jwtDecode(token);
      console.log('Decoded token:', decoded);

      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      return userId ? Number(userId) : null;
    } catch (error) {
      console.error('Error decoding token:', error);
      return null;
    }
  }

  private hasValidToken(): boolean {
    const token = localStorage.getItem('accessToken');
    return !!token;
  }

}
