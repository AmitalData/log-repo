import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TableTopService } from './service/table-top.service';
import { NgFor, NgIf, } from '@angular/common';
import { FilterPopupComponent } from '../filter-popup/filter-popup.component';

@Component({
	selector: 'app-table-top',
	standalone: true,
	imports: [NgIf, NgFor, FilterPopupComponent],
	templateUrl: './table-top.component.html',
	styleUrl: './table-top.component.css',
})
export class TableTopComponent {
	service: TableTopService;
	@Output() clearResults = new EventEmitter<void>();

	@Input() countSearchResult: number = 0;
	@Input() searchMode: TableTopState = TableTopState.ViewAll;

	constructor() {
		this.service = new TableTopService();
	}
}
export enum TableTopState {
	Search = 'search',
	ViewAll = 'viewAll',
}
