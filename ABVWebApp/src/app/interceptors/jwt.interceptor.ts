import { Injectable } from '@angular/core';
import {
    HttpInterceptor,
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpErrorResponse,
} from '@angular/common/http';
import { Observable, BehaviorSubject, throwError, of } from 'rxjs';
import { catchError, switchMap, finalize, filter, take } from 'rxjs/operators';
import { AuthenticationService } from '../services/authentication.service';
import { AuthResponse } from '../interfaces/auth/IAuthResponse';

@Injectable({ providedIn: 'root' })
export class JwtInterceptor implements HttpInterceptor {
    private isTokenRefreshing: boolean = false;

    tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>(null);

    constructor(private authService: AuthenticationService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(
            catchError(
                (err): Observable<any> => {
                    if (err instanceof HttpErrorResponse) {
                        switch ((<HttpErrorResponse>err).status) {
                            case 401:
                                console.log('Token expired. Attempting refresh ...');
                                return this.handleHttpResponseError(request, next);
                        }
                    } else {
                        return throwError(err.message);
                    }
                }
            )
        );
    }

    private handleHttpResponseError(request: HttpRequest<any>, next: HttpHandler) {
        const addTokenAndGetRequest = () => {
            return request.clone({
                setHeaders: {
                    Authorization: `Bearer ${localStorage.getItem('accessToken')}`,
                },
            });
        };
        if (!this.isTokenRefreshing) {
            this.isTokenRefreshing = true;

            this.tokenSubject.next(null);

            /// call the API to refresh the token
            return this.authService.getNewRefreshToken().pipe(
                switchMap((tokenresponse: AuthResponse) => {
                    if (tokenresponse) {
                        this.tokenSubject.next(tokenresponse.accessToken);

                        console.log('Token refreshed...');
                        return next.handle(addTokenAndGetRequest());
                    }
                    return <any>this.authService.logout();
                }),
                catchError((err) => {
                    this.authService.logout();
                    return throwError(err.message);
                }),
                finalize(() => {
                    this.isTokenRefreshing = false;
                })
            );
        } else {
            this.isTokenRefreshing = false;
            return this.tokenSubject.pipe(
                filter((token) => token != null),
                take(1),
                switchMap((token) => next.handle(addTokenAndGetRequest()))
            );
        }
    }
}
