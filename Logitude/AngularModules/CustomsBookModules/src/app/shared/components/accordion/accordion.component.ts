import { Component } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { faChevronLeft, faChevronDown } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-accordion',
  standalone: true,
  imports: [FontAwesomeModule],
  templateUrl: './accordion.component.html',
  styleUrl: './accordion.component.css',
})
export class AccordionComponent {
  expanded: boolean = true;
  faChevronLeft = faChevronLeft;
  faChevronDown = faChevronDown;
}
