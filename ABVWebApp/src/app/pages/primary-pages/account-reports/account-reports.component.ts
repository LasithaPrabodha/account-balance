import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { map, mergeMap, takeUntil } from 'rxjs/operators';
import { AccountResponse } from 'src/app/interfaces/account/IAccountResponse';
import { Transaction } from 'src/app/interfaces/transaction/ITransaction';
import { TransactionResponse } from 'src/app/interfaces/transaction/ITransactionResponse';
import { AccTrans } from 'src/app/interfaces/types';
import { AccountService } from 'src/app/services/account.service';
import { TransactionService } from 'src/app/services/transaction.service';
import { BaseComponent } from 'src/app/shared/base-component';
import { format } from 'date-fns';

const mergeAccTrans = (transactions) =>
    map((accounts: AccTrans[]) => ({
        transactions,
        accounts,
    }));

@Component({
    selector: 'app-account-reports',
    templateUrl: './account-reports.component.html',
    styleUrls: ['./account-reports.component.css'],
})
export class AccountReportsComponent extends BaseComponent implements OnInit {
    datepickerConfig = {
        dateInputFormat: 'MM-YYYY',
        containerClass: 'theme-default',
        isAnimated: true,
    };
    start = {
        minDate: null,
        maxDate: new Date(),
    };
    end = {
        minDate: null,
        maxDate: new Date(),
    };
    datepickerRangeValue: Date[];
    showError = false;
    showInfo = false;
    errorMessage = '';
    infoMessage = '';

    accounts: (AccountResponse & { transactions: TransactionResponse[] })[] = [];

    constructor(
        private transactionService: TransactionService,
        private accountService: AccountService
    ) {
        super();
    }

    get dateRangeForm() {
        return this.form;
    }

    get endDate(): AbstractControl {
        return this.form.get('endDate');
    }
    get startDate(): AbstractControl {
        return this.form.get('startDate');
    }

    ngOnInit(): void {
        const startDate = new Date();
        startDate.setFullYear(startDate.getFullYear() - 1);
        startDate.setDate(1);
        const endDate = new Date();
        endDate.setDate(1);
        this.form = new FormGroup({
            startDate: new FormControl(startDate),
            endDate: new FormControl(endDate),
        });
        this.getTransactions();
    }

    private getTransactions() {
        const request: Transaction = {
            startDate: format(this.startDate.value, 'yyyy-MM-dd'),
            endDate: format(this.endDate.value, 'yyyy-MM-dd'),
        };

        this.transactionService
            .getTransactionsForRange(request)
            .pipe(
                mergeMap((transactions) =>
                    this.accountService.getAccounts().pipe(mergeAccTrans(transactions))
                ),
                takeUntil(this.onDestroy$)
            )
            .subscribe(
                ({ transactions, accounts }) => {
                    if (!transactions.length) {
                        this.showInfo = true;
                        this.infoMessage = 'No updates to display.';
                    } else {
                        this.showInfo = false;
                        this.accounts = accounts.map((account) => ({
                            ...account,
                            transactions: transactions.filter(
                                (transaction) => transaction.accountId === account.id
                            ),
                        }));
                        this.showError = false;
                    }
                },
                (err) => {
                    this.showError = true;
                    this.errorMessage = err.message;
                }
            );
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    onValueChange(date, type: 'start' | 'end'): void {
        if (type === 'start') {
            if (this.endDate.value && this.endDate.value < new Date(date)) {
                this.endDate.setValue(new Date(date));
            }
            this.end.minDate = date;
        }
    }

    onSubmit() {
        this.getTransactions();
    }
}
