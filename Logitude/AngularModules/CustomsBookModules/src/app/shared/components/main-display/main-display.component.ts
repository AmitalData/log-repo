declare var window: any;
import { AfterViewInit, Component, HostListener, Input, OnInit, SimpleChanges } from '@angular/core';
import { DataRowComponent } from '../data-row/data-row.component';
import { DetailsFrameComponent } from '../details-frame/details-frame.component';
import { TableTopComponent, TableTopState } from '../table-top/table-top.component';
import { CommonModule, NgFor, NgForOf, NgIf, NgStyle } from '@angular/common';
import { trigger, style, animate, transition } from '@angular/animations';
//@ts-ignore
import { mockData } from '../../../../../mock_data';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { BehaviorSubject, filter } from 'rxjs';
import { SearchBy, SearchService } from '../page-top/service/top-page.service';
import { FormsModule, NgModel } from '@angular/forms';
import { HeaderService, searchState } from '../app-header/service/header.service';
import { FilterPopupService, FiltersSearch } from '../filter-popup/service/filter-popup.service';
import { AddCommentComponent } from '../add-comment/add-comment.component';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { FeatureLocator } from '../../../core/Infrastructure/Utilities/FeatureLocator';
import { InfrastructureDomainService } from '../../../core/Infrastructure/Services/InfrastructureDomainService';
import { LoginService } from '../../../core/Infrastructure/Services/LoginService';
import { ActivatedRoute, Router } from '@angular/router';
import { RomanToolService } from '../../services/roman-tool.service';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { PreferenceMenuComponent } from '../preference-menu/preference-menu';
import { PreferencesService } from '../preference-menu/PreferencesService';
import { AppTool } from '../../../core/Infrastructure/Tools';

@Component({
	selector: 'app-main-display',
	standalone: true,
	imports: [NgFor, NgForOf, NgIf, DataRowComponent, DetailsFrameComponent, TableTopComponent, AddCommentComponent, FormsModule, NgStyle, PreferenceMenuComponent],
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
	@Input() isFeaturePermessionCB: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	selectSearchBy: string = SearchBy.searchBy_form01;
	showDetails: boolean = false;
	showCommentsIsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	showRulesIsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);

	showAddComment: boolean = false;
	showCommentSidebar: boolean = false;
	private _filters;
	data: CB_CustomsItemComputedDataList[] = [];
	fullData: CB_CustomsItemComputedDataList[] = [];
	originalDataByIsDiscountCodes: CB_CustomsItemComputedDataList[] = [];

	KeyValue = Object.keys;
	Object: ObjectConstructor = Object;
	cbTariffList: CB_TariffList[];
	cbRequirementComputedDataList: CB_RequirementComputedDataList[];
	isExpand: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	defualtCbCollapseSearchHierarchy: boolean = false;

	constructor(private API_MainService: API_MainService, private searchService: SearchService, private headerService: HeaderService, private preferencesService: PreferencesService,
		private route: ActivatedRoute, private filterPopupService: FilterPopupService, private addCommentService: AddCommentService, private loginService: LoginService,
		private myInfrastructureDomainService: InfrastructureDomainService, private router: Router, private romanTool: RomanToolService) {
		this.screenWidth = window.innerWidth;
	}
	searchState: string = searchState.יבוא;


	ngOnInit() {
		sessionStorage.removeItem('FullClassification');
		this.headerService.searchState$.subscribe((data) => {
			if (!searchState[data]) return;

			if (this.searchState != searchState[data]) {
				this.searchState = searchState[data];
				this.InitData();
			}
		});
		// listen to loading mode changes:
		this.isLoadingMode.subscribe((isLoading) => {
			this.isLoading = isLoading;
		});
		this.checkDefaultCB_CollapseSearchHierarchy();
		this.ListenToItemsSearched();
		this.getByIsDiscountCodes();
		this.updateFullClassificationByClick();
	}

	InitData() {
		if (SessionInfo.LoggedUserTenant == 0) this.GetAllCustomsBookMainView();
		else this.checkIsFeaturePermessionCustomsBook(() => this.GetAllCustomsBookMainView());

		this.getCustomsBookLastUpdateDate();
		this.isFeaturePermessionCB.subscribe((isFeaturePermessionCB) => {
			this.isFeaturePermessionCBMsg = isFeaturePermessionCB;
		});
	}

	getCustomsBookLastUpdateDate() {

		this.API_MainService.GetCustomsBookLastUpdateDateByTenant(SessionInfo.LoggedUserTenant).subscribe(
			(data: any) => {

				const result = data.body;
				if (!result) return;
				console.log(result);
				this.headerService.setLastUpdateTaskScheduled(result);
			},
			(error) => {
				this.isLoadingMode.next(false);
				this.itemsData.next([]);
				console.log(error.message);
			}
		);
	}

	checkDefaultCB_CollapseSearchHierarchy() {
		this.API_MainService.GetDefaultCB_CollapseSearchHierarchy(SessionInfo.LoggedUserTenant).subscribe((data: any) => {
			if (!data?.body) return;
			this.defualtCbCollapseSearchHierarchy = data?.body === true;
		});
	}

	errorPermessionCustomsBook: string = "You have no permission to access this feature";
	checkIsFeaturePermessionCustomsBook(onSuccess: () => void) {
		this.loginService.GetObjectTables().subscribe((myResult: any) => {
			if (!myResult) return true;
			window.ObjectTables = myResult;
			this.myInfrastructureDomainService.GetAllowedFeaturesForLoggedUser().subscribe((myResponse: any) => {
				if (!myResponse) return true;
				if (!FeatureLocator.HasFeaturePermession("Customs.CB_CustomsItemComputedData", "CustomsBookFeature")) {
					this.data = [];
					this.fullData = [];
					this.originalDataByIsDiscountCodes = [];
					this.isLoadingMode.next(false);
					this.isFeaturePermessionCB.next(true);
				}
				else {
					this.isFeaturePermessionCB.next(false);
					onSuccess();
				}
			});
		});
	}
	getByIsDiscountCodes() {
		this.headerService.IsDiscountCodes.subscribe((value) => {
			this.IsDiscountCodes = value;
			if (this.searchService.GetSearchText()) this.getSearchDataByFilter(this.filterPopupService.getFilters());
			else this.InitData();
		});
	}

	IsDiscountCodes: boolean = false;
	GetAllCustomsBookMainView() {
		this.isLoadingMode.next(true);
		let filters: Filters = {
			CustomsBookType: this.searchState,
			Tenant: SessionInfo.LoggedUserTenant,
			SearchFields: ''
		};
		filters.IsDiscountCodes = this.headerService.IsDiscountCodes?.getValue();
		this.getRulesData();
		this.getCommentsData(SessionInfo.LoggedUserTenant);

		this.API_MainService.GetCustomsBookMainView(filters).subscribe((data: any) => {
			const result: CB_CustomsItemComputedDataList[] = data.body;
			if (!result) return; // TODO: add error message
			this.countSearchResult = 0;
			this.handleClearResults();

			this.data = this.orderedData(result);
			if (this.data.length > 0) {
				if (this.IsDiscountCodes) this.originalDataByIsDiscountCodes = this.data;
				else this.fullData = this.data;
			}

			this.searchMode = TableTopState.ViewAll;
			this.isLoadingMode.next(false);
			this.isFeaturePermessionCB.next(false);
		});
	}

	allRulesData = [];
	getRulesData() {
		this.API_MainService.GetAllCustomsBookRulesData().subscribe((data: any) => {
			if (!data.body) return; // TODO: add error message
			// Clean up spaces by replacing multiple &nbsp; with a single space, then condense extra spaces
			let rules = data.body;
			rules.forEach(rule => {
				rule.Rules = rule.Rules.replace(/(&nbsp;)+/g, ' ').replace(/\s+/g, ' ').trim();
			});
			this.allRulesData = rules;
		});
	}

	allCommentsData = [];
	getCommentsData(tenant) {
		this.API_MainService.GetAllComments(tenant).subscribe((data: any) => {
			if (!data.body) return; // TODO: add error message
			this.allCommentsData = data?.body;
			this.addCommentService.fullCommentsData.next(this.allCommentsData);
		});
	}

	isLoading: boolean = false;
	isFeaturePermessionCBMsg: boolean = false;
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
				if (this.defualtCbCollapseSearchHierarchy && this.searchService.selectSearchBy === SearchBy.searchBy_form01)
					this.toggleVisibilitySearch(true, this.data);
				else
					this.toggleVisibility(true, this.data);
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
	showRulesOpen(isOpenRule: boolean) {
		this.showRulesIsOpen.next(isOpenRule);
	}

	onToggleAll(event: Event, item: CB_CustomsItemComputedDataList): void {
		const checked = (event.target as HTMLInputElement)?.checked;
		if (item.ItemHierarchicLocationID == "2")
			item.checked = checked;
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

	toggleVisibilitySearch(expend: boolean, data: CB_CustomsItemComputedDataList[]) {
		const searchTextLength = this.searchService.GetSearchText().length;
		const maxLevel = searchTextLength <= 3 ? 2 : searchTextLength - 1;

		data.forEach(item => {
			const itemLevel = Number(item.ItemHierarchicLocationID);
			item.checked = itemLevel > maxLevel ? false : expend;
			this.showChildern(item.checked, item, item.checked);

			if (item.children?.length) {
				this.toggleVisibilitySearch(expend, item.children);
			}
		});
	}


	toggleVisibility(expend: boolean, data: CB_CustomsItemComputedDataList[]) {
		data.forEach(item => {
			if (item.ItemHierarchicLocationID == "2")
				item.checked = expend;
			this.showChildern(expend, item, true);

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
			this.updateShowDetailsClick(item);

			return;
		}
		else if (!this.showDetails) {
			this.updateShowDetailsClick();
		}
		this.currentItem.next(item);
		return this.showDetails;
	}

	updateShowDetailsClick(item?: CB_CustomsItemComputedDataList) {
		this.showDetails = !this.showDetails;
		this.showDetailsOpen.next(this.showDetails);
		this.preferencesService.showSettingsClick(false);
		if (!this.showDetails) {
			this.showCommentsIsOpen.next(false);
			this.showRulesIsOpen.next(false);
		}
		else {
			this.filterPopupService.toggleFilterPopup(false);
			if (item)
				this.currentItem.next(item);
		}
	}

	updateUrlWithClassification(fullClassification: string) {
		sessionStorage.setItem('FullClassification', fullClassification);
		const url = this.searchService.getDecodeUrl(window.location.href);
		const currentUrl = new URL(url);
		if (fullClassification) {
			currentUrl.searchParams.set("FullClassification", fullClassification);
			window.history.replaceState({}, "", currentUrl.toString());
		}
		else {
			currentUrl.searchParams.delete("FullClassification");
			window.history.replaceState({}, "", currentUrl.toString());
		}
	}

	updateFullClassificationByClick() {
		this.showDetailsOpen.subscribe((isOpen) => {
			if (!isOpen)
				this.updateUrlWithClassification(null);
		});
		this.currentItem.subscribe((item) => {
			if (item && this.showDetails)
				this.updateUrlWithClassification(item?.FullClassification);
		});
	}

	ngOnChanges(changes: SimpleChanges) {
		if (changes['showDetails']) {
			this.showDetails = changes['showDetails'].currentValue;
			this.showDetailsOpen.next(this.showDetails);
			if (!this.showDetails) {
				this.showCommentsIsOpen.next(false);
				this.showRulesIsOpen.next(false)
			}
		}
		if (changes['itemsData']) {
			this.itemsData = changes['itemsData'].currentValue;
		}
	}


	showChildern(openAction: any, item: CB_CustomsItemComputedDataList, isMultiOpen: boolean = false) {
		item.IsShowChildren = openAction; // #109074- fix open children display
		if (openAction) this.scrollDown(item, isMultiOpen);  // #114429- fix scroll to the last child
	}

	scrollDown(item: CB_CustomsItemComputedDataList, isMultiOpen: boolean) {
		setTimeout(() => {
			const element = document.getElementById(`${item.CustomsItemID}`);
			if (element) {
				const { bottom } = element.getBoundingClientRect();
				if (bottom > window.innerHeight - 150 && !isMultiOpen)
					window.scrollBy({ top: bottom - window.innerHeight + 200, behavior: 'smooth' });
			}
		}, 0);
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
		if (SessionInfo.LoggedUserTenant == 0) this.getSearchDataByFilter(filtersSearch);
		else this.checkIsFeaturePermessionCustomsBook(() => this.getSearchDataByFilter(filtersSearch));
	}

	getSearchDataByFilter(filtersSearch: FiltersSearch) {
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
		if (this.IsDiscountCodes)
			filters.IsDiscountCodes = this.IsDiscountCodes;

		this.selectSearchBy = this.searchService.selectSearchBy;

		if (SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.isLoadingMode.next(true); // update loading mode
			if (filters.CustomsItemHierarchic === '') {
				// filters.Reamarks = true;
				// filters.Rules = true;
				filters.Reamarks = false;
				filters.Rules = false;
				filters.CustomsItemHierarchic = this.searchService.customsItemHierarchicDefault;
			}

			if (filters.CustomsItemHierarchic == "6" || filters.CustomsItemHierarchic == "7" || filters.CustomsItemHierarchic == "6,7") {
				filters.CustomsItemHierarchic = null;
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
				filters.Reamarks = false;
				filters.Rules = false;
				// filters.Reamarks = true;
				// filters.Rules = true;
			}

			if (filters.CustomsItemHierarchic == "6" || filters.CustomsItemHierarchic == "7" || filters.CustomsItemHierarchic == "6,7") {
				filters.CustomsItemHierarchic = null;
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

	clearSearchValue() {
		const url = this.searchService.getDecodeUrl(window.location.href);
		let searchValue = this.searchService.getParameterByName('searchValue', url);
		if (!AppTool.IsNullOrEmpty(searchValue)) {
			const currentUrl = new URL(url);
			currentUrl.searchParams.set("searchValue", "");
			window.history.replaceState({}, "", currentUrl.toString());
			window.location.reload();
		}
	}

	handleClearResults() {
		this.clearSearchValue();
		this.searchMode = TableTopState.ViewAll;
		this.selectedItemId = null;
		this.showDetails = false;
		this.showDetailsOpen.next(this.showDetails);
		this.showCommentsIsOpen.next(false);
		this.showRulesIsOpen.next(false)
		this.data = [];
		this.searchValue = "";
		this.countSearchResult = 0;
		this.filterPopupService.toggleFilterPopup(false);
		this.data = !this.IsDiscountCodes ? this.fullData : this.originalDataByIsDiscountCodes;
		if (this.IsDiscountCodes && this.originalDataByIsDiscountCodes?.length == 0) this.GetAllCustomsBookMainView();
		this.toggleVisibility(false, this.data);
		this.preferencesService.showSettingsClick(false);
	}

	public orderedDataForSearch = (data) => {
		const getChildren = (parentItem) => {
			const children = data.filter((item) => item?.CI_Parent_CustomsItemIDNum === parentItem?.CustomsItemID);
			children.forEach((child) => {
				child.children = getChildren(child);
				// delete from rootItems value the children :
				rootItems = deleteFromRootChildrens(child.CustomsItemID);
			});
			return children;
		};

		const deleteFromRootChildrens = (CustomsItemID) => {
			rootItems = rootItems.filter((x) => x.CustomsItemID != CustomsItemID);
			return rootItems;
		};

		if (this.allRulesData?.length > 0 || this.allCommentsData?.length > 0) {
			data.forEach(item => {
				if (this.allRulesData?.length > 0)
					item.rulesData = this.allRulesData?.filter((x) => x.CustomsItemID == item.CustomsItemID);
				if (this.allCommentsData?.length > 0)
					item.remarksClassificationList = this.allCommentsData?.filter((x) => x.CustomsItemsID == item.CustomsItemID);
			});
		}

		let rootItems = data;
		if (rootItems?.length == 0) return;

		this.romanTool.sortArry(rootItems, 'FullClassification');

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

		if (this.allRulesData?.length > 0 || this.allCommentsData?.length > 0) {
			data.forEach(item => {
				if (this.allRulesData?.length > 0)
					item.rulesData = this.allRulesData?.filter((x) => x.CustomsItemID == item.CustomsItemID);
				if (this.allCommentsData?.length > 0)
					item.remarksClassificationList = this.allCommentsData?.filter((x) => x.CustomsItemsID == item.CustomsItemID);

			});
		}

		let rootItems = data.filter((item) => !item?.CI_Parent_CustomsItemIDNum);
		if (rootItems.length == 0) return;

		this.romanTool.sortArry(rootItems, 'FullClassification');

		const orderedData = rootItems.map((rootItem) => {
			const children = getChildren(rootItem);
			return { ...rootItem, children };
		});

		return orderedData;
	};

	screenWidth: number;
	// Get current screen width
	@HostListener('window:resize', ['$event'])
	onResize(event: Event): void {
		this.screenWidth = (event.target as Window).innerWidth;
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

export class MainEntity {
	CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[];
	CB_TariffList: CB_TariffList[];
	CB_RequirementComputedDataList: CB_RequirementComputedDataList[];
	CustomItemClassifGuidanceResult: CustomItemClassifGuidanceResult[];
	Mekach: Mekach[];

	constructor(CB_CustomsItemComputedDataList: CB_CustomsItemComputedDataList[], CB_TariffList: CB_TariffList[], CB_RequirementComputedDataList: CB_RequirementComputedDataList[], CustomItemClassifGuidanceResult: CustomItemClassifGuidanceResult[], Mekach: Mekach[]) {
		this.CB_CustomsItemComputedDataList = CB_CustomsItemComputedDataList;
		this.CB_TariffList = CB_TariffList;
		this.CB_RequirementComputedDataList = CB_RequirementComputedDataList;
		this.CustomItemClassifGuidanceResult = CustomItemClassifGuidanceResult;
		this.Mekach = Mekach;
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
	CustomsRate: string; // ממס קניה
	PurchaseTax: string; // מכס כללי
	OptionalTaxAddition?: number;
	MeasurementUnitName: string;
	Remarks: string;
	SearchByTextResult: string;
	children: CB_CustomsItemComputedDataList[];
	IsShowChildren: boolean;
	checked: boolean;
	remarksClassificationList?: RemarksClassificationList[];
	rulesDetailsList?: RulesDetailsList[];
	agreementsList?: CB_TariffList[];
	requirementComputedDataList?: CB_RequirementComputedDataList[];
	Rules: boolean;
	rulesData?: any[];
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
	IsVoluntaryOrImporterOfTrust: boolean;
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
	Index: string;
	ParentID: number;
	UpdateDate: Date;
	ChangeRequestTypePriority: number;
	OrderinalPostion: number;
	EntityStatusID: string;
	customsItemId?: number;
	CB_ID?: number;
}

export class CustomItemClassifGuidanceResult {
	classificationGuidanceNumber: string;
	title: string;
	classificationGuidanceTypeName: string;
	fullClassification: string;
	publicationDate?: Date;
	customsItemId?: number;
}

export class ClassifGuidanceDetailsResponseData {
	classificationGuidanceNumber: string;
	title: string;
	classificationGuidanceTypeName: string;
	fullClassificationItem: string;
	createDate: Date;
	expirationDate?: Date;
	publicationDate: Date;
	classificationGuidanceTextRTF: string;
	classifGuidanceAttached: ClassifGuidanceAttached[] = [];
}

export class ClassifGuidanceAttached {
	fullClassification: string;
	attachedCustomsItemID: number;
}

export class Mekach {
	mekachNumber: number; // מס מק"ת/מק"ח
	attachedMekahFile: string; // מזהה קובץ מצורף
	validityDate: Date; // בתוקף מיום
	changeDescription: string; // דברי הסבר
	customsItemId?: number;
	tenant?: number;
}
export class AttachedMekahFileData {
	attachmentID: string;
	fileName: string;
	content: string;
}

export class AttachmentResponseData {
	AttachedMekahFileData: AttachedMekahFileData;
}
