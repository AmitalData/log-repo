import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { NewQuoteAutocomplateService } from '../new-quote-autocomplate/new-quote-autocomplate.service';

@Component({
  selector: 'app-new-quote-pickup',
  templateUrl: './new-quote-pickup.component.html',
  styleUrls: ['./new-quote-pickup.component.scss']
})
export class NewQuotePickupComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});
  
  pickupCountrySelected: string[] = []
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
    if (!this.formGroup.contains('pickupInclude'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('pickupInclude', new FormControl(''));
    this.formGroup.addControl('pickupZipCode', new FormControl(''));
    this.formGroup.addControl('pickupCity', new FormControl(''));
    this.formGroup.addControl('pickupCountry', new FormControl(''));
  }


  pickupCountrySearch(event: any) {
    this.pickupCountrySelected = this.countryList.filter(x => x.includes(event.query));
  }

}
