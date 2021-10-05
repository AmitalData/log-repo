import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filterIsNotNull, NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-shipper',
  templateUrl: './new-quote-shipper.component.html',
  styleUrls: ['./new-quote-shipper.component.scss']
})
export class NewQuoteShipperComponent implements OnInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;

  shipperNames: CardList[] = []
  shipperContacts: ContactList[] = []
  Address: AddressList = null as any;

  constructor(
    private newQuoteDataService: NewQuoteDataService,
  ) { }

  ngOnInit(): void {
    this.initShipperNames();
    this.formGroup.controls.shipperNotes.disable();
    this.test();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('shipperName')) {
      this.addFormControls();
      this.subscribeShipperName();
      this.subscribeCtrls();
    }
  }

  async test() {
    while (!this.shipperNames.length) {
      console.log('wait')
      await new Promise(resolve => setTimeout(resolve, 100));
    }
    this.formGroup.controls.shipperName.setValue(this.shipperNames[16])
    this.onSelectedName(this.shipperNames[16])

    while (!this.shipperContacts.length) {
      console.log('wait')
      await new Promise(resolve => setTimeout(resolve, 100));
    }

    this.formGroup.controls.shipperContact.setValue(this.shipperContacts[0])
    this.onSelectedContact(this.shipperContacts[0])
  }

  private async initShipperNames() {
    this.shipperNames = await this.newQuoteDataService.getCardsTable();
  }

  addFormControls() {
    this.formGroup.addControl('shipperName', new FormControl(''));
    this.formGroup.addControl('shipperContact', new FormControl(''));
    this.formGroup.addControl('shipperNotes', new FormControl(''));
    this.formGroup.addControl('shipperReference1', new FormControl(''));
    this.formGroup.addControl('shipperReference2', new FormControl(''));
  }

  private subscribeShipperName() {
    this.formGroup.controls.shipperName.valueChanges.pipe(filterIsNotNull()).subscribe((shipperName: CardList) => {
      this.formGroup.controls.shipperContact.reset();
      this.formGroup.controls.shipperNotes.setValue(shipperName.Notes);
      this.initShipperContacts(shipperName.Id);
      this.setShipperAddress(shipperName);
    })
  }

  private subscribeCtrls() {
    this.formGroup.controls.shipperNotes.valueChanges.subscribe(newVal => this.EntityPM.ShipperNote = newVal);
    this.formGroup.controls.shipperReference1.valueChanges.subscribe(newVal => this.EntityPM.ShipperReference1 = newVal);
    this.formGroup.controls.shipperReference2.valueChanges.subscribe(newVal => this.EntityPM.ShipperReference2 = newVal);
  }

  private async setShipperAddress(shipperName: CardList): Promise<void> {
    this.Address = await this.newQuoteDataService.getAddress(shipperName.Id, shipperName.Tenant)
  }

  private async initShipperContacts(cardId: string) {
    this.shipperContacts = await this.newQuoteDataService.getContactsTable(cardId);
  }

  onSelectedName(val: CardList) {
    this.EntityPM.ShipperName = val.EnglishName;
    this.EntityPM.ShipperId = val.Id;
    this.EntityPM.ShipperMainAddressId = val.MainAddressId;
    this.EntityPM.ShipperPickAddressId = val.PickAddressId;
  }

  onSelectedContact(val: ContactList) {
    this.EntityPM.ShipperContactId = val.Id;
  }
}