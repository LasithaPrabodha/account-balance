import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { AccountService } from 'src/app/services/account.service';
import { BaseComponent } from 'src/app/shared/base-component';

@Component({
    selector: 'app-account-balances',
    templateUrl: './account-balances.component.html',
    styleUrls: ['./account-balances.component.css'],
})
export class AccountBalancesComponent extends BaseComponent implements OnInit {
    accounts$ = new Observable();
    isLoading = true;
    constructor(private accountService: AccountService) {
        super();
    }

    ngOnInit(): void {
        this.accounts$ = this.accountService
            .getAccounts()
            .pipe(finalize(() => (this.isLoading = false)));
    }
}
