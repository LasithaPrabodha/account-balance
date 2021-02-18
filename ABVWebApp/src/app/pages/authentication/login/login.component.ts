import { UserForAuthentication } from '../../../interfaces/user/IUserForAuthentication';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../../../services/authentication.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit, OnDestroy {
  private returnUrl: string;
  private onDestroy$: Subject<void> = new Subject<void>();
  loginForm: FormGroup;
  errorMessage = '';
  showError: boolean;

  constructor(
    private authService: AuthenticationService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl('', [Validators.required]),
      password: new FormControl('', [Validators.required]),
    });

    this.returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
  }

  ngOnDestroy(): void {
    this.onDestroy$.next();
  }

  validateControl(controlName: string): boolean {
    return (
      this.loginForm.controls[controlName].invalid &&
      this.loginForm.controls[controlName].touched
    );
  }

  hasError(controlName: string, errorName: string): boolean {
    return this.loginForm.controls[controlName].hasError(errorName);
  }

  loginUser(): void {
    if (this.loginForm.valid) {
      this.showError = false;
      const login = { ...this.loginForm.value };
      const userForAuth: UserForAuthentication = {
        email: login.username,
        password: login.password,
      };

      this.authService
        .loginUser(userForAuth)
        .pipe(takeUntil(this.onDestroy$))
        .subscribe(
          (res) => {
            localStorage.setItem('token', res.token);
            this.authService.sendAuthStateChangeNotification(
              res.isAuthSuccessful
            );
            this.router.navigate([this.returnUrl]);
          },
          (error) => {
            this.errorMessage = error;
            this.showError = true;
          }
        );
    }
  }
}
