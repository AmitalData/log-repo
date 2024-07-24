import { Component, Output, EventEmitter, Input, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold, faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faStar, faCommentDots, faSquareCaretRight, faFileText } from '@fortawesome/free-regular-svg-icons';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgIf, NgClass } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList } from '../main-display/main-display.component';

@Component({
	selector: 'app-data-row',
	standalone: true,
	imports: [FontAwesomeModule, NgIf, NgClass],
	templateUrl: './data-row.component.html',
	styleUrl: './data-row.component.css',
})
export class DataRowComponent implements OnInit {
	@Output() showDetails: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Output() showChildern: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Input() data: any;
	@Input() isSelected?: boolean = true;
	@Input() isExpand: BehaviorSubject<boolean>;

	@Input() state = 'search';
	@Input() searchItem?: string = '';

	faStar = faStar;
	faStarBold = faStarBold;
	faComments = faCommentDots;
	faCaretSquareRight = faSquareCaretRight;
	faArrowAltCircleLeft = faChevronLeft;
	faChevronDown = faChevronDown;
	faFileArchive = faFileText;
	checked: boolean = false;
	selected: boolean = false;
	showAddComment = this.addCommentService.getIsOpened();

	constructor(private addCommentService: AddCommentService) { }

	ngOnInit() {
		this.listerToExpand();
	}

	listerToExpand() {
		this.isExpand.subscribe((value) => {
			this.selected = value;
		});
	}


	highlight(text: string, search: string): string {
		if (!search) {
			return text;
		}
		const regex = new RegExp(`(${search})`, 'gi');
		return text.replace(regex, `<mark>$1</mark>`);
	}

	expandClick() {
		this.selected = !this.selected;
		this.showChildern.emit(this.selected);
	}

	showAddCommentSidebar(data: CB_CustomsItemComputedDataList) {
		this.addCommentService.setIsOpened(true, data);
	}

	getTooltipText(text: string): string {
		return text.length > 20 ? text : '';
	}

	ClassificationNoDisplay(item, value): string {
		if (item.IsLeaf) return value;

		const regex = /^(\d*[^0])\d*$/;
		const match = value.match(regex);
		if (match && match[1]) {
			return match[1];
		}
		return '';
	}
}
