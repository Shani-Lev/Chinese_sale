import { TestBed } from '@angular/core/testing';

import { UserTicketService } from './user-ticket.service';

describe('UserTicketService', () => {
  let service: UserTicketService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserTicketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
