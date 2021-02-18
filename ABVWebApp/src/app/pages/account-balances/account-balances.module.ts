import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { AccountBalancesComponent } from './account-balances.component';

@NgModule({
  declarations: [AccountBalancesComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([{ path: '', component: AccountBalancesComponent }]),
  ],
})
export class AccountBalancesModule {}
