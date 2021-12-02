import { TestBed } from '@angular/core/testing';

import { NewQuoteDataShareService } from './new-quote-data-share.service';

describe('NewQuoteDataShareService', () => {
  let service: NewQuoteDataShareService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteDataShareService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
