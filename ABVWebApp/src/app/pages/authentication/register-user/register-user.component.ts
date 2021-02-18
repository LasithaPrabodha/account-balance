import { Router } from '@angular/router';
import { PasswordConfirmationValidatorService } from '../../../validators/password-confirmation-validator.service';
import { UserForRegistration } from '../../../interfaces/user/IUserForRegistration';
import { AuthenticationService } from '../../../services/authentication.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css'],
})
export class RegisterUserComponent implements OnInit, OnDestroy {
  private onDestroy$: Subject<void> = new Subject<void>();

  registerForm: FormGroup = new FormGroup({
    firstName: new FormControl(''),
    lastName: new FormControl(''),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    confirm: new FormControl(''),
  });
  errorMessage = '';
  showError: boolean;

  constructor(
    private authService: AuthenticationService,
    private passConfValidator: PasswordConfirmationValidatorService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registerForm
      .get('confirm')
      .setValidators([
        Validators.required,
        this.passConfValidator.validateConfirmPassword(
          this.registerForm.get('password')
        ),
      ]);
  }
  ngOnDestroy(): void {
    this.onDestroy$.next();
  }

  validateControl(controlName: string): boolean {
    return (
      this.registerForm.controls[controlName].invalid &&
      this.registerForm.controls[controlName].touched
    );
  }

  hasError(controlName: string, errorName: string): boolean {
    return this.registerForm.controls[controlName].hasError(errorName);
  }

  registerUser(): void {
    if (this.registerForm.valid) {
      this.showError = false;
      const formValues = { ...this.registerForm.value };

      const user: UserForRegistration = {
        firstName: formValues.firstName,
        lastName: formValues.lastName,
        email: formValues.email,
        password: formValues.password,
        confirmPassword: formValues.confirm,
      };

      this.authService
        .registerUser(user)
        .pipe(takeUntil(this.onDestroy$))
        .subscribe(
          (_) => {
            this.router.navigate(['/authentication/login']);
          },
          (error) => {
            this.errorMessage = error;
            this.showError = true;
          }
        );
    }
  }
}
