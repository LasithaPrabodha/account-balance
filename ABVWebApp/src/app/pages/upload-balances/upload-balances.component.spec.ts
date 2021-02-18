import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadBalancesComponent } from './upload-balances.component';

describe('UploadBalancesComponent', () => {
  let component: UploadBalancesComponent;
  let fixture: ComponentFixture<UploadBalancesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadBalancesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadBalancesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
