import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MenuItem } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { DialogsService, PartnerType } from '../../Services/dialogs/dialogs.service';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-partner',
  templateUrl: './new-quote-partner.component.html',
  styleUrls: ['./new-quote-partner.component.scss']
})
export class NewQuotePartnerComponent implements OnInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() type: 'shipper' | 'consignee';

  cardsList: CardList[] = []
  contactsList: ContactList[] = []
  Address: AddressList = null as any;
  ddl: MenuItem[] = [];


  partnerform: FormGroup = new FormGroup({
    partner: new FormControl(),
    contact: new FormControl(),
    notes: new FormControl(),
    reference1: new FormControl(),
    reference2: new FormControl(),
  })

  get partnerType (): PartnerType  {
    return this.type === 'shipper' ? 'SH' : 'CO';
  }

  get capitalizeType(): string {
    return this.capitalizeFirstLetter(this.type);
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private dialogsService: DialogsService,
    // private cdr: ChangeDetectorRef,
  ) { }

  ngOnInit(): void {
    this.initCards();
    this.partnerform.controls.notes.disable();
    this.initDdl();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains(this.type)) {
      this.addFormControls();
      this.subscribePartner();
      this.subscribeCtrls();
    }
  }

  private async initCards() {
    this.cardsList = await this.newQuoteDataService.getCardsTable();
  }

  addFormControls() {
    this.formGroup.addControl(this.type, this.partnerform);
  }

  private subscribePartner() {
    this.partnerform.controls.partner.valueChanges.subscribe((partner: CardList) => {
      this.partnerform.controls.contact.reset();
      this.partnerform.controls.notes.setValue(partner?.Notes);
      this.initContacts(partner?.Id);
      this.setAddress(partner);
      this.onSelectedName(partner);
      // this.cdr.detectChanges();
    })
  }

  private subscribeCtrls() {
    this.partnerform.controls.notes.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Note'] = newVal);
    this.partnerform.controls.reference1.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Reference1'] = newVal);
    this.partnerform.controls.reference2.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Reference2'] = newVal);
  }

  private async setAddress(partner: CardList): Promise<void> {
    this.Address = partner ? await this.newQuoteDataService.getAddress(partner.Id, partner.Tenant) : null
  }

  private async initContacts(cardId: string) {
    this.contactsList = cardId ? await this.newQuoteDataService.getContactsTable(cardId) : null;
  }

  private capitalizeFirstLetter(str: string): string {
    return str?.charAt(0).toUpperCase() + str?.slice(1);
  }

  private initDdl() {
    this.ddl = [
      { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.Add' + this.capitalizeType), command: ()=> this.dialogsService.addCustomer(this.capitalizeType) },
      { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.AddPotential'+ this.capitalizeType), command: ()=> this.dialogsService.addPotentialCustomer(this.capitalizeType) },
    ];
  }

  onSelectedName(val: CardList) {
    this.EntityPM[this.capitalizeType + 'Name'] = val?.EnglishName;
    this.EntityPM[this.capitalizeType + 'Id'] = val?.Id;
    this.EntityPM[this.capitalizeType + 'MainAddressId'] = val?.MainAddressId;
    this.EntityPM[this.capitalizeType + 'PickAddressId'] = val?.PickAddressId;
  }

  onSelectedContact(val: ContactList) {
    this.EntityPM[this.capitalizeType + 'ContactId'] = val?.Id;
  }

  addContact(){
    this.dialogsService.addContact(this.partnerform.controls.partner.value.Id, this.partnerType )
  }
}
