import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllGiftComponent } from './all-gift.component';

describe('AllGiftComponent', () => {
  let component: AllGiftComponent;
  let fixture: ComponentFixture<AllGiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AllGiftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllGiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
