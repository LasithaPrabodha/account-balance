import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { takeUntil } from 'rxjs/operators';
import { UploadBalance } from 'src/app/interfaces/account/IUploadBalance';
import { AccountService } from 'src/app/services/account.service';
import { BaseComponent } from 'src/app/shared/base-component';

@Component({
    selector: 'app-upload-balances',
    templateUrl: './upload-balances.component.html',
    styleUrls: ['./upload-balances.component.css'],
})
export class UploadBalancesComponent extends BaseComponent implements OnInit {
    showError = false;
    showSuccess = false;
    bsConfig = {
        dateInputFormat: 'MM-YYYY',
        containerClass: 'theme-default',
        isAnimated: true,
    };
    responseMessage = '';

    @ViewChild('fileInput', { static: true }) fileInput: ElementRef;

    get uploadForm() {
        return this.form;
    }

    constructor(private accountService: AccountService) {
        super();
    }

    ngOnInit(): void {
        this.form = new FormGroup({
            yearMonth: new FormControl('', [Validators.required]),
            file: new FormControl('', [Validators.required]),
        });
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    submitForm() {
        if (this.form.valid && this.fileInput.nativeElement.files.length) {
            const transactionDate = new Date(this.form.value.yearMonth);
            const request: UploadBalance = {
                transactionDate: transactionDate.toUTCString(),
                file: this.fileInput.nativeElement.files[0],
            };
            this.accountService
                .uploadBalance(request)
                .pipe(takeUntil(this.onDestroy$))
                .subscribe(
                    (result) => {
                        this.showSuccess = true;
                        this.responseMessage = result.message;
                        this.fileInput.nativeElement.value = '';
                    },
                    (err) => {
                        this.showSuccess = false;
                        this.showError = true;
                        this.responseMessage = err.message;
                    }
                );
        }
    }
}
