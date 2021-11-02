import { Component, OnInit } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARPaymentPM } from '../../../../Invoice/EntityPMs/ARPaymentPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AccountingPeriodListService } from '../../../../Accounting/Services/StandardLists/AccountingPeriodListService';
import { AccountingPeriodList } from '../../../../Accounting/EntityLists/AccountingPeriodList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { BankAccountPM } from 'Accounting/EntityPMs/BankAccountPM';
import { ARPaymentBankTranferPM } from 'Invoice/EntityPMs/ARPaymentBankTranferPM';

declare var window: any;

@Component({
    templateUrl: './ARPaymentMultiBankTransfersComponent.html',
})
export class ARPaymentMultiBankTransfersComponent extends BaseComponent {
    public ObjectTableName: string = 'ARPayment';
    public DataContext = this;
    public paymentPM: ARPaymentPM;
    public ItemsSource: ObservableCollection;
    public ValidationErrorsList: string[] = [];
    public OriginalItemPM: ARPaymentPM;
    public ClonedItemPM: ARPaymentPM;
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    IsHeaderVisible: boolean = false;
    IsFromCustomsAnswers: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public BankTransfersCounter: number;
    public TotalAmount: number;
    public isLTR: boolean;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate(
            'General.M.FieldIsRequired'
        );
        this.isLTR = ObjectsLocator.GlobalSetting.LayoutDirection == 'ltr';
        this.CalculateTotal();
    }

    SetWindowArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.paymentPM = args.EntityPM;
            this.OriginalItemPM = args.EntityPM;
            this.ClonedItemPM = this.CloneEntity(args.EntityPM);
            this.FillItemSource();
            this.AddFirstBankTransferRecord();
            this.CalculateTotal();
            this.UpdateBankTransferCounter();
            this.IsDisplayOnly =
                this.paymentPM.StatusCode == 'AD' ||
                this.paymentPM.StatusCode == 'VD' || 
                this.paymentPM.StatusCode == 'BT'
                    ? true
                    : false;
        }
    }
    UpdateBankTransferCounter() {
        this.BankTransfersCounter = this.paymentPM.ARPaymentBankTranfers.length;
    }
    FillItemSource() {
        this.ItemsSource.Clear();
        for (let item of this.paymentPM.ARPaymentBankTranfers) {
            this.ItemsSource.Insert(new PaymentBankTransferLine(item, this));
        }
    }
    AddFirstBankTransferRecord() {
        if (this.paymentPM.ARPaymentBankTranfers.length == 0) {
            var bankTransfer: ARPaymentBankTranferPM = new ARPaymentBankTranferPM(this.paymentPM);
            (bankTransfer.PaymentId = this.paymentPM.Id),
                (bankTransfer.Tenant = this.paymentPM.Tenant);
            bankTransfer.LineNumber = 1;
            bankTransfer.BankAccountId = this.paymentPM.BankAccountId;
            bankTransfer.BankAccount = this.paymentPM.BankAccount;
            bankTransfer.ValueDate = this.paymentPM.ValueDate;
            bankTransfer.PaymentRef = this.paymentPM.ChequeOrPaymentRef;
            bankTransfer.ForeignAmount = this.paymentPM.AmountInPaymentCurrency;
            bankTransfer.PaymentId = '-';
            this.UpdatePaymentBankTransferList(bankTransfer);
        }
    }

    UpdatePaymentBankTransferList(bankTranfer: ARPaymentBankTranferPM) {
        if (!this.paymentPM.ARPaymentBankTranfers.includes(bankTranfer)) {
            this.paymentPM.AddARPaymentBankTranferPM(bankTranfer);
            this.ItemsSource.Insert(
                new PaymentBankTransferLine(bankTranfer, this)
            );
        }
    }
    AddNewBankTransfer() {
        if (!this.IsDisplayOnly) {
            if (this.CheckRequiredFileds()) {
                var latestLineNumber: number = 0;
                latestLineNumber = this.GetLatestBankTransferLineNumber();
                latestLineNumber += 1;
                var bankTransfer: ARPaymentBankTranferPM =
                    new ARPaymentBankTranferPM(this.paymentPM);
                (bankTransfer.PaymentId = this.paymentPM.Id),
                    (bankTransfer.Tenant = this.paymentPM.Tenant);
                bankTransfer.LineNumber = latestLineNumber;
                bankTransfer.PaymentId = '-';
                this.UpdatePaymentBankTransferList(bankTransfer);
                this.CalculateTotal();
                this.BankTransfersCounter = latestLineNumber;
            }
        }
    }
    GetLatestBankTransferLineNumber() {
        var counter: number = 0;
        if (this.paymentPM.ARPaymentBankTranfers.length > 0) {
            var items = this.paymentPM.ARPaymentBankTranfers.sort((a, b) => {
                return a.LineNumber === b.LineNumber
                    ? 0
                    : a.LineNumber < b.LineNumber
                    ? -1
                    : 1;
            });
            if (items.length == 0) counter = 0;
            else {
                counter =
                    items[this.paymentPM.ARPaymentBankTranfers.length - 1]
                        .LineNumber;
            }
        }
        return counter;
    }

    CalculateTotal() {
        this.TotalAmount = 0;
        for (let bankTransfer of this.ItemsSource.Collection) {
            if (!AppTool.IsNullOrEmpty(bankTransfer.ForeignAmount))
                this.TotalAmount += bankTransfer.ForeignAmount;
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    CloneEntity(entityToClone: ARPaymentPM) {
        var clonedEntity: ARPaymentPM = new ARPaymentPM();
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        clonedEntity.ARPaymentBankTranfers = [];
        entityToClone.ARPaymentBankTranfers.forEach((itemMod) => {
            var clonedItemMod = new ARPaymentBankTranferPM(this.paymentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.ARPaymentBankTranfers.push(clonedItemMod);
        });

        return clonedEntity;
    }
    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    }

    MapEntitytoEntity(
        srcEntity: any,
        targetEntity: any,
        takeKeysFromTarget: boolean = false
    ) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }
    CheckRequiredFileds() {
        this.ValidationErrorsList = [];
        for (let bankTransfer of this.paymentPM.ARPaymentBankTranfers) {
            this.ValidateBankTransferFields(bankTransfer);
        }
        if (this.ValidationErrorsList.length == 0) {
            return true;
        }
    }

    onSearchTextChangeEvent($event) {
        if(!AppTool.IsNullOrEmpty($event)){
            this.ItemsSource.Collection = this.ItemsSource.Collection.filter(x => x.PaymentRef == $event);
        }
    }

    OkButtonClicked() {
        if (this.CheckRequiredFileds()) {
            this.CurrentSession.CloseCurrentWindowEmit('ok');
        }
    }
    private ValidateBankTransferFields(bankTransfer: ARPaymentBankTranferPM) {
        if (AppTool.IsNullOrEmpty(bankTransfer.BankAccountId)) {
            this.ValidationErrorsList.push(
                this.FIELD_IS_REQUIERD.replace(
                    '%FieldName',
                    TextCodeTranslator.Translate('ARPayment.F.BankAccountLiteId')
                )
            );
        }
        if (AppTool.IsNullOrEmpty(bankTransfer.ValueDate)) {
            this.ValidationErrorsList.push(
                this.FIELD_IS_REQUIERD.replace(
                    '%FieldName',
                    TextCodeTranslator.Translate('ARPayment.F.ValueDate')
                )
            );
        }
        if (AppTool.IsNullOrEmpty(bankTransfer.ForeignAmount)) {
            this.ValidationErrorsList.push(
                this.FIELD_IS_REQUIERD.replace(
                    '%FieldName',
                    TextCodeTranslator.Translate(
                        'ARPayment.F.AmountInPaymentCurrency'
                    )
                )
            );
        }

        // this.ValidateDuplicateBankTransferNumbers(bankTransfer.PaymentRef);
    }

    // ValidateDuplicateBankTransferNumbers(bankTransferRef: string) {
    //     var isDuplicateChequeNumber =
    //         this.paymentPM.ARPaymentBankTranfers?.filter(
    //             (c) => c.PaymentRef == bankTransferRef
    //         ).length > 1;
    //     if (isDuplicateChequeNumber) {
    //         this.ValidationErrorsList.push(
    //             TextCodeTranslator.Translate(
    //                 'Accounting.M.MoreThanChequeWithTheSameChequeNumber'
    //             )
    //         );
    //     }
    // }
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    OnRowEnded($event) {
        console.log('this.ItemsSource.Length : ' + this.ItemsSource.Length);
        if ($event == this.ItemsSource.Length) {
            this.AddNewBankTransfer();
        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.AddNewBankTransfer();
        }
    }
}

export class PaymentBankTransferLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = 'ARPaymentBankTranfer';
    public entityPM: ARPaymentBankTranferPM;
    parent: ARPaymentMultiBankTransfersComponent;
    constructor(
        EntityPM: ARPaymentBankTranferPM,
        Parent: ARPaymentMultiBankTransfersComponent
    ) {
        super();
        this.entityPM = EntityPM;
        this.parent = Parent;
    }

    get LineNumber() {
        return this.entityPM.LineNumber;
    }
    set LineNumber(value: number) {
        if (this.entityPM.LineNumber != value) {
            this.entityPM.LineNumber = value;
        }
    }

    get ForeignAmount() {
        return this.entityPM.ForeignAmount;
    }
    set ForeignAmount(value: number) {
        if (this.entityPM.ForeignAmount != value) {
            this.entityPM.ForeignAmount = value;
            this.parent.CalculateTotal();
        }
    }

    get ValueDate() {
        return this.entityPM.ValueDate;
    }
    set ValueDate(value: Date) {
        if (this.entityPM.ValueDate != value) {
            this.entityPM.ValueDate = value;
        }
    }

    get PaymentRef() {
        return this.entityPM.PaymentRef;
    }
    set PaymentRef(value: string) {
        if (this.entityPM.PaymentRef != value) {
            this.entityPM.PaymentRef = value;
        }
    }

    get BankAccountId() {
        return this.entityPM.BankAccountId;
    }
    set BankAccountId(value: string) {
        if (this.entityPM.BankAccountId != value) {
            this.entityPM.BankAccountId = value;
        }
    }
    
    get BankAccount(){
        return  this.entityPM.BankAccount;
    }

    set BankAccount(value: BankAccountPM) {
        if (this.entityPM.BankAccount != value) {
            this.entityPM.BankAccount = value;
        }
    }

    DeleteButtonClicked() {
        this.parent.ItemsSource.Remove(this);
        if (
            this.parent.paymentPM.ARPaymentBankTranfers.includes(this.entityPM)
        ) {
            this.parent.paymentPM.RemoveARPaymentBankTranferPM(this.entityPM);
        }
        this.ResetLineNumber();
        this.parent.CalculateTotal();
        --this.parent.BankTransfersCounter;
    }
    ResetLineNumber() {
        var sequence: number = 1;
        this.parent.ItemsSource.Collection.forEach(
            (item: PaymentBankTransferLine) => {
                item.LineNumber = sequence;
                sequence++;
            }
        );
    }
}
