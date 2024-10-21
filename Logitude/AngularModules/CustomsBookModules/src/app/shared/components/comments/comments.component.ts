
import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { AddCommentService, CommentState } from '../add-comment/service/add-comment.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RemarksClassificationList, RemarksClassificationPM } from '../main-display/main-display.component';
import { BehaviorSubject } from 'rxjs';
import { NgClass, NgFor, NgIf } from '@angular/common';
import { SearchService } from '../page-top/service/top-page.service';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [FontAwesomeModule, NgIf, NgFor, NgClass],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit, OnChanges {
  @Output() closeComments = new EventEmitter<boolean>();
  @Input() showComments: boolean = false;
  // @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);
  @Input() currentItem: CB_CustomsItemComputedDataList;
  allComments: RemarksClassificationList[] = [];
  remarksClassificationPM: RemarksClassificationPM;
  clickPin: boolean = true;
  searchText: string = '';
  // faPlusCircle = faPlusCircle;
  faChevronLeft = faChevronLeft;
  faChevronDown = faChevronDown;
  expandedArea: boolean = false;

  constructor(private addCommentService: AddCommentService, private API_MainService: API_MainService, private searchService: SearchService) { }

  showAddCommentSidebar() {
    this.addCommentService.setIsOpened(true, this.currentItem);
  }

  ngOnInit(): void {
    this.addCommentService.allComments.subscribe((data: RemarksClassificationList[]) => {
      this.allComments = data;
      this.showComments = this.allComments.length > 0 ? true : false;
      this.expandedArea = !this.showComments;

      this.showMenuOpen = false; // initialize the menu to be closed
    });
    this.addCommentService.currentRemark.subscribe((data: RemarksClassificationPM) => {
      this.remarksClassificationPM = data;
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['showComments']) {
      this.showComments = changes['showComments'].currentValue;

    }
  }
  showMenuOpen: boolean = false;
  showMenu() {
    this.showMenuOpen = !this.showMenuOpen;
  }

  editComment(event: any, comment: RemarksClassificationList) {
    this.remarksClassificationPM = this.convertToRemarksClassificationPM(comment);
    this.addCommentService.setIsOpened(true, this.currentItem, CommentState.Edit, this.remarksClassificationPM);
    event.preventDefault();
  }

  deleteComment(event: any, comment: RemarksClassificationList) {
    this.remarksClassificationPM = this.convertToRemarksClassificationPM(comment);
    this.addCommentService.setIsOpened(false, this.currentItem, CommentState.Delete, this.remarksClassificationPM);
    event.preventDefault();
  }


  convertToRemarksClassificationPM(comment: RemarksClassificationList): RemarksClassificationPM {
    this.remarksClassificationPM = {
      id: comment.Id,
      tenant: comment.Tenant,
      customsItemsID: comment.CustomsItemsID,
      remarkDescription: comment.RemarkDescription
    };
    return this.remarksClassificationPM;
  }

  // closeCommentsClick() {
  //   this.showComments = false;
  //   this.closeComments.emit();
  // }
  
  highlight(text: string): string {
    this.searchText = this.searchService.GetSearchText();
    if (!this.searchText) return text;
    const regex = new RegExp(`(${this.searchText})`, 'gi');
    return text.replace(regex, `<strong>$1</strong>`);
  }
}

