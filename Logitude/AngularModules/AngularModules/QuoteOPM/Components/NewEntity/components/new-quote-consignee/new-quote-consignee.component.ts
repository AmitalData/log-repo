import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NewQuoteAutocomplateService } from '../new-quote-autocomplate/new-quote-autocomplate.service';

@Component({
  selector: 'app-new-quote-consignee',
  templateUrl: './new-quote-consignee.component.html',
  styleUrls: ['./new-quote-consignee.component.scss']
})
export class NewQuoteConsigneeComponent implements OnInit {

  consigneeNamesSelected: string[] = []
  consigneeNames: string[] = ['ac', 'bb']

  consigneeContactSelected: string[] = []
  consigneeContacts: string[] = ['a', 'b']

  @Input() formGroup: FormGroup = new FormGroup({});
  keyUp: any;

  constructor(
    private autocomplateService: NewQuoteAutocomplateService
  ) {
    this.keyUp = autocomplateService.keyUp;
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('consigneeName'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('consigneeName', new FormControl(''));
    this.formGroup.addControl('consigneeContact', new FormControl(''));
    this.formGroup.addControl('consigneeAddress', new FormControl(''));
    this.formGroup.addControl('consigneeNotes', new FormControl(''));
    this.formGroup.addControl('consigneeReference1', new FormControl(''));
    this.formGroup.addControl('consigneeReference2', new FormControl(''));
  }


  searchConsigneeNames(event: any) {
    this.consigneeNamesSelected = this.consigneeNames.filter(x => x.includes(event.query));
  }

  searchConsigneeContact(event: any) {
    this.consigneeContactSelected = this.consigneeContacts.filter(x => x.includes(event.query));
  }

}
