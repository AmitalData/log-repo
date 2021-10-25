import { TestBed } from '@angular/core/testing';

import { NewQuoteValidateEntityService } from './new-quote-validate-entity.service';

describe('NewQuoteValidateEntityService', () => {
  let service: NewQuoteValidateEntityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteValidateEntityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
