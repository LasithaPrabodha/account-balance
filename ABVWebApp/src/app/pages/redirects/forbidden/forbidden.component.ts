import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
    selector: 'app-forbidden',
    templateUrl: './forbidden.component.html',
    styleUrls: ['./forbidden.component.css'],
})
export class ForbiddenComponent implements OnInit {
    private returnUrl: string;

    constructor(
        private router: Router,
        private route: ActivatedRoute,
        private authService: AuthenticationService
    ) {}

    ngOnInit(): void {
        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
    }

    navigateToLogin(): void {
        this.authService.logout();
        this.router.navigate(['/auth/login'], {
            queryParams: { returnUrl: this.returnUrl },
        });
    }
}
