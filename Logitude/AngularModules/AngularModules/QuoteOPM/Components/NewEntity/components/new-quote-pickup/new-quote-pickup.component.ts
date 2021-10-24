import { ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { CountryCityList } from 'Common/EntityLists/CountryCityList';
import { CountryList } from 'Common/EntityLists/CountryList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { AddressCode, DialogsService } from '../../Services/dialogs/dialogs.service';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-pickup',
  templateUrl: './new-quote-pickup.component.html',
  styleUrls: ['./new-quote-pickup.component.scss']
})
export class NewQuotePickupComponent implements OnInit {
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
  cardList: CardList[] = []
  Address: AddressList = null;
  AddressList: AddressList[] = [];

  get partnerCtrl(): string {
    return this.type ==='delivery' ? 'consigneeName' : 'shipperName';
  }
  
  get addressCode(): AddressCode {
    return this.type ==='delivery' ? 'D' : 'P';    
  }

  get capitalizeType(): string {
    return this.type ==='delivery' ? 'Delivery' : 'Pickup';    
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private dialogsService: DialogsService,
    private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.InitCountries();
    this.InitCity();
    this.initCardList()
  }

  private async InitCountries() {
    this.countryList = await this.newQuoteDataService.getCounriesTable();
  }

  private async InitCity() {
    this.cityListAll = await this.newQuoteDataService.getCityTable();
    this.cityList = this.cityListAll;
  }

  private async initCardList() {
    this.cardList = await this.newQuoteDataService.getCardsTable();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.propertyForm.contains(this.type)) {
      this.addForm()
      this.subscribeCtrls();
      this.initDefaultValue()
    }
  }

  addForm() {
    this.propertyForm.addControl(this.type, this.addressForm)
  }

  subscribeCtrls() {
    this.addressForm.controls.address.valueChanges.subscribe((addressId: AddressList) => {
      this.Address = this.AddressList.find(x => x == addressId)
      this.cdr.detectChanges()
    });

    this.qouteForm.controls[this.partnerCtrl].valueChanges.subscribe(async (partner: CardList) => {
      if (partner) {
        this.AddressList = await this.newQuoteDataService.getAddresses(partner.Id, partner.Tenant);
        this.Address = this.AddressList.length ? this.AddressList[0] : null
        this.addressForm.controls.address.setValue(this.Address)
      } else {
        this.Address = null
        this.AddressList = []
        this.addressForm.controls.address.setValue(null)
      }
    });
  }

  initDefaultValue() {
    this.addressForm.controls.include.setValue(true)
  }

  addAddress() {
    this.dialogsService.addAddress(this.addressCode, this.qouteForm.controls[this.partnerCtrl].value.Id)
  }

  editAddress() {
    this.dialogsService.editAddress(this.Address.Id)
  }
}
