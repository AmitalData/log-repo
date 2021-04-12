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
   
    FIELD_IS_REQUIERD: string;
    IsDisplayOnly: boolean;
    IsHeaderVisible: boolean = false;
    IsFromCustomsAnswers: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.ItemsSource = new ObservableCollection([]);
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
     
    }
   
    SetWindowArgs(args: any) {
    
        if (!AppTool.IsNullOrEmpty(args)) {
            this.paymentPM = args.EntityPM;
        }
    }


  

    BuildChequesList() {
        this.ItemsSource.Clear();
    }

    Add() {

        var counter: number = 0;


        if (this.paymentPM.ARPaymentChequeReplicas.length > 0) {

            var items = this.paymentPM.ARPaymentChequeReplicas.sort((a, b) => { return (a.LineNumber === b.LineNumber) ? 0 : (a.LineNumber < b.LineNumber) ? -1 : 1 });
            if (items.length == 0) counter = 0;
            else {
                counter = items[this.paymentPM.ARPaymentChequeReplicas.length - 1].LineNumber;
            }


        }

        counter += 1;
        var cheque: ARPaymentChequeReplicaPM = new ARPaymentChequeReplicaPM(this.paymentPM);

        cheque.PaymentId = this.paymentPM.Id,
            cheque.Tenant = this.paymentPM.Tenant;
        cheque.LineNumber = counter;
        if (!this.paymentPM.ARPaymentChequeReplicas.includes(cheque)) {
            this.paymentPM.AddARPaymentChequeReplicaPM(cheque);
            this.ItemsSource.Insert(new PaymentChequeLine(cheque, this));

        }
    }



    //RejectChanges() {
    //    this.MapEntitytoEntity(this.ClonedItemPM, this.OriginalItemPM, true);
    //}

    //MapEntitytoEntity(srcEntity: any, targetEntity: any, takeKeysFromTarget: boolean = false) {
    //    var keys;
    //    keys = Object.keys(takeKeysFromTarget ? targetEntity : srcEntity);
    //    for (var key in keys) {
    //        var property = keys[key];
    //        targetEntity[property] = srcEntity[property];
    //    }
    //}
    CancelButtonClicked() {


        //if (this.paymentPM.IsDirty ) {
        //    var confirm = new ConfirmWindow();

        //    confirm.YesButtonText = TextCodeTranslator.Translate("General.B.Yes");

        //    confirm.ShowNoButton = true;
        //    confirm.Show(TextCodeTranslator.Translate("Customs.Declaration.O.Cancel"));
        //    confirm.WindowClosed.subscribe((event: any) => {
        //        if (confirm.Yes) {
        //            confirm.Close();
        //            this.OkButtonClicked();



        //        }
        //        else {
                   
        //            this.CurrentSession.CloseCurrentWindow();
        //        }

        //    });

        //}
        //else {
            this.CurrentSession.CloseCurrentWindowEmit('cancel');
        //}



    }

    isValid: boolean;
    inValid: boolean;
    hasRequest: boolean;
    notMandatoryIsNotEmpty: boolean = false;
    OkButtonClicked() {
        this.ValidationErrorsList = [];
        var errors: string[] = [];
        this.isValid = true;
        this.inValid = false;
        this.CurrentSession.CloseCurrentWindowEmit('ok');
        for (let item of this.paymentPM.ARPaymentChequeReplicas) {
        

        }


  
    }

    public SelectedRow: any = null;
    OnRowSelected(itemComponent: any) {
        this.SelectedRow = itemComponent;
    }

    OnRowEnded($event) {
        console.log("this.ItemsSource.Length : " + this.ItemsSource.Length);
        if (($event) == this.ItemsSource.Length) {
            this.Add();

        }
    }

    OnFocus() {
        if (this.ItemsSource.Length == 0) {
            this.Add();
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

    //#region properties


    get LineNumber() { return this.entityPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.entityPM.LineNumber != value) {
            this.entityPM.LineNumber = value;

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


    DeleteButtonClicked() {

        this.parent.ItemsSource.Remove(this);
        if (this.parent.paymentPM.ARPaymentChequeReplicas.includes(this.entityPM)) {
            this.parent.paymentPM.RemoveARPaymentChequeReplicaPM(this.entityPM);
        }

    }


}
