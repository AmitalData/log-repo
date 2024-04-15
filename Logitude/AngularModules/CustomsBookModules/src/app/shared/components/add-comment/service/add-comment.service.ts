import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AddCommentService {
  isOpened: BehaviorSubject<boolean>;

  constructor() {
    this.isOpened = new BehaviorSubject<boolean>(false);
  }

  setIsOpened(value: boolean) {
    this.isOpened.next(value);
  }
  getIsOpened() {
    return this.isOpened;
  }
}
