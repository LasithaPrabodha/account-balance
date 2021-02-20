import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../interfaces/transaction/ITransaction';
import { Observable } from 'rxjs';
import { TransactionResponse } from '../interfaces/transaction/ITransactionResponse';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class TransactionService {
    constructor(private httpClient: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) {}

    getTransactionsForRange(monthRange: Transaction): Observable<TransactionResponse[]> {
        const route = `${this.baseUrl}/api/transaction/range`;

        return this.httpClient
            .get<TransactionResponse[]>(route, { params: <any>monthRange })
            .pipe(tap((val) => console.log(val)));
    }
}
