declare var window: any;
import {Component, AfterViewInit, ChangeDetectorRef, Output, Input, EventEmitter}  from '@angular/core';
import {EntityArgs} from '../../../../../Infrastructure/DataContracts/EntityArgs';
import {AppTool, ArrayTool} from '../../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import {BaseComponent} from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ObservableCollection} from '../../../../../Infrastructure/Utilities/ObservableCollection';
import {ConfirmWindow} from '../../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../../Controls/Windows/MessageWindow';
import {ServiceResponse} from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../../Controls/Windows/LogitudeWindow';
import { DeclarationDisplayOnlyChecks, DisplayOnlyCheckResult } from '../../../../../Customs/Utilities/DeclarationDisplayOnlyChecks';
import {ServiceHelper} from '../../../../../Infrastructure/Utilities/ServiceHelper';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import {ApiQueryFilters} from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationValidator } from '../../../../../Customs/Validators/DeclarationValidator';
import {CustomsRequiredFieldListService} from '../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';

import {DeclarationPM} from '../../../../../Customs/EntityPMs/DeclarationPM';
import {SupplierInvoicePM} from '../../../../../Customs/EntityPMs/SupplierInvoicePM';
import { DeclarationEventManager } from '../../../../../Customs/Utilities/DeclarationEventManager';
import {CustomsExchangeRateExtendedPMService} from '../../../../../Customs/Services/ExtendedPMs/CustomsExchangeRateExtendedPMService'

import {DeclarationPMService} from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';

import { SupplierInvoiceItemPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { SupplierInvoiceModificationPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceModificationPM';
import {AddEditSupplierInvoiceComponent} from './AddEditSupplierInvoiceComponent';
import { SupplierInvoiceUCRPM } from '../../../../../Customs/EntityPMs/SupplierInvoiceUCRPM';
import { SupplierInvoicePaymentPM } from '../../../../../Customs/EntityPMs/SupplierInvoicePaymentPM';


@Component({
    
    templateUrl: './SupplierInvoiceMoreTabComponent.html',
})

export class SupplierInvoiceMoreTabComponent extends BaseComponent {
    @Output() FillValidationErrorList: EventEmitter<any> = new EventEmitter();
    public DataContext: any = this;
    public InvoicePM: SupplierInvoicePM;
    declarationPM: DeclarationPM;
    public ObjectTableName: string = "Customs.SupplierInvoice";
    public IsDisplayOnly: boolean = false;
    public Parent: AddEditSupplierInvoiceComponent;
    public TypeCodeFilterItems: ApiQueryFilters;



    //Services
    private declarationPMService: DeclarationPMService = new DeclarationPMService;

    constructor() {
        super();
        this.ModificationsList = new ObservableCollection([]);
       
    }

    SetTabArgs(args: any) {
        if (!AppTool.IsNullOrEmpty(args)) {
            this.InvoicePM = args.InvoicePM;
            this.declarationPM = args.DeclarationPM; 
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.Parent = args.Parent;

            this.Parent.ReloadModificationEvent.subscribe(() => {
                this.FillGridData();
            });

            this.FillGridData();
        }
    }

    FillGridData() {
        this.TypeCodeFilterItems = new ApiQueryFilters();
        this.TypeCodeFilterItems.addAdditionalFilter("Code", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        if(this.declarationPM.Direction == "E"){
            this.TypeCodeFilterItems.addAdditionalFilter("IsRelevantInvoiceExport", true, null, null, "Equals", false, false, false, "boolean");
        }else{
            this.TypeCodeFilterItems.addAdditionalFilter("IsRelevantInvoice", true, null, null, "Equals", false, false, false, "boolean");
        }
        this.ModificationsList = new ObservableCollection([]);
        for (let item of this.InvoicePM.SupplierInvoiceModifications) {
            if (item.TypeCode != "I02" && item.TypeCode != "67" && item.TypeCode != "144") {
                if (this.declarationPM.Direction == "E" && item.TypeCode != "160") {
                    this.ModificationsList.Insert(new ModificationItemModel(item, this));
                }
                else {
                    this.ModificationsList.Insert(new ModificationItemModel(item, this));
                }
            }
        }

        if (this.declarationPM.Direction == "E") {
            this.TypeCodeFilterItems = new ApiQueryFilters();
            this.TypeCodeFilterItems.addAdditionalFilter("Code", "160", null, null, "NotContains", false, false, false, "string", false, true);
            this.UCRList = new ObservableCollection([]);
            for (let item of this.InvoicePM.SupplierInvoiceUCRs) {
                     this.UCRList.Insert(new UCRItemModel(item));
            }


            this.PaymentsList = new ObservableCollection([]);
            for (let item of this.InvoicePM.SupplierInvoicePayments) {
                this.PaymentsList.Insert(new PaymentItemModel(item));
            }



        }

    }

    //#region Properties

    get ActualPayedCurrencyTypeCode() { return this.InvoicePM.ActualPayedCurrencyTypeCode; }
    set ActualPayedCurrencyTypeCode(value: string) {
        if (this.InvoicePM.ActualPayedCurrencyTypeCode != value) {
            this.InvoicePM.ActualPayedCurrencyTypeCode = value;

        }
    }

    get ActualPayedAmount() { return this.InvoicePM.ActualPayedAmount; }
    set ActualPayedAmount(value: number) {
        if (this.InvoicePM.ActualPayedAmount != value) {
            this.InvoicePM.ActualPayedAmount = value;

        }
    }

    //#endregion

    //#region Modification List

    ModificationsList: ObservableCollection;
    NewModificationAdded: boolean;

    UCRList: ObservableCollection;
    NewUCRAdded: boolean;


    PaymentsList: ObservableCollection;
    NewPaymentsAdded: boolean;

    AddPaymentButton() {

        // 1- check if list have invalid items
        //if (this.PaymentsList.Length > 0) {
        //    var exist = this.PaymentsList.Collection.find(d => !d.isValid);
        //    if (exist) {
        //        return;
        //    }
        //}

        // 2- get counter
        var paymentCounter = 0;
        if (this.PaymentsList.Length > 0) {
            paymentCounter = this.getMax(this.PaymentsList.Collection, "SequenceNumeric");
        }
        paymentCounter += 1;

        // 3- build new item
        var item = new SupplierInvoicePaymentPM(this.InvoicePM);
        item.DeclarationId = this.InvoicePM.DeclarationId;
        item.InvoiceCounterKey = this.InvoicePM.InvoiceCounterKey;
        item.Tenant = SessionLocator.Tenant;
        item.ChangeSetOp = "Insert";
        item.SequenceNumeric = paymentCounter;

        // 4- add it to entityPM
        this.InvoicePM.AddSupplierInvoicePayment(item);

        // 5- add it to observable list
        this.PaymentsList.Insert(new PaymentItemModel(item));

        this.NewPaymentsAdded = true;

    }

    AddUCRButton() {

        // 1- check if list have invalid items
        //if (this.PaymentsList.Length > 0) {
        //    var exist = this.PaymentsList.Collection.find(d => !d.isValid);
        //    if (exist) {
        //        return;
        //    }
        //}
         // 2- get counter
        var ucrCounter = 0;
        if (this.PaymentsList.Length > 0) {
            ucrCounter = this.getMax(this.UCRList.Collection, "SequenceNumeric");
        }
        ucrCounter += 1;

        // 3- build new item
        var item = new SupplierInvoiceUCRPM(this.InvoicePM);
        item.DeclarationId = this.InvoicePM.DeclarationId;
        item.InvoiceCounterKey = this.InvoicePM.InvoiceCounterKey;
        item.Tenant = SessionLocator.Tenant;
        item.ChangeSetOp = "Insert";
        item.SequenceNumeric = ucrCounter;

        // 4- add it to entityPM
        this.InvoicePM.AddSupplierInvoiceUCR(item);

        // 5- add it to observable list
        this.UCRList.Insert(new UCRItemModel(item));

        this.NewUCRAdded = true;

    }

    AddModificationButton() {

        // 1- check if list have invalid items
        if (this.ModificationsList.Length > 0) {
            var exist = this.ModificationsList.Collection.find(d => !d.isValid);
            if (exist) {
                return;
            }
        }

        // 2- get counter
        var modificationCounter = 0;
        if (this.ModificationsList.Length > 0) {
            modificationCounter = this.getMax(this.ModificationsList.Collection, "ModificationCounterKey");
        }
        modificationCounter += 1;

        // 3- build new item
        var item = new SupplierInvoiceModificationPM(this.InvoicePM);
        item.DeclarationId = this.InvoicePM.DeclarationId;
        item.InvoiceCounterKey = this.InvoicePM.InvoiceCounterKey;
        item.Tenant = SessionLocator.Tenant;
        item.ChangeSetOp = "Insert";
        item.ModificationCounterKey = modificationCounter;

        // 4- add it to entityPM
        this.InvoicePM.AddSupplierInvoiceModification(item);

        // 5- add it to observable list
        this.ModificationsList.Insert(new ModificationItemModel(item, this));

        this.NewModificationAdded = true;

    }
    RemoveModification(item: ModificationItemModel) {
        console.log("... Removing ", item);
        this.ModificationsList.Remove(item);
        this.InvoicePM.RemoveSupplierInvoiceModification(item.ModificationPM); // remove from entity
    }

    RemovePayment(item: PaymentItemModel) {
         console.log("... Removing ", item);
        this.PaymentsList.Remove(item);
        this.InvoicePM.RemoveSupplierInvoicePayment(item.SupplierInvoicePayment); // remove from entity
    }

    RemoveUCR(item: UCRItemModel) {
        console.log("... Removing ", item);
        this.UCRList.Remove(item);
        this.InvoicePM.RemoveSupplierInvoiceUCR(item.SupplierInvoiceUCR); // remove from entity
    }
    OnRowEnded($event) {
        console.log("Length : " + this.ModificationsList.Length);
        if (($event) == this.ModificationsList.Length) {
            this.AddModificationButton();
        }
    }


    OnFocus() {
        //if (this.ModificationsList.Length == 0) {
        //    this.AddModificationButton();
        //}
    }

    getMax(list: any[], propertyName: string) {
        var max = -99999;
        var maxObj = list && list.length>0 ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    }

    //#endregion

    FillErrors(errors: string[]) {
        console.log("[ERRORs] SupplierInvoiceMoreTabComponent: ", errors);
        this.FillValidationErrorList.emit(errors); // clear validation msgs
    }

}

export class PaymentItemModel extends BaseComponent {
    public SupplierInvoicePayment: SupplierInvoicePaymentPM = null;
    public ObjectTableName = "Customs.SupplierInvoicePayment";
    public DataContext = this;

    constructor(private _supplierInvoicePayment: SupplierInvoicePaymentPM) {
        super();
        this.SupplierInvoicePayment = _supplierInvoicePayment;
    }

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

    get DeclarationId() { return this.SupplierInvoicePayment.DeclarationId; }
    set DeclarationId(value: string) {
        if (this.SupplierInvoicePayment.DeclarationId != value) {
            this.SupplierInvoicePayment.DeclarationId = value;

        }
    }


    get InvoiceCounterKey() { return this.SupplierInvoicePayment.InvoiceCounterKey; }
    set InvoiceCounterKey(value: number) {
        if (this.SupplierInvoicePayment.InvoiceCounterKey != value) {
            this.SupplierInvoicePayment.InvoiceCounterKey = value;

        }
    }

    get SequenceNumeric() { return this.SupplierInvoicePayment.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        if (this.SupplierInvoicePayment.SequenceNumeric != value) {
            this.SupplierInvoicePayment.SequenceNumeric = value;

        }
    }

    get Tenant() { return this.SupplierInvoicePayment.Tenant; }
    set Tenant(value: number) {
        if (this.SupplierInvoicePayment.Tenant != value) {
            this.SupplierInvoicePayment.Tenant = value;

        }
    }

    get PaymentAmount() { return this.SupplierInvoicePayment.PaymentAmount; }
    set PaymentAmount(value: number) {
        if (this.SupplierInvoicePayment.PaymentAmount != value) {
            this.SupplierInvoicePayment.PaymentAmount = value;

        }
    }
    get PaymentTypeCode() { return this.SupplierInvoicePayment.PaymentTypeCode; }
    set PaymentTypeCode(value: string) {
        if (this.SupplierInvoicePayment.PaymentTypeCode != value) {
            this.SupplierInvoicePayment.PaymentTypeCode = value;

        }
    }

    get PaymentTypeName() { return this.SupplierInvoicePayment.PaymentTypeName; }
    set PaymentTypeName(value: string) {
        if (this.SupplierInvoicePayment.PaymentTypeName != value) {
            this.SupplierInvoicePayment.PaymentTypeName = value;

        }
    }
}
export class UCRItemModel extends BaseComponent {

    public SupplierInvoiceUCR: SupplierInvoiceUCRPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceUCR";
    public DataContext = this;

    constructor(private _supplierInvoiceUCRPM: SupplierInvoiceUCRPM) {
        super();
        this.SupplierInvoiceUCR = _supplierInvoiceUCRPM;
    }

    get DeclarationId() { return this.SupplierInvoiceUCR.DeclarationId; }
    set DeclarationId(value: string) {
        if (this.SupplierInvoiceUCR.DeclarationId != value) {
            this.SupplierInvoiceUCR.DeclarationId = value;

        }
    }


    get InvoiceCounterKey() { return this.SupplierInvoiceUCR.InvoiceCounterKey; }
    set InvoiceCounterKey(value: number) {
        if (this.SupplierInvoiceUCR.InvoiceCounterKey != value) {
            this.SupplierInvoiceUCR.InvoiceCounterKey = value;

        }
    }

    get SequenceNumeric() { return this.SupplierInvoiceUCR.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        if (this.SupplierInvoiceUCR.SequenceNumeric != value) {
            this.SupplierInvoiceUCR.SequenceNumeric = value;

        }
    }

    get Tenant() { return this.SupplierInvoiceUCR.Tenant; }
    set Tenant(value: number) {
        if (this.SupplierInvoiceUCR.Tenant != value) {
            this.SupplierInvoiceUCR.Tenant = value;

        }
    }

    get AgentChargeID() { return this.SupplierInvoiceUCR.AgentChargeID; }
    set AgentChargeID(value: string) {
        if (this.SupplierInvoiceUCR.AgentChargeID != value) {
            this.SupplierInvoiceUCR.AgentChargeID = value;

        }
    }


    get SupplierChargeID() { return this.SupplierInvoiceUCR.SupplierChargeID; }
    set SupplierChargeID(value: string) {
        if (this.SupplierInvoiceUCR.SupplierChargeID != value) {
            this.SupplierInvoiceUCR.SupplierChargeID = value;

        }
    }
}

export class ModificationItemModel extends BaseComponent {
    public ModificationPM: SupplierInvoiceModificationPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceModification";
    public DataContext = this;
    isValid: boolean;
    public customsExchangeRateExtendedPMService: CustomsExchangeRateExtendedPMService = new CustomsExchangeRateExtendedPMService();

    constructor(private modificationPM: SupplierInvoiceModificationPM, private parent: SupplierInvoiceMoreTabComponent) {
        super();
        this.ModificationPM = modificationPM;
        this.isValid = true;
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.parent.InvoicePM.InvoiceCurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe((response:any) => {
            if (response) {
                if (response.Result) {
                    var rate = response.Result[0];
                    if (rate) {
                        this.InvoiceCurrencyExchangeRtae = rate.ExchangeRate;
                    }
                }
            }
        });
    }

    //#region Properties

    get TypeCode() { return this.ModificationPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.ModificationPM.TypeCode != value) {
            //this.ModificationPM.TypeCode = value;

            if (value == "I02") {
                this.ModificationPM.TypeCode = value;
                this.isValid = false;
                var errors = [];
                errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.CalculatedFee") + " - מסך נוספים");
                this.parent.FillErrors(errors);
            } else {

                var exists;
                if (this.parent.InvoicePM.SupplierInvoiceModifications.length != 0) {
                    exists = this.parent.InvoicePM.SupplierInvoiceModifications.find(d => d.TypeCode == value);
                }
                if (exists) {
                    this.ModificationPM.TypeCode = value;
                    //ModificationAndDiscountTypeList modificationAndDiscountType = ModificationAndDiscountTypeDataProvider.GetCachedList<ModificationAndDiscountTypeList>().Where(d => d.Code == value).FirstOrDefault();
                    //TypeName = modificationAndDiscountType != null ? modificationAndDiscountType.LocalName : null;
                    this.isValid = false;
                    var errors = [];
                    errors.push(TextCodeTranslator.Translate("Customs.Declaration.O.ExistingType") + " - מסך נוספים");
                    this.parent.FillErrors(errors);
                } else {
                    this.ModificationPM.TypeCode = value;
                    //FirePropertyChanged("TypeCode");
                    this.isValid = true;
                    //ModificationAndDiscountTypeList modificationAndDiscountType = ModificationAndDiscountTypeDataProvider.GetCachedList<ModificationAndDiscountTypeList>().Where(d => d.Code == value).FirstOrDefault();
                    //TypeName = modificationAndDiscountType != null ? modificationAndDiscountType.LocalName : null;
                    var errors = [];
                    this.parent.FillErrors(errors);
                }

            }

        }
    }

    get TypeName() { return this.ModificationPM.TypeName; }
    set TypeName(value: string) {
        if (this.ModificationPM.TypeName != value) {
            this.ModificationPM.TypeName = value;

        }
    }

    get CurrencyTypeCode() { return this.ModificationPM.CurrencyTypeCode; }
    set CurrencyTypeCode(value: string) {
        if (this.ModificationPM.CurrencyTypeCode != value) {
            this.ModificationPM.CurrencyTypeCode = value;

        }
    }

    get CurrencyTypeName() { return this.ModificationPM.CurrencyTypeName; }
    set CurrencyTypeName(value: string) {
        if (this.ModificationPM.CurrencyTypeName != value) {
            this.ModificationPM.CurrencyTypeName = value;

        }
    }

    get Amount() { return this.ModificationPM.Amount; }
    set Amount(value: number) {
        if (this.ModificationPM.Amount != value) {
            this.ModificationPM.Amount = value;
        //    this.doCalculate = true;
        }
           
    }
    doCalculate: boolean = false;

    OriginalText: string;
    AmountOriginalText(originalText: string) {
        this.OriginalText = originalText;
        if (this.OriginalText ) {
            if ((this.OriginalText + "").indexOf('%')> -1) {
                this.doCalculate = true;
            }
        }

        this.OriginalText = null;
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
    DiscountInNIS: number = 0;
    InvoiceCurrencyExchangeRtae: number = 0;
    DiscountInDsicCurrency: number = 0;

    OnAmountLostFocus() {
        if (this.doCalculate) {
            var value = this.Amount;
           
        
            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.CurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe((response:any) => {
                if (this.parent.InvoicePM.InvoiceAmount) {
                    this.DiscountInNIS = this.parent.InvoicePM.InvoiceAmount * this.InvoiceCurrencyExchangeRtae * value;
                  //  value = value * this.parent.InvoicePM.InvoiceAmount;
                    if (response) {
                        if (response.Result) {
                            var result = response.Result[0];
                            if (result) {

                                var amount = this.DiscountInNIS / result.ExchangeRate;
                                if (amount) {
                                    this.Amount = amount;
                                }


                            }
                        }
                    }
                }
                this.doCalculate = false;


            });
        }

    }
}
