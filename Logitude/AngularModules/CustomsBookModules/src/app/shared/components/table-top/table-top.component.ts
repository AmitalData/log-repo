import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';
import { TableTopService } from './service/table-top.service';
import { NgClass, NgFor, NgIf, NgStyle, } from '@angular/common';
import { FilterPopupComponent } from '../filter-popup/filter-popup.component';
import { FiltersSearch } from '../filter-popup/service/filter-popup.service';

@Component({
	selector: 'app-table-top',
	standalone: true,
	imports: [NgIf, NgFor, FilterPopupComponent, NgClass, NgStyle],
	templateUrl: './table-top.component.html',
	styleUrl: './table-top.component.css',
})
export class TableTopComponent {
	service: TableTopService;
	@Output() clearResults = new EventEmitter<void>();
	@Output() filterClick: EventEmitter<FiltersSearch> = new EventEmitter();

	@Input() countSearchResult: number = 0;
	@Input() searchMode: TableTopState = TableTopState.ViewAll;
	@Input() showDetails: boolean;

	constructor() {
		this.service = new TableTopService();
		this.screenWidth = window.innerWidth;
	}

	filterClickEvent(filters: FiltersSearch) {
		this.filterClick.emit(filters);
	}


	screenWidth: number;
	// Get current screen width
	@HostListener('window:resize', ['$event'])
	onResize(event: Event): void {
		this.screenWidth = (event.target as Window).innerWidth;
	}
}
export enum TableTopState {
	Search = 'search',
	ViewAll = 'viewAll',
}
