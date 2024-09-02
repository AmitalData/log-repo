import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList, RemarksClassificationList, RemarksClassificationPM } from '../../main-display/main-display.component';

@Injectable({
  providedIn: 'root'
})
export class AddCommentService {
  isOpened: BehaviorSubject<boolean>;
  itemData: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);
  CommentMode: BehaviorSubject<CommentState> = new BehaviorSubject<CommentState>(null);
  currentRemark: BehaviorSubject<RemarksClassificationPM> = new BehaviorSubject<RemarksClassificationPM>(null);
  allComments: BehaviorSubject<RemarksClassificationList[]> = new BehaviorSubject<RemarksClassificationList[]>([]);

  constructor() {
    this.isOpened = new BehaviorSubject<boolean>(false);
  }

  setIsOpened(value: boolean, data?: CB_CustomsItemComputedDataList, commentState?: CommentState, currentRemark?: RemarksClassificationPM) {
    this.isOpened.next(value);
    if (data)
      this.itemData.next(data);
    this.currentRemark.next(currentRemark);
    this.CommentMode.next(commentState);
  }

  getIsOpened() {
    return this.isOpened;
  }
  getItemData() {
    return this.itemData.getValue();
  }
}

// enum for the state of the comment: Add, Edit, Delete
export enum CommentState {
  Add = 1,
  Edit = 2,
  Delete = 3
}
