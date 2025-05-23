import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddDonorComponent } from './add-donor.component';

describe('AddDonorComponent', () => {
  let component: AddDonorComponent;
  let fixture: ComponentFixture<AddDonorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddDonorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddDonorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
