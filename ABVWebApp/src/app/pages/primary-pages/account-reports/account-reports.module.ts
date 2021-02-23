import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { AccountReportsComponent } from './account-reports.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ComponentsModule } from 'src/app/components/components.module';

@NgModule({
    declarations: [AccountReportsComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        ComponentsModule,
        BsDatepickerModule.forRoot(),
        RouterModule.forChild([{ path: '', component: AccountReportsComponent }]),
    ],
})
export class AccountReportsModule {}
