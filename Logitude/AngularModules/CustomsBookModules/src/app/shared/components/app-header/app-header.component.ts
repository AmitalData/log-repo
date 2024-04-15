import { Component, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar } from '@fortawesome/free-regular-svg-icons';
import { HeaderService } from './service/header.service';

@Component({
	selector: 'app-header',
	standalone: true,
	imports: [FontAwesomeModule],
	templateUrl: './app-header.component.html',
	styleUrl: './app-header.component.css',
})
export class AppHeaderComponent {
	faStar = faStar;
	selected: string = 'יבוא';

	constructor(private headerService: HeaderService) {}

	ngOnInit(): void {
		this.selected = this.headerService.getSearchState();
	}

	selectState = (state: string) => {
		this.selected = state;
		this.headerService.setSearchState(state);
	};
}
