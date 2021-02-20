import { Router } from '@angular/router';
import { AuthenticationService } from './../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
    selector: 'app-menu',
    templateUrl: './menu.component.html',
    styleUrls: ['./menu.component.css'],
})
export class MenuComponent implements OnInit {
    isUserAuthenticated$: Observable<boolean>;
    username: string;
    constructor(private authService: AuthenticationService, private router: Router) {}

    ngOnInit(): void {
        this.isUserAuthenticated$ = this.authService.authChanged;
        this.username = this.authService.username;
    }

    logout(): void {
        this.authService.logout();
        this.router.navigate(['/']);
    }
}
