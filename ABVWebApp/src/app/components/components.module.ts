import { NgModule } from '@angular/core';

import { ReportComponent } from './report/report.component';
import { MenuComponent } from './menu/menu.component';
import { ChartsModule } from 'ng2-charts';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@NgModule({
    imports: [CommonModule, RouterModule, ChartsModule],
    exports: [ReportComponent, MenuComponent],
    declarations: [ReportComponent, MenuComponent],
    providers: [],
})
export class ComponentsModule {}
