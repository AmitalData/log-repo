import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class FilterPopupService {
	private _showFilterPopup: boolean = false;
	private _filterMarked: Filters = {
		parts: false,
		chapters: false,
		details: false,
		sections: false,
		customsDetails: false,
		rules: false,
		comments: false,
	};
	constructor() {}

	toggleFilterPopup() {
		this._showFilterPopup = !this._showFilterPopup;
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
			comments: false,
		};
	}
}

interface Filters {
	parts: boolean;
	chapters: boolean;
	details: boolean;
	sections: boolean;
	customsDetails: boolean;
	rules: boolean;
	comments: boolean;
}
