import { AfterViewInit, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { DataRowComponent } from '../data-row/data-row.component';
import { DetailsFrameComponent } from '../details-frame/details-frame.component';
import { TableTopComponent, TableTopState } from '../table-top/table-top.component';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { trigger, style, animate, transition } from '@angular/animations';
//@ts-ignore
import { mockData } from '../../../../../mock_data';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { BehaviorSubject, filter } from 'rxjs';
import { SearchBy, SearchService } from '../page-top/service/top-page.service';
import { FormsModule } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { FiltersSearch } from '../filter-popup/service/filter-popup.service';
import { AddCommentComponent } from '../add-comment/add-comment.component';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
@Component({
	selector: 'app-main-display',
	standalone: true,
	imports: [NgFor, NgForOf, NgIf, DataRowComponent, DetailsFrameComponent, TableTopComponent, AddCommentComponent, FormsModule],
	templateUrl: './main-display.component.html',
	styleUrl: './main-display.component.css',
	animations: [
		trigger('inOutAnimation', [
			transition(':enter', [
				style({ height: '0px', opacity: '0' }),
				animate('0.5s ease-in-out', style({ height: '*', opacity: '1' })),
			]),
			transition(':leave', [
				style({ height: '*', opacity: '1' }),
				animate('0.5s ease-in-out', style({ height: '0px', opacity: '0' })),
			]),
		]),
	],
})
export class MainDisplayComponent implements OnInit {
	@Input() showChiledren: boolean = false;
	@Input() itemsData: BehaviorSubject<CB_CustomsItemComputedDataList[]>;
	@Input() isLoadingMode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	selectSearchBy: string = SearchBy.searchBy_form01;
	showDetails: boolean = false;
	showCommentsIsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

	showAddComment: boolean = false;
	showCommentSidebar: boolean = false;
	// childrenToDesplay: number[] = [];
	private _filters;
	//data: any | never | undefined = {};
	data: CB_CustomsItemComputedDataList[] = [];
	fullData: CB_CustomsItemComputedDataList[] = [];
	KeyValue = Object.keys;
	Object: ObjectConstructor = Object;
	cbTariffList: CB_TariffList[];
	cbRequirementComputedDataList: CB_RequirementComputedDataList[];
	isExpand: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

	constructor(private API_MainService: API_MainService, private searchService: SearchService, private headerService: HeaderService) { }
	searchState: string = searchState.יבוא;

	ngOnInit() {
		this.headerService.searchState$.subscribe((data) => {
			if (!searchState[data]) return;
			this.searchState = searchState[data];
			this.InitData();
			this.ListenToItemsSearched();
		});
	}

	InitData() {
		let filters: Filters = {
			CustomsBookType: this.searchState,
			Tenant: SessionInfo.LoggedUserTenant,
			SearchFields: ''
		};
		this.isLoadingMode.next(true);
		this.API_MainService.GetCustomsBookMainView(filters).subscribe((data: any) => {
			const result: CB_CustomsItemComputedDataList[] = data.body;
			if (!result) return; // TODO: add error message
			this.countSearchResult = 0;
			this.handleClearResults();
			this.fullData = this.orderedData(result);
			this.data = this.fullData;
			this.searchMode = TableTopState.ViewAll;
			this.isLoadingMode.next(false);
		});

		// listen to loading mode changes:
		this.isLoadingMode.subscribe((isLoading) => {
			this.isLoading = isLoading;
		});
	}

	isLoading: boolean = false;
	searchValue: string = '';
	countSearchResult: number = 0;
	ListenToItemsSearched() {
		// listen to search text changes:
		this.searchService.searchText$.subscribe((searchText) => {
			if (searchText === "") this.handleClearResults();
		});


		// listen to itemsData changes:
		this.itemsData.subscribe((data: CB_CustomsItemComputedDataList[] = []) => {
			if (data.length == 0 && this.searchService.GetSearchText() !== "") {
				this.data = [];
				this.countSearchResult = 0;
				this.searchMode = TableTopState.Search;
				this.searchValue = "";
				return;
			}

			if (this.itemsData.getValue().length > 0) {
				// remove duplicates customsItemID:
				data = data.filter((v, i, a) => a.findIndex(t => (t.CustomsItemID === v.CustomsItemID)) === i);

				this.countSearchResult = data.length;
				// update list:
				this.data = this.orderedDataForSearch(data);
				this.searchToggleAllChildren(true); // expand all 

				this.searchMode = TableTopState.Search;
				this.searchValue = this.searchService.GetSearchText();
				if (this.showDetails) {
					this.selectedItemId = null;
					this.updateShowDetailsClick();
				}
			}
			else this.countSearchResult = 0;
		});
	}

	showCommentsOpen(isOpenComment: boolean) {
		this.showCommentsIsOpen.next(isOpenComment);
	}

	onToggleAll(event: Event, item: CB_CustomsItemComputedDataList): void {
		const checked = (event.target as HTMLInputElement)?.checked;
		this.toggleVisibility(checked, item.children);
	}


	searchToggleAllChildren(expend: boolean) {
		this.toggleVisibilitySearch(expend, this.data); // Assuming this.data is your main data array

		// Find all child checkboxes using class selector and update their checked state class name-.mainTable_itemChkAllCheckBox
		setTimeout(() => {
			const childCheckboxes: HTMLCollection = document.getElementsByClassName('mainTable_itemChkAllCheckBox');
			for (let i = 0; i < childCheckboxes.length; i++) {
				(childCheckboxes[i] as HTMLInputElement).checked = expend;
			}
		}, 0);
	}

	toggleVisibilitySearch(expend: boolean, data: CB_CustomsItemComputedDataList[]): boolean {

		let shouldExpandParent = false;

		data.forEach(item => {
			// Check if the current item's FullClassification contains the search text
			const searchText = this.searchService.GetSearchText();
			const containsSearchText = item.FullClassification.includes(searchText);

			// Recursively check if any children should be expanded
			let shouldExpandChildren = false;
			if (item.children && item.children.length > 0) {
				shouldExpandChildren = this.toggleVisibilitySearch(expend, item.children);
			}

			// Determine if the current item should be expanded
			if (containsSearchText || shouldExpandChildren) {

				// #109247
				const itemHierarchicLocationID: number = Number(item.ItemHierarchicLocationID);
				if (searchText.length == 2 && itemHierarchicLocationID > 2) {
					return;
				}
				if (searchText.length == 4 && itemHierarchicLocationID > 3) {
					return;
				}
				this.showChildern(expend, item);
				shouldExpandParent = true;
			}
		});

		// Return whether this branch should be expanded to the parent call
		return shouldExpandParent;
	}

	toggleVisibility(expend: boolean, data: CB_CustomsItemComputedDataList[]) {
		data.forEach(item => {
			this.showChildern(expend, item);

			if (item.children && item.children.length > 0) {
				this.toggleVisibility(expend, item.children); // Recursively toggle children
			}
		});
	};

	showDetailsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	searchMode: TableTopState = TableTopState.ViewAll;
	selectedItemId: number | null = null;
	currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);

	showDetailsClick(CustomsItemID: number, item: CB_CustomsItemComputedDataList) {
		this.selectedItemId = CustomsItemID;
		if (this.currentItem.getValue()?.CustomsItemID == CustomsItemID) {
			// this.showDetails = !this.showDetails;
			// this.showDetailsOpen.next(this.showDetails);
			this.updateShowDetailsClick();
			return;
		}
		else if (!this.showDetails) {
			// this.showDetails = !this.showDetails;
			// this.showDetailsOpen.next(this.showDetails);
			this.updateShowDetailsClick();
		}
		this.currentItem.next(item);
		return this.showDetails;
	}

	updateShowDetailsClick() {
		this.showDetails = !this.showDetails;
		this.showDetailsOpen.next(this.showDetails);
		if (!this.showDetails) this.showCommentsIsOpen.next(false);
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes['showDetails']) {
			this.showDetails = changes['showDetails'].currentValue;
			this.showDetailsOpen.next(this.showDetails);
			if (!this.showDetails) this.showCommentsIsOpen.next(false);
		}
		if (changes['itemsData']) {
			this.itemsData = changes['itemsData'].currentValue;
		}
	}


	showChildern(openAction: any, item: CB_CustomsItemComputedDataList) {
		item.IsShowChildren = openAction; // #109074- fix open children display

		// const isShown = this.childrenToDesplay.indexOf(item.CustomsItemID);
		// if (openAction && isShown === -1) {
		// 	this.childrenToDesplay.push(item.CustomsItemID);
		// }
		// else if (!openAction && isShown !== -1) {
		// 	this.childrenToDesplay.splice(isShown);
		// }
	}

	getCustomsItemHierarchic(filtersSearch: FiltersSearch): string {
		const selectedFilters = [];
		if (filtersSearch.parts) selectedFilters.push(FilterOption.Parts);
		if (filtersSearch.chapters) selectedFilters.push(FilterOption.Chapters);
		if (filtersSearch.details) selectedFilters.push(FilterOption.Details);
		if (filtersSearch.sections) selectedFilters.push(FilterOption.Sections);
		if (filtersSearch.customsDetails) selectedFilters.push(FilterOption.CustomsDetails);
		if (filtersSearch.rules) selectedFilters.push(FilterOption.Rules);
		if (filtersSearch.remarks) selectedFilters.push(FilterOption.Remarks);
		return selectedFilters.join(',');
	}

	filtersSearchClick(filtersSearch: FiltersSearch) {
		let filters: Filters = {
			SearchFields: this.searchService.GetSearchText(),
			CustomsBookType: this.searchState,
			CustomsItemHierarchic: this.getCustomsItemHierarchic(filtersSearch),
			Reamarks: filtersSearch.remarks,
			Rules: filtersSearch.rules,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: SessionInfo.LoggedUserTenant
		};

    this.selectSearchBy = this.searchService.selectSearchBy;

		if (SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.isLoadingMode.next(true); // update loading mode
			if (filters.CustomsItemHierarchic === '') {
				filters.Reamarks = true;
				filters.Rules = true;
				filters.CustomsItemHierarchic = this.searchService.customsItemHierarchicDefault;
			}
			this.API_MainService.GetCustomsBookMainViewSearchByClassification(filters).subscribe(
				(data: any) => {
					const result: CB_CustomsItemComputedDataList[] = data.body;
					if (!result) return; // TODO: add error message
					this.itemsData.next(result);
					this.isLoadingMode.next(false); // update loading mode
				},
				(error) => {
					this.isLoadingMode.next(false); // update loading mode
					this.itemsData.next([]);
					console.log(error.message);
				}
			);
		}
		else if (SearchBy.pageSearch_form02 == this.selectSearchBy) {// spacial search by text
			this.isLoadingMode.next(true); // update loading mode
			if (filters.CustomsItemHierarchic === '') {
				filters.CustomsItemHierarchic = this.searchService.customsItemHierarchicDefault;
				filters.Reamarks = true;
				filters.Rules = true;
			}
			this.API_MainService.GetCustomsBookMainViewSearchByText(filters).subscribe(
				(data: any) => {
					const result: CB_CustomsItemComputedDataList[] = data.body;
					if (!result) return; // TODO: add error message
					this.itemsData.next(result);
					this.isLoadingMode.next(false); // update loading mode
				},
				(error) => {
					this.isLoadingMode.next(false); // update loading mode
					this.itemsData.next([]);
					console.log(error.message);
				}
			);
		}
	}

	handleClearResultsClick() {
		this.handleClearResults();
		this.searchService.SetSearchText("");
	}

	handleClearResults() {
		// if (this.searchMode === TableTopState.ViewAll) return;
		this.searchMode = TableTopState.ViewAll;
		this.selectedItemId = null;
		this.showDetails = false;
		this.showDetailsOpen.next(this.showDetails);
		this.showCommentsIsOpen.next(false);
		this.data = [];
		this.searchValue = "";
		this.countSearchResult = 0;
		this.data = this.fullData;
		this.searchToggleAllChildren(false);
	}

	public orderedDataForSearch = (data) => {
		const getChildren = (parentItem) => {
			const children = data.filter((item) => item?.CI_Parent_CustomsItemIDNum === parentItem?.CustomsItemID);
			children.forEach((child) => {
				child.children = getChildren(child);
			});
			return children;
		};

		let rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);
		if (rootItems.length === 0) {
			rootItems = data;
		}
		this.sortByFullClassification(rootItems);

		const orderedData = rootItems.map((rootItem) => {
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});

		// Remove root items that are found as children of other items
		const removeRootItemsAsChildren = (items) => {
			return items.filter((item) => {
				const foundAsChild = items.some((otherItem) => {
					if (otherItem.children) {
						return otherItem.children.some((child) => child.CustomsItemID === item.CustomsItemID);
					}
					return false;
				});
				return !foundAsChild;
			});
		};

		// Filter out root items that are found as children
		const filteredOrderedData = removeRootItemsAsChildren(orderedData);

		return filteredOrderedData;
	};

	public orderedData = (data) => {
		const getChildren = (parentItem) => {
			const children = data.filter((item) => item?.CI_Parent_CustomsItemIDNum === parentItem?.CustomsItemID);
			children.forEach((child) => {
				child.children = getChildren(child);
			});
			return children;
		};
		let rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);
		if (rootItems.length == 0) return;

		// order by FullClassification number
		this.sortByFullClassification(rootItems);

		const orderedData = rootItems.map((rootItem) => {
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});

		return orderedData;
	};

	// Function to convert Roman numeral to integer
	private romanToInt(roman: string): number {
		const romanMap: { [key: string]: number } = { I: 1, V: 5, X: 10, L: 50, C: 100, D: 500, M: 1000 };
		return roman.split('').reduce((num, char, i, arr) =>
			num + (romanMap[char] < romanMap[arr[i + 1]] ? -romanMap[char] : romanMap[char]), 0);
	}

	// Function to sort the list based on FullClassification
	sortByFullClassification(items: CB_CustomsItemComputedDataList[]): CB_CustomsItemComputedDataList[] {
		return items.sort((a, b) => this.romanToInt(a.FullClassification) - this.romanToInt(b.FullClassification));
	}
}

