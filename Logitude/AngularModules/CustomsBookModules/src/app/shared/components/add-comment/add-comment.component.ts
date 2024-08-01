import { Component, Input, SimpleChanges } from '@angular/core';
import { AddCommentService } from './service/add-comment.service';
import { CB_CustomsItemComputedDataList, RemarksClassificationPM } from '../main-display/main-display.component';
import { API_MainService, Filters } from '../../../core/API_MainService';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SessionInfo } from '../../../core/Infrastructure/Utilities/SessionInfo';

@Component({
  selector: 'app-add-comment',
  standalone: true,
  imports: [],
  templateUrl: './add-comment.component.html',
  styleUrl: './add-comment.component.css'
})
export class AddCommentComponent {
  constructor(private addCommentService: AddCommentService, private API_MainService: API_MainService, private snackBar: MatSnackBar) { }
  remarksClassificationPM: RemarksClassificationPM;
  showAddComment: boolean = false;
  commentText: string = '';
  currentItem: CB_CustomsItemComputedDataList;

  ngOnInit() {
    this.addCommentService.isOpened.subscribe((isOpened: boolean) => {
      this.showAddComment = isOpened;
    });
    this.addCommentService.itemData.subscribe((data: CB_CustomsItemComputedDataList) => {
      this.currentItem = data;
    });
  }

  updateCommentText(event: any) {
    this.commentText = event.target.value;
  }

  ngOnChanges(changes: SimpleChanges) {

    if (changes['showAddComment']) {
      this.showAddComment = changes['showAddComment'].currentValue;
      this.addCommentService.setIsOpened(this.showAddComment);
      console.log('showAddComment', this.showAddComment);
    }
  }

  // send comment to server:
  sendComment() {

    let remarksClassificationPM: RemarksClassificationPM = {
      tenant: SessionInfo.LoggedUserTenant,
      customsItemsID: this.currentItem?.CustomsItemID,
      remarkDescription: this.commentText != null && this.commentText != '' ? this.commentText : '',
    };
    if (remarksClassificationPM.remarkDescription == '' || !remarksClassificationPM.customsItemsID) return;
    this.addCommentService.setIsOpened(false);

    this.API_MainService.AddNEWRemarksClassification(remarksClassificationPM).subscribe((data: any) => {
      if (!data.body) this.showMessage('שגיאה בהוספת הערה'); // change color
      this.showMessage('הערה נוספה בהצלחה');
      this.addCommentService.allComments.next([...this.addCommentService.allComments.getValue(), data.body]);
    });

    this.commentText = '';
  }

  showMessage(message?: string): void {
    if (message === "") return;
    this.snackBar.open(message, 'סגור',
      {
        duration: 2000
      });
  }
}
