import { Injectable } from '@angular/core';
import { Router, CanLoad, Route, UrlSegment } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AdminGuard implements CanLoad {
    constructor(private authService: AuthenticationService, private router: Router) {}

    canLoad(route: Route, segments: UrlSegment[]): boolean {
        if (this.authService.isUserAdmin && this.authService.isUserAuthenticated) return true;

        this.router.navigate(['/forbidden'], {
            queryParams: { returnUrl: route.path },
        });
        return false;
    }
}
