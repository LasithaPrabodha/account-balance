import { Router } from '@angular/router';
import { PasswordConfirmationValidatorService } from '../../../validators/password-confirmation-validator.service';
import { UserForRegistration } from '../../../interfaces/user/IUserForRegistration';
import { AuthenticationService } from '../../../services/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { BaseComponent } from 'src/app/shared/base-component';

@Component({
    selector: 'app-register-user',
    templateUrl: './register-user.component.html',
    styleUrls: ['./register-user.component.css'],
})
export class RegisterUserComponent extends BaseComponent implements OnInit {
    errorMessage = '';
    showError: boolean;

    constructor(
        private authService: AuthenticationService,
        private passConfValidator: PasswordConfirmationValidatorService,
        private router: Router
    ) {
        super();
    }

    get registerForm() {
        return this.form;
    }

    ngOnInit(): void {
        this.form = new FormGroup({
            firstName: new FormControl(''),
            lastName: new FormControl(''),
            email: new FormControl('', [Validators.required, Validators.email]),
            password: new FormControl('', [Validators.required]),
            confirm: new FormControl(''),
            role: new FormControl('0', [Validators.required]),
        });

        this.form
            .get('confirm')
            .setValidators([
                Validators.required,
                this.passConfValidator.validateConfirmPassword(this.form.get('password')),
            ]);
    }

    registerUser(): void {
        if (this.form.valid) {
            this.showError = false;
            const formValues = { ...this.form.value };

            const user: UserForRegistration = {
                firstName: formValues.firstName,
                lastName: formValues.lastName,
                email: formValues.email,
                password: formValues.password,
                confirmPassword: formValues.confirm,
                role: formValues.role,
            };

            this.authService
                .registerUser(user)
                .pipe(takeUntil(this.onDestroy$))
                .subscribe(
                    (_) => {
                        this.router.navigate(['/auth/login']);
                    },
                    (error) => {
                        this.errorMessage = error;
                        this.showError = true;
                    }
                );
        }
    }
}
