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


@Component({
    moduleId: module.id,
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


        // initialize query filters for Parent Account
        this.TypeCodeFilterItems = new ApiQueryFilters();
        this.TypeCodeFilterItems.addAdditionalFilter("Code", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        //this.TypeCodeFilterItems.addAdditionalFilter("LocalName", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        this.TypeCodeFilterItems.addAdditionalFilter("LocalName", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
        //this.TypeCodeFilterItems.addAdditionalFilter("SearchFields", "I02", null, null, "Exclude", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294
      this.TypeCodeFilterItems.addAdditionalFilter("SearchFields", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294


      this.TypeCodeFilterItems = new ApiQueryFilters();
      this.TypeCodeFilterItems.addAdditionalFilter("Code", "I02", null, null, "NotContains", false, false, false, "string", false, true); //Task 36745: Supplier Invoice Modifications - Logic for Code "I02" CALL#302294

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
        this.ModificationsList = new ObservableCollection([]);
        for (let item of this.InvoicePM.SupplierInvoiceModifications) {
            if (item.TypeCode != "I02")
                this.ModificationsList.Insert(new ModificationItemModel(item, this));
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
        var maxObj = list ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current }) : null;
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
        this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.parent.InvoicePM.InvoiceCurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe(response => {
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
           
        
            this.customsExchangeRateExtendedPMService.GetCustomsExchangeRateForCurrencyAndDate(this.CurrencyTypeCode, this.parent.declarationPM.TaxationDateTime).subscribe(response => {
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
