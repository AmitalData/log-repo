import { Component, EventEmitter, Output, ViewChild } from '@angular/core';
import { SearchService } from './service/top-page.service';
import { FormsModule, } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { FilterPopupService } from '../filter-popup/service/filter-popup.service';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { RomanToolService } from '../../services/roman-tool.service';
import { SearchCustomsItemAutocomplateComponent } from './search-customs-item-autocomplate/search-customs-item-autocomplate.component';


@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [FormsModule, MatAutocompleteModule, SearchCustomsItemAutocomplateComponent],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	@ViewChild(SearchCustomsItemAutocomplateComponent) searchCustomsItemAutocomplateComponent: SearchCustomsItemAutocomplateComponent;
	@Output() searchClick = new EventEmitter<string | number>();
	textToSearch: string = '';
	searchHeader: string = '????? ??? ???/????/????? ?????';
	
	constructor(
		public searchService: SearchService,
		private headerService: HeaderService,
		private filterPopupService: FilterPopupService,
		public romanTool: RomanToolService,
	) { }

	public text: string = '';
	public checked: string | number = '';
	public searchBy = SearchByParam;
	public selectedSearchOption: SearchByParam = this.searchBy.Classification;
	public currentSearchState: string = searchState.????;
	public SearchByValidation: SearchBy = SearchBy.searchBy_form01;

	ngOnInit() {
		// this.text = this.searchService.SearchBy('searchBy_form01'); // #112160 
		this.text = this.searchHeader;
		this.checked = this.searchService.GetDefaultValue();
		this.headerService.searchState$.subscribe((searchText) => {
			this.currentSearchState = searchText;
		});
	}

	// #112160
	searchByNumOrText: SearchBy = SearchBy.searchBy_form01;
	isNumeric(value: string): boolean {
		let res = /^\d*$/.test(value);
		if (res) {
			this.searchService.SearchBy('searchBy_form01');
			this.searchByNumOrText = SearchBy.searchBy_form01
		}
		else {
			this.searchService.SearchBy('pageSearch_form02');
			this.searchByNumOrText = SearchBy.pageSearch_form02
		}
		return res; // Returns true if value contains only digits
	}

	public search(id: string) {
		this.checked = id;
		this.text = this.searchService.SearchBy(id);
	}

	onChange(event: any) {
		this.searchService.SetSearchText(event.target.value);
	}

	clickSearch() {
		this.searchCustomsItemAutocomplateComponent.clearAutocomplete();

		if (this.textToSearch.trim() === "") {
			this.textToSearch = "";
			return;
		}

		this.searchClick.emit(this.searchByNumOrText);
		// this.searchClick.emit(this.searchService.selectSearchBy);// #112160
		this.filterPopupService.toggleFilterPopup(false);

		this.searchService.searchText$.subscribe((searchText) => {
			// reset search input in html:
			if (searchText === "") this.textToSearch = "";
		});
	}

	onCustomsItemSelected(textToSearch: string) {
		this.search('searchBy_form01');
		this.searchService.SetSearchText(textToSearch);
		this.clickSearch();
	}
}

export enum SearchByParam {
	Classification = "??? ???",
	WordCombination = "????/????? ?????"
}
