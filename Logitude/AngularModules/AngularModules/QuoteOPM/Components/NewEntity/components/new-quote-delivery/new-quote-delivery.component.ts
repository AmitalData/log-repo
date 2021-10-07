import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-delivery',
  templateUrl: './new-quote-delivery.component.html',
  styleUrls: ['./new-quote-delivery.component.scss']
})
export class NewQuoteDeliveryComponent implements OnInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;

  countryList: CountryList[] = []
  cityListAll: CountryCityList[] = []
  cityList: CountryCityList[] = []

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.InitCountries();
    this.InitCity();
  }

  private async InitCountries() {
    this.countryList = await this.newQuoteDataService.getCounriesTable();
  }

  private async InitCity() {
    this.cityListAll = await this.newQuoteDataService.getCityTable();
    this.cityList = this.cityListAll;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('deliveryInclude')) {
      this.addFormControls()
      this.subscribeCtrls();
      this.initDefaultValue()
    }
  }

  addFormControls() {
    this.formGroup.addControl('deliveryInclude', new FormControl(''));
    this.formGroup.addControl('deliveryZipCode', new FormControl(''));
    this.formGroup.addControl('deliveryCity', new FormControl(''));
    this.formGroup.addControl('deliveryCountry', new FormControl(''));
  }

  subscribeCtrls() {
    this.formGroup.controls.deliveryCity.valueChanges.subscribe(val => this.EntityPM.ToAddressCity = val)
    this.formGroup.controls.deliveryZipCode.valueChanges.subscribe(val => this.EntityPM.FromAddressZipCode = val);
    this.formGroup.controls.deliveryCity.valueChanges.subscribe(val => this.EntityPM.FromAddressCity = val); // need change when update field to autocomplate
  }

  initDefaultValue() {
    this.formGroup.controls.deliveryInclude.setValue(false)
  }

  includeCheckboxChange(e: { checked: boolean, originalEvent: PointerEvent }) {
    if (e)
      this.EntityPM.IncludeDelivery = e.checked;
  }

  // onSelectedCity(e) {
  //   console.log(e)    
  // }

  onSelectedCountry(country: CountryList) {
    this.EntityPM.ToAddressCountryId = country.Id;
  }
}
