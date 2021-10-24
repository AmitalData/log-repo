import { ChangeDetectorRef, Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MenuItem } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { DialogsService } from '../../Services/dialogs/dialogs.service';
import { filterIsNotNull, NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-consignee',
  templateUrl: './new-quote-consignee.component.html',
  styleUrls: ['./new-quote-consignee.component.scss']
})
export class NewQuoteConsigneeComponent implements OnInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;

  consigneeNames: CardList[] = []
  consigneeContacts: ContactList[] = []
  Address: AddressList = null as any;
  ddl: MenuItem[] = [
    { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.Consignee'), command: ()=> this.dialogsService.addCustomer('Consignee') },
    { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.AddPotentialConsignee'), command: ()=> this.dialogsService.addPotentialCustomer('Consignee') },
  ];

  addContact(){
    this.dialogsService.addContact(this.formGroup.controls.consigneeName.value.Id, 'CO')
  }
  constructor(
    private dialogsService: DialogsService,
    private newQuoteDataService: NewQuoteDataService,
    // private cdref: ChangeDetectorRef,

  ) {}

  ngOnInit(): void {
    this.initConsigneeNames();
    this.formGroup.controls.consigneeNotes.disable();
    // this.test();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains('consigneeName')) {
      this.addFormControls();
      this.subscribeConsigneeName();
      this.subscribeCtrls();
    }
  }

  // async test() {
  //   while (!this.consigneeNames.length) {
  //     console.log('wait')
  //     await new Promise(resolve => setTimeout(resolve, 100));
  //   }
  //   this.formGroup.controls.consigneeName.setValue(this.consigneeNames[16])
  //   this.onSelectedName(this.consigneeNames[16])

  //   while (!this.consigneeContacts.length) {
  //     console.log('wait')
  //     await new Promise(resolve => setTimeout(resolve, 100));
  //   }

  //   this.formGroup.controls.consigneeContact.setValue(this.consigneeContacts[0])
  //   this.onSelectedContact(this.consigneeContacts[0])
  // }

  private async initConsigneeNames() {
    this.consigneeNames = await this.newQuoteDataService.getCardsTable();
  }

  addFormControls() {
    this.formGroup.addControl('consigneeName', new FormControl(''));
    this.formGroup.addControl('consigneeContact', new FormControl(''));
    this.formGroup.addControl('consigneeNotes', new FormControl(''));
    this.formGroup.addControl('consigneeReference1', new FormControl(''));
    this.formGroup.addControl('consigneeReference2', new FormControl(''));
  }

  private subscribeConsigneeName() {
    this.formGroup.controls.consigneeName.valueChanges.subscribe((consigneeName: CardList) => {
      this.formGroup.controls.consigneeContact.reset();
      this.formGroup.controls.consigneeNotes.setValue(consigneeName?.Notes);
      this.initConsigneeContacts(consigneeName?.Id);
      this.setConsigneeAddress(consigneeName);
      this.onSelectedName(consigneeName);
    })
  }

  private subscribeCtrls() {
    this.formGroup.controls.consigneeNotes.valueChanges.subscribe(newVal => this.EntityPM.ConsigneeNote = newVal);
    this.formGroup.controls.consigneeReference1.valueChanges.subscribe(newVal => this.EntityPM.ConsigneeReference1 = newVal);
    this.formGroup.controls.consigneeReference2.valueChanges.subscribe(newVal => this.EntityPM.ConsigneeReference2 = newVal);
  }

  private async setConsigneeAddress(consigneeName: any): Promise<void> {
    this.Address = consigneeName ? await this.newQuoteDataService.getAddress(consigneeName.Id, consigneeName.Tenant) : null
  }

  private async initConsigneeContacts(cardId: string) {
    this.consigneeContacts = cardId ? await this.newQuoteDataService.getContactsTable(cardId) : null
  }

  onSelectedName(val: CardList) {
      this.EntityPM.ConsigneeName = val?.EnglishName;
      this.EntityPM.ConsigneeId = val?.Id;
      this.EntityPM.ConsigneeMainAddressId = val?.MainAddressId;
      this.EntityPM.ConsigneePickAddressId = val?.PickAddressId;
  }
  
  onSelectedContact(val: ContactList){
    this.EntityPM.ConsigneeContactId = val?.Id;
  }
}
