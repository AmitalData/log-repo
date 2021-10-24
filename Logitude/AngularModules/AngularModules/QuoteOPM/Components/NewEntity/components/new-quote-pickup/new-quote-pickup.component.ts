import { ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { DialogsService } from '../../Services/dialogs/dialogs.service';
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
    private dialogsService: DialogsService,
    private cdr: ChangeDetectorRef,
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
    this.propertyForm.addControl('pickupAddress', new FormControl(''));
  }

  subscribeCtrls() {
    this.propertyForm.controls.pickupAddress.valueChanges.subscribe((addressId: AddressList) => {
      this.Address = this.AddressList.find(x => x == addressId)
      this.cdr.detectChanges()
    });

    this.qouteForm.controls.shipperName.valueChanges.subscribe(async (shipperName: CardList) => {
      if (shipperName) {
        this.AddressList = await this.newQuoteDataService.getAddresses(shipperName.Id, shipperName.Tenant);
        this.Address = this.AddressList.length ? this.AddressList[0] : null
        this.propertyForm.controls.pickupAddress.setValue(this.Address)
      } else {
        this.Address = null
        this.AddressList = []
        this.propertyForm.controls.pickupAddress.setValue(null)
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

  addAddress() {
    this.dialogsService.addAddress('P', this.qouteForm.controls.shipperName.value.Id)
  }

  editAddress() {
    this.dialogsService.editAddress(this.Address.Id)
  }
}
