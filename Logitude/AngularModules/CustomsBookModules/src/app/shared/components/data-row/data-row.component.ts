import { Component, Output, EventEmitter, Input, OnInit, ViewChild, ElementRef, Renderer2, HostListener } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold, faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faStar, faCommentDots, faSquareCaretRight, faFileText } from '@fortawesome/free-regular-svg-icons';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgIf, NgClass } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList, CB_TariffList, RemarksClassificationList, RulesDetailsList } from '../main-display/main-display.component';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { SearchBy, SearchService } from '../page-top/service/top-page.service';

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
	isSearchItemExistRule: boolean = false;
	isSearchItemExistRemark: boolean = false;
	showAddComment = this.addCommentService.getIsOpened();
	selectedSearchBy: SearchBy = SearchBy.searchBy_form01;
	screenWidth: number;
	widthSmaller: boolean = false;

	constructor(private addCommentService: AddCommentService, private renderer: Renderer2, private API_MainService: API_MainService, private searchService: SearchService) {
		this.screenWidth = window.innerWidth;
	}

	ngOnInit() {
		this.getCustomsBookAgreementLevelData();
		this.showRulesData(this.data.CustomsItemID);
		this.selectedSearchBy = this.searchService.selectSearchBy;
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
		// return original text when search by classification:
		if (SearchBy.searchBy_form01 == this.selectedSearchBy) {
			return text;
		}

		// return bold + mark text when search by word:
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
				this.buildSetWidth();
				// if (!this.showTaxData || (!this.isShowDetailsOpen && this.TariffListData?.length === 0) || this.screenWidth <= 1900) 
					// this.buildSetWidth();
				// else {
				// 	this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "27%");
				// 	this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'white-space', 'nowrap');
				// 	this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'text-overflow', 'ellipsis');
				// }
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
	comments: RemarksClassificationList[] = [];
	countOfComments: number;
	showCommentsData() {
		this.API_MainService.GetAllCommentsByCustomsItemId(this.data.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
			this.comments = data.body;

			if (!this.comments) return; // TODO: add error message

			if (this.searchItem != "" && this.comments[0]?.RemarkDescription?.includes(this.searchItem)) {
				this.isSearchItemExistRemark = true;
			}
			else this.isSearchItemExistRemark = false;

			this.countOfComments = this.comments?.length > 0 ? this.comments.length : 0;
		});
	}

	showRulesData(customsItemID: number) {
		this.API_MainService.GetCustomsBookRulesData(customsItemID).subscribe((data: any) => {
			const result: RulesDetailsList[] = data.body;
			if (!data.body) return; // TODO: add error message

			if (this.searchItem == "") return;
			result.forEach((rule: RulesDetailsList) => {
				if (rule.Rules.includes(this.searchItem)) {
					this.isSearchItemExistRule = true;
					return;
				}
			});
		});
	}


	showCommentsClick() {
		this.showComments = !this.showComments;
		this.data.remarksClassificationList = this.comments;
		this.data.agreementsList = this.TariffListData;
		this.showCommentsOpen.emit(true);
		this.showDetails.emit();
	}
	showRulesClick() {
		this.showRules = !this.showRules;
		this.showRulesOpen.emit(true);
	}


	// Get current screen width
	@HostListener('window:resize', ['$event'])
	onResize(event: Event): void {
		this.screenWidth = (event.target as Window).innerWidth;
		// if(this.isShowDetailsOpen) this.dynamicDivClick();
		// else this.buildSetWidth();
		this.buildSetWidth();
	}

	buildSetWidth() {
		if (this.screenWidth <= 620) this.widthSmaller = true;
		else this.widthSmaller = false;

		// Set width:
		if (this.showTaxData && this.screenWidth > 1399 && this.screenWidth < 1610) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "10%");
		}
		else if (this.showTaxData && this.screenWidth >= 1611 && this.screenWidth < 1750) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "16%");
		}
		else if (this.showTaxData && this.screenWidth >= 1751 && this.screenWidth < 1900) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "22%");
		}
		else if (this.showTaxData && this.screenWidth >= 1901 && this.screenWidth < 2100) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "35%");
		}
		else if (this.showTaxData && this.screenWidth >= 2101 && this.screenWidth < 2250) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "40%");
		}
		else if (this.showTaxData && this.screenWidth >= 2250) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "50%");
		}
		else if (this.screenWidth <= 550) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "60%");
		}
		else if (this.screenWidth > 550 && this.screenWidth <= 800) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "65%");
		}
		else if (this.screenWidth > 800 && this.screenWidth <= 900) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "70%");
		}
		else if (this.screenWidth > 900 && this.screenWidth <= 990) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "80%");
		}
		else if (this.screenWidth > 990 && this.screenWidth <= 1350) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "83%");
		}
		else if (this.screenWidth > 1350 && this.screenWidth <= 1550) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "90%");
		}
		else if (this.screenWidth > 1550) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "95%");
		}

	}
}

