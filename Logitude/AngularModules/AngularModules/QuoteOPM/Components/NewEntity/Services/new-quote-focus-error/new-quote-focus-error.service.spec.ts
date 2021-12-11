import { TestBed } from '@angular/core/testing';

import { NewQuoteFocusErrorService } from './new-quote-focus-error.service';

describe('NewQuoteFocusErrorService', () => {
  let service: NewQuoteFocusErrorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteFocusErrorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
