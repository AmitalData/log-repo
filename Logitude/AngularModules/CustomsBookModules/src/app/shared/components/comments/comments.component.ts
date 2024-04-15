
import { Component, Input, SimpleChanges } from '@angular/core';
import { AddCommentService } from '../add-comment/service/add-comment.service';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


@Component({
  selector: 'app-comments',
  standalone: true,
  imports: [FontAwesomeModule],
  templateUrl: './comments.component.html',
  styleUrl: './comments.component.css'
})
export class CommentsComponent {
  @Input() showComments: boolean = false;
  // faPlusCircle = faPlusCircle;

  constructor(private addCommentService: AddCommentService) { }
  showAddComment = this.addCommentService.getIsOpened();

  showAddCommentSidebar() {
    this.addCommentService.setIsOpened(true);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['showComments']) {
      this.showComments = changes['showComments'].currentValue;

    }
  }
}

