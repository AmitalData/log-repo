import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-accordion',
  templateUrl: './accordion.component.html',
  styleUrls: ['./accordion.component.scss']
})
export class AccordionComponent implements OnInit {
  @Input() label: string = '';
  @Input() isHidden:boolean = true;
  @Output() isHiddenChange = new EventEmitter<boolean>();

  toggleHidden() {
    this.isHidden = !this.isHidden;
    this.isHiddenChange.emit(this.isHidden)
  }

  constructor() { }

  ngOnInit(): void {
  }

}
