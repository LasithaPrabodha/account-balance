import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UploadBalance } from '../interfaces/account/IUploadBalance';
import { Observable } from 'rxjs';
import { AccountResponse } from '../interfaces/account/IAccountResponse';

@Injectable({ providedIn: 'root' })
export class AccountService {
    constructor(private httpClient: HttpClient, @Inject('BASE_API_URL') private baseUrl: string) {}

    uploadBalance(body: UploadBalance): Observable<{ message: string }> {
        const route = `${this.baseUrl}/api/account/update`;

        const formData = new FormData();
        Object.keys(body).forEach((key) => {
            formData.append(key, body[key]);
        });
        return this.httpClient.post<{ message: string }>(route, formData);
    }

    getAccounts(): Observable<AccountResponse[]> {
        const route = `${this.baseUrl}/api/account`;

        return this.httpClient.get<AccountResponse[]>(route);
    }
}
