import { Component, EventEmitter, Output } from '@angular/core';
import { SearchBy, SearchService } from './service/top-page.service';
import { FormsModule, } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { AsyncPipe, CommonModule, NgIf } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BehaviorSubject, catchError, debounceTime, EMPTY, Subject, switchMap } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { FilterPopupService } from '../filter-popup/service/filter-popup.service';
import { AllClassification, CustomClassification, CustomsItemsAutocomplate, GetFromTypesenseResponse, GroupedCustomsItems } from './page-top.interface';
import {MatSelectModule} from '@angular/material/select';
import { CacheService } from '../../../core/Services/cache.service';
import { HttpEvent, HttpResponse } from '@angular/common/http';
import { RomanToolService } from '../../services/roman-tool.service';


@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [FormsModule, NgIf, MatAutocompleteModule, AsyncPipe, MatSelectModule, CommonModule],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	// @Output() searchClick = new EventEmitter();
	@Output() searchClick = new EventEmitter<string | number>();
	customsItemsAutocomplateList: Subject<CustomsItemsAutocomplate[]> = new Subject<CustomsItemsAutocomplate[]>();
	groupCustomsItemsAutocomplateList: Subject<string[]> = new Subject<string[]>();
	autocompleateShortList: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
	textToSearch: string = '';
	
	constructor(
		public searchService: SearchService, 
		private headerService: HeaderService, 
		private API_MainService: API_MainService, 
		private filterPopupService: FilterPopupService,
		private cacheService: CacheService,
		public romanTool: RomanToolService,
	) { }

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

		this.getClassifications();
		this.applyAutocomplate();
	}

	private applyAutocomplate() {		
		this.searchService.searchText$.pipe(
			debounceTime(100),
			switchMap((searchText) =>
				this.API_MainService.GetFromTypesense(searchText, this.headerService.getSearchState(true), SessionInfo.LoggedUserTenant).pipe(catchError((error) => EMPTY)))
		).subscribe(async (res: any) => {
			const regex = new RegExp(`(${this.textToSearch})`, 'gi');
			const result: GetFromTypesenseResponse = res.body;

			let customsItems = [...result.CustomsItems, ...result.Remarks.map((remark) => { return { ...remark.CustomsItem, remark: remark.Remark.RemarkDescription } })];
			let customsItemsAutocomplateList: CustomsItemsAutocomplate[] = customsItems.map((item) => {
				let text = item.FullClassification + ' | ' + ((<any>item).remark || item.CIH_GoodsDescription);
				text = text.replace(regex, `<mark>$1</mark>`);
				return { FullClassification: item.FullClassification, text: text, BaseCustomsItemID: item.BaseCustomsItemID };
			});

			this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
			
			if(customsItemsAutocomplateList.length < 10) {
				this.autocompleateShortList.next(true);
				this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
				this.groupCustomsItemsAutocomplateList.next([]);
			} else {
				this.autocompleateShortList.next(false);

				const classificationType: AllClassification = await this.getClassifications();
				const customsBookType: string = this.headerService.getSearchState(true);
				const classifications: CustomClassification = classificationType[customsBookType];

				// let group = 
				// const groupedItems: GroupedCustomsItems[] = classification.map((item) => { return { name: item.Name, classification: item.Classification, items: [] } });
				const groupedItems: GroupedCustomsItems =  {};
				(classifications.sortedClassifications as string[]).forEach((key: string) => groupedItems[key] = []);

				customsItemsAutocomplateList.forEach((item) => {
					const classification: string = classifications[item.BaseCustomsItemID]  as string || "-"; // classifications ={["090000000"]:"XV"}
					groupedItems[classification].push(item);
				});
				// this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
				this.groupedItems = groupedItems;
				this.groupCustomsItemsAutocomplateList.next(classifications.sortedClassifications as string[]);
				this.customsItemsAutocomplateList.next([]);
			}

			// this.groupedItems[0].items = customsItemsAutocomplateList;
			// console.log(this.groupedItems);
		});
	}
	groupedItems: GroupedCustomsItems =  {};


	private async getClassifications() {
		return await this.cacheService.getByPromise('classifications', async () => {
			const respnse = await this.API_MainService.GetClassifications().toPromise();
			const allClassifications: AllClassification = (respnse as HttpResponse<any>).body;
			delete allClassifications["$id"];
			
			for (const bookType in allClassifications) {
				const classifications = allClassifications[bookType];
				delete classifications["$id"];
				classifications["-"] = "-";		
				allClassifications[bookType].sortedClassifications = Object.values(classifications).sort(this.romanTool.comparetor) as string[];				
			}

			return allClassifications;
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

