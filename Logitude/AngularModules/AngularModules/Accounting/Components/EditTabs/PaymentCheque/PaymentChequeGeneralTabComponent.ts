

import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {PaymentChequePM} from '../../../EntityPMs/PaymentChequePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ObservableCollection} from '../../../../Infrastructure/Utilities/ObservableCollection';

import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {GLAccountPM} from '../../../EntityPMs/GLAccountPM';
import {PaymentChequeLinePM} from '../../../EntityPMs/PaymentChequeLinePM';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {BankAccountPM} from '../../../EntityPMs/BankAccountPM';
import {BankAccountPMService} from '../../../Services/StandardPMs/BankAccountPMService';




@Component({
    selector: 'PaymentChequeGeneralTabComponent',
    moduleId: module.id,
    templateUrl: './PaymentChequeGeneralTabComponent.html',
})


export class PaymentChequeGeneralTabComponent extends BaseComponent implements  OnDestroy {
    ObjectTableName: string = "PaymentCheque";
    DataContext: any = this;
    entityPM: PaymentChequePM;
    filterAgrs: ApiQueryFilters;
    public Lines: ObservableCollection = new ObservableCollection([]);
    DisableFieldsEvent: any = null;
    AddLineEnabled: boolean = true;
    visible: boolean = false;
    AmountHeader: string;
    LayoutDirection: string;
    EntityResourceService: EntityResourceService = new EntityResourceService();
    BankAccountPMService: BankAccountPMService = new BankAccountPMService();
    private CurrentSession = SessionLocator.SelectedSession;
    PayToGLAccountIdOldValue: string;
    public isRTL: boolean = false;
    constructor(private entityArgs: EntityArgs)
    {
        super();

        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");

        this.entityPM = entityArgs.EntityPM;
        this.SetFilters();
        this.LayoutDirection=ObjectsLocator.GlobalSetting.LayoutDirection
        this.EntityResourceService.getEntityResourceByTableName("PaymentCheque").subscribe((response: any) => {
            this.EntityResourceService.getEntityResourceByTableName("PaymentChequeLine").subscribe((response: any) => {
                this.EntityResourceService.getEntityResourceByTableName("ChartOfAccount").subscribe((response: any) => {

                    this.visible = true;
                    this.AmountHeader = TextCodeTranslator.Translate("PaymentChequeLine.F.Amount") + " (" + this.entityPM.CurrencyCode + ")";
                });
            });
        });

                if (this.entityPM.BankLocalName) {
                    this.BankName = "LocalName";
                }
                else {
                    this.BankName = "EnglishName";
                }




        this.BuildPaymentChequeLinesList();
        if (this.entityPM.PaymentChequeStatusCode == "2" || this.entityPM.IsCancelled) {
            this.DisableFieldsMethod();
        }
        if (this.DisableFieldsEvent == null) {
            this.DisableFieldsEvent = this.CurrentSession.DisableFieldsEvent.subscribe((res) => {
                this.DisableFieldsMethod();
                        });
        }
    }
    SetFilters() {
        this.filterAgrs = new ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("AccountTypeCode", "4,5", null, null, "Exclude", false, false, false, "string");

    }
    DisableFieldsMethod() {
       this.UIProperties.SetEnabled("PayToGLAccountId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("PayToName", "PaymentCheque", false);
        this.UIProperties.SetEnabled("CurrencyId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("LocalAmount", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ValueDate", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ExchangeRate", "PaymentCheque", false);
        this.UIProperties.SetEnabled("BankAccountId", "PaymentCheque", false);
        this.UIProperties.SetEnabled("ForeignAmount", "PaymentCheque", false);

        this.AddLineEnabled = false;
  }
    get PayToGLAccountId() { return this.entityPM.PayToGLAccountId; }
    set PayToGLAccountId(value: string) {
        if (this.entityPM.PayToGLAccountId != value) {
            this.PayToGLAccountIdOldValue = this.entityPM.PayToGLAccountId;
            this.entityPM.PayToGLAccountId = value;
        }
    }

    get PayToName() { return this.entityPM.PayToName; }
    set PayToName(value: string) {
        if (this.entityPM.PayToName != value) {
            this.entityPM.PayToName = value;
        }
    }

    get CurrencyId() { return this.entityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.entityPM.CurrencyId != value) {
            this.entityPM.CurrencyId = value;

        }
    }

    get ForeignAmount() { return this.entityPM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.entityPM.ForeignAmount != value) {
            this.entityPM.ForeignAmount = value;
        }
    }
    get LocalAmount() { return this.entityPM.LocalAmount; }
    set LocalAmount(value: number) {
        if (this.entityPM.LocalAmount != value) {
            this.entityPM.LocalAmount = value;
        }
    }

