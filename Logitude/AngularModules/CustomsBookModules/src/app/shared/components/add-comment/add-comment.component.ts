import { Component, Input, OnChanges, SimpleChanges, OnInit } from '@angular/core';
import { AddCommentService } from './service/add-comment.service';
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
      this.currentItem = data;
      this.allCommentCount = this.addCommentService.allComments.getValue().length;
      this.showComment();
    });
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['showAddComment']) {
      this.showAddComment = changes['showAddComment'].currentValue;
      this.addCommentService.setIsOpened(this.showAddComment);
      console.log('showAddComment', this.showAddComment);
    }
  }

  updateCommentText(event: any) {
    this.commentText = event.target.value;
  }

  sendComment() {
    let remarksClassificationPM: RemarksClassificationPM = {
      tenant: SessionInfo.LoggedUserTenant,
      customsItemsID: this.currentItem?.CustomsItemID,
      remarkDescription: this.commentText || '',
    };

    if (!remarksClassificationPM.remarkDescription || !remarksClassificationPM.customsItemsID) return;

    this.addCommentService.setIsOpened(false);

    if (this.addCommentService.allComments.getValue().length >= 1) {
      // this.showMessage('לא ניתן להוסיף יותר מהערה 1');
      return;
    }
    this.API_MainService.AddNEWRemarksClassification(remarksClassificationPM).subscribe((data: any) => {

      if (!data.body) {
        this.showMessage('שגיאה בהוספת הערה');
        return;
      }
      this.showMessage('הערה נוספה בהצלחה');
      this.addCommentService.allComments.next([...this.addCommentService.allComments.getValue(), data.body]);
    });
    this.commentText = '';
  }

  showMessage(message: string): void {
    if (!message) return;
    this.snackBar.open(message, 'סגור', { duration: 1000 });
  }

  existingComment: RemarksClassificationList;
  showComment() {
    this.API_MainService.GetAllCommentsByCustomsItemId(this.currentItem.CustomsItemID, SessionInfo.LoggedUserTenant).subscribe((data: any) => {
      const result: RemarksClassificationList = data?.body[0];
      
      if (!result) return; // TODO: add error message

      this.commentText = result.RemarkDescription;
    });
  }

}
