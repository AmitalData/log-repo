import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

export const transporations: RadioBtnList = [
  { value: 'Air', icon: 'assets/icons/airplane.png' },
  { value: 'Ocean', icon: 'assets/icons/ocean.png' },
  { value: 'Inland', icon: 'assets/icons/inland.png' },
]

@Component({
  selector: 'app-new-quote-left-side',
  templateUrl: './new-quote-left-side.component.html',
  styleUrls: ['./new-quote-left-side.component.scss']
})
export class NewQuoteLeftSideComponent implements OnInit {
  transporations = transporations;
  
  directions: RadioBtnList = [
    { value: 'Export', icon: 'assets/icons/box-up.png' },
    { value: 'Import', icon: 'assets/icons/house.png' },
    { value: 'Domestic', icon: 'assets/icons/box-down.png' },
    { value: 'Drop', icon: 'assets/icons/recycle.png' },
  ]
  
  shipmentTypes: RadioBtnList = [
    { value: 'FTL', icon: 'assets/icons/inland.png' },
    { value: 'LTL', icon: 'assets/icons/inland.png' },
  ]


  @Input() formGroup: FormGroup = new FormGroup({});
  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('shipmentType'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('direction', new FormControl(''));
    this.formGroup.addControl('transporation', new FormControl(''));
    this.formGroup.addControl('shipmentType', new FormControl(''));
  }
}

type RadioBtnList = {
  value: string
  icon: string
}[]
