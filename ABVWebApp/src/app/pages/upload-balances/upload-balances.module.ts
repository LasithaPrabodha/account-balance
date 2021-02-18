import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { UploadBalancesComponent } from './upload-balances.component';

@NgModule({
  declarations: [UploadBalancesComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule.forChild([{ path: '', component: UploadBalancesComponent }]),
  ],
})
export class UploadBalancesModule {}
