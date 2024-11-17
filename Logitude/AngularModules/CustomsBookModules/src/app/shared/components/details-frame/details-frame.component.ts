import { Component, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold } from '@fortawesome/free-solid-svg-icons';
import { faSquareCaretRight, faFileText, faSquareCheck, faCommentAlt, faStar, faCommentDots, faPenToSquare, faTrashCan } from '@fortawesome/free-regular-svg-icons';
import { AccordionComponent } from '../accordion/accordion.component';
import { Output, Input, EventEmitter } from '@angular/core';
import { CommentsComponent } from '../comments/comments.component';
import { CB_CustomsItemComputedDataList, ItemData, RemarksClassificationList } from '../main-display/main-display.component';
import { BehaviorSubject } from 'rxjs';
import { API_MainService } from '../../../core/API_MainService';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { NgClass, NgFor, NgForOf, NgIf } from '@angular/common';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { RulesComponent } from "../rules/rules.component";


@Component({
  selector: 'app-details-frame',
  standalone: true,
  imports: [NgClass, FontAwesomeModule, AccordionComponent, CommentsComponent, RulesComponent, NgFor, NgForOf, NgIf],
  templateUrl: './details-frame.component.html',
  styleUrl: './details-frame.component.css'
})


export class DetailsFrameComponent implements OnInit {
  @Output() showDetails = new EventEmitter<boolean>();
  @Input() showAddComment: boolean = false;
  @Input() showCommentsIsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  @Input() showRulesIsOpen: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  @Input() showDetailsStatus: boolean;
  // @Input() itemData: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>(null);
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList>;
  item: CB_CustomsItemComputedDataList;
  showComments: boolean = false;
  showRules: boolean = false;
  show: boolean = false;
  checked: boolean = false;
  faSquareCheck = faSquareCheck;
  faCommentAlt = faCommentAlt;
  faStar = faStar;
  faStarBold = faStarBold;
  faComments = faCommentDots;
  faPenToSquare = faPenToSquare;
  faTrashCan = faTrashCan;
  faCaretSquareRight = faSquareCaretRight;
  faFileArchive = faFileText;
  countOfComments: number = 0;
  isLoading: boolean = false;

  constructor(private API_MainService: API_MainService, private addCommentService: AddCommentService) { }


  ngOnInit() {
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      if (data?.CustomsItemID != null) {
        this.item = data

        this.countOfComments = this.item?.remarksClassificationList?.length > 0 ? this.item?.remarksClassificationList?.length : 0;
        this.addCommentService.allComments.next(this.item?.remarksClassificationList);
        this.showCommentsIsOpen.subscribe((isOpen: boolean) => {
          this.showComments = isOpen;
        });
        this.showRulesIsOpen.subscribe((isOpen: boolean) => {
          this.showRules = isOpen;
        });

        this.addCommentService.allComments.subscribe((data: RemarksClassificationList[]) => {
          this.countOfComments = data?.length > 0 ? data.length : 0;
        });
      }
    });
  }

  showCommentsData() {
    this.API_MainService.GetAllCommentsByCustomsItemId(this.item?.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
      this.item.remarksClassificationList = data?.body;

      if (!this.item.remarksClassificationList) return; // TODO: add error message

      this.countOfComments = this.item.remarksClassificationList?.length > 0 ? this.item.remarksClassificationList.length : 0;
    });
  }

  showAddCommentSidebar() {
    this.addCommentService.setIsOpened(true, this.item);
  }

  closeComments() {
    this.showCommentsIsOpen.next(false);
  }
  closeRules() {
    this.showRulesIsOpen.next(false);
  }
}

