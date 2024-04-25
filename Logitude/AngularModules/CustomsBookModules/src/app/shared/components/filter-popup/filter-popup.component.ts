import { Component, EventEmitter, Output } from '@angular/core';
import { FilterPopupService } from './service/filter-popup.service';
import { NgClass, NgIf } from '@angular/common';

@Component({
	selector: 'app-filter-popup',
	standalone: true,
	imports: [NgClass, NgIf],
	templateUrl: './filter-popup.component.html',
	styleUrl: './filter-popup.component.css',
})
export class FilterPopupComponent {
	service: FilterPopupService;
	private _initFilters;
	openPopup = false;
	numberOfFilters = 0;

	@Output() filterClick = new EventEmitter();

	constructor() {
		this.service = new FilterPopupService();
	}

	ngOnInit() {
		this._initFilters = this.service.getFilters();
	}

	pickFilter(id: string) {
		this.service.setFilterMarked(id);
		this.numberOfFilters = Object.values(this._initFilters).filter((value) => value === true).length;
	}

	clearFilter() {
		this.service.clearFilterMarked();
		this._initFilters = this.service.getFilters();
		this.numberOfFilters = 0;
	}

	isMarked(id: string) {
		return this._initFilters[id];
	}
}
