import { Component, SimpleChanges } from '@angular/core';
import { AddCommentService } from './service/add-comment.service';

@Component({
  selector: 'app-add-comment',
  standalone: true,
  imports: [],
  templateUrl: './add-comment.component.html',
  styleUrl: './add-comment.component.css'
})
export class AddCommentComponent {

  constructor(private addCommentService: AddCommentService) { }
  showAddComment: boolean = false;
  commentText: string = '';

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

}
