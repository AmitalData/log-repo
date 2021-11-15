import { TestBed } from '@angular/core/testing';

import { PriceCheckDataService } from './price-check-data.service';

describe('PriceCheckDataService', () => {
  let service: PriceCheckDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PriceCheckDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
