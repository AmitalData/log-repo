import { Component, EventEmitter, Output } from '@angular/core';
import { Service } from './service/top-page.service';

@Component({
	selector: 'app-page-top',
	standalone: true,
	imports: [],
	templateUrl: './page-top.component.html',
	styleUrl: './page-top.component.css',
})
export class PageTopComponent {
	@Output() searchClick = new EventEmitter();

	constructor(public service: Service) {
		this.service = new Service();
	}

	public text: string = '';
	public checked: string | number = '';

	ngOnInit() {
		this.text = this.service.SearchBy('searchBy_form01');
		this.checked = this.service.GetDefaultValue();
	}

	public search(id: string) {
		this.checked = id;
		this.text = this.service.SearchBy(id);
	}

	onChange(event: any) {
		this.service.SetSearchText(event.target.value);
	}
}
