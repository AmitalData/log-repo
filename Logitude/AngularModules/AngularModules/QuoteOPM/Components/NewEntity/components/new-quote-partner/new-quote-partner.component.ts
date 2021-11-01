import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AddressList } from 'Common/EntityLists/AddressList';
import { CardList } from 'Common/EntityLists/CardList';
import { ContactList } from 'Common/EntityLists/ContactList';
import { CardPM } from 'Common/EntityPMs/CardPM';
import { ContactListService } from 'Common/Services/StandardLists/ContactListService';
import { CardPMService } from 'Common/Services/StandardPMs/CardPMService';
import { ContactPMService } from 'Common/Services/StandardPMs/ContactPMService';
import { any } from 'cypress/types/bluebird';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';
import { DirectionList } from 'Infrastructure/EntityLists/DirectionList';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { MenuItem } from 'primeng/api';
import { QuoteOPPM } from 'QuoteOPM/EntityPMs/QuoteOPPM';
import { filter } from 'rxjs/operators';
import { DialogsService, PartnerType } from '../../Services/dialogs/dialogs.service';
import { NewQuoteDataService } from '../../Services/new-quote-data/new-quote-data.service';

@Component({
  selector: 'app-new-quote-partner',
  templateUrl: './new-quote-partner.component.html',
  styleUrls: ['./new-quote-partner.component.scss']
})
export class NewQuotePartnerComponent extends BaseComponent implements OnInit, AfterViewInit {
  @Input() formGroup: FormGroup = null as any;
  @Input() EntityPM: QuoteOPPM = null as any;
  @Input() type: 'shipper' | 'consignee';

  cardsList: CardList[] = []
  contactsList: ContactList[] = []
  Address: AddressList = null as any;
  ddl: MenuItem[] = [];
  ShipperContact: any;
  ConsigneeContact: any;
  ShipperId: any;
  ConsigneeId: any;
  isHidden: boolean = true;

  partnerform: FormGroup = new FormGroup({
    partner: new FormControl(),
    contact: new FormControl(),
    notes: new FormControl(),
    reference1: new FormControl(),
    reference2: new FormControl(),
  })

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
    private CD: ChangeDetectorRef,
  ) {
    super();
  }

  ngAfterViewInit(): void {
    if (this.EntityPM != null && this.EntityPM.Id != null) {
      this.setPartners();
    }
  }

  setPartners() {
    const cardService = new CardPMService();
    const contactService = new ContactListService();
    if (this.EntityPM[this.capitalizeType + 'Id'] != null) {
      cardService.get(this.EntityPM[this.capitalizeType + 'Id']).subscribe((cardResponse: any) => {
        if (cardResponse.Result != null) {
          this.partnerform.controls.partner.setValue(cardResponse.Result);
        }
        if (this.EntityPM[this.capitalizeType + 'ContactId'] != null) {
          contactService.getSingle(this.EntityPM[this.capitalizeType + 'ContactId']).subscribe((ContactResponse: any) => {
            if (ContactResponse.Result != null) {
              this.partnerform.controls.contact.setValue(ContactResponse.Result);
            }
          });
        }
      });
    }
    if (this.EntityPM[this.capitalizeType + 'Reference1'] != null) {
      this.partnerform.controls.reference1.setValue(this.EntityPM[this.capitalizeType + 'Reference1']);
    }
    if (this.EntityPM[this.capitalizeType + 'Reference2'] != null) {
      this.partnerform.controls.reference2.setValue(this.EntityPM[this.capitalizeType + 'Reference2']);
    }

    if (this.EntityPM[this.capitalizeType + 'Note'] != null) {
      this.partnerform.controls.ConsigneeNote.setValue(this.EntityPM[this.capitalizeType + 'Note']);
    }
  }

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
      if (partner != null) {
        this.partnerform.controls.contact.reset();
        this.partnerform.controls.contact.setValidators(partner ? Validators.required : null)
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
    this.partnerform.controls.reference2.valueChanges.subscribe(newVal => this.EntityPM[this.capitalizeType + 'Reference2'] = newVal);
    this.formGroup.controls.direction.valueChanges.subscribe((val: DirectionList) =>
      this.isHidden = !((val.Name === 'Import' && this.type === 'consignee') || (val.Name === 'Export' && this.type === 'shipper')));
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
}
