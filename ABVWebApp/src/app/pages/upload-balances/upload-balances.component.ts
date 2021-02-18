import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-upload-balances',
  templateUrl: './upload-balances.component.html',
  styleUrls: ['./upload-balances.component.css'],
})
export class UploadBalancesComponent implements OnInit {
  showError = false;
  errorMessage = '';

  uploadForm: FormGroup = new FormGroup({
    year: new FormControl(),
    month: new FormControl(),
    file: new FormControl(),
  });
  constructor() {}

  ngOnInit(): void {}
}
