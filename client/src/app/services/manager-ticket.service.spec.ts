import { TestBed } from '@angular/core/testing';

import { ManagerTicketService } from './manager-ticket.service';

describe('ManagerTicketService', () => {
  let service: ManagerTicketService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ManagerTicketService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
