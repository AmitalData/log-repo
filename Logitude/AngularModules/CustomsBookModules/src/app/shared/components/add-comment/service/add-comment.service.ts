import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CB_CustomsItemComputedDataList } from '../../main-display/main-display.component';

@Injectable({
  providedIn: 'root'
})
export class AddCommentService {
  isOpened: BehaviorSubject<boolean>;
  itemData: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);

  constructor() {
    this.isOpened = new BehaviorSubject<boolean>(false);
  }

  setIsOpened(value: boolean, data?: CB_CustomsItemComputedDataList) {
    this.isOpened.next(value);
    if (data)
      this.itemData.next(data);
  }

  getIsOpened() {
    return this.isOpened;
  }
  getItemData() {
    return this.itemData.getValue();
  }
}
