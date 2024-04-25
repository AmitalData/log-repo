import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class HeaderService {
	public searchState: string = 'יבוא';

	constructor() {
		this.setSearchState();
		sessionStorage.setItem('searchState', this.searchState);
	}

	setSearchState(value?: string) {
		!value ? (this.searchState = 'יבוא') : (this.searchState = value);
		sessionStorage.setItem('searchState', this.searchState);
		return this.searchState;
	}

	getSearchState(byid?: boolean) {
		!byid
			? this.searchState
			: (this.searchState = searchState[sessionStorage.getItem('searchState') as keyof typeof searchState]);
		return this.searchState;
	}
}

enum searchState {
	'יבוא' = '1',
	'יצוא' = '2',
	'אוטונומיה' = '3',
}
