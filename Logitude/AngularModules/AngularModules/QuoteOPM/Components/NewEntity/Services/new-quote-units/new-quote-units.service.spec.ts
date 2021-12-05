import { TestBed } from '@angular/core/testing';

import { NewQuoteUnitsService } from './new-quote-units.service';

describe('NewQuoteUnitsService', () => {
  let service: NewQuoteUnitsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteUnitsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
