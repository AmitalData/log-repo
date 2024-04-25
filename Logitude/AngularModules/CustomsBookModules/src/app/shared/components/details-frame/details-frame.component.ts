import { Component } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faStar as faStarBold } from '@fortawesome/free-solid-svg-icons';
import { faSquareCaretRight, faFileText, faSquareCheck, faCommentAlt, faStar, faCommentDots, faPenToSquare, faTrashCan } from '@fortawesome/free-regular-svg-icons';
import { AccordionComponent } from '../accordion/accordion.component';
import { Output, Input, EventEmitter } from '@angular/core';
import { CommentsComponent } from '../comments/comments.component';




@Component({
  selector: 'app-details-frame',
  standalone: true,
  imports: [FontAwesomeModule, AccordionComponent, CommentsComponent],
  templateUrl: './details-frame.component.html',
  styleUrl: './details-frame.component.css'
})


export class DetailsFrameComponent {
  @Output() showDetails = new EventEmitter<boolean>();
  @Input() showAddComment: boolean = false;
  showComments: boolean = false;
  show: boolean = false;
  checked: boolean = false;
  faSquareCheck = faSquareCheck;
  faCommentAlt = faCommentAlt;
  faStar = faStar;
  faStarBold = faStarBold;
  faComments = faCommentDots;
  faPenToSquare = faPenToSquare;
  faTrashCan = faTrashCan;
  faCaretSquareRight = faSquareCaretRight;
  faFileArchive = faFileText;
}

