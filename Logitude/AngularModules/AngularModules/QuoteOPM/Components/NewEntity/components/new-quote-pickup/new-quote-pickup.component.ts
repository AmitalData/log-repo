import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-pickup',
  templateUrl: './new-quote-pickup.component.html',
  styleUrls: ['./new-quote-pickup.component.scss']
})
export class NewQuotePickupComponent implements OnInit {
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
    if (!this.formGroup.contains('pickupInclude')) {
      this.addFormControls()
      this.subscribeCtrls();
      this.initDefaultValue()
    }
  }

  addFormControls() {
    this.formGroup.addControl('pickupInclude', new FormControl(''));
    this.formGroup.addControl('pickupZipCode', new FormControl(''));
    this.formGroup.addControl('pickupCity', new FormControl(''));
    this.formGroup.addControl('pickupCountry', new FormControl(''));
  }

  subscribeCtrls() {
    this.formGroup.controls.pickupCity.valueChanges.subscribe(val=>this.EntityPM.FromAddressCity = val)
      this.formGroup.controls.pickupZipCode.valueChanges.subscribe(val=> this.EntityPM.FromAddressZipCode = val);
      this.formGroup.controls.pickupCity.valueChanges.subscribe(val=> this.EntityPM.FromAddressCity = val); // need change when update field to autocomplate
  }
    
  initDefaultValue() {
    this.formGroup.controls.pickupInclude.setValue(false)
  }

  includeCheckboxChange(e: {checked: boolean, originalEvent: PointerEvent}) {
    if(e)
      this.EntityPM.IncludePickUp = e.checked;
  }
  
  // onSelectedCity(e) {
  //   console.log(e)    
  // }
  
  onSelectedCountry(country: CountryList) {
    this.EntityPM.FromAddressCountryId = country.Id;
  }
}
