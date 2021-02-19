import { Component, OnInit } from '@angular/core';
import { map, mergeMap, takeUntil } from 'rxjs/operators';
import { AccountResponse } from 'src/app/interfaces/account/IAccountResponse';
import { Transaction } from 'src/app/interfaces/transaction/ITransaction';
import { TransactionResponse } from 'src/app/interfaces/transaction/ITransactionResponse';
import { AccountService } from 'src/app/services/account.service';
import { TransactionService } from 'src/app/services/transaction.service';
import { BaseComponent } from 'src/app/shared/base-component';

const mergeAccTrans = (transactions) =>
    map((accounts: (AccountResponse & { transactions: TransactionResponse[] })[]) => ({
        transactions,
        accounts,
    }));

@Component({
    selector: 'app-account-reports',
    templateUrl: './account-reports.component.html',
    styleUrls: ['./account-reports.component.css'],
})
export class AccountReportsComponent extends BaseComponent implements OnInit {
    bsConfig = {
        dateInputFormat: 'MM-YYYY',
        containerClass: 'theme-default',
        isAnimated: true,
    };
    accounts: (AccountResponse & { transactions: TransactionResponse[] })[] = [];

    constructor(
        private transactionService: TransactionService,
        private accountService: AccountService
    ) {
        super();
    }

    ngOnInit(): void {
        const request: Transaction = {
            startYear: 2020,
            startMonth: 1,
            endYear: 2021,
            endMonth: 12,
        };
        this.transactionService
            .getTransactionsForRange(request)
            .pipe(
                mergeMap((transactions) =>
                    this.accountService.getAccounts().pipe(mergeAccTrans(transactions))
                ),
                takeUntil(this.onDestroy$)
            )
            .subscribe(({ transactions, accounts }) => {
                this.accounts = accounts.map((account) => ({
                    ...account,
                    transactions: transactions.filter(
                        (transaction) => transaction.accountId === account.id
                    ),
                }));
            });
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }
}
