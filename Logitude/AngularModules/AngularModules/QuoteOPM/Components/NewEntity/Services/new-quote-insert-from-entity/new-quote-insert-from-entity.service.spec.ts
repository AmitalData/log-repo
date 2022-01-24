import { TestBed } from '@angular/core/testing';

import { NewQuoteInsertFromEntityService } from './new-quote-insert-from-entity.service';

describe('NewQuoteInsertFromEntityService', () => {
  let service: NewQuoteInsertFromEntityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NewQuoteInsertFromEntityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
