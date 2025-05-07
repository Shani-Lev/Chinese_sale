import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddGiftComponent } from './add-gift.component';

describe('AddGiftComponent', () => {
  let component: AddGiftComponent;
  let fixture: ComponentFixture<AddGiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddGiftComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddGiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
