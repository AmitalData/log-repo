import { Component, OnInit } from '@angular/core';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ARPaymentPM } from '../../../../Invoice/EntityPMs/ARPaymentPM';
import { ARPaymentChequeReplicaPM } from '../../../../Invoice/EntityPMs/ARPaymentChequeReplicaPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AccountingPeriodListService } from '../../../../Accounting/Services/StandardLists/AccountingPeriodListService';
import { AccountingPeriodList } from '../../../../Accounting/EntityLists/AccountingPeriodList';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';

declare var window: any;

@Component({

    templateUrl: './ARPaymentMultiChequesComponent.html',
})

export class ARPaymentMultiChequesComponent extends BaseComponent {
    public ObjectTableName: string = "ARPayment";
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
    public ChequesCounter: number;
    public TotalAmount: number;

    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
       
    }
   
    SetWindowArgs(args: any) {
    
        if (!AppTool.IsNullOrEmpty(args)) {
            this.paymentPM = args.EntityPM;
            this.OriginalItemPM = args.EntityPM;
            this.ClonedItemPM = this.CloneEntity(args.EntityPM);
            this.FillItemSource();
            this.AddFirstChequeRecord();
            this.CalculateTotal();
            this.UpdateChequeCounter();

        }
    }
    UpdateChequeCounter() {
        this.ChequesCounter = this.paymentPM.ARPaymentChequeReplicas.length;
    }
    FillItemSource() {
        this.ItemsSource.Clear();
        for (let item of this.paymentPM.ARPaymentChequeReplicas) {
            this.ItemsSource.Insert(new PaymentChequeLine(item, this));
        }
      

    }
    AddFirstChequeRecord() {
        if (this.paymentPM.ARPaymentChequeReplicas.length == 0) {
            var cheque: ARPaymentChequeReplicaPM = new ARPaymentChequeReplicaPM(this.paymentPM);
            cheque.PaymentId = this.paymentPM.Id,
            cheque.Tenant = this.paymentPM.Tenant;
            cheque.LineNumber = 1;
            cheque.BankId = this.paymentPM.Bank;
            cheque.BankBranch = this.paymentPM.BankBranch;
            cheque.BankAccount = this.paymentPM.Account;
            cheque.ValueDate = this.paymentPM.ValueDate;
            cheque.ChequeNumber = this.paymentPM.ChequeOrPaymentRef;
            cheque.ForeignAmount = this.paymentPM.AmountInPaymentCurrency;
            this.UpdatePaymentChequeList(cheque);          
        }
    }
  
    UpdatePaymentChequeList(cheque: ARPaymentChequeReplicaPM) {
        if (!this.paymentPM.ARPaymentChequeReplicas.includes(cheque)) {
            this.paymentPM.AddARPaymentChequeReplicaPM(cheque);
            this.ItemsSource.Insert(new PaymentChequeLine(cheque, this));
        }
    }
    AddNewCheque() {
        if (this.CheckRequiredFileds()) {
            var latestLineNumber: number = 0;
            latestLineNumber = this.GetLatestChequeLineNumber()
            latestLineNumber += 1;
            var cheque: ARPaymentChequeReplicaPM = new ARPaymentChequeReplicaPM(this.paymentPM);
            cheque.PaymentId = this.paymentPM.Id,
                cheque.Tenant = this.paymentPM.Tenant;
            cheque.LineNumber = latestLineNumber;
            this.UpdatePaymentChequeList(cheque);
            this.CalculateTotal();
            this.ChequesCounter = latestLineNumber;
        }
    }
    GetLatestChequeLineNumber() {
       var counter: number = 0;
    if (this.paymentPM.ARPaymentChequeReplicas.length > 0) {

        var items = this.paymentPM.ARPaymentChequeReplicas.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
        if (items.length == 0) counter = 0;
        else {
            counter = items[this.paymentPM.ARPaymentChequeReplicas.length - 1].LineNumber;
        }
       
        return counter;
    }
}
    CalculateTotal() {
        this.TotalAmount = 0;
        for (let cheque of this.ItemsSource.Collection) {
            if (!AppTool.IsNullOrEmpty(cheque.ForeignAmount))
            this.TotalAmount += cheque.ForeignAmount;
        }
         
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    CloneEntity(entityToClone: ARPaymentPM) {
       var clonedEntity: ARPaymentPM = new ARPaymentPM();
        this.MapEntitytoEntity(entityToClone, clonedEntity);
        clonedEntity.ARPaymentChequeReplicas = [];
        entityToClone.ARPaymentChequeReplicas.forEach((itemMod) => {
            var clonedItemMod = new ARPaymentChequeReplicaPM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.ARPaymentChequeReplicas.push(clonedItemMod);
        });

        return clonedEntity;
    }
    RejectChanges() {
        this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    }

    MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
        var keys;
        keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
        for (var key in keys) {
            var property = keys[key];
            targetEntity[property] = srcEntity[property];
        }
    }
    CheckRequiredFileds() {
        this.ValidationErrorsList = [];
        for (let cheque of this.paymentPM.ARPaymentChequeReplicas) {
            this.ValidateChequeFields(cheque);
        }
        if (this.ValidationErrorsList.length == 0) {
            return true;
        } 
        
    }
    OkButtonClicked() {       
        if (this.CheckRequiredFileds()) {      
            this.CurrentSession.CloseCurrentWindowEmit('ok');
        } 
    }
    private ValidateChequeFields(cheque: ARPaymentChequeReplicaPM) {
        if (AppTool.IsNullOrEmpty(cheque.ChequeNumber)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef")));
        }
        if (AppTool.IsNullOrEmpty(cheque.BankId)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.Bank")));
        }
        if (AppTool.IsNullOrEmpty(cheque.BankAccount)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.Account")));
        }
        if (AppTool.IsNullOrEmpty(cheque.BankBranch)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.BankBranch")));
        }
        if (AppTool.IsNullOrEmpty(cheque.ValueDate)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPayment.F.ValueDate")));
        }
        if (AppTool.IsNullOrEmpty(cheque.ForeignAmount)) {
            this.ValidationErrorsList.push(this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("ARPaymentCheque.F.ForeignAmount")));
        }
    }
    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    OnRowEnded($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.AddNewCheque();

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.AddNewCheque();
        }
    }

  

}

