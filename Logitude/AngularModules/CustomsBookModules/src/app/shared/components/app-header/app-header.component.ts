import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar } from '@fortawesome/free-regular-svg-icons';
import { HeaderService } from './service/header.service';
import { PageTopComponent } from '../page-top/page-top.component';

@Component({
	selector: 'app-header',
	standalone: true,
	imports: [FontAwesomeModule, PageTopComponent],
	templateUrl: './app-header.component.html',
	styleUrl: './app-header.component.css',
})
export class AppHeaderComponent {
	faStar = faStar;
	selected: string = 'יבוא';
	@Output() searchClick = new EventEmitter<string | number>();

	constructor(private headerService: HeaderService) { }

	ngOnInit(): void {
		this.selected = this.headerService.getSearchState();
	}

	selectState = (state: string) => {
		this.selected = state;
		this.headerService.setSearchState(state);
	};

	SearchByText(searchBy: any) {
		this.searchClick.emit(searchBy);
	}
}
