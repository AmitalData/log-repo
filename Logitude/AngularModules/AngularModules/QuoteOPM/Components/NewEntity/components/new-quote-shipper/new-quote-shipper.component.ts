import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CardList } from 'Common/EntityLists/CardList';
import { CardListService } from 'Common/Services/StandardLists/CardListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { NewQuoteAutocomplateService } from '../new-quote-autocomplate/new-quote-autocomplate.service';

@Component({
  selector: 'app-new-quote-shipper',
  templateUrl: './new-quote-shipper.component.html',
  styleUrls: ['./new-quote-shipper.component.scss']
})
export class NewQuoteShipperComponent implements OnInit {
  @Input() formGroup: FormGroup = new FormGroup({});

  shipperNamesSelected: ShipperData[] = []
  shipperNames: ShipperData[] = []

  shipperContactSelected: string[] = []
  shipperContacts: string[] = ['a', 'b']

  keyUp: any;

  constructor(
    private autocomplateService: NewQuoteAutocomplateService
  ) {
    this.keyUp = autocomplateService.keyUp;
  }

  ngOnInit(): void {
    this.initShipperNames();
  }

  private async initShipperNames() {
    const cards: CardList[] = await this.getCards();
    this.shipperNames = cards.map(({ Code, EnglishName, Address1, CityName, CountryName, PartnerTypeName }) => ({ Code, EnglishName, Address1, CityName, CountryName, PartnerTypeName }));
    console.log(this.shipperNames)
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('shipperName'))
      this.addFormControls()
  }

  addFormControls() {
    this.formGroup.addControl('shipperName', new FormControl(''));
    this.formGroup.addControl('shipperContact', new FormControl(''));
    this.formGroup.addControl('shipperAddress', new FormControl(''));
    this.formGroup.addControl('shipperNotes', new FormControl(''));
    this.formGroup.addControl('shipperReference1', new FormControl(''));
    this.formGroup.addControl('shipperReference2', new FormControl(''));
  }


  searchShipperNames(event: any) {
    console.log(event.query, this.shipperNames)
    this.shipperNamesSelected = this.shipperNames.filter(x => 
      x.Code?.toLowerCase().includes(event.query) ||
      x.EnglishName?.toLowerCase().includes(event.query) ||
      x.Address1?.toLowerCase().includes(event.query) ||
      x.CityName?.toLowerCase().includes(event.query) ||
      x.CountryName?.toLowerCase().includes(event.query) ||
      x.PartnerTypeName?.toLowerCase().includes(event.query)
      // x..includes(event.query) 
    );
  }

  onBlurName() {
    
  }

  searchShipperContact(event: any) {
    this.shipperContactSelected = this.shipperContacts.filter(x => x.includes(event.query));
  }

  private async getCards(): Promise<CardList[]> {
    const cards: ServiceResponse = await new CardListService().getAll().toPromise() as ServiceResponse;
    console.log(JSON.stringify(cards.Result))
    return cards.Result as CardList[];
  }
}

type ShipperData = { Code: string; EnglishName: string; Address1: string; CityName: string; CountryName: string; PartnerTypeName: string; }