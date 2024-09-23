import { Component, EventEmitter, Output } from '@angular/core';
import { SearchBy, SearchService } from './service/top-page.service';
import { FormsModule, } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { AsyncPipe, NgIf } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { debounceTime, Subject, switchMap } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { FilterPopupService } from '../filter-popup/service/filter-popup.service';


@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [FormsModule, NgIf, MatAutocompleteModule, AsyncPipe],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	// @Output() searchClick = new EventEmitter();
	@Output() searchClick = new EventEmitter<string | number>();
	customsItemsAutocomplateList: Subject<CustomsItemsAutocomplate[]> = new Subject<CustomsItemsAutocomplate[]>();
	textToSearch: string = '';
	
	constructor(public searchService: SearchService, private headerService: HeaderService, private API_MainService: API_MainService, private filterPopupService: FilterPopupService,) { }

	public text: string = '';
	public checked: string | number = '';
	public searchBy = SearchByParam;
	public selectedSearchOption: SearchByParam = this.searchBy.Classification;
	public currentSearchState: string = searchState.יבוא;
	public SearchByValidation: SearchBy = SearchBy.searchBy_form01;
	ngOnInit() {
		this.text = this.searchService.SearchBy('searchBy_form01');
		this.checked = this.searchService.GetDefaultValue();
		this.headerService.searchState$.subscribe((searchText) => {
			this.currentSearchState = searchText;
		});

		this.applyAutocomplate();
	}

	private applyAutocomplate() {		
		this.searchService.searchText$.pipe(
			debounceTime(100),
			switchMap((searchText) =>
				this.API_MainService.GetFromTypesense(searchText, this.headerService.getSearchState(true), SessionInfo.LoggedUserTenant))
		).subscribe(async (res: any) => {
			const regex = new RegExp(`(${this.textToSearch})`, 'gi');
			const result: GetFromTypesenseResponse = res.body;

			let customsItems = [...result.CustomsItems, ...result.Remarks.map((remark) => { return { ...remark.CustomsItem, remark: remark.Remark.RemarkDescription } })];
			let customsItemsAutocomplateList: CustomsItemsAutocomplate[] = customsItems.map((item) => {
				let text = item.FullClassification + ' | ' + ((<any>item).remark || item.CIH_GoodsDescription);
				text = text.replace(regex, `<mark>$1</mark>`);
				return { FullClassification: item.FullClassification, text: text }
			});

			this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
		});
	}

	public search(id: string) {
		this.checked = id;
		this.text = this.searchService.SearchBy(id);
	}

	onChange(event: any) {
		this.searchService.SetSearchText(event.target.value);
	}

	clickSearch() {
		if (this.textToSearch.trim() === "") {
			this.textToSearch = "";
			return;
		}

		this.searchClick.emit(this.searchService.selectSearchBy);
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
	Classification = "פרט מכס",
	WordCombination = "מילה/צירוף מילים"
}


export interface GetFromTypesenseResponse {
	$id: string
	Remarks: RemarkWithCustomsItem[]
	CustomsItems: CustomsItem[]
}

export interface CustomsItem {
	$id: string
	CB_ID: string
	ID: number
	CustomsItemID: string
	FullClassification: string
	IsLeaf: boolean
	CustomsItemDetailsHistoryID: number
	PropertiesDetailsHistoryID: number
	PH_MeasurementUnitID: number
	IsHistoryExists: boolean
	IsRulesExists: boolean
	StartDate: string
	StartDateInt: number
	EndDate: string
	EndDateInt: number
	CI_Parent_CustomsItemIDNum: number
	CI_BaseFullClassification: string
	CI_ComputedCheckDigit: string
	CI_CustomsBookTypeIDNum: string
	CI_CustomsItemCategoryIDNum: string
	ItemHierarchicLocationID: string
	CIH_Title: string
	CIH_GoodsDescription: string
	CustomsItemEntityStatusIDNum: number
	PH_IsCarItem: boolean
	FullGoodsDescription: string
}

export interface RemarkWithCustomsItem {
	CustomsItem: CustomsItem
	Remark: Remark
}

export interface Remark {
	$id: string
	Id: string
	Tenant: number
	Drop_CB_ID: string
	CustomsItemsID: number
	RemarkDescription: string
}

export interface CustomsItemsAutocomplate {
	FullClassification: string;
	text: string;
}