import { Component, Output, EventEmitter, Input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold, faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faStar, faCommentDots, faSquareCaretRight, faFileText } from '@fortawesome/free-regular-svg-icons';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgIf } from '@angular/common';

@Component({
	selector: 'app-data-row',
	standalone: true,
	imports: [FontAwesomeModule, NgIf],
	templateUrl: './data-row.component.html',
	styleUrl: './data-row.component.css',
})
export class DataRowComponent {
	@Output() showDetails: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Output() showChildern: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Input() data: any;
	@Input() state = 'search';
	faStar = faStar;
	faStarBold = faStarBold;
	faComments = faCommentDots;
	faCaretSquareRight = faSquareCaretRight;
	faArrowAltCircleLeft = faChevronLeft;
	faChevronDown = faChevronDown;
	faFileArchive = faFileText;
	checked: boolean = false;
	selected: boolean = false;
	constructor(private addCommentService: AddCommentService) { }
	showAddComment = this.addCommentService.getIsOpened();

	showAddCommentSidebar() {
		this.addCommentService.setIsOpened(true);
	}

	ngOnInit() { }


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
