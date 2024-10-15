import { Component, Input, OnChanges, SimpleChanges, OnInit } from '@angular/core';
import { AddCommentService, CommentState } from './service/add-comment.service';
import { CB_CustomsItemComputedDataList, RemarksClassificationList, RemarksClassificationPM } from '../main-display/main-display.component';
import { API_MainService } from '../../../core/API_MainService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-add-comment',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-comment.component.html',
  styleUrls: ['./add-comment.component.css']
})
export class AddCommentComponent implements OnInit, OnChanges {
  remarksClassificationPM: RemarksClassificationPM;
  showAddComment: boolean = false;
  commentText: string = '';
  currentItem: CB_CustomsItemComputedDataList;
  allCommentCount: number = 0;
  commentMode: CommentState;
  allComments: RemarksClassificationList[] = [];

  constructor(
    private addCommentService: AddCommentService,
    private API_MainService: API_MainService,
    private snackBar: MatSnackBar
  ) { }

  ngOnInit() {
    this.addCommentService.isOpened.subscribe((isOpened: boolean) => {
      this.showAddComment = isOpened;
    });
    this.addCommentService.itemData.subscribe((data: CB_CustomsItemComputedDataList) => {

      if(!data?.CustomsItemID) return
      this.currentItem = data;
      this.allCommentCount = this.addCommentService.allComments.getValue().length;
      this.showComments();
    });

    this.addCommentService.currentRemark.subscribe((remarksClassification: RemarksClassificationPM) => {
      this.remarksClassificationPM = remarksClassification;
    });

    this.addCommentService.CommentMode.subscribe((mode: CommentState) => {
      this.commentMode = mode;
      if (mode == CommentState.Edit || mode == CommentState.Delete) {
        this.showComment();
      }
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['showAddComment']) {
      this.showAddComment = changes['showAddComment'].currentValue;
      this.addCommentService.setIsOpened(this.showAddComment);
      console.log('showAddComment', this.showAddComment);
    }
  }

  showComments() {
    this.API_MainService.GetAllCommentsByCustomsItemId(this.currentItem.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
      const result: RemarksClassificationList[] = data.body;
      this.allComments = result;
    });
  }

  updateCommentText(event: any) {
    this.commentText = event.target.value;
  }

  sendComment() {
    if (!this.remarksClassificationPM?.id || this.addCommentService.CommentMode.getValue() == CommentState.Add) {
      this.remarksClassificationPM = { // init new
        tenant: SessionInfo.LoggedUserTenant,
        customsItemsID: this.currentItem?.CustomsItemID,
        remarkDescription: this.commentText || '',
      };

      if (this.allComments.length >= 1) {
        return;
      }
      if (!this.remarksClassificationPM?.remarkDescription || !this.remarksClassificationPM?.customsItemsID) return;
      this.API_MainService.AddNEWRemarksClassification(this.remarksClassificationPM).subscribe((data: any) => {

        if (!data.body) {
          this.showMessage('שגיאה בהוספת הערה');
          return;
        }
        this.showMessage('הערה נוספה בהצלחה');
        this.addCommentService.allComments.next([...this.addCommentService.allComments.getValue(), data.body]);
      });
    }
    else if (this.addCommentService.CommentMode.getValue() == CommentState.Edit) {
      this.remarksClassificationPM.remarkDescription = this.commentText;

      this.API_MainService.EditRemarksClassification(this.remarksClassificationPM).subscribe((data: any) => {

        if (!data.body) {
          this.showMessage('שגיאה בעריכת הערה');
          return;
        }
        this.showMessage('הערה נערכה בהצלחה');

        this.addCommentService.allComments.next([data.body]);
      });
    }
    this.addCommentService.setIsOpened(false);
    this.commentText = '';
  }

  showMessage(message: string): void {
    if (!message) return;
    this.snackBar.open(message, 'סגור', { duration: 1000 });
  }

  showComment() {

    if (!this.currentItem?.CustomsItemID) return;
    if (this.commentMode == CommentState.Add) {
      this.API_MainService.GetAllCommentsByCustomsItemId(this.currentItem?.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {

        const result: RemarksClassificationList = data?.body[0];

        if (!result) return; // TODO: add error message
        this.remarksClassificationPM = {
          id: result.Id,
          tenant: result.Tenant,
          customsItemsID: result.CustomsItemsID,
          remarkDescription: result.RemarkDescription || '',
        };
      });
    }
    else if (this.commentMode == CommentState.Edit) {
      this.commentText = this.remarksClassificationPM.remarkDescription;
    }
    else if (this.commentMode == CommentState.Delete) {
      this.API_MainService.DeleteRemarksClassification(this.remarksClassificationPM).subscribe((data: any) => {

        if (!data.body) {
          this.showMessage('שגיאה במחיקת הערה');
          return;
        }
        this.showMessage('הערה נמחקה בהצלחה');
        this.addCommentService.allComments.next([]);
      });
    }
  }

}
