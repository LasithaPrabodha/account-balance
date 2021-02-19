import { AuthResponse } from '../interfaces/auth/IAuthResponse';
import { RegistrationResponse } from '../interfaces/auth/IRegistrationResponse';
import { UserForAuthentication } from './../interfaces/user/IUserForAuthentication';
import { UserForRegistration } from './../interfaces/user/IUserForRegistration';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private authChange$ = new BehaviorSubject<boolean>(false);
    authChanged = this.authChange$.asObservable();

    constructor(
        private http: HttpClient,
        private jwtHelper: JwtHelperService,
        @Inject('BASE_API_URL') private baseUrl: string
    ) {}

    registerUser(body: UserForRegistration): Observable<RegistrationResponse> {
        const route = `${this.baseUrl}/api/auth/registration`;
        return this.http.post<RegistrationResponse>(route, body);
    }

    loginUser(body: UserForAuthentication): Observable<AuthResponse> {
        const route = `${this.baseUrl}/api/auth/login`;
        return this.http.post<AuthResponse>(route, body).pipe(
            map((response: AuthResponse) => {
                const token = response.accessToken;
                const refreshToken = response.refreshToken;
                localStorage.setItem('accessToken', token);
                localStorage.setItem('refreshToken', refreshToken);

                return response;
            })
        );
    }

    logout(): void {
        localStorage.removeItem('accessToken');
        localStorage.removeItem('refreshToken');
        this.sendAuthStateChangeNotification(false);
    }

    sendAuthStateChangeNotification(isAuthenticated: boolean): void {
        this.authChange$.next(isAuthenticated);
    }

    getNewRefreshToken(): Observable<AuthResponse> {
        const accessToken = localStorage.getItem('accessToken');
        const refreshToken = localStorage.getItem('refreshToken');

        const route = `${this.baseUrl}/api/token/refresh`;

        return this.http
            .post<AuthResponse>(route, { accessToken, refreshToken })
            .pipe(
                map((result: AuthResponse) => {
                    if (result && result.isAuthSuccessful) {
                        this.sendAuthStateChangeNotification(true);
                        localStorage.setItem('accessToken', result.accessToken);
                        localStorage.setItem('refreshToken', result.refreshToken);
                    }

                    return result;
                })
            );
    }
    get isUserAuthenticated(): boolean {
        const accessToken = localStorage.getItem('accessToken');
        const refreshToken = localStorage.getItem('refreshToken');

        return accessToken != null && refreshToken != null;
    }

    get isUserAdmin(): boolean {
        const token = localStorage.getItem('accessToken');
        const decodedToken = this.jwtHelper.decodeToken(token);
        const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

        return role === 'Administrator';
    }
}
