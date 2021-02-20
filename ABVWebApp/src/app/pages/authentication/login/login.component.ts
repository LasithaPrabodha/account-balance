import { UserForAuthentication } from '../../../interfaces/user/IUserForAuthentication';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { BaseComponent } from 'src/app/shared/base-component';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
})
export class LoginComponent extends BaseComponent implements OnInit {
    private returnUrl: string;
    errorMessage = '';
    showError: boolean;
    isLoading = false;

    constructor(
        private authService: AuthenticationService,
        private router: Router,
        private route: ActivatedRoute
    ) {
        super();
    }

    get loginForm() {
        return this.form;
    }

    ngOnInit(): void {
        this.form = new FormGroup({
            email: new FormControl('', [Validators.required, Validators.email]),
            password: new FormControl('', [Validators.required]),
        });

        this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
    }

    loginUser(): void {
        if (this.form.valid) {
            this.isLoading = true;
            this.showError = false;
            const login = { ...this.form.value };
            const userForAuth: UserForAuthentication = {
                email: login.email,
                password: login.password,
            };

            this.authService
                .loginUser(userForAuth)
                .pipe(takeUntil(this.onDestroy$))
                .subscribe(
                    (res) => {
                        this.authService.sendAuthStateChangeNotification(res.isAuthSuccessful);
                        res.isAuthSuccessful && this.router.navigate([this.returnUrl]);
                        this.isLoading = false;
                    },
                    (error) => {
                        this.errorMessage = error.message;
                        this.showError = true;
                        this.isLoading = false;
                    }
                );
        }
    }
}
