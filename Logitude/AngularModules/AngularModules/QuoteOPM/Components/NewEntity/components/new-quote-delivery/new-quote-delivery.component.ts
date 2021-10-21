import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filterIsNotNull, NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-delivery',
  templateUrl: './new-quote-delivery.component.html',
  styleUrls: ['./new-quote-delivery.component.scss']
})
export class NewQuoteDeliveryComponent implements OnInit {
  @Input() qouteForm: FormGroup
  @Input() propertyForm: FormGroup
  @Input() EntityPM: QuoteOPPM = null as any;

  countryList: CountryList[] = []
  cityListAll: CountryCityList[] = []
  cityList: CountryCityList[] = []
  consigneeNames: any[] = []
  Address: AddressList = null;
  AddressList: AddressList[] = [];

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.InitCountries();
    this.InitCity();
    this.initConsigneeNames();
  }

  private async InitCountries() {
    this.countryList = await this.newQuoteDataService.getCounriesTable();
  }

  private async InitCity() {
    this.cityListAll = await this.newQuoteDataService.getCityTable();
    this.cityList = this.cityListAll;
  }

  private async initConsigneeNames() {
    this.consigneeNames = await this.newQuoteDataService.getCardsTable();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.propertyForm.contains('deliveryInclude')) {
      this.addFormControls()
      this.subscribeCtrls();
      this.initDefaultValue()
    }
  }

  addFormControls() {
    this.propertyForm.addControl('deliveryInclude', new FormControl(''));
    this.propertyForm.addControl('deliveryZipCode', new FormControl(''));
    this.propertyForm.addControl('deliveryCity', new FormControl(''));
    this.propertyForm.addControl('deliveryCountry', new FormControl(''));
    this.propertyForm.addControl('deliveryAddressId', new FormControl(''));
  }

  subscribeCtrls() {
    this.qouteForm.controls.consigneeName.valueChanges.subscribe(async (consigneeName: CardList) => {
      if (consigneeName) {
        this.AddressList = consigneeName ? await this.newQuoteDataService.getAddresses(consigneeName.Id, consigneeName.Tenant) : null;
        this.Address = this.AddressList.length ? this.AddressList[0] : null
        this.propertyForm.controls.deliveryAddressId.setValue(this.Address)
      } else {
        this.Address = null
        this.AddressList = []
        this.propertyForm.controls.deliveryAddressId.setValue(null)
      }
    });
  }

  initDefaultValue() {
    this.propertyForm.controls.deliveryInclude.setValue(true)
  }

  includeCheckboxChange(e: { checked: boolean, originalEvent: PointerEvent }) {
    // if (e)
    //   this.EntityPM.IncludeDelivery = e.checked;
  }

  onSelectedCountry(country: CountryList) {
    // this.EntityPM.ToAddressCountryId = country.Id;
  }
}
