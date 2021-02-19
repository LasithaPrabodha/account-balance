import { Injectable } from '@angular/core';
import { Router, CanLoad, UrlSegment, Route } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
    providedIn: 'root',
})
export class AuthGuard implements CanLoad {
    constructor(private authService: AuthenticationService, private router: Router) {}

    canLoad(route: Route, segments: UrlSegment[]) {
        if (this.authService.isUserAuthenticated) {
            return true;
        }

        this.router.navigate(['/auth/login'], {
            queryParams: { returnUrl: route.path },
        });
        return false;
    }
}
