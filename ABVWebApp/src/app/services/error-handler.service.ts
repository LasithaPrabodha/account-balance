import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable({ providedIn: 'root' })
export class ErrorHandlerService implements HttpInterceptor {
  constructor(private router: Router) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        const errorMessage = this.handleError(error);
        return throwError(errorMessage);
      })
    );
  }

  private handleError(error: HttpErrorResponse): string {
    switch (error.status) {
      case 404:
        return this.handleNotFound(error);
      case 400:
        return this.handleBadRequest(error);
      case 401:
        return this.handleUnauthorized(error);
      case 403:
        return this.handleForbidden(error);

      default:
        break;
    }
  }

  private handleForbidden(error: HttpErrorResponse): string {
    this.router.navigate(['/forbidden'], {
      queryParams: { returnUrl: this.router.url },
    });
    return 'Forbidden';
  }

  private handleUnauthorized(error: HttpErrorResponse): string {
    if (this.router.url === '/auth/login') {
      return 'Authenitcation failed. Wrong Username or Password';
    } else {
      this.router.navigate(['/auth/login'], {
        queryParams: { returnUrl: this.router.url },
      });
      return error.message;
    }
  }

  private handleNotFound(error: HttpErrorResponse): string {
    this.router.navigate(['/404']);
    return error.message;
  }

  private handleBadRequest(error: HttpErrorResponse): string {
    if (this.router.url === '/auth/register') {
      let message = '';
      const values = Object.values(error.error.errors);
      values.map((m: string) => {
        message += m + '<br>';
      });

      return message.slice(0, -4);
    } else {
      return error.error || error.message;
    }
  }
}
