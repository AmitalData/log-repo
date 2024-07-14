import { Component, Input, SimpleChanges } from '@angular/core';
import { AddCommentService } from './service/add-comment.service';
import { RemarksClassificationPM } from '../main-display/main-display.component';
import { API_MainService, Filters } from '../../../core/API_MainService';

@Component({
  selector: 'app-add-comment',
  standalone: true,
  imports: [],
  templateUrl: './add-comment.component.html',
  styleUrl: './add-comment.component.css'
})
export class AddCommentComponent {
	constructor(private addCommentService: AddCommentService, private API_MainService: API_MainService) { } 
  remarksClassificationPM: RemarksClassificationPM;
  showAddComment: boolean = false;
  commentText: string = '';
  @Input() customsItemsID: number;

  updateCommentText(event: any) {
    this.commentText = event.target.value;
  }

  ngOnInit() {
    this.addCommentService.isOpened.subscribe((isOpened: boolean) => {
      this.showAddComment = isOpened;
    });
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
    console.log(this.commentText);
    
    let remarksClassificationPM: RemarksClassificationPM = {
      tenant: 0,
      customsItemsID: this.customsItemsID,
      remarkDescription: this.commentText != null && this.commentText != '' ? this.commentText : '',
    };
    if (remarksClassificationPM.remarkDescription == '' || !remarksClassificationPM.customsItemsID) return; 

    this.API_MainService.RemarksClassification(remarksClassificationPM).subscribe((data: any) => {
      console.log(data);
    });

    this.commentText = '';
  }

}
