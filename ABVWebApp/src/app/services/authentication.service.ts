import { AuthResponse } from './../interfaces/response/IAuthResponse';
import { RegistrationResponse } from './../interfaces/response/IRegistrationResponse';
import { UserForAuthentication } from './../interfaces/user/IUserForAuthentication';
import { UserForRegistration } from './../interfaces/user/IUserForRegistration';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { EnvironmentService } from './environment.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private authChange$ = new BehaviorSubject<boolean>(false);
  authChanged = this.authChange$.asObservable();

  constructor(
    private http: HttpClient,
    private envUrl: EnvironmentService,
    private jwtHelper: JwtHelperService
  ) {}

  registerUser(body: UserForRegistration): Observable<RegistrationResponse> {
    const route = `${this.envUrl.urlAddress}/Registration`;
    return this.http.post<RegistrationResponse>(route, body);
  }

  loginUser(body: UserForAuthentication): Observable<AuthResponse> {
    const route = `${this.envUrl.urlAddress}/Login`;
    return this.http.post<AuthResponse>(route, body);
  }

  logout(): void {
    localStorage.removeItem('token');
    this.sendAuthStateChangeNotification(false);
  }

  sendAuthStateChangeNotification(isAuthenticated: boolean): void {
    this.authChange$.next(isAuthenticated);
  }

  get isUserAuthenticated(): boolean {
    const token = localStorage.getItem('token');

    return token && !this.jwtHelper.isTokenExpired(token);
  }

  get isUserAdmin(): boolean {
    const token = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(token);
    const role =
      decodedToken[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ];

    return role === 'Administrator';
  }
}
