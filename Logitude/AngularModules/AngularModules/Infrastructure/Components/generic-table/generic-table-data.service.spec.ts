import { TestBed } from '@angular/core/testing';

import { GenericTableDataService } from './generic-table-data.service';

describe('GenericTableDataService', () => {
  let service: GenericTableDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GenericTableDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