export enum FilterOption {
	Parts = '1',
	Chapters = '2',
	Details = '3',
	Sections = '4',
	CustomsDetails = '5',
	Rules = '6',
	Remarks = '7'
}

export interface ItemData {
	customsItemId: number;
	measurementUnitMalamId: number;
}
export class MainEntity {
	CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[];
	CB_TariffList: CB_TariffList[];
	CB_RequirementComputedDataList: CB_RequirementComputedDataList[];

	constructor(CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[], CB_TariffList: CB_TariffList[], CB_RequirementComputedDataList: CB_RequirementComputedDataList[]) {
		this.CB_CustomsItemComputedDataList = CB_CustomsItemComputedDataList;
		this.CB_TariffList = CB_TariffList;
		this.CB_RequirementComputedDataList = CB_RequirementComputedDataList;
	}
}


export interface CB_CustomsItemComputedDataList {
	CB_ID: string;
	ID: number;
	CustomsItemID: number;
	FullClassification: string;
	IsLeaf: boolean;
	CustomsItemDetailsHistoryID: number;
	PropertiesDetailsHistoryID: number;
	PH_MeasurementUnitID?: number;
	IsHistoryExists: boolean;
	IsRulesExists: boolean;
	StartDate: Date;
	EndDate: Date;
	CI_Parent_CustomsItemIDNum?: number;
	CI_BaseFullClassification: string;
	CI_ComputedCheckDigit: string;
	CI_CustomsBookTypeIDNum: string;
	CI_CustomsItemCategoryIDNum: string;
	ItemHierarchicLocationID: string;
	CIH_Title: string;
	CIH_GoodsDescription: string;
	CustomsItemEntityStatusIDNum: number;
	PH_IsCarItem?: boolean;
	FullGoodsDescription: string;
	Agreements?: number;
	CustomsRate: string;
	PurchaseTax: string;
	OptionalTaxAddition?: number;
	MeasurementUnitName: string;
	Remarks: string;
	SearchByTextResult: string;
	children: CB_CustomsItemComputedDataList[];
	IsShowChildren: boolean;
}

