import { TestBed } from '@angular/core/testing';

import { FilterPopupService } from './filter-popup.service';

describe('FilterPopupService', () => {
  let service: FilterPopupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FilterPopupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
