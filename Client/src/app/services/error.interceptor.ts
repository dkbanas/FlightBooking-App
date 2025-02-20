import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpHandlerFn,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
import {catchError, Observable, switchMap, throwError} from 'rxjs';
import {inject} from '@angular/core';
import {AuthService} from './auth.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError(error => {
      if (error instanceof HttpErrorResponse && error.status === 401) {
        if (!req.url.includes('/login')) {
          return handle401Error(req, next);
        }
      }
      return throwError(() => error);
    })
  );
};

const handle401Error = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);

  return authService.refreshToken().pipe(
    switchMap(() => {
      return next(req);
    }),
    catchError(error => {
      console.error("Failed to refresh token:", error);
      authService.logout();
      return throwError(() => error);
    })
  );
};

const addToken = (request: HttpRequest<any>): HttpRequest<any> => {
  const accessToken = localStorage.getItem('accessToken');
  if (accessToken) {
    return request.clone({
      setHeaders: {
        Authorization: `Bearer ${accessToken}`
      }
    });
  }
  return request;
};
