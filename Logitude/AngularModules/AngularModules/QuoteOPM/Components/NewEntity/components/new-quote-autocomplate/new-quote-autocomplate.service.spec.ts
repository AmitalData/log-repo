import { TestBed } from '@angular/core/testing';

import { NewQuoteAutocomplateService } from './new-quote-autocomplate.service';

describe('NewQuoteAutocomplateService', () => {
  let service: NewQuoteAutocomplateService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteAutocomplateService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
