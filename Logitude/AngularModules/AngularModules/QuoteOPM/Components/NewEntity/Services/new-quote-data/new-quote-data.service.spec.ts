import { TestBed } from '@angular/core/testing';

import { NewQuoteDataService } from './new-quote-data.service';

describe('NewQuoteDataService', () => {
  let service: NewQuoteDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
