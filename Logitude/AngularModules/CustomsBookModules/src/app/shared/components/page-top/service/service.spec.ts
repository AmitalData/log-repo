import { TestBed } from '@angular/core/testing';

import { Service } from './top-page.service';

describe('Service', () => {
	let service: Service;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(Service);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	it('should set selectSearchBy to default value if no value is provided', () => {
		expect(service.selectSearchBy).toEqual('חיפוש פרט מכס... ');
	});

	it('should set selectSearchBy to the provided value', () => {
		/**
		 * The new value for searching or combining words.
		 */
		const newValue = 'חיפוש מילה/ צירוף מילים...';
		service.SearchBy(newValue);
		expect(service.selectSearchBy).toEqual(newValue);
	});
});
