import { Component, Input, OnInit } from '@angular/core';
import { ChartDataSets, ChartOptions } from 'chart.js';
import { Color, Label } from 'ng2-charts';
import { AccTrans } from 'src/app/interfaces/types';

const monthNames = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
];

@Component({
    selector: 'app-report',
    templateUrl: './report.component.html',
    styleUrls: ['./report.component.css'],
})
export class ReportComponent implements OnInit {
    @Input() accountData: AccTrans = null;
    lineChartData: ChartDataSets[] = [{ lineTension: 0 }];
    lineChartLabels: Label[] = [];
    lineChartOptions: ChartOptions = {
        responsive: true,
        scales: {
            yAxes: [
                {
                    scaleLabel: {
                        display: true,
                        labelString: 'Rs.',
                    },
                },
            ],
            xAxes: [
                {
                    scaleLabel: {
                        display: true,
                        labelString: 'Month',
                    },
                },
            ],
        },
    };
    lineChartColors: Color[] = [
        {
            borderColor: 'black',
        },
    ];

    constructor() {}

    ngOnInit(): void {
        const sortedTransactions = this.accountData.transactions.sort(
            (x, y) => Number(new Date(x.transactionDate)) - Number(new Date(y.transactionDate))
        );

        this.lineChartData[0].data = sortedTransactions.map((trans) => trans.amount);

        this.lineChartLabels = sortedTransactions.map((trans) => {
            const date = new Date(trans.transactionDate);

            return `${monthNames[date.getMonth()]} ${date.getFullYear()}`;
        });
    }
}
