import { Component, EventEmitter, Output } from '@angular/core';
import { SearchBy, SearchService } from './service/top-page.service';
import { FormsModule } from '@angular/forms';

@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [FormsModule],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	// @Output() searchClick = new EventEmitter();
	@Output() searchClick = new EventEmitter<string | number>();

	textToSearch: string = '';
	constructor(public searchService: SearchService) { }

	public text: string = '';
	public checked: string | number = '';
	public searchBy = SearchByParam;
	public selectedSearchOption:SearchByParam = this.searchBy.Classification;

	ngOnInit() {
		this.text = this.searchService.SearchBy('searchBy_form01');
		this.checked = this.searchService.GetDefaultValue();

		
	}

	public search(id: string) {
		this.checked = id;
		this.text = this.searchService.SearchBy(id);
		
	}

	onChange(event: any) {
		this.searchService.SetSearchText(event.target.value);
	}

	clickSearch() {
		this.searchClick.emit(this.searchService.selectSearchBy);

		this.searchService.searchText$.subscribe((searchText) => {
				// reset search input in html:
			if (searchText === "") this.textToSearch = "";
		});
	}
}

export enum SearchByParam {
	Classification = "פרט מכס",
	WordCombination = "מילה/צירוף מילים"
}

		
