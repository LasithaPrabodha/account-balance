import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UploadBalancesComponent } from './upload-balances.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
    declarations: [UploadBalancesComponent],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        BsDatepickerModule.forRoot(),
        RouterModule.forChild([{ path: '', component: UploadBalancesComponent }]),
    ],
    providers: [],
})
export class UploadBalancesModule {}