    get ValueDate() { return this.entityPM.ValueDate; }
    set ValueDate(value: Date) {
        if (this.entityPM.ValueDate != value) {
            this.entityPM.ValueDate = value;
        }
    }
    get ChequeNumber() { return this.entityPM.ChequeNumber; }
    set ChequeNumber(value: string) {
        if (this.entityPM.ChequeNumber != value) {
            this.entityPM.ChequeNumber = value;
        }
    }
    get ExchangeRate() { return this.entityPM.ExchangeRate; }
    set ExchangeRate(value: number) {
        if (this.entityPM.ExchangeRate != value) {
            this.entityPM.ExchangeRate = value;
        }
    }
    get BankAccountId() { return this.entityPM.BankAccountId; }
    set BankAccountId(value: string) {
        if (this.entityPM.BankAccountId != value) {
            this.entityPM.BankAccountId = value;
        }
    }

    public bankName: string;
    get BankName() { return this.bankName; }
    set BankName(value: string) {
        if (this.bankName != value) {
            this.bankName = value;
        }
    }


    private bankAccount: BankAccountPM;
    get BankAccount() { return this.bankAccount; }
    set BankAccount(value: BankAccountPM) {
        if (this.bankAccount != value) {
            this.bankAccount = value;
            if (value) {
                if (value.LocalName) {
                    this.BankName = "LocalName";
                }
                else {
                    this.BankName = "EnglishName";
                }
            }
        }
    }

    private account: GLAccountPM;
    get Account() { return this.account; }
    set Account(value: GLAccountPM) {
        if ((!AppTool.IsNullOrEmpty(this.PayToGLAccountIdOldValue)) && this.PayToGLAccountIdOldValue != value.Id) {
        if (this.account != value) {
            this.account = value;
            if (value != null) {

                if (value.AccountTypeCode == "3" && this.entityPM.APPaymentId == null) {

                    this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, false, TextCodeTranslator.Translate("Accounting.General.O.VendorsGLAccount"));
                }
                else {
                    this.UIProperties.SetValidity("PayToGLAccountId", this.ObjectTableName, true, null);


                        this.PayToName = value.LocalName;

                }
            }
            }
        }
    }

    Add() {
        if (this.AddLineEnabled) {
            var line: number = 0;
            var sequence: number = 0;
            //if (this.entityPM.PaymentChequeLines.length > 0) {
            //    if (isNaN(this.entityPM.PaymentChequeLineLastLine)) this.entityPM.PaymentChequeLineLastLine = 0;
            //    line = this.entityPM.PaymentChequeLineLastLine;
            //    this.entityPM.PaymentChequeLineLastLine = this.entityPM.PaymentChequeLineLastLine + 1;
            //}
            //else {
            //    this.entityPM.PaymentChequeLineLastLine = 1;
            //    line = 0;
            //}
            var items = this.entityPM.PaymentChequeLines.sort((a, b) => { return (a.SequenceNumeric === b.SequenceNumeric) ? 0 : (a.SequenceNumeric < b.SequenceNumeric) ? -1 : 1 });
            if (items.length == 0) sequence = 0;
            else {
                sequence = items[this.entityPM.PaymentChequeLines.length - 1].SequenceNumeric;
            }
            line += 1;
            sequence += 1;
            var item: PaymentChequeLinePM = new PaymentChequeLinePM(this.entityPM);
            item.PaymentChequeId = this.entityPM.Id;
            item.Tenant = this.entityPM.Tenant;
            //item.Line = line;
            item.SequenceNumeric= sequence
            item.ChangeSetOp = "Insert";

            this.entityPM.AddPaymentChequeLine(item);
            this.BuildPaymentChequeLinesList();
        }

    }
    BuildPaymentChequeLinesList() {

        this.Lines.Clear();
        for (let item of this.entityPM.PaymentChequeLines) {
            this.Lines.Insert(new PaymentChequeLine(item, this));
        }

    }



    ngOnDestroy() {
        if (this.DisableFieldsEvent) {
            this.DisableFieldsEvent.unsubscribe();
            this.DisableFieldsEvent = null;
        }
    }
}
export class PaymentChequeLine extends BaseComponent {
    entity: PaymentChequeLinePM;
    parent: PaymentChequeGeneralTabComponent;
    constructor(Entity: PaymentChequeLinePM, Parent: PaymentChequeGeneralTabComponent ) {
        super();
        this.entity = Entity;
        this.parent = Parent;
    }

    AmountHeader: string;
    get SequenceNumeric() { return this.entity.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        if (this.entity.SequenceNumeric != value) {
            this.entity.SequenceNumeric = value;
        }
    }
    get Line() { return this.entity.Line; }
    set Line(value: number) {
        if (this.entity.Line != value) {
            this.entity.Line = value;
        }
    }
    get Amount() { return this.entity.Amount; }
    set Amount(value: number) {
        if (this.entity.Amount != value) {
            this.entity.Amount = value;
        }
    }
    get Note() { return this.entity.Notes; }
    set Note(value: string) {
        if (this.entity.Notes != value) {
            this.entity.Notes = value;
        }
    }


    DeleteButtonClicked() {
        var sequence = 1;
        this.parent.Lines.Remove(this);
        this.parent.entityPM.RemovePaymentChequeLine(this.entity);
        this.parent.Lines.Collection.forEach((item: PaymentChequeLine) => {
            item.SequenceNumeric = sequence;
            sequence++;
        });
    }







}
