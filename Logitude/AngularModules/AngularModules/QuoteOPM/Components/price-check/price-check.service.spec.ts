import { TestBed } from '@angular/core/testing';

import { PriceCheckService } from './price-check.service';

describe('PriceCheckService', () => {
  let service: PriceCheckService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PriceCheckService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
