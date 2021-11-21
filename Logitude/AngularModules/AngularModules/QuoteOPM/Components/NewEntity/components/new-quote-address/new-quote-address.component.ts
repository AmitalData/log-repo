import { ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl, AbstractControl, Validators, ValidationErrors, ValidatorFn } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { AddressCode, DialogsService } from '../../Services/dialogs/dialogs.service';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-address',
  templateUrl: './new-quote-address.component.html',
  styleUrls: ['./new-quote-address.component.scss']
})
export class NewQuoteAddressComponent implements OnInit {
  @Input() qouteForm: FormGroup
  @Input() propertyForm: FormGroup
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() type: 'pickup' | 'delivery';

  addressForm: FormGroup = new FormGroup({
    include: new FormControl(),
    zipCode: new FormControl(),
    city: new FormControl(),
    country: new FormControl(),
    address: new FormControl(),
  })

  countryList: CountryList[] = []
  cityListAll: CountryCityList[] = []
  cityList: CountryCityList[] = []
  // cardList: CardList[] = []
  Address: AddressList = null;
  AddressList: AddressList[] = [];

  get partnerFrom(): string {
    return this.type === 'delivery' ? 'consignee' : 'shipper';
  }

  get addressCode(): AddressCode {
    return this.type === 'delivery' ? 'D' : 'P';
  }

  get capitalizeType(): string {
    return this.type === 'delivery' ? 'Delivery' : 'Pickup';
  }

  get partnerCtrl(): FormControl {
    return (this.qouteForm.controls[this.partnerFrom] as FormGroup).controls.partner as FormControl;
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private dialogsService: DialogsService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.InitCountries();
    this.InitCity();
    // this.initCardList()
    this.addForm()
    this.subscribeCtrls();
    this.initDefaultValue()
    this.setvalidatorToCityAndCountry()   
  }

  private async InitCountries() {
    this.countryList = await this.newQuoteDataService.getCounriesTable();
  }

  private async InitCity() {
    this.cityListAll = await this.newQuoteDataService.getCityTable();
    this.cityList = this.cityListAll;
  }

  // private async initCardList() {
  //   this.cardList = await this.newQuoteDataService.getCardsTable();
  // }

  addForm() {
    this.propertyForm.addControl(this.type, this.addressForm)
  }

  subscribeCtrls() {
    this.addressForm.controls.include.valueChanges.subscribe(() => {
      this.setvalidatorToCityAndCountry();
    });

    this.addressForm.controls.address.valueChanges.subscribe((addressId: AddressList) => {
      if (!addressId && !this.Address) return;

      this.Address = this.AddressList.find(x => x == addressId)
      this.cdr.detectChanges()
    });

    this.partnerCtrl.valueChanges.subscribe(async (partner: CardList) => {
      this.setvalidatorToCityAndCountry();

      if (partner)
        await this.initPartnerData(partner);
      else {
        this.AddressList = []
        this.Address = null
        this.addressForm.controls.address.setValue(null)
      }
    });
  }

  private setvalidatorToCityAndCountry() {
    const validator: ValidatorFn | null = !this.partnerCtrl.value && this.addressForm.controls.include.value ? Validators.required : null;
    this.addressForm.controls.city.setValidators(validator);
    this.addressForm.controls.city.updateValueAndValidity();
    this.addressForm.controls.country.setValidators(validator);
    this.addressForm.controls.country.updateValueAndValidity();
  }

  private async initPartnerData(partner: CardList) {
    this.AddressList = await this.newQuoteDataService.getAddresses(partner.Id, partner.Tenant);
    this.Address = this.AddressList.length ? this.AddressList[0] : null;
    this.addressForm.controls.address.setValue(this.Address);
  }

  async initDefaultValue() {
    const partner: CardList = this.partnerCtrl.value;
    if (partner)
      await this.initPartnerData(partner);
  }

  addAddress() {
    this.dialogsService.addAddress(this.addressCode, this.partnerCtrl.value?.Id)
  }

  editAddress() {
    this.dialogsService.editAddress(this.Address.Id)
  }
}
