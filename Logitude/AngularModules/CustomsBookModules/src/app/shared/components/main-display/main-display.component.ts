import { AfterViewInit, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { DataRowComponent } from '../data-row/data-row.component';
import { DetailsFrameComponent } from '../details-frame/details-frame.component';
import { TableTopComponent, TableTopState } from '../table-top/table-top.component';
import { AddCommentComponent } from '../add-comment/add-comment.component';
import { NgFor, NgForOf, NgIf } from '@angular/common';
import { trigger, style, animate, transition } from '@angular/animations';
//@ts-ignore
import { mockData } from '../../../../../mock_data';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { BehaviorSubject, filter } from 'rxjs';
import { SearchService } from '../page-top/service/top-page.service';
import { FormsModule } from '@angular/forms';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HeaderService, searchState } from '../app-header/service/header.service';
@Component({
	selector: 'app-main-display',
	standalone: true,
	imports: [NgFor, NgForOf, NgIf, DataRowComponent, DetailsFrameComponent, TableTopComponent, AddCommentComponent, FormsModule, MatProgressSpinnerModule],
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
	showDetails: boolean = false;
	showAddComment: boolean = false;
	showCommentSidebar: boolean = false;
	childrenToDesplay: string[] = [];
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
			Tenant: 0,
			SearchFields: ''
		};
		this.API_MainService.GetCustomsBookMainView(filters).subscribe((data: CB_CustomsItemComputedDataList[]) => {
			this.countSearchResult = 0;

			this.fullData = this.orderedData(data);
			this.data = this.fullData;
			this.searchMode = TableTopState.ViewAll;
			this.isExpand.next(false);
		});
	}

	searchValue: string = '';
	countSearchResult: number = 0;
	ListenToItemsSearched() {
		// listen to search text changes:
		this.searchService.searchText$.subscribe((searchText) => {
			if (searchText === "") this.handleClearResults();
			// else if (this.itemsData.getValue().length > 0) {
			// 	this.searchMode = TableTopState.Search;
			// }
		});

		// listen to itemsData changes:
		this.itemsData.subscribe((data: CB_CustomsItemComputedDataList[] = []) => {
			if (data.length == 0 && this.searchService.GetSearchText() !== "") {
				//TODO: Add not results found message
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
				this.isExpand.next(true);

				this.searchMode = TableTopState.Search;
				this.searchValue = this.searchService.GetSearchText();
			}
			else this.countSearchResult = 0;
		});
	}

	onToggleAll(event: Event, item: CB_CustomsItemComputedDataList): void {
		const checked = (event.target as HTMLInputElement)?.checked;
		this.toggleVisibility(checked, item.children);
	}


	searchToggleAllChildren(expend: boolean) {
		this.toggleVisibility(expend, this.data); // Assuming this.data is your main data array

		// Find all child checkboxes using class selector and update their checked state class name-.mainTable_itemChkAllCheckBox
		setTimeout(() => {
			const childCheckboxes: HTMLCollection = document.getElementsByClassName('mainTable_itemChkAllCheckBox');
			for (let i = 0; i < childCheckboxes.length; i++) {
				(childCheckboxes[i] as HTMLInputElement).checked = expend;
			}
		}, 0);
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
	// itemDataBehaviorSubject: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>({ customsItemId: 0, measurementUnitMalamId: 0 });
	// itemData: ItemData = { customsItemId: 0, measurementUnitMalamId: 0 };
	showDetailsClick(CustomsItemID: number, item: CB_CustomsItemComputedDataList) {
		this.selectedItemId = CustomsItemID;
		if (this.currentItem.getValue()?.CustomsItemID == CustomsItemID) {
			this.showDetails = !this.showDetails;
			this.showDetailsOpen.next(this.showDetails);
			return;
		}
		else if (!this.showDetails) {
			this.showDetails = !this.showDetails;
			this.showDetailsOpen.next(this.showDetails);
		}

		// this.itemData.customsItemId = CustomsItemID;
		// this.itemData.measurementUnitMalamId = 0; // change it
		// this.itemDataBehaviorSubject.next(this.itemData);
		this.currentItem.next(item);
		return this.showDetails;
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes['showDetails']) {
			this.showDetails = changes['showDetails'].currentValue;
			this.showDetailsOpen.next(this.showDetails);

		}
		if (changes['itemsData']) {
			this.itemsData = changes['itemsData'].currentValue;
		}
	}


	showChildern(openAction: any, item: CB_CustomsItemComputedDataList) {
		const isShown = this.childrenToDesplay.indexOf(item.CIH_GoodsDescription);
		if (openAction && isShown === -1) {
			this.childrenToDesplay.push(item.CIH_GoodsDescription);
		}
		else if (!openAction && isShown !== -1) {
			this.childrenToDesplay.splice(isShown);
		}
		// isShown === -1 ? this.childrenToDesplay.push(id) : this.childrenToDesplay.splice(isShown);
		// return Boolean(isShown >= 0);
	}



	handleClearResultsClick() {
		this.handleClearResults();
		this.searchService.SetSearchText("");
	}

	handleClearResults() {
		if (this.searchMode === TableTopState.ViewAll) return;
		this.searchMode = TableTopState.ViewAll;
		this.selectedItemId = null;
		this.showDetails = false;
		this.showDetailsOpen.next(this.showDetails);

		this.data = [];
		this.searchValue = "";
		this.countSearchResult = 0;
		// this.InitData();
		this.data = this.fullData;
		this.searchToggleAllChildren(false);
		this.isExpand.next(false);
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
			// CHECK MOKE DATA:
			//const children = data.filter((item) => item?.Parent_CustomsItemID === parentItem?.ID);

			children.forEach((child) => {
				child.children = getChildren(child);
			});
			return children;
		};
		let rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);
		// CHECK MOKE DATA:
		//const rootItems = data.filter((item) => !item?.Parent_CustomsItemID);
		if (rootItems.length == 0) return;

		const orderedData = rootItems.map((rootItem) => {
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});

		return orderedData;
	};

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
