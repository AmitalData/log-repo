
import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { API_MainService } from '../../../core/API_MainService';
import { CB_CustomsItemComputedDataList, RemarksClassificationList } from '../main-display/main-display.component';
import { BehaviorSubject } from 'rxjs';


@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [FontAwesomeModule],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent implements OnInit, OnChanges {
  @Input() showComments: boolean = false;
  //@Input() currentItem: BehaviorSubject<CB_CustomsItemComputedDataList> = new BehaviorSubject<CB_CustomsItemComputedDataList>(null);

  // faPlusCircle = faPlusCircle;

  constructor(private addCommentService: AddCommentService, private API_MainService: API_MainService) { }
  showAddComment = this.addCommentService.getIsOpened();

  showAddCommentSidebar() {
    this.addCommentService.setIsOpened(true);
  }

  ngOnInit(): void {
    this.showCommentsByClick();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['showComments']) {
      this.showComments = changes['showComments'].currentValue;

    }
  }


  
  showCommentsByClick() {
    //this.API_MainService.GetAllCommentsByCustomsItemId(this.currentItem.getValue().CustomsItemID, this.API_MainService.getTenant()).subscribe((data: RemarksClassificationList[]) => {
    //  console.log(data);
    //  this.addCommentService.allComments.next(data);
   // });
  }
}

