import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar } from '@fortawesome/free-regular-svg-icons';
import { HeaderService } from './service/header.service';
import { PageTopComponent } from '../page-top/page-top.component';
import { SearchService } from '../page-top/service/top-page.service';
import { NgIf } from '@angular/common';
import { FilterPopupService } from '../filter-popup/service/filter-popup.service';
import { PreferenceMenuComponent } from '../preference-menu/preference-menu';
import { CommonModule } from '@angular/common';

@Component({
	selector: 'app-header',
	standalone: true,
	imports: [FontAwesomeModule, PageTopComponent, NgIf, PreferenceMenuComponent, CommonModule],
	templateUrl: './app-header.component.html',
	styleUrl: './app-header.component.css',
})
export class AppHeaderComponent {
	faStar = faStar;
	selected: string = 'יבוא';
	@Output() searchClick = new EventEmitter<string | number>();
	discountCodes: string = 'קודי הנחה';
	IsDiscountCodes: boolean = false;
	lastUpdateTaskScheduled: Date;

	constructor(private headerService: HeaderService, private searchService: SearchService, private filterPopupService: FilterPopupService) { }

	ngOnInit(): void {
		this.selected = this.headerService.getSearchState();
		this.headerService.lastUpdateTaskScheduled.subscribe((date: Date) => {
			this.lastUpdateTaskScheduled = date;
		});
		this.headerService.IsDiscountCodes.subscribe((value) => {
			this.IsDiscountCodes = value;
		});
	}

	selectState = (state: string) => {
		this.selected = state;
		this.headerService.setSearchState(state);
		this.searchService.SetSearchText("");
		if (state == 'אוטונומיה') {
			this.IsDiscountCodes = false;
			this.headerService.setIsDiscountCodes(this.IsDiscountCodes);
		}
	}

	SearchByText(searchBy: any) {
		this.searchClick.emit(searchBy);
	}

	updateIsDiscountCodes = () => {
		this.IsDiscountCodes = !this.IsDiscountCodes;
		this.headerService.setIsDiscountCodes(this.IsDiscountCodes);
		if (this.IsDiscountCodes) this.filterPopupService.isClearFilter.next(true);
	}
}
