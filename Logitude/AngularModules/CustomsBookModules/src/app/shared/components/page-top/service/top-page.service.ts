import { Injectable } from '@angular/core';

/**
 * Sets the value of selectSearchBy based on the provided value.
 * If no value is provided, it sets the default value.
 * @param value - The value to set selectSearchBy to.
 * @returns The updated value of selectSearchBy.
 */
@Injectable({
	providedIn: 'root',
})
export class Service {
	private _Default_selectSearchBy: number | string = 'pageSearch_form01';
	selectSearchBy = SearchBy[this._Default_selectSearchBy as keyof typeof SearchBy];
	private _SearchText: string;

	ngOnInit() {
		sessionStorage.removeItem('searchText');
	}

	SearchBy(value?: number | string) {
		!value
			? (this.selectSearchBy = SearchBy[this._Default_selectSearchBy as keyof typeof SearchBy])
			: (this.selectSearchBy = SearchBy[value as keyof typeof SearchBy]);
		return this.selectSearchBy;
	}

	GetDefaultValue() {
		sessionStorage.removeItem('searchText');
		return this._Default_selectSearchBy;
	}

	GetSearchText() {
		this._SearchText = sessionStorage.getItem('searchText') || '';
		return this._SearchText;
	}

	SetSearchText(value: string) {
		this._SearchText = value;
		sessionStorage.setItem('searchText', value);
	}
}

enum SearchBy {
	'searchBy_form01' = 'חיפוש פרט מכס... ',
	'pageSearch_form02' = 'חיפוש מילה/ צירוף מילים...',
}