export class PaymentChequeLine extends BaseComponent {
    public DataContext = this;
    public ObjectTableName: string = "ARPaymentChequeReplica";
    public entityPM: ARPaymentChequeReplicaPM;
    parent: ARPaymentMultiChequesComponent;
    constructor(EntityPM: ARPaymentChequeReplicaPM, Parent: ARPaymentMultiChequesComponent) {
        super();
        this.entityPM = EntityPM;
        this.parent = Parent;

    }

  
    get LineNumber() { return this.entityPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.entityPM.LineNumber != value) {
            this.entityPM.LineNumber = value;

        }
    }

    get ForeignAmount() { return this.entityPM.ForeignAmount; }
    set ForeignAmount(value: number) {
        if (this.entityPM.ForeignAmount != value) {
            this.entityPM.ForeignAmount = value;
            this.parent.CalculateTotal();
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

    get BankAccount() { return this.entityPM.BankAccount; }
    set BankAccount(value: string) {
        if (this.entityPM.BankAccount != value) {
            this.entityPM.BankAccount = value;

        }
    }

    get BankBranch() { return this.entityPM.BankBranch; }
    set BankBranch(value: string) {
        if (this.entityPM.BankBranch != value) {
            this.entityPM.BankBranch = value;

        }
    }

    get BankNumber() { return this.entityPM.BankNumber; }
    set BankNumber(value: string) {
        if (this.entityPM.BankNumber != value) {
            this.entityPM.BankNumber = value;

        }
    }

    get BankId() { return this.entityPM.BankId; }
    set BankId(value: string) {
        if (this.entityPM.BankId != value) {
            this.entityPM.BankId = value;

        }
    }


    DeleteButtonClicked() {
    
        this.parent.ItemsSource.Remove(this);
        if (this.parent.paymentPM.ARPaymentChequeReplicas.includes(this.entityPM)) {
            this.parent.paymentPM.RemoveARPaymentChequeReplicaPM(this.entityPM);
        }
        this.parent.CalculateTotal();
    }


}
