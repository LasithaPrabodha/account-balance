import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { AccountReportsComponent } from './account-reports.component';

@NgModule({
  declarations: [AccountReportsComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([{ path: '', component: AccountReportsComponent }]),
  ],
})
export class AccountReportsModule {}
