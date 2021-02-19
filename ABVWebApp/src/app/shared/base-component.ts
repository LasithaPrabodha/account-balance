import { Directive, OnDestroy } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subject } from 'rxjs';

@Directive()
export class BaseComponent implements OnDestroy {
    protected onDestroy$: Subject<void> = new Subject<void>();
    protected form: FormGroup;

    validateControl(controlName: string): boolean {
        return this.form.controls[controlName].invalid && this.form.controls[controlName].touched;
    }

    hasError(controlName: string, errorName: string): boolean {
        return this.form.controls[controlName].hasError(errorName);
    }

    ngOnDestroy(): void {
        this.onDestroy$.next();
    }
}
