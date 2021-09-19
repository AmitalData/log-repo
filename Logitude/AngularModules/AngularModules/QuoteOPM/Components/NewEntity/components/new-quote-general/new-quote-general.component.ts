import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NewQuoteAutocomplateService } from '../new-quote-autocomplate/new-quote-autocomplate.service';

@Component({
  selector: 'app-new-quote-general',
  templateUrl: './new-quote-general.component.html',
  styleUrls: ['./new-quote-general.component.scss']
})
export class NewQuoteGeneralComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});

  quoteTypes: string[] = ['Spot Rate', 'Routiing Rated']

  moveTypeSelected: string[] = []
  moveTypeList: string[] = ['a', 'b']

  keyUp:any;

  constructor(
    private autocomplateService: NewQuoteAutocomplateService
  ) { 
    this.keyUp = autocomplateService.keyUp;
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('quoteType'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('quoteType', new FormControl(''));
    this.formGroup.addControl('startDate', new FormControl(''));
    this.formGroup.addControl('moveType', new FormControl(''));
    this.formGroup.addControl('ExpirationDays', new FormControl(''));
    this.formGroup.addControl('closeAutomaitcally', new FormControl(''));
    this.formGroup.addControl('expirationDate', new FormControl(''));
    this.formGroup.addControl('closeDate', new FormControl(''));
  }

  moveTypeSearch(event: any) {
    this.moveTypeSelected = this.moveTypeList.filter(x => x.includes(event.query));
  }

  expirationDaysChange(e: any) {
    let date: Date = this.formGroup.controls.startDate.value;
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.expirationDate.setValue(date);
    }
  }

  closeDaysChange(e: any) {
    let date: Date = this.formGroup.controls.startDate.value;
    if (-1 < e.value) {
      date.setDate(date.getDate() + e.value)
      this.formGroup.controls.closeDate.setValue(date);
    }
  }
}
