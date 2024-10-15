import { Component, Output, EventEmitter, Input, OnInit, ViewChild, ElementRef, Renderer2 } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold, faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faStar, faCommentDots, faSquareCaretRight, faFileText } from '@fortawesome/free-regular-svg-icons';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgIf, NgClass } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList, CB_TariffList, RemarksClassificationList } from '../main-display/main-display.component';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Component({
	selector: 'app-data-row',
	standalone: true,
	imports: [FontAwesomeModule, NgIf, NgClass],
	templateUrl: './data-row.component.html',
	styleUrl: './data-row.component.css',
})
export class DataRowComponent implements OnInit {
	@Output() showRulesOpen: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Output() showCommentsOpen: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Output() showDetails: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Output() showChildern: EventEmitter<boolean> = new EventEmitter<boolean>();
	@Input() data: CB_CustomsItemComputedDataList;
	@Input() isSelected?: boolean = true;

	@Input() showTaxData: boolean = false;
	@Input() showDetailsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	@Input() state = 'search';
	@Input() searchItem?: string = '';
	@Input() fullClassificationLengthCharToDisplay?: number = 0;

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

	constructor(private addCommentService: AddCommentService, private renderer: Renderer2, private API_MainService: API_MainService) { }

	ngOnInit() {
		this.getCustomsBookAgreementLevelData();
		this.addCommentService.allComments.subscribe((data: RemarksClassificationList[]) => {
			this.showCommentsData();
		});
	}

	TariffList1: CB_TariffList;
	TariffList2: CB_TariffList;
	TariffListCount: number = 0;
	TariffListData: CB_TariffList[] = [];
	getCustomsBookAgreementLevelData() {
		if (!this.showTaxData || !this.data.CustomsItemID || !this.data?.PH_MeasurementUnitID) return;
		this.API_MainService.GetCustomsBookAgreementLevelData(this.data?.CustomsItemID, this.data?.PH_MeasurementUnitID).subscribe((data: any) => {
			this.TariffListData = data.body;
			if (!this.TariffListData) return;
			this.TariffList1 = this.TariffListData.find(x => x.TradeAgreementName == 'מכס כללי');
			this.TariffList2 = this.TariffListData.find(x => x.TradeAgreementName == 'מס קניה');
			this.TariffListCount = this.TariffListData.filter(x => x.TradeAgreementName != 'מס קניה').length;
			this.contentWidth();
		});

	}

	highlight(text: string, search: string): string {
		if (!search) {
			return text;
		}
		const regex = new RegExp(`(${search})`, 'gi');
		return text.replace(regex, `<mark><strong>$1</strong></mark>`);
	}

	expandClick(isShowChildren: boolean) {
		this.showChildern.emit(!isShowChildren);
	}

	showAddCommentSidebar(data: CB_CustomsItemComputedDataList) {
		this.addCommentService.setIsOpened(true, data);
	}

	getTooltipText(text: string) {
		return text.length > 20 ? text : '';
	}

	ClassificationNoDisplay(item, value: string): string {
		if (item.IsLeaf || !this.fullClassificationLengthCharToDisplay || this.fullClassificationLengthCharToDisplay > 5) return value;
		if (value.length >= this.fullClassificationLengthCharToDisplay) {
			return value.substring(0, this.fullClassificationLengthCharToDisplay);
		}
		return '';
	}

	isShowDetailsOpen: boolean = false;
	@ViewChild('dynamicDiv') dynamicDiv: ElementRef;
	ngAfterViewInit(): void {
		this.contentWidth();
	}

	contentWidth(): void {
		this.showDetailsOpen.subscribe((value) => {
			this.isShowDetailsOpen = value;
			if (value) this.dynamicDivClick(); // when window open
			else {
				if (!this.showTaxData) this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "90%");
				else { // if close display window tax
					if (!this.isShowDetailsOpen && this.TariffListData?.length == 0) {// if display close and not exist data in tax list
						this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "90%")
						return;
					}
					this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "27%");
					this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'white-space', 'nowrap');
					this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'text-overflow', 'ellipsis');
				}
			}
		});
	}

	// display text shorter when the div is smaller width 
	dynamicDivClick() {
		const containerWidth: number = this.dynamicDiv.nativeElement.offsetWidth;
		const spans = this.dynamicDiv.nativeElement.querySelectorAll('span');
		let totalSpanWidth = (Array.from(spans).reduce((total: number, span) => total + (span as HTMLElement).offsetWidth, 0)) as number;
		if (containerWidth - 100 < totalSpanWidth || this.data.CustomsItemID) {
			let calculatedWidth = (containerWidth - 100) + "px";
			let width = Math.min(parseInt(calculatedWidth), 75) + "%";

			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', width);
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'white-space', 'nowrap');
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'text-overflow', 'ellipsis');
		}
	}

	showComments: boolean = false;
	showRules: boolean = false;

	countOfComments: number;
	showCommentsData() {
		this.API_MainService.GetAllCommentsByCustomsItemId(this.data.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
			const result: RemarksClassificationList[] = data.body;

			if (!result) return; // TODO: add error message

			this.countOfComments = result?.length > 0 ? result.length : 0;
		});
	}

	showCommentsClick() {
		this.showComments = !this.showComments;
		this.showCommentsOpen.emit(true);
	}
	showRulesClick() {
		this.showRules = !this.showRules;
		this.showRulesOpen.emit(true);
	}
}

