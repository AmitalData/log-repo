
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AddCommentService, CommentState } from '../add-comment/service/add-comment.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RemarksClassificationList, RemarksClassificationPM } from '../main-display/main-display.component';
import { BehaviorSubject } from 'rxjs';
import { NgFor, NgIf } from '@angular/common';


@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [FontAwesomeModule, NgIf, NgFor],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit, OnChanges {
  @Input() showComments: boolean = false;
  // @Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);
  @Input() currentItem: CB_CustomsItemComputedDataList;
  allComments: RemarksClassificationList[] = [];
  remarksClassificationPM: RemarksClassificationPM;

  // faPlusCircle = faPlusCircle;

  constructor(private addCommentService: AddCommentService, private API_MainService: API_MainService) { }

  showAddCommentSidebar() {
    this.addCommentService.setIsOpened(true, this.currentItem);
  }

  ngOnInit(): void {
    this.addCommentService.allComments.subscribe((data: RemarksClassificationList[]) => {
      this.allComments = data;
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
}

