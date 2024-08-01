import { Component, Output } from '@angular/core';
import { PageTopComponent } from '../../shared/components/page-top/page-top.component';
import { CB_CustomsItemComputedDataList, MainDisplayComponent } from '../../shared/components/main-display/main-display.component';
import { AddCommentComponent } from '../../shared/components/add-comment/add-comment.component';
import { CommonModule } from '@angular/common';
import { API_MainService, Filters } from '../../core/API_MainService';
import { BehaviorSubject, Observable } from 'rxjs';
import { FilterPopupService } from '../../shared/components/filter-popup/service/filter-popup.service';
import { SearchBy, SearchService } from '../../shared/components/page-top/service/top-page.service';
import { HeaderService } from '../../shared/components/app-header/service/header.service';
import { AppHeaderComponent } from '../../shared/components/app-header/app-header.component';
import { SessionInfo } from '../../core/Infrastructure/Utilities/SessionInfo';

@Component({
	selector: 'app-main-page',
	standalone: true,
	imports: [PageTopComponent, MainDisplayComponent, AddCommentComponent, CommonModule, AppHeaderComponent],
	templateUrl: './main-page.component.html',
	styleUrl: './main-page.component.css',
})
export class MainPageComponent {
	HeaderService = new HeaderService();
	showAddComment: boolean = false;
	filterService = new FilterPopupService();
	itemsData: BehaviorSubject<CB_CustomsItemComputedDataList[]> = new BehaviorSubject<CB_CustomsItemComputedDataList[]>([]);
	private _filters;
	selectSearchBy: string;

	constructor(private API_MainService: API_MainService, private headerService: HeaderService, private searchService: SearchService) {
		this._filters = this.filterService.getFilters();
	}

	SearchByText(searchBy: any) {
		this.selectSearchBy = searchBy;

		let filters: Filters = {
			SearchFields: this.searchService.GetSearchText(),
			CustomsBookType: this.HeaderService.getSearchState(true),
			CustomsItemHierarchic: '1,2,3,4',
			Reamarks: false,
			Rules: true,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: SessionInfo.Tenant
		};

		if (filters.SearchFields === "") return;
		if (SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByClassification(filters).subscribe((data: any) => {
				const result: CB_CustomsItemComputedDataList[] = data.body;
				if (!result) return; // TODO: add error message
				this.itemsData.next(result);
			});
		}
		else if (SearchBy.pageSearch_form02 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByText(filters).subscribe((data: any) => {
				const result: CB_CustomsItemComputedDataList[] = data.body;
				if (!result) return; // TODO: add error message
				this.itemsData.next(result);
			});
		}
	}
}
