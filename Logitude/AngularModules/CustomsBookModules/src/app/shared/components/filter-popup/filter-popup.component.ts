import { Component, EventEmitter, Output } from '@angular/core';
import { FilterPopupService, FiltersSearch } from './service/filter-popup.service';
import { NgClass, NgIf } from '@angular/common';
import { SearchService } from '../page-top/service/top-page.service';

@Component({
	selector: 'app-filter-popup',
	standalone: true,
	imports: [NgClass, NgIf],
	templateUrl: './filter-popup.component.html',
	styleUrl: './filter-popup.component.css',
})
export class FilterPopupComponent {
	// service: FilterPopupService;
	private _initFilters: FiltersSearch;
	openPopup = false;
	numberOfFilters = 0;
	@Output() filterClick: EventEmitter<any> = new EventEmitter();

	constructor(private service: FilterPopupService, private searchService: SearchService) { }

	ngOnInit() {
		this._initFilters = this.service.getFilters();

		this.service._showFilterPopup.subscribe((value) => {
			this.openPopup = value;
		});
	}

	clickFilterEvent() {
		this.service.toggleFilterPopup(!this.openPopup);
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

	filterClickEvent() {
		this.openPopup = false;
		if (this.searchService.GetSearchText()?.trim() === "") return;
		this.filterClick.emit(this.service.getFilters());
	}
}
