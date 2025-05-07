import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllDonorsComponent } from './all-donors.component';

describe('AllDonorsComponent', () => {
  let component: AllDonorsComponent;
  let fixture: ComponentFixture<AllDonorsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllDonorsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllDonorsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
