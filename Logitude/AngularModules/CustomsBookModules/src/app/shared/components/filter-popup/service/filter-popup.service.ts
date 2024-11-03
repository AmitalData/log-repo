import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class FilterPopupService {
	public _showFilterPopup: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	private _filterMarked: FiltersSearch = {
		parts: false,
		chapters: false,
		details: false,
		sections: false,
		customsDetails: false,
		rules: false,
		remarks: false,
	};
	constructor() {}

	toggleFilterPopup(openPopup: boolean) {
		this._showFilterPopup.next(openPopup);
	}

	getFilters() {
		return this._filterMarked;
	}

	setFilterMarked(id: string) {
		this._filterMarked[id] = !this._filterMarked[id];
	}

	clearFilterMarked() {
		this._filterMarked = {
			parts: false,
			chapters: false,
			details: false,
			sections: false,
			customsDetails: false,
			rules: false,
			remarks: false,
		};
	}
}

export interface FiltersSearch {
	parts: boolean;
	chapters: boolean;
	details: boolean;
	sections: boolean;
	customsDetails: boolean;
	rules: boolean;
	remarks: boolean;
}
