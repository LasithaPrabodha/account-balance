import { AccountResponse } from './account/IAccountResponse';
import { TransactionResponse } from './transaction/ITransactionResponse';

export type AccTrans = AccountResponse & { transactions: TransactionResponse[] };
