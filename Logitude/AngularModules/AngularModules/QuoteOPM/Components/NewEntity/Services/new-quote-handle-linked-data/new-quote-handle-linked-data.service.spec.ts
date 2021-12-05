import { TestBed } from '@angular/core/testing';

import { NewQuoteHandleLinkedDataService } from './new-quote-handle-linked-data.service';

describe('NewQuoteHandleLinkedDataService', () => {
  let service: NewQuoteHandleLinkedDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteHandleLinkedDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
