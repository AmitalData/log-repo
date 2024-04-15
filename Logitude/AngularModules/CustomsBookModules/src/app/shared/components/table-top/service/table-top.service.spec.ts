import { TestBed } from '@angular/core/testing';

import { TableTopService } from './table-top.service';

describe('TableTopService', () => {
	let service: TableTopService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(TableTopService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
