import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { CardPM } from 'Common/EntityPMs/CardPM';
import { ContactPM } from 'Common/EntityPMs/ContactPM';
import { ContactListService } from 'Common/Services/StandardLists/ContactListService';
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MenuItem } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { BehaviorSubject, Observable } from 'rxjs';
import { DialogsService, PartnerType } from '../../Services/dialogs/dialogs.service';
import { NewQuoteDataShareService } from '../../Services/new-quote-data-share/new-quote-data-share.service';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-partner',
  templateUrl: './new-quote-partner.component.html',
  styleUrls: ['./new-quote-partner.component.scss']
})
export class NewQuotePartnerComponent extends BaseComponent implements OnInit, AfterViewInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() type: 'shipper' | 'consignee' = null as any;

  cardsList: CardList[] = []
  contactsList: ContactList[] = []
  Address: AddressList = null as any;
  ddl: MenuItem[] = [];
  ShipperContact: any;
  ConsigneeContact: any;
  ShipperId: any;
  ConsigneeId: any;
  isSubscribePartner: boolean = false;;
  isHidden: BehaviorSubject<boolean>;

  partnerform: FormGroup = new FormGroup({
    partner: new FormControl(null, Validators.required),
    contact: new FormControl(),
    notes: new FormControl(),
    reference1: new FormControl(),
    // reference2: new FormControl(),
  })

  get secondPartner(): string {
    return this.type === 'shipper' ? 'consignee' : 'shipper';
  }

  get partnerType(): PartnerType {
    return this.type === 'shipper' ? 'SH' : 'CO';
  }

  get capitalizeType(): string {
    return this.capitalizeFirstLetter(this.type);
  }

  constructor(
    private newQuoteDataService: NewQuoteDataService,
    private dialogsService: DialogsService,
    public entityArgs: EntityArgs,
    private cdr: ChangeDetectorRef,
    private dataShareService: NewQuoteDataShareService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.isHidden = this.dataShareService.partnersHidden[this.type as any];

    this.initCards();
    this.partnerform.controls.notes.disable();
    this.initDdl();
    this.resetForm()
    this.subscribeSecondPartner();
  }

  ngAfterViewInit(): void {
    this.setDataFromEntity();
  }

  setDataFromEntity() {
    if (!this.EntityPM || !this.EntityPM.Id) return;

    this.setPartner();
    this.setContact();
    this.partnerform.controls.reference1.setValue(this.EntityPM[this.capitalizeType + 'Reference1']);
    this.partnerform.controls.ConsigneeNote.setValue(this.EntityPM[this.capitalizeType + 'Note']);
  }

  private setValidatorBySecondPrtner() {
    (this.formGroup.controls[this.secondPartner] as FormGroup).controls.partner.valueChanges.subscribe((value: CardList) => {
      const validator = value ? null : Validators.required;
      if (this.partnerform.controls.partner.validator != validator) {
        this.partnerform.controls.partner.setValidators(validator);
        this.partnerform.controls.partner.updateValueAndValidity();
      }
    });
  }

  async setPartner() {
    const prtnerId = this.EntityPM[this.capitalizeType + 'Id']
    if (prtnerId) {
      const card: CardPM = await this.newQuoteDataService.getCard(prtnerId)
      this.partnerform.controls.partner.setValue(card)
    }
  }
  
  async setContact() {
    const contactId = this.EntityPM[this.capitalizeType + 'ContactId']
    if (contactId) {
      const contact: ContactPM = await this.newQuoteDataService.getContact(contactId)
      this.partnerform.controls.contact.setValue(contact)
    }
  }

  resetForm() {
    this.newQuoteDataService.$resetForm.subscribe(() => {
      // ['notes', 'reference1', 'reference2', 'partner'].forEach(ctrl => this.formGroup.controls[ctrl]?.setValue(null))
      this.partnerform.reset()
      this.setAddress(null as any);
      this.onSelectedName(null as any);
      // this.partnerform.updateValueAndValidity()
      // this.cdr.detectChanges()
    })
  }


  ngOnChanges(changes: SimpleChanges) {
    if (!this.formGroup.contains(this.type)) {
      this.addFormControls();
      this.subscribeCtrls();
      this.subscribePartner();
    }
  }

  private async subscribeSecondPartner() {
    const s = this.formGroup.valueChanges.subscribe(() => {
      if (this.formGroup.contains(this.secondPartner) && !this.isSubscribePartner) {
        this.isSubscribePartner = true;
        this.setValidatorBySecondPrtner();
        s.unsubscribe()
      }
    })
  }

  private async initCards() {
    this.cardsList = await this.newQuoteDataService.getCardsTable();
  }

  addFormControls() {
    this.formGroup.addControl(this.type, this.partnerform);
  }

  private subscribePartner() {
    this.partnerform.controls.partner.valueChanges.subscribe((partner: CardList) => {
      if (partner != null) {
        this.partnerform.controls.contact.reset();
        // this.partnerform.controls.contact.setValidators(partner ? Validators.required : null)
        this.partnerform.controls.contact.updateValueAndValidity()

        if (!!partner.Notes)
          this.partnerform.controls.notes.setValue(partner?.Notes);

        this.initContacts(partner?.Id);
        this.setAddress(partner);
        this.onSelectedName(partner);
      }
      // this.cdr.detectChanges();
    })
  }

  private subscribeCtrls() {
    this.partnerform.controls.notes.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Note'] = newVal);
    this.partnerform.controls.reference1.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Reference1'] = newVal);
    // this.partnerform.controls.reference2.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Reference2'] = newVal);
    // this.formGroup.controls.direction.valueChanges.subscribe((val: DirectionList) =>
    // this.isHidden = !((val?.Name === 'Import' && this.type === 'consignee') || (val?.Name === 'Export' && this.type === 'shipper')));
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
      { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.Add' + this.capitalizeType), command: () => this.dialogsService.addCustomer(this.capitalizeType) },
      { label: TextCodeTranslator.Translate('QuoteOP.B.NewQuote.AddPotential' + this.capitalizeType), command: () => this.dialogsService.addPotentialCustomer(this.capitalizeType) },
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

  addContact() {
    this.dialogsService.addContact(this.partnerform.controls.partner.value.Id, this.partnerType)
  }

  toggle() {
    this.isHidden.next(!this.isHidden.value)
  }
}
