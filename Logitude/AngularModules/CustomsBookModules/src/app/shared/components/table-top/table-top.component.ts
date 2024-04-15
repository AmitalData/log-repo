import { Component, Input } from '@angular/core';
import { TableTopService } from './service/table-top.service';
import { NgFor } from '@angular/common';
import { FilterPopupComponent } from '../filter-popup/filter-popup.component';

@Component({
	selector: 'app-table-top',
	standalone: true,
	imports: [NgFor, FilterPopupComponent],
	templateUrl: './table-top.component.html',
	styleUrl: './table-top.component.css',
})
export class TableTopComponent {
	service: TableTopService;

	@Input() state: string = TableTopState.Search;

	constructor() {
		this.service = new TableTopService();
	}
}
enum TableTopState {
	Search = 'search',
	ViewAll = 'viewAll',
}
