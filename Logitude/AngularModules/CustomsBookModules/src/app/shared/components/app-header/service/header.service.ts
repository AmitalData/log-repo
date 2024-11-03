import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class HeaderService {
	public searchState: string = 'יבוא';
	public searchState$: BehaviorSubject<string> = new BehaviorSubject<string>(this.searchState);

	constructor() {
		this.setSearchState();
		sessionStorage.setItem('searchState', this.searchState);
	}

	setSearchState(value?: string) {
		!value ? (this.searchState = 'יבוא') : (this.searchState = value);
		sessionStorage.setItem('searchState', this.searchState);
		this.searchState$.next(this.searchState);
		return this.searchState;
	}

	getSearchState(byid?: boolean) {
		!byid
			? this.searchState
			: (this.searchState = searchState[sessionStorage.getItem('searchState') as keyof typeof searchState]);
		return this.searchState;
	}
}

export enum searchState {
	'יבוא' = '1',
	'יצוא' = '2',
	'אוטונומיה' = '3',
}
