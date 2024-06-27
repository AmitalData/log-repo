import { Component, Output } from '@angular/core';
import { PageTopComponent } from '../../shared/components/page-top/page-top.component';
import { MainDisplayComponent } from '../../shared/components/main-display/main-display.component';
import { AddCommentComponent } from '../../shared/components/add-comment/add-comment.component';
import { CommonModule } from '@angular/common';
import { API_MainService, Filters } from '../../core/API_MainService';
import { Observable } from 'rxjs';
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
	itemsData: Observable<any>;
	private _filters;
  selectSearchBy: string;

	constructor(private API_MainService: API_MainService, private headerService: HeaderService) {
		this._filters = this.filterService.getFilters();
	}

	SearchByText(searchBy: any) {
		this.selectSearchBy = searchBy;
		console.log(this.selectSearchBy );
		
		
	
		let object:Filters	 = {
			SearchFields: this.Service.GetSearchText(),
			CustomsBookType: this.HeaderService.getSearchState(true),
			CustomsItemHierarchic: '1,2,3',
			Reamarks: false,
			Rules: true,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: 0
		};

		if(SearchBy.searchBy_form01 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByClassification(object).subscribe((data: any) => {
				
				this.itemsData = data;
				console.log("searchBy_form01");
				console.log(data);
			});
		}
		else if(SearchBy.pageSearch_form02 == this.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByText(object).subscribe((data: any) => {
				
				this.itemsData = data;
				console.log("pageSearch_form02");
				console.log(data);
			});
		}
	}



	
	SearchByTextd() {
		
		let object:Filters = {
			SearchFields: this.Service.GetSearchText(),
			CustomsBookType: this.HeaderService.getSearchState(true),
			CustomsItemHierarchic: '1,2,3',
			Reamarks: false,
			Rules: true,
			SkippedRows: 0,
			PageSize: 0,
			Tenant: 0
		};

		
		
		if(SearchBy.searchBy_form01 == this.Service.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByClassification(object).subscribe((data: any) => {
				debugger
				this.itemsData = data;
				console.log(data);
			});
		}
		else if(SearchBy.pageSearch_form02 == this.Service.selectSearchBy) {
			this.API_MainService.GetCustomsBookMainViewSearchByText(object).subscribe((data: any) => {
				debugger
				this.itemsData = data;
				console.log(data);
			});
		}
	}
}
