import { Component, Output, EventEmitter, Input, OnInit, ViewChild, ElementRef, Renderer2, HostListener, ChangeDetectorRef } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold, faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';
import { faStar, faCommentDots, faSquareCaretRight, faFileText } from '@fortawesome/free-regular-svg-icons';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgIf, NgClass, NgStyle } from '@angular/common';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList, CB_TariffList, RemarksClassificationList, RulesDetailsList } from '../main-display/main-display.component';
import { API_MainService } from '../../../core/API_MainService';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { SearchBy, SearchService } from '../page-top/service/top-page.service';
import { PreferencesService, PreferenceType } from '../preference-menu/PreferencesService';

@Component({
	selector: 'app-data-row',
	standalone: true,
	imports: [FontAwesomeModule, NgIf, NgClass, NgStyle],
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

	@Input() showDetailsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
	@Input() state = 'search';
	@Input() searchItem?: string = '';
	@Input() fullClassificationLengthCharToDisplay?: number = 0;
	@Input() level: number = 0;

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
	defualtBackgroundColor: string = '#F3F5F7';
	constructor(private preferencesService: PreferencesService, private addCommentService: AddCommentService, private renderer: Renderer2, private API_MainService: API_MainService, private searchService: SearchService,
		private cdr: ChangeDetectorRef) {
		this.screenWidth = window.innerWidth;
	}
	
	ngOnInit() {
		this.cdr.detectChanges(); 
		this.showRulesData();
		this.selectedSearchBy = this.searchService.selectSearchBy;
		this.addCommentService.fullCommentsData.subscribe((data: RemarksClassificationList[]) => {
			this.data.remarksClassificationList = data.filter(x => x.CustomsItemsID == this.data.CustomsItemID);
			this.showCommentsData();
		});
	}

	getBackgroundColor(): string {
		const color = this.preferencesService.getPreference(this.level, PreferenceType.Background);
		return color !== this.defualtBackgroundColor ? color : null;
	}

	getTextColor(): string {
		return this.preferencesService.getPreference(this.level, PreferenceType.Text);
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
		if (this.fullClassificationLengthCharToDisplay > 0 || this.fullClassificationLengthCharToDisplay === null) return value;

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
			else this.buildSetWidth();
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
		this.comments = this.data?.remarksClassificationList;

		if (!this.comments) return; // TODO: add error message

		if (this.searchItem != "" && this.comments[0]?.RemarkDescription?.includes(this.searchItem)) {
			this.isSearchItemExistRemark = true;
		}
		else this.isSearchItemExistRemark = false;

		this.countOfComments = this.comments?.length > 0 ? this.comments.length : 0;
	}


	showRulesData() {
		if (this.data?.rulesData?.length == 0) return;

		if (this.searchItem == "") return;
		this.data?.rulesData?.forEach((rule: RulesDetailsList) => {
			if (rule.Rules.includes(this.searchItem)) {
				this.isSearchItemExistRule = true;
				return;
			}
		});
	}

	showCommentsClicked(event: MouseEvent) {
		let selection = window.getSelection();
		let isTextSelected = selection && selection?.toString().length > 0;
		if (isTextSelected) return;
		this.showCommentsClick();
	}

	showCommentsClick() {
		this.showComments = !this.showComments;
		this.data.remarksClassificationList = this.comments;
		// this.data.agreementsList = this.TariffListData;#114817
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
		if (this.isShowDetailsOpen) this.dynamicDivClick();
		else this.buildSetWidth();
	}

	buildSetWidth() {
		if (this.screenWidth <= 620) this.widthSmaller = true;
		else this.widthSmaller = false;
		//#115478 delete using showTaxData input
		let isExistData = !this.data?.PurchaseTax && !this.data?.MeasurementUnitName && !this.data?.CustomsRate && !this.data?.OptionalTaxAddition ? false : true;
		isExistData = !isExistData && this.data?.MeasurementUnitName && this.level > 3 ? true : isExistData;
		// Set width:
		if (isExistData && this.screenWidth > 1199 && this.screenWidth < 1300) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "30%");
		}
		else if (isExistData && this.screenWidth >= 1301 && this.screenWidth < 1350) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "35%");
		}
		else if (isExistData && this.screenWidth >= 1351 && this.screenWidth < 1700) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "36%");
		}
		else if (isExistData && this.screenWidth >= 1701 && this.screenWidth < 1900) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "40%");
		}
		else if (isExistData && this.screenWidth >= 1901 && this.screenWidth < 2250) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "45%");
		}
		else if (isExistData && this.screenWidth >= 2250) {
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
		else if (this.screenWidth > 990 && this.screenWidth <= 1351) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "83%");
		}
		else if (this.screenWidth > 1351 && this.screenWidth <= 1550) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "88%");
		}
		else if (this.screenWidth > 1550) {
			this.renderer.setStyle(this.dynamicDiv.nativeElement.children[0], 'width', "90%");
		}

	}
}