export interface CB_RequirementComputedDataList {
	CB_ID: string;
	ID: number;
	RegularityRequirementID: number;
	CountryID?: number;
	CustomsItemID?: number;
	StartDate: Date;
	EndDate: Date;
	RegularitySourceCodeID: string;
	CreateDate?: Date;
	InceptionCodeID: string;
	RegularityPublicationCodeID: string;
	AutonomyCustomsItemID?: number;
	IsAllCustomsItems: boolean;
	RequirementValidOrigin: string;
	RequirementGoodsDescription: string;
	Authority: string;
	ConfirmationType: string;
	InterConditionsRelationship: string;
	TextualCondition: string;
	IsPersonalImportIncluded?: boolean;
	IsCarnetIncluded?: boolean;
	FromEpisodeDetail: string;
	AutonomyRegion: string;
}

export interface CB_TariffList {
	ID: number;
	CreateDate: Date;
	UpdateDate?: Date;
	TradeAgreementID?: number;
	CustomsItemID: number;
	Title: string;
	CB_ID: string;
	Country: string;
	CustomsRate: string;
	CustomsRateWithinQuota: string;
	QuotaID?: number;
	MeasurementUnitName: string;
	OptionalTaxAddition?: number;
	StartDate?: Date;
	EndDate?: Date;
	TradeAgreementName: string;
}
export interface RemarksClassificationPM {
	id?: string;
	tenant: number;
	customsItemsID: number;
	remarkDescription: string;
}

export interface RemarksClassificationList {
	Id: string;
	Tenant: number;
	CustomsItemsID: number;
	RemarkDescription: string;
}
export interface RulesDetailsList {
	ID: number;
	RuleID: number;
	Title: string;
	Rules: string;
	UpdateDate: Date;
	ChangeRequestTypePriority: number;
	OrderinalPostion: number;
	EntityStatusID: string;
	Parent_RuleDetailsHistoryID: number;
}
