import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NewQuoteAutocomplateService } from '../new-quote-autocomplate/new-quote-autocomplate.service';

@Component({
  selector: 'app-new-quote-delivery',
  templateUrl: './new-quote-delivery.component.html',
  styleUrls: ['./new-quote-delivery.component.scss']
})
export class NewQuoteDeliveryComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  
  deliveryCountrySelected: string[] = []
  countryList: string[] = ['a', 'b']

  keyUp:any;
  constructor(
    private autocomplateService: NewQuoteAutocomplateService
  ) { 
    this.keyUp = autocomplateService.keyUp;
  }

  ngOnInit(): void {
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('deliveryInclude'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('deliveryInclude', new FormControl(''));
    this.formGroup.addControl('deliveryZipCode', new FormControl(''));
    this.formGroup.addControl('deliveryCity', new FormControl(''));
    this.formGroup.addControl('deliveryCountry', new FormControl(''));
  }


  deliveryCountrySearch(event: any) {
    this.deliveryCountrySelected = this.countryList.filter(x => x.includes(event.query));
  }
}
