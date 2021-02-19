import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';

@Component({
    selector: 'app-report',
    templateUrl: './report.component.html',
    styleUrls: ['./report.component.css'],
})
export class ReportComponent implements OnInit {
    @Input() lineChartData: ChartDataSets[] = [
        { data: [656, 569, 860, -81, 56, 555, 40], label: 'RnD', lineTension: 0 },
    ];
    @Input() accountName = 'RnD';
    lineChartLabels: Label[] = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
    lineChartOptions: ChartOptions = {
        responsive: true,
    };
    lineChartColors: Color[] = [
        {
            borderColor: 'black',
        },
    ];

    constructor() {}

    ngOnInit(): void {}
}
