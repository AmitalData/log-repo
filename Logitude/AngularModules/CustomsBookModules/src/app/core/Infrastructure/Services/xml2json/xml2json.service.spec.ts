import { TestBed } from '@angular/core/testing';

import { Xml2jsonService } from './xml2json.service';

describe('Xml2jsonService', () => {
  let service: Xml2jsonService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Xml2jsonService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
