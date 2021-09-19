import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-new-quote-expected-order',
  templateUrl: './new-quote-expected-order.component.html',
  styleUrls: ['./new-quote-expected-order.component.scss']
})
export class NewQuoteExpectedOrderComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});

  constructor() { }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('grrosWeight'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('grrosWeight', new FormControl(''));
    this.formGroup.addControl('volume', new FormControl(''));
    this.formGroup.addControl('chargeableWeight', new FormControl(''));
    this.formGroup.addControl('numberOfPackages', new FormControl(''));
    this.formGroup.addControl('dangerousGoods', new FormControl(''));
    this.formGroup.addControl('descriptionOfGoods', new FormControl(''));
    this.formGroup.addControl('notes', new FormControl(''));
  }
}
