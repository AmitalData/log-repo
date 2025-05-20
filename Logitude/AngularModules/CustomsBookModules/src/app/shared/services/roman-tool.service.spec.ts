import { TestBed } from '@angular/core/testing';

import { RomanToolService } from './roman-tool.service';

describe('RomanToolService', () => {
  let service: RomanToolService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RomanToolService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
