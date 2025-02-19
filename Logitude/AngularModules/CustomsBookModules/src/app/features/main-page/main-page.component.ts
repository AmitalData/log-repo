declare var window: any;
import { Component, OnInit } from '@angular/core';
import { PageTopComponent } from '../../shared/components/page-top/page-top.component';
import { CB_CustomsItemComputedDataList, FilterOption, MainDisplayComponent } from '../../shared/components/main-display/main-display.component';
import { AddCommentComponent } from '../../shared/components/add-comment/add-comment.component';
import { CommonModule } from '@angular/common';
import { API_MainService, Filters } from '../../core/API_MainService';
import { BehaviorSubject } from 'rxjs';
import { FilterPopupService, FiltersSearch } from '../../shared/components/filter-popup/service/filter-popup.service';
import { SearchBy, SearchService } from '../../shared/components/page-top/service/top-page.service';
import { HeaderService } from '../../shared/components/app-header/service/header.service';
import { AppHeaderComponent } from '../../shared/components/app-header/app-header.component';
import { SessionInfo } from '../../core/Infrastructure/Utilities/SessionInfo';
import { LoginService } from '../../core/Infrastructure/Services/LoginService';
import { InfrastructureDomainService } from '../../core/Infrastructure/Services/InfrastructureDomainService';
import { FeatureLocator } from '../../core/Infrastructure/Utilities/FeatureLocator';
import { PreferenceMenuComponent } from '../../shared/components/preference-menu/preference-menu';
import { CB_Preference, PreferencesService } from '../../shared/components/preference-menu/PreferencesService';

@Component({
	selector: 'app-main-page',
	standalone: true,
	imports: [PageTopComponent, MainDisplayComponent, AddCommentComponent, CommonModule, AppHeaderComponent, PreferenceMenuComponent],
	templateUrl: './main-page.component.html',
	styleUrl: './main-page.component.css',
})
export class MainPageComponent implements OnInit {
	HeaderService = new HeaderService();
	showAddComment: boolean = false;
	filterService = new FilterPopupService();
	itemsData: BehaviorSubject<CB_CustomsItemComputedDataList[]> = new BehaviorSubject<CB_CustomsItemComputedDataList[]>([]);
	isLoadingMode: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);;
	isFeaturePermessionCB: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);;
	private _filters;
	selectSearchBy: string;

	constructor(private API_MainService: API_MainService, private searchService: SearchService, private filterPopupService: FilterPopupService,
		private preferencesService: PreferencesService,
		private loginService: LoginService, private myInfrastructureDomainService: InfrastructureDomainService, private headerService: HeaderService) {
		this._filters = this.filterService.getFilters();
		this.getByIsDiscountCodes();
	}

	ngOnInit() {
		// this.preferencesService.getPreferencesByUserId(SessionInfo.LoggedUserId, SessionInfo.LoggedUserTenant);
		this.API_MainService.GetCB_PreferenceByUserIdAndTenant(SessionInfo.LoggedUserId, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
			let PreferencesList: CB_Preference[] = data?.body;
			this.preferencesService.allPreferences.next(PreferencesList);
			if (PreferencesList.length == 0) {
				this.preferencesService.AddAllCB_Preferences(this.preferencesService.defualtDataPreferences);
			}
		});
	}
	getByIsDiscountCodes() {
		this.headerService.IsDiscountCodes.subscribe((value) => {
			this.IsDiscountCodes = value;
		});
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

	SearchByText(searchBy: any) {
		if (SessionInfo.LoggedUserTenant == 0) this.getSearchData(searchBy);
		else this.checkIsFeaturePermessionCustomsBook(() => this.getSearchData(searchBy));
	}

	IsDiscountCodes: boolean = false;
	getSearchData(searchBy: any) {
		this.selectSearchBy = searchBy;
		let filtersSearch: FiltersSearch = this.filterPopupService.getFilters();

		let filters: Filters = {
			SearchFields: this.searchService.GetSearchText(),
			CustomsBookType: this.HeaderService.getSearchState(true),
			CustomsItemHierarchic: this.getCustomsItemHierarchic(filtersSearch),
			Reamarks: filtersSearch.remarks,
			Rules: filtersSearch.rules,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: SessionInfo.LoggedUserTenant
		};
		if (this.IsDiscountCodes)
			filters.IsDiscountCodes = this.IsDiscountCodes;

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
		// add prevent another search while loading
		if (this.isLoadingMode.getValue()) {
			return;
		}

		if (filters.SearchFields === "") return;
		if (SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.isLoadingMode.next(true); // update loading mode

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
		else if (SearchBy.pageSearch_form02 == this.selectSearchBy) {
			this.isLoadingMode.next(true); // update loading mode

			// build base  object data:CB_CustomsItemComputedDataList
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

	checkIsFeaturePermessionCustomsBook(onSuccess: () => void) {
		this.loginService.GetObjectTables().subscribe((myResult: any) => {
			if (!myResult) return true;
			window.ObjectTables = myResult;
			this.myInfrastructureDomainService.GetAllowedFeaturesForLoggedUser().subscribe((myResponse: any) => {
				if (!myResponse) return true;
				if (!FeatureLocator.HasFeaturePermession("Customs.CB_CustomsItemComputedData", "CustomsBookFeature")) {
					this.itemsData.next([]);
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
}
