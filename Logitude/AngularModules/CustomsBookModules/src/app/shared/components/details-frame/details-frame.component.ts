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


@Component({
  selector: 'app-details-frame',
  standalone: true,
  imports: [FontAwesomeModule, AccordionComponent, CommentsComponent],
  templateUrl: './details-frame.component.html',
  styleUrl: './details-frame.component.css'
})


export class DetailsFrameComponent implements OnInit {
  @Output() showDetails = new EventEmitter<boolean>();
  @Input() showAddComment: boolean = false;
  // @Input() itemData: BehaviorSubject<ItemData> = new BehaviorSubject<ItemData>(null);
  @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList>;
  item: CB_CustomsItemComputedDataList;
  showComments: boolean = false;
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
  constructor(private API_MainService: API_MainService, private addCommentService: AddCommentService) { }


  ngOnInit() {
    // this.currentItem.FullClassification = "";
    this.currentItem.subscribe((data: CB_CustomsItemComputedDataList) => {
      if (data?.CustomsItemID != null) {
        this.item = data
        this.showCommentsByClick();
      }
    });
  }

  showCommentsByClick() {
    this.API_MainService.GetAllCommentsByCustomsItemId(this.currentItem.getValue().CustomsItemID, this.API_MainService.getTenant()).subscribe((data: RemarksClassificationList[]) => {
      // console.log(data);
      this.addCommentService.allComments.next(data);
    });
  }
}

