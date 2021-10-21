import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filterIsNotNull, NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-pickup',
  templateUrl: './new-quote-pickup.component.html',
  styleUrls: ['./new-quote-pickup.component.scss']
})
export class NewQuotePickupComponent implements OnInit {
  @Input() qouteForm: FormGroup
  @Input() propertyForm: FormGroup
  @Input() EntityPM: QuoteOPPM = null as any;

  countryList: CountryList[] = []
  cityListAll: CountryCityList[] = []
  cityList: CountryCityList[] = []
  shipperNames: CardList[] = []
  Address: AddressList = null;
  AddressList: AddressList[] = [];

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.InitCountries();
    this.InitCity();
    this.initShipperNames()
  }

  private async InitCountries() {
    this.countryList = await this.newQuoteDataService.getCounriesTable();
  }

  private async InitCity() {
    this.cityListAll = await this.newQuoteDataService.getCityTable();
    this.cityList = this.cityListAll;
  }

  private async initShipperNames() {
    this.shipperNames = await this.newQuoteDataService.getCardsTable();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.propertyForm.contains('pickupInclude')) {
      this.addFormControls()
      this.subscribeCtrls();
      this.initDefaultValue()
    }
  }

  addFormControls() {
    this.propertyForm.addControl('pickupInclude', new FormControl(''));
    this.propertyForm.addControl('pickupZipCode', new FormControl(''));
    this.propertyForm.addControl('pickupCity', new FormControl(''));
    this.propertyForm.addControl('pickupCountry', new FormControl(''));
    this.propertyForm.addControl('pickupAddressId', new FormControl(''));
  }

  subscribeCtrls() {
    this.qouteForm.controls.shipperName.valueChanges.subscribe(async (shipperName: CardList) => {
      if (shipperName) {
        this.AddressList = shipperName ? await this.newQuoteDataService.getAddresses(shipperName.Id, shipperName.Tenant) : null;
        this.Address = this.AddressList.length ? this.AddressList[0] : null
        this.propertyForm.controls.pickupAddressId.setValue(this.Address)
      } else {
        this.Address = null
        this.AddressList = []
        this.propertyForm.controls.pickupAddressId.setValue(null)
      }
    });
  }

  initDefaultValue() {
    this.propertyForm.controls.pickupInclude.setValue(true)
  }

  includeCheckboxChange(e: { checked: boolean, originalEvent: PointerEvent }) {
    // if (e) 
    //   this.EntityPM.IncludePickUp = e.checked;
  }

  onSelectedCountry(country: CountryList) {
    // this.EntityPM.FromAddressCountryId = country.Id;
  }
}
