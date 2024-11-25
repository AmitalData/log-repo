import { ChangeDetectorRef, Component, EventEmitter, Output } from '@angular/core';
import { SearchBy, SearchService } from './service/top-page.service';
import { FormsModule, } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { AsyncPipe, CommonModule, NgIf } from '@angular/common';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { BehaviorSubject, catchError, debounceTime, EMPTY, of, Subject, switchMap } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { FilterPopupService } from '../filter-popup/service/filter-popup.service';
import { AllClassification, Classification, CustomClassification, CustomsItemsAutocomplate, GetFromTypesenseResponse, GroupedCustomsItems } from './page-top.interface';
import { MatSelectModule } from '@angular/material/select';
import { CacheService } from '../../../core/Services/cache.service';
import { HttpResponse } from '@angular/common/http';
import { RomanToolService } from '../../services/roman-tool.service';
import { MatExpansionModule } from '@angular/material/expansion';


@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [FormsModule, NgIf, MatAutocompleteModule, AsyncPipe, MatSelectModule, CommonModule, MatExpansionModule],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	@Output() searchClick = new EventEmitter<string | number>();
	customsItemsAutocomplateList: Subject<CustomsItemsAutocomplate[]> = new Subject<CustomsItemsAutocomplate[]>();
	groupCustomsItemsAutocomplateList: Subject<Classification[]> = new Subject<Classification[]>();
	autocompleateShortList: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(true);
	textToSearch: string = '';

	constructor(
		public searchService: SearchService,
		private headerService: HeaderService,
		private API_MainService: API_MainService,
		private filterPopupService: FilterPopupService,
		private cacheService: CacheService,
		public romanTool: RomanToolService,
		private cdr: ChangeDetectorRef,
	) { }

	public text: string = '';
	public checked: string | number = '';
	public searchBy = SearchByParam;
	public selectedSearchOption: SearchByParam = this.searchBy.Classification;
	public currentSearchState: string = searchState.יבוא;
	public SearchByValidation: SearchBy = SearchBy.searchBy_form01;
	public groupedItems: GroupedCustomsItems = {};

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
			switchMap((searchText: string) =>
				!!this.textToSearch ?
					this.API_MainService.GetFromTypesense(searchText, this.headerService.getSearchState(true), SessionInfo.LoggedUserTenant).pipe(catchError((error) => EMPTY)) :
					of(() => EMPTY)
			)
		).subscribe(async (res: any) => {
			if (!res.body)
				return this.customsItemsAutocomplateList.next([]);

			const regex = new RegExp(`(${this.textToSearch})`, 'gi');
			const result: GetFromTypesenseResponse = res.body;
			result.Remarks.forEach((remark) => remark.CustomsItem.BaseCustomsItemID = -1);
			let customsItems = [...result.CustomsItems, ...result.Remarks.map((remark) => { return { ...remark.CustomsItem, remark: remark.Remark.RemarkDescription } })];
			let customsItemsAutocomplateList: CustomsItemsAutocomplate[] = customsItems.map((item) => {
				let text = item.FullClassification + ' | ' + ((<any>item).remark || item.CIH_GoodsDescription);
				text = text.replace(regex, `<mark>$1</mark>`);
				return { FullClassification: item.FullClassification, text: text, BaseCustomsItemID: item.BaseCustomsItemID };
			});

			if (customsItemsAutocomplateList.length < 10) {
				this.customsItemsAutocomplateList.next(customsItemsAutocomplateList);
				this.groupCustomsItemsAutocomplateList.next([]);
				this.autocompleateShortList.next(true);
			} else {
				this.autocompleateShortList.next(false);

				const classificationType: AllClassification = await this.getClassifications();
				const customsBookType: string = this.headerService.getSearchState(true);
				const classifications: CustomClassification = classificationType[customsBookType];

				const groupedItems: GroupedCustomsItems = {};
				(classifications.sortedClassifications as any[]).forEach((key: Classification) => groupedItems[key.Classification] = []);

				customsItemsAutocomplateList.forEach((item) => {
					const classification: Classification = classifications[item.BaseCustomsItemID] as any || "Unrecognised"; // classifications ={["090000000"]:"XV"}
					groupedItems[classification.Classification].push(item);
				});

				this.groupedItems = groupedItems;
				this.groupCustomsItemsAutocomplateList.next(classifications.sortedClassifications as any[]);
				this.customsItemsAutocomplateList.next([]);
			}
		});
	}

	private async getClassifications() {
		return await this.cacheService.getByPromise('classifications', async () => {
			const respnse = await this.API_MainService.GetClassifications().toPromise();
			const allClassifications: AllClassification = (respnse as HttpResponse<any>).body;
			delete allClassifications["$id"];

			for (const bookType in allClassifications) {
				const classifications = allClassifications[bookType];
				delete classifications["$id"];
				allClassifications[bookType].sortedClassifications = Object.values(classifications).sort((a, b) => this.romanTool.comparetorObject(a, b, 'Classification')) as any[];

				(<any>classifications)[-1] = { Classification: "Remark", Description: "הערות משתמש" };
				(<Classification[]>allClassifications[bookType].sortedClassifications).unshift(<any>classifications[-1]);
				(<any>classifications)["-"] = { Classification: "Unrecognised", Description: "אחר" };
				(<Classification[]>allClassifications[bookType].sortedClassifications).push(<any>classifications['-']);
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
		this.customsItemsAutocomplateList.next([]);
		
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
