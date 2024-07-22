import { Component, Output } from '@angular/core';
import { PageTopComponent } from '../../shared/components/page-top/page-top.component';
import { CB_CustomsItemComputedDataList, MainDisplayComponent } from '../../shared/components/main-display/main-display.component';
import { AddCommentComponent } from '../../shared/components/add-comment/add-comment.component';
import { CommonModule } from '@angular/common';
import { API_MainService, Filters } from '../../core/API_MainService';
import { BehaviorSubject, Observable } from 'rxjs';
import { FilterPopupService } from '../../shared/components/filter-popup/service/filter-popup.service';
import { SearchBy, Service } from '../../shared/components/page-top/service/top-page.service';
import { HeaderService } from '../../shared/components/app-header/service/header.service';

@Component({
	selector: 'app-main-page',
	standalone: true,
	imports: [PageTopComponent, MainDisplayComponent, AddCommentComponent, CommonModule],
	templateUrl: './main-page.component.html',
	styleUrl: './main-page.component.css',
})
export class MainPageComponent {
	Service = new Service();
	HeaderService = new HeaderService();
	showAddComment: boolean = false;
	filterService = new FilterPopupService();
	itemsData: BehaviorSubject<CB_CustomsItemComputedDataList[]> = new BehaviorSubject<CB_CustomsItemComputedDataList[]>([]);
	private _filters;
	selectSearchBy: string;

	constructor(private API_MainService: API_MainService, private headerService: HeaderService) {
		this._filters = this.filterService.getFilters();		
	}

	SearchByText(searchBy: any) {
		this.selectSearchBy = searchBy;

		let filters: Filters = {
			SearchFields: this.Service.GetSearchText(),
			CustomsBookType: this.HeaderService.getSearchState(true),
			CustomsItemHierarchic: '1,2,3,4',
			Reamarks: false,
			Rules: true,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: 0
		};
		
		if(this.itemsData.value.length > 0 && filters.SearchFields == ''){
			return;
		}
		else if(filters.SearchFields == '' || filters.SearchFields == null ||  filters.SearchFields.trim().length === 0) {
			this.itemsData.next([]);	
			return;
		}

		if (SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByClassification(filters).subscribe((data: CB_CustomsItemComputedDataList[]) => {
				this.itemsData.next(data);				
			});
		}
		else if (SearchBy.pageSearch_form02 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByText(filters).subscribe((data: CB_CustomsItemComputedDataList[]) => {
				this.itemsData.next(data);
			});
		}
	}
}
