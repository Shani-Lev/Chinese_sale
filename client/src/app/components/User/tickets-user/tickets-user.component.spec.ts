import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TicketsUserComponent } from './tickets-user.component';

describe('TicketsUserComponent', () => {
  let component: TicketsUserComponent;
  let fixture: ComponentFixture<TicketsUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TicketsUserComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TicketsUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
