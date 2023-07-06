declare var window: any;
import { Component, ChangeDetectorRef, EventEmitter } from '@angular/core';
import { AppTool, ArrayTool } from '../../../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { LogTab } from '../../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { SessionLocator } from '../../../../../../Infrastructure/Utilities/SessionLocator';
import { ServiceHelper } from '../../../../../../Infrastructure/Utilities/ServiceHelper';
import { ConfirmWindow } from '../../../../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObservableCollection } from '../../../../../../Infrastructure/Utilities/ObservableCollection';
import { EntityResourceService } from '../../../../../../Infrastructure/Services/EntityResourceService';
import { ApiQueryFilters } from '../../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { DeclarationValidator } from '../../../../../../Customs/Validators/DeclarationValidator';
import { LogitudeWindow } from '../../../../../../Controls/Windows/LogitudeWindow';
import { FeatureLocator } from '../../../../../../Infrastructure/Utilities/FeatureLocator';
import { SupplierInvoiceItemPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { SupplierInvoiceItemsModPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsModPM';
import { SupplierInvoiceItemProcesTypePM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemProcesTypePM';
import { SupplierInvoiceItemsConDeclarPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsConDeclarPM';
import { SupplierInvoiceItemsSerialNumPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsSerialNumPM';
import { SupplierInvoiceItemsDescriptPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsDescriptPM';
import { SupplierInvoiceItemsProdIdentPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsProdIdentPM';
import { SupplierInvoiceItemsLevyPM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsLevyPM';
import { CustomsItemPM } from '../../../../../../Customs/EntityPMs/CustomsItemPM';
import { Subject } from 'rxjs';

import { CustomsRequiredFieldListService } from '../../../../../../Customs/Services/StandardLists/CustomsRequiredFieldListService';

import { LuhnAlgorithm } from '../../../../../../Customs/Utilities/LuhnAlgorithm';
import { ObjectsLocator } from '../../../../../../Infrastructure/Locators/ObjectsLocator';
import { SupplierInvoiceItemsPricePM } from '../../../../../../Customs/EntityPMs/SupplierInvoiceItemsPricePM';
import { SuppInvoiceItemsAbachStatementPM } from '../../../../../../Customs/EntityPMs/SuppInvoiceItemsAbachStatementPM';
import { CustomsRequiredFieldExtendedListService } from '../../../../../../Customs/Services/ExtendedLists/CustomsRequiredFieldExtendedListService';
import { customsItemsService } from 'QuoteOPM/Utilities/customsItems.service';
import { SupplierInvoiceSharedService } from '../Services/SupplierInvoiceSharedService';


@Component({

    templateUrl: './EditSupplierInvoiceItem.html',
})
export class EditSupplierInvoiceItem extends BaseComponent {
    public DataContext: any = this;
    public entityResourceService: EntityResourceService = new EntityResourceService();
    public declarationValidator: DeclarationValidator = new DeclarationValidator();
    public ValidationErrorsList: any[] = [];
    public OriginalItemPM: SupplierInvoiceItemPM;
    public ClonedItemPM: SupplierInvoiceItemPM;
    public TypeCodeFilterItems: ApiQueryFilters;
    public AdditionalPriceTypeCodeFilterItems: ApiQueryFilters;
    public ProcessTypeCodeFilterItems: ApiQueryFilters;
    //public OriginalItemPM: SupplierInvoiceItemPM; // screen bindingObjectsLocator 

    public CustomsBookTypeFilterItems: ApiQueryFilters;
    public ObjectTableName: string = "Customs.SupplierInvoiceItem";
    LayoutDirection: string = 'ltr';
    public IsDisplayOnly: boolean = false;
    CustomItemErrorMessage: string;
    private CurrentSession = SessionLocator.SelectedSession;
    allowExport: boolean = false;
    taxExemptCodeTypesFilter: ApiQueryFilters;

    public InvoiceNumberText: string = "Customs.SupplierInvoiceItemsConDeclar.F.InvoiceNumber";
    constructor(private cd: ChangeDetectorRef, private supplierInvoiceSharedService:SupplierInvoiceSharedService) {
        super();
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
        this.CustomsBookTypeFilterItems = new ApiQueryFilters();
        this.CustomsBookTypeFilterItems.addAdditionalFilter("Code", "2", null, null, "Exclude", false, false, false, "string", false, true);
        this.TypeCodeFilterItems = new ApiQueryFilters();
        if (this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E") {
            this.TypeCodeFilterItems.addAdditionalFilter("IsRelevantGoodsItemExport", true, null, null, "Equals", false, false, false, "boolean",false,true);
        } else {
            this.TypeCodeFilterItems.addAdditionalFilter("IsRelevantGoodsItem", true, null, null, "Equals", false, false, false, "boolean",false,true);
        }
        this.ProcessTypeCodeFilterItems = new ApiQueryFilters();
        if (this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E") {
            this.ProcessTypeCodeFilterItems.addAdditionalFilter("LeadDocumentTypeID", this.CurrentSession.CurrentEditComponent.EntityPM.DeclarationTypeCode, null, null, "Equals", false, false, false, "string",false,true);
        }

        this.AdditionalPriceTypeCodeFilterItems = new ApiQueryFilters();
        if (this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E") {
            this.AdditionalPriceTypeCodeFilterItems.addAdditionalFilter("Code", "1", null, null, "Exclude", false, false, false, "string",false,true);
        }

        this.BuildTabs();

        // Initilize lists
        this.ModificationsList = new ObservableCollection([]);
        this.PricesList = new ObservableCollection([]);
        this.ProcessTypesList = new ObservableCollection([]); 
        this.ConDeclarList = new ObservableCollection([]);
        this.SerialNumbersList = new ObservableCollection([]);
        this.DescriptionsList = new ObservableCollection([]);
        this.IdentificationsList = new ObservableCollection([]);
        this.LevyList = new ObservableCollection([]);
    }
    SetWindowArgs(args: any) {       
        if (!AppTool.IsNullOrEmpty(args)) {
            this.IsDisplayOnly = args.IsDisplayOnly;
            this.allowExport = args.allowExport;
            this.allowExport ? this.InvoiceNumberText="Customs.SupplierInvoiceItemsConnectedDeclaration.O.InvoiceSequence": this.InvoiceNumberText;    
            this.OriginalItemPM = args.SupplierInvoiceItemPM;
            this.ClonedItemPM = this.CloneEntity(args.SupplierInvoiceItemPM);
            

            this.CustomsItem = this.OriginalItemPM.TaxExemptCode;

            this.FillGridsData(); // copy  grids data from entity PM to ItemSource arrays
            this.taxExemptCodeTypesFilter = customsItemsService.initTaxExemptCodeTypesFilter(args.allowExport);

            if (this.IsDisplayOnly) {
                this.SetScreenFieldsEditability();
            }
        }
        console.log("--> EditSupplierInvoiceItem window argument passed: ", args);
        this.CheckRequrierdFieldsForSend();
        
    }

    FillGridsData() {
        
        // Modification List
        this.ModificationsList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsMods) {
            this.ModificationsList.Insert(new ModificationItemModel(item,this.supplierInvoiceSharedService));
        }

        this.PricesList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsPrices) {
            this.PricesList.Insert(new PriceItemModel(item));
        }

        this.AbachsList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SuppInvoiceItemsAbachStatements) {
            this.AbachsList.Insert(new AbachItemModel(item));
        }

        // ProcessTypes List
        this.ProcessTypesList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemProcesTypes) {
            this.ProcessTypesList.Insert(new ProcessTypeItemModel(item));
        }

        // ConDeclar List 
        this.ConDeclarList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsConDeclars) {
            this.ConDeclarList.Insert(new ConDeclarItemModel(item));
        }

        // SerialNumbers List 
        this.SerialNumbersList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsSerialNums) {
            this.SerialNumbersList.Insert(new SerialNoItemModel(item));
        }

        // Descriptions List 
        this.DescriptionsList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsDescripts) {
            this.DescriptionsList.Insert(new DescribtionItemModel(item));
        }

        // Identifications List 
        this.IdentificationsList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemsProdIdents) {
            this.IdentificationsList.Insert(new ProdIdentItemModel(item));
        }

        // Levies List 
        this.LevyList = new ObservableCollection([]);
        for (let item of this.OriginalItemPM.SupplierInvoiceItemLevies) {
            this.LevyList.Insert(new LevyItemModel(item));
        }
    }

    SetScreenFieldsEditability() {
        this.UIProperties.SetEnabled("StatisticQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("StatisticQuantityType", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AdditionalQuantity", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("AdditionalQuantityType", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("CustomsBookTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("PreferenceDocumentNumber", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ActualInvoiceLines", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeferredCustomsTax", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DeferredPurchaseTax", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("SalesTaxExemptionTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TaxExemptCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("OptionalTamaPercentage", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("NonCustomsItemPrice", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("NonCustomsItemPriceCurCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WholeSaleItemPrice", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("WholeSaleItemPriceCurrencyCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("IsUsed", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ManufactureIdentifier", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DangerousClassificationCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("DangerousPackingGroupTypeCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("TransactionNatureCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ClaimReasonCode", this.ObjectTableName, !this.IsDisplayOnly);
        this.UIProperties.SetEnabled("ClassificationTypeCode", this.ObjectTableName, !this.IsDisplayOnly);

    }

    CheckRequrierdFieldsForSend() {
        var isExport = false;
        if (this.allowExport) {
            isExport = true;
        }

        var customsRequiredFieldListService: CustomsRequiredFieldListService = new CustomsRequiredFieldListService();
        var table = window.ObjectTables.filter(d => d.Name == 'Customs.SupplierInvoiceItem')[0];
        var filters = new ApiQueryFilters();
        filters.addAdditionalFilter("ObjectTableId", table.Id, null, null, "Equals", false, false, false, "string");
        var customsRequiredFieldExtendedListService: CustomsRequiredFieldExtendedListService = new CustomsRequiredFieldExtendedListService();
        filters = customsRequiredFieldExtendedListService.GetFilter(filters, isExport)


        customsRequiredFieldListService.getAllFromCache(filters).subscribe((response: any) => {
            var requiredFields = response.Result;
            requiredFields.forEach((field) => {
                var objectField = window.ObjectFields.filter(d => d.FieldCode == field.ObjectfieldCode)[0];
                this.UIProperties.SetWarning(objectField.FieldName, 'Customs.SupplierInvoiceItem', true);
            });
        });
    }

    //#region Tabs Code
    TabsSource: any[] = [];
    SelectedTab: string = "";

    BuildTabs() {
        this.SelectedTab = "Details";
        this.TabsSource.push({ Name: "Details", isSelected: true, Header: TextCodeTranslator.Translate("Customs.Declaration.O.Details") });
        this.TabsSource.push({ Name: "Declarations", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.Declarations") });
        this.TabsSource.push({ Name: "SerialNumbers", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.SerialNumbers") });
        this.TabsSource.push({ Name: "Levies", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.Levies") });
        if (FeatureLocator.IsFeatureGrantedByCode("ItemPackageTab")) {
            this.TabsSource.push({ Name: "Packages", isSelected: false, Header: TextCodeTranslator.Translate("Customs.Declaration.O.Packages") });
        }
    }
    SelectionChanged(tab: any) {

        this.TabsSource.forEach(item => { // reset selection
            item.isSelected = false;
        });

        var index = this.TabsSource.indexOf(tab);
        if (index < 0) {
            console.log("The tab was not found, cant not delete it :( ", tab); return;
        }
        var item = this.TabsSource[index];
        item.isSelected = true;
        this.SelectedTab = item.Name;
    }
    //#endregion

    //#region Tab: Details

    //#region Properties
    get StatisticQuantity() { return this.OriginalItemPM.StatisticQuantity; }
    set StatisticQuantity(value: number) {
        if (this.OriginalItemPM.StatisticQuantity != value) {
            this.OriginalItemPM.StatisticQuantity = value;

        }
    }

    get StatisticQuantityType() { return this.OriginalItemPM.StatisticQuantityType; }
    set StatisticQuantityType(value: string) {
        if (this.OriginalItemPM.StatisticQuantityType != value) {
            this.OriginalItemPM.StatisticQuantityType = value;

        }
    }

    get AdditionalQuantity() { return this.OriginalItemPM.AdditionalQuantity; }
    set AdditionalQuantity(value: number) {
        if (this.OriginalItemPM.AdditionalQuantity != value) {
            this.OriginalItemPM.AdditionalQuantity = value;

        }
    }

    get AdditionalQuantityType() { return this.OriginalItemPM.AdditionalQuantityType; }
    set AdditionalQuantityType(value: string) {
        if (this.OriginalItemPM.AdditionalQuantityType != value) {
            this.OriginalItemPM.AdditionalQuantityType = value;

        }
    }

    get CustomsBookTypeCode() { return this.OriginalItemPM.CustomsBookTypeCode; }
    set CustomsBookTypeCode(value: string) {
        if (this.OriginalItemPM.CustomsBookTypeCode != value) {
            this.OriginalItemPM.CustomsBookTypeCode = value;

        }
    }

    get PreferenceDocumentNumber() { return this.OriginalItemPM.PreferenceDocumentNumber; }
    set PreferenceDocumentNumber(value: string) {
        if (this.OriginalItemPM.PreferenceDocumentNumber != value) {
            this.OriginalItemPM.PreferenceDocumentNumber = value;

        }
    }

    get ActualInvoiceLines() { return this.OriginalItemPM.ActualInvoiceLines; }
    set ActualInvoiceLines(value: string) {
        if (this.OriginalItemPM.ActualInvoiceLines != value) {
            this.OriginalItemPM.ActualInvoiceLines = value;

        }
    }



    get DeferredCustomsTax() { return this.OriginalItemPM.DeferredCustomsTax; }
    set DeferredCustomsTax(value: number) {
        if (this.OriginalItemPM.DeferredCustomsTax != value) {
            this.OriginalItemPM.DeferredCustomsTax = value;

        }
    }

    get DeferredPurchaseTax() { return this.OriginalItemPM.DeferredPurchaseTax; }
    set DeferredPurchaseTax(value: number) {
        if (this.OriginalItemPM.DeferredPurchaseTax != value) {
            this.OriginalItemPM.DeferredPurchaseTax = value;

        }
    }

    get SalesTaxExemptionTypeCode() { return this.OriginalItemPM.SalesTaxExemptionTypeCode; }
    set SalesTaxExemptionTypeCode(value: string) {
        if (this.OriginalItemPM.SalesTaxExemptionTypeCode != value) {
            this.OriginalItemPM.SalesTaxExemptionTypeCode = value;

        }
    }

    get TaxExemptCode() { return this.OriginalItemPM.TaxExemptCode; }
    set TaxExemptCode(value: string) {
        if (this.OriginalItemPM.TaxExemptCode != value) {
            this.OriginalItemPM.TaxExemptCode = value;

        }
    }

    get OptionalTamaPercentage() { return this.OriginalItemPM.OptionalTamaPercentage; }
    set OptionalTamaPercentage(value: number) {
        if (this.OriginalItemPM.OptionalTamaPercentage != value) {
            this.OriginalItemPM.OptionalTamaPercentage = value;

        }
    }



    get NonCustomsItemPrice() { return this.OriginalItemPM.NonCustomsItemPrice; }
    set NonCustomsItemPrice(value: number) {
        if (this.OriginalItemPM.NonCustomsItemPrice != value) {
            this.OriginalItemPM.NonCustomsItemPrice = value;

        }
    }

    get NonCustomsItemPriceCurCode() { return this.OriginalItemPM.NonCustomsItemPriceCurCode; }
    set NonCustomsItemPriceCurCode(value: string) {
        if (this.OriginalItemPM.NonCustomsItemPriceCurCode != value) {
            this.OriginalItemPM.NonCustomsItemPriceCurCode = value;

        }
    }

    get WholeSaleItemPrice() { return this.OriginalItemPM.WholeSaleItemPrice; }
    set WholeSaleItemPrice(value: number) {
        if (this.OriginalItemPM.WholeSaleItemPrice != value) {
            this.OriginalItemPM.WholeSaleItemPrice = value;

        }
    }

    get WholeSaleItemPriceCurrencyCode() { return this.OriginalItemPM.WholeSaleItemPriceCurrencyCode; }
    set WholeSaleItemPriceCurrencyCode(value: string) {
        if (this.OriginalItemPM.WholeSaleItemPriceCurrencyCode != value) {
            this.OriginalItemPM.WholeSaleItemPriceCurrencyCode = value;

        }
    }

    get IsUsed() { return this.OriginalItemPM.IsUsed; }
    set IsUsed(value: boolean) {
        if (this.OriginalItemPM.IsUsed != value) {
            this.OriginalItemPM.IsUsed = value;

        }
    }

    get ManufactureIdentifier() { return this.OriginalItemPM.ManufactureIdentifier; }
    set ManufactureIdentifier(value: string) {
        if (this.OriginalItemPM.ManufactureIdentifier != value) {
            this.OriginalItemPM.ManufactureIdentifier = value;

        }
    }

    get DangerousClassificationCode() { return this.OriginalItemPM.DangerousClassificationCode; }
    set DangerousClassificationCode(value: string) {
        if (this.OriginalItemPM.DangerousClassificationCode != value) {
            if (value) {
                this.OriginalItemPM.DangerousClassificationCode = value + "";
            } else {
                this.OriginalItemPM.DangerousClassificationCode = null;
            }

        }
    }

    get DangerousPackingGroupTypeCode() { return this.OriginalItemPM.DangerousPackingGroupTypeCode; }
    set DangerousPackingGroupTypeCode(value: string) {
        if (this.OriginalItemPM.DangerousPackingGroupTypeCode != value) {
            this.OriginalItemPM.DangerousPackingGroupTypeCode = value;

        }
    }

    public get ClassificationTypeCode() { return this.OriginalItemPM ? this.OriginalItemPM.ClassificationTypeCode : null; }
    public set ClassificationTypeCode(newValue: string) {
        this.OriginalItemPM.ClassificationTypeCode = newValue;
    }

    public get TransactionNatureCode() { return this.OriginalItemPM ? this.OriginalItemPM.TransactionNatureCode : null; }
    public set TransactionNatureCode(newValue: string) {

        this.OriginalItemPM.TransactionNatureCode = newValue;
    }

    public get ClaimReasonCode() { return this.OriginalItemPM ? this.OriginalItemPM.ClaimReasonCode : null; }
    public set ClaimReasonCode(newValue: string) {
        this.OriginalItemPM.ClaimReasonCode = newValue;
    }

    //#endregion

    AbachsList: ObservableCollection;
    abachCounter = 0;

    AddAbachButton() {
        var item = new SuppInvoiceItemsAbachStatementPM(this.OriginalItemPM);

        if (this.AbachsList.Length > 0) {
            this.abachCounter = this.getMax(this.AbachsList.Collection, "SequenceNumeric");
        }
        item.DeclarationId = this.OriginalItemPM.DeclarationId,
            item.Tenant = this.OriginalItemPM.Tenant,
            item.InvoiceCounterKey = this.OriginalItemPM.CounterKey,
            item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber,
            item.SequenceNumeric = this.abachCounter + 1,
            item.ChangeSetOp = "Insert",

            this.OriginalItemPM.AddSuppInvoiceItemsAbachStatement(item);
        this.AbachsList.Insert(new AbachItemModel(item));
        //this.CurrentSession.ResetRowIndex();

    }

    RemoveAbach(item: AbachItemModel) {
        console.log("... Removing ", item);
        this.AbachsList.Remove(item);
        this.OriginalItemPM.RemoveSuppInvoiceItemsAbachStatement(item.AbachPM); // remove from entity
    }



    PricesList: ObservableCollection;
    priceCounter = 0;

    AddPriceButton() {
        var item = new SupplierInvoiceItemsPricePM(this.OriginalItemPM);

        if (this.PricesList.Length > 0) {
            this.priceCounter = this.getMax(this.PricesList.Collection, "LineNumber");
        }
        item.DeclarationId = this.OriginalItemPM.DeclarationId,
            item.Tenant = this.OriginalItemPM.Tenant,
            item.InvoiceCounterKey = this.OriginalItemPM.CounterKey,
            item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber,
            item.LineNumber = this.priceCounter + 1,

            item.ChangeSetOp = "Insert",

            this.OriginalItemPM.AddSupplierInvoiceItemsPrice(item);
        this.PricesList.Insert(new PriceItemModel(item));
        //this.CurrentSession.ResetRowIndex();

    }

    RemovePrice(item: PriceItemModel) {
        console.log("... Removing ", item);
        this.PricesList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsPrice(item.PricePM); // remove from entity
    }


    //#region Modification List

    ModificationsList: ObservableCollection;
    modificationCounter = 0;

    AddModificationButton() {
        if (this.IsModificationValid(this.ModificationsList.Collection[this.ModificationsList.Collection.length - 1])) {
            var item = new SupplierInvoiceItemsModPM(this.OriginalItemPM);

            if (this.ModificationsList.Length > 0) {
                this.modificationCounter = this.getMax(this.ModificationsList.Collection, "ModificationCounterKey");
            }
            item.DeclarationId = this.OriginalItemPM.DeclarationId,
                item.Tenant = this.OriginalItemPM.Tenant,
                item.InvoiceCounterKey = this.OriginalItemPM.CounterKey,
                item.LineNumber = this.OriginalItemPM.LineNumber,
                item.ModificationCounterKey = this.modificationCounter,

                item.ChangeSetOp = "Insert",

                this.OriginalItemPM.AddSupplierInvoiceItemsMod(item);
            this.ModificationsList.Insert(new ModificationItemModel(item,this.supplierInvoiceSharedService));
            //this.CurrentSession.ResetRowIndex();
        }
    }
    RemoveModification(item: ModificationItemModel) {
        console.log("... Removing ", item);
        this.ModificationsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsMod(item.ModificationPM); // remove from entity
        this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E" ? this.supplierInvoiceSharedService.DifferenceAndTotalForeignCurrency$.next() : '';

        }  
    IsModificationValid(modificationM: ModificationItemModel) {
        if (!AppTool.IsNullOrEmpty(modificationM)) {
            // requierd fields for last row
            if (!AppTool.IsNullOrEmpty(modificationM.TypeCode) && !AppTool.IsNullOrEmpty(modificationM.CurrencyTypeCode)) {
                return true;
            }
            return false;
        }
        return true;
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

    //#endregion

    //#region ProcessType List

    ProcessTypesList: ObservableCollection;

    ptNumber = 0;
    AddProcessTypeButton() {
        var pm = new SupplierInvoiceItemProcesTypePM(this.OriginalItemPM);

        //max
        if (this.ProcessTypesList.Length > 0) {
            this.ptNumber = this.getMax(this.ProcessTypesList.Collection, "LineNumber");
        }

        pm.LineNumber = ++this.ptNumber;
        pm.DeclarationId = this.OriginalItemPM.DeclarationId;
        pm.Tenant = this.OriginalItemPM.Tenant;
        pm.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        pm.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;


        this.ProcessTypesList.Insert(new ProcessTypeItemModel(pm));
        this.OriginalItemPM.AddSupplierInvoiceItemProcesType(pm);
    }
    RemoveProcessType(item: ProcessTypeItemModel) {
        this.ProcessTypesList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemProcesType(item.ProcessTypePM);
    }

    //#endregion

    OnDangClasCodeBlur() {
        if (!AppTool.IsNullOrEmpty(this.DangerousClassificationCode)) { // because we convert the number value to text (+"")
            if (this.DangerousClassificationCode.length < 8) {
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort"));
            } else if (this.DangerousClassificationCode.length > 8) {
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, false, TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong"));
            } else { //valid
                this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, true, "");
            }
        } else {
            this.UIProperties.SetValidity("DangerousClassificationCode", this.ObjectTableName, true, "");
        }
    }


    customsItem: string;
    get CustomsItem() { return this.customsItem; }
    set CustomsItem(value: string) {
        this.customsItem = value;
    }

    customsItemTextValue: string;
    get CustomsItemTextValue() { return this.customsItemTextValue; }
    set CustomsItemTextValue(value: string) {
        this.customsItemTextValue = value;
    }

    CustomsItemClicked(item: CustomsItemPM) {
        if (item) {
            this.CustomsItem = null;
            this.cd.detectChanges();
            this.CustomsItem = item.FullClassification;
            //this.CustomsItem = item.FullClassification + (item.ComputedCheckDigit ? item.ComputedCheckDigit : "");
        }
    }
    CustomsItemTextChanged(text: string) {
        this.CustomsItemTextValue = text;
    }
    CustomsItemLostFocus(text: string) {
        if (!AppTool.IsNullOrEmpty(text))
            this.ValidateCustomsItemField(text);
        this.TaxExemptCode = this.CustomsItemTextValue;
        this.CustomsItem = this.CustomsItemTextValue;
    }

    //#endregion

    //#region Tab: Declaration
    ConDeclarList: ObservableCollection;
    myNumber = 0;
    AddConnDeclarButton() {
        var item = new SupplierInvoiceItemsConDeclarPM(this.OriginalItemPM);

        //max
        if (this.ConDeclarList.Length > 0) {
            this.myNumber = this.getMax(this.ConDeclarList.Collection, "LineNumber");
        }

        item.LineNumber = ++this.myNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;

        this.OriginalItemPM.AddSupplierInvoiceItemsConDeclar(item);
        this.ConDeclarList.Insert(new ConDeclarItemModel(item));
    }
    RemoveConnDeclar(item: ConDeclarItemModel) {
        this.ConDeclarList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsConDeclar(item.ConnDeclarPM); // remove from entity
    }
    
    onApprove =  new Subject(); 

    OpenExportOrImportDecData(ConDeclarItem: ConDeclarItemModel) {
        
        this.onApprove.subscribe((ConDeclaration: any) => {
            var declarationType = ConDeclaration?.IsExport ? "2" : "1";
            ConDeclaration.requestList.forEach((requestLine) => {
                if((ConDeclarItem.DeclarationNumber == ConDeclaration.DeclarationNumber && ConDeclarItem.DeclarationTypeCode == declarationType)
                    || (AppTool.IsNullOrEmpty(ConDeclarItem.DeclarationTypeCode) && AppTool.IsNullOrEmpty(ConDeclarItem.DeclarationNumber)))
                        this.RemoveConnDeclar(ConDeclarItem)

                var item = new SupplierInvoiceItemsConDeclarPM(this.OriginalItemPM);
                if (this.ConDeclarList.Length > 0) {
                    this.myNumber = this.getMax(this.ConDeclarList.Collection, "LineNumber");
                }
        
                item.LineNumber = ++this.myNumber;
                item.DeclarationId = this.OriginalItemPM.DeclarationId;
                item.Tenant = this.OriginalItemPM.Tenant;
                item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
                item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;
                item.DeclarationNumber = ConDeclaration?.DeclarationNumber; 
                item.DeclarationTypeCode = declarationType;
                item.DeclarationTypeName = declarationType == "2" ? TextCodeTranslator.Translate("Customs.Declaration.O.Export") : 
                    TextCodeTranslator.Translate("Customs.ImporterDeclarationQuery.O.DeclarationConect.ImportDeclaration");

                item.InvoiceNumber = requestLine?.InvoiceSequenceNumber;
                item.ItemSequence = requestLine?.SequenceNumber;
                item.Quantity = requestLine?.ValueQuantity;
                if(this.ValidateConDeclaration(item)){
                    this.OriginalItemPM.AddSupplierInvoiceItemsConDeclar(item);
                    this.ConDeclarList.Insert(new ConDeclarItemModel(item));
                }
               
                    });
                });


        var windowArgs: any = {};
        var logWindow = new LogitudeWindow();
        logWindow.Width = 700;
        logWindow.Height = 720;
        windowArgs.FromConnectedDeclarations = true;
        windowArgs.DeclarationTypeCode = ConDeclarItem?.DeclarationTypeCode;
        windowArgs.DeclarationNumber = ConDeclarItem?.DeclarationNumber;
        windowArgs.OnApprove = this.onApprove;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = TextCodeTranslator.Translate("Customs.General.O.DecDataQuery");
        logWindow.Show('./CustomsModules/CustomsRequests/Components/DeclarationRequests/ExportOrImportDeclarationDataComponent');


    }

    ValidateConDeclaration(conDec: any){
    var a = this.ConDeclarList.Collection.findIndex(item=>
            item.DeclarationNumber == conDec.DeclarationNumber && item.DeclarationTypeCode == conDec.DeclarationTypeCode
                && item.InvoiceNumber == conDec.InvoiceNumber && item.ItemSequence == conDec.ItemSequence && item.Quantity == conDec.Quantity)
                   if(a != -1)
                    return false
        return true;
    }
    //#endregion

    //#region Tab: Serial Numbers
    SerialNumbersList: ObservableCollection;
    DescriptionsList: ObservableCollection;
    IdentificationsList: ObservableCollection;

    snNumber = 0;
    AddSerialNumberButton() {
        var item = new SupplierInvoiceItemsSerialNumPM(this.OriginalItemPM);

        //max
        if (this.SerialNumbersList.Length > 0)
            this.snNumber = this.getMax(this.SerialNumbersList.Collection, "LineNumber");

        item.LineNumber = ++this.snNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;

        this.OriginalItemPM.AddSupplierInvoiceItemsSerialNum(item);
        this.SerialNumbersList.Insert(new SerialNoItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    }
    RemoveSerialNumber(item: SerialNoItemModel) {
        this.SerialNumbersList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsSerialNum(item.SerialNoPM); // remove from entity
    }

    descNumber = 0;
    AddDescriptionButton() {
        var item = new SupplierInvoiceItemsDescriptPM(this.OriginalItemPM);

        //max
        if (this.DescriptionsList.Length > 0) {
            this.descNumber = this.getMax(this.DescriptionsList.Collection, "LineNumber");
        }

        item.LineNumber = ++this.descNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;


        this.OriginalItemPM.AddSupplierInvoiceItemsDescript(item);
        this.DescriptionsList.Insert(new DescribtionItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    }
    RemoveDescription(item: DescribtionItemModel) {
        this.DescriptionsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsDescript(item.DescribtionPM); // remove from entity
    }

    prodIdNumber = 0;
    AddIdentificationButton() {
        var item = new SupplierInvoiceItemsProdIdentPM(this.OriginalItemPM);

        //max
        if (this.IdentificationsList.Length > 0) {
            this.prodIdNumber = this.getMax(this.IdentificationsList.Collection, "LineNumber");
        }

        item.LineNumber = ++this.prodIdNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;

        this.OriginalItemPM.AddSupplierInvoiceItemsProdIdent(item);
        this.IdentificationsList.Insert(new ProdIdentItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    }
    RemoveIdentification(item: ProdIdentItemModel) {
        this.IdentificationsList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsProdIdent(item.ProdIdentPM); // remove from entity
    }


    //#endregion

    //#region Tab: Levies
    LevyList: ObservableCollection;
    LevyNumber = 0;
    AddLevyButton() {
        var item = new SupplierInvoiceItemsLevyPM(this.OriginalItemPM);

        //max
        if (this.LevyList.Length > 0) {
            this.LevyNumber = this.getMax(this.LevyList.Collection, "LineNumber");
        }

        item.LineNumber = ++this.LevyNumber;
        item.DeclarationId = this.OriginalItemPM.DeclarationId;
        item.Tenant = this.OriginalItemPM.Tenant;
        item.InvoiceCounterKey = this.OriginalItemPM.CounterKey;
        item.InvoiceItemLineNumber = this.OriginalItemPM.LineNumber;

        this.OriginalItemPM.AddSupplierInvoiceItemsLevy(item);
        this.LevyList.Insert(new LevyItemModel(item));
        //this.CurrentSession.ResetRowIndex();
    }
    Removelevy(item: LevyItemModel) {
        this.LevyList.Remove(item);
        this.OriginalItemPM.RemoveSupplierInvoiceItemsLevy(item.LevyPM); // remove from entity
    }

    //#endregion

    //#region Tab: Packages

    get PackageQuantity() { return this.OriginalItemPM.PackageQuantity; }
    set PackageQuantity(value: number) {
        if (this.OriginalItemPM.PackageQuantity != value) {
            this.OriginalItemPM.PackageQuantity = value;
        }
    }

    get Weight() { return this.OriginalItemPM.Weight; }
    set Weight(value: number) {
        if (this.OriginalItemPM.Weight != value) {
            this.OriginalItemPM.Weight = value;
        }
    }

    get MarksAndNumbers() { return this.OriginalItemPM.MarksAndNumbers; }
    set MarksAndNumbers(value: string) {
        if (this.OriginalItemPM.MarksAndNumbers != value) {
            this.OriginalItemPM.MarksAndNumbers = value;
        }
    }

    //#endregion

    //#region Ok, Cancel Buttons Handler
    hasDash: boolean = false;
    OkButtonClicked() {

        var isValid = this.ValidateCustomsItemField();
        if (!isValid)
            return;

        this.TaxExemptCode = this.CustomsItemTextValue;


        var errors = [];
        this.ValidationErrorsList = errors;
        errors = this.declarationValidator.ValidateSupplierInvoiceItem(this.OriginalItemPM);

        if (this.StatisticQuantity != null || this.AdditionalQuantity != null || this.CustomsBookTypeCode != null || this.PreferenceDocumentNumber != null || this.ActualInvoiceLines != null
            || this.DeferredCustomsTax != null || this.DeferredPurchaseTax != null || this.SalesTaxExemptionTypeCode != null || this.TaxExemptCode != null || this.OptionalTamaPercentage != null
            || this.NonCustomsItemPrice != null || this.WholeSaleItemPrice != null || this.IsUsed || this.ManufactureIdentifier != null || this.DangerousClassificationCode != null || this.DangerousPackingGroupTypeCode != null || this.ClaimReasonCode ||this.TransactionNatureCode
            || this.OriginalItemPM.SupplierInvoiceItemsMods.length > 0 || this.OriginalItemPM.SupplierInvoiceItemProcesTypes.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsConDeclars.length > 0 || this.OriginalItemPM.SupplierInvoiceItemLevies.length > 0
            || this.OriginalItemPM.SupplierInvoiceItemsDescripts.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsSerialNums.length > 0 || this.OriginalItemPM.SupplierInvoiceItemsProdIdents.length > 0) {

            this.OriginalItemPM.ItemAdditionalStatus = true;

        }
        else {
            this.OriginalItemPM.ItemAdditionalStatus = false;

        }
 
        //console.log("Ok, New -> ", this.ClonedItemPM)
        //console.log("    Old -> ", this.OldItemPM)

        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
        } else {
            this.CurrentSession.CloseCurrentWindow();
        }
    }
    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
        this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E" ? this.supplierInvoiceSharedService.DifferenceAndTotalForeignCurrency$.next() : '';
    }

    digit: string = null;
    checkDigit: number = 0;
    ValidateCustomsItemField(text: string = null) {

        this.CustomItemErrorMessage = null;

        var valid: boolean = true;

        if (!this.CustomsItemTextValue && this.CustomsItem) // set the value when entering the window
            this.CustomsItemTextValue = this.CustomsItem;

        if (text) {
            this.CustomsItemTextValue = text;
            this.CustomsItem = text;
        }

        var value: string = this.CustomsItemTextValue;
        if (value) {
            this.hasDash = value.includes("-");
            value = value.replace("-", "");
        }



        if (AppTool.IsNullOrEmpty(value)) {
            this.CustomItemErrorMessage = null;
        }
        else if (value.toString().length > 12) {
            this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CodeLong");
            valid = false;
        }
        else if (value.toString().length < 8) {
            this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CodeShort");
            valid = false;
        }
        else if (value.toString().length == 8) {
            value = value + "00";

            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + this.checkDigit;
        }
        else if (value.toString().length == 9) {
            this.digit = value.toString().substring(8);

            value = value.toString().substring(0, 8) + "00" + value.toString().substring(8);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value.substring(0, 10));

            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {
            }

        }
        else if (value.toString().length == 10) {
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value);
            if (this.checkDigit)
                value = value + "" + this.checkDigit;

        }
        else if (value.toString().length == 11) {
            this.digit = value.toString().substring(10);
            this.checkDigit = LuhnAlgorithm.CalculateLuhnAlgorithm(value.toString().substring(0, 10));
            if (this.digit != this.checkDigit.toString()) {
                this.CustomItemErrorMessage = TextCodeTranslator.Translate("Customs.Declaration.O.CorrectDigit") + this.checkDigit.toString();
                valid = false;
            }
            else {

            }


        }
        else {
            this.CustomItemErrorMessage = null;
        }

        this.CustomsItemTextValue = value;

        if (this.hasDash)
            this.CustomsItemTextValue = "-" + this.CustomsItemTextValue;

        return valid;
    }
    //#endregion

    //#region (Clone) Code

    CloneEntity(entityToClone: SupplierInvoiceItemPM) {

        var clonedEntity: SupplierInvoiceItemPM;
        clonedEntity = new SupplierInvoiceItemPM(entityToClone.EntityParentPM); // check it !!

        this.MapEntitytoEntity(entityToClone, clonedEntity);
       
        // --------------------------[ Arrays ]------------------------------
        // Modification
        clonedEntity.SupplierInvoiceItemsMods = [];
        entityToClone.SupplierInvoiceItemsMods.forEach((itemMod) => {
            var clonedItemMod = new SupplierInvoiceItemsModPM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvoiceItemsMods.push(clonedItemMod);
        });

        //Process Types
        clonedEntity.SupplierInvoiceItemProcesTypes = [];
        entityToClone.SupplierInvoiceItemProcesTypes.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemProcesTypePM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemProcesTypes.push(clonedItem);
        });

        // ConDeclar List 
        clonedEntity.SupplierInvoiceItemsConDeclars = [];
        entityToClone.SupplierInvoiceItemsConDeclars.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemsConDeclarPM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsConDeclars.push(clonedItem);
        });

        // SerialNumbers List 
        clonedEntity.SupplierInvoiceItemsSerialNums = [];
        entityToClone.SupplierInvoiceItemsSerialNums.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemsSerialNumPM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsSerialNums.push(clonedItem);
        });

        // Descriptions List 
        clonedEntity.SupplierInvoiceItemsDescripts = [];
        entityToClone.SupplierInvoiceItemsDescripts.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemsDescriptPM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsDescripts.push(clonedItem);
        });

        // Identifications List 
        clonedEntity.SupplierInvoiceItemsProdIdents = [];
        entityToClone.SupplierInvoiceItemsProdIdents.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemsProdIdentPM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemsProdIdents.push(clonedItem);
        });

        // Levies List 
        clonedEntity.SupplierInvoiceItemLevies = [];
        entityToClone.SupplierInvoiceItemLevies.forEach((item) => {
            var clonedItem = new SupplierInvoiceItemsLevyPM(item.EntityParentPM);
            this.MapEntitytoEntity(item, clonedItem);
            clonedEntity.SupplierInvoiceItemLevies.push(clonedItem);
        });
        // Prices List
        clonedEntity.SupplierInvoiceItemsPrices = []; 
        entityToClone.SupplierInvoiceItemsPrices.forEach((itemMod) => {
            var clonedItemMod = new SupplierInvoiceItemsPricePM(itemMod.EntityParentPM);
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SupplierInvoiceItemsPrices.push(clonedItemMod);
        });
        // AbachStatements List
        clonedEntity.SuppInvoiceItemsAbachStatements = []; 
        entityToClone.SuppInvoiceItemsAbachStatements.forEach((itemMod) => {
            var clonedItemMod = new SuppInvoiceItemsAbachStatementPM(itemMod.EntityParentPM); 
            this.MapEntitytoEntity(itemMod, clonedItemMod);
            clonedEntity.SuppInvoiceItemsAbachStatements.push(clonedItemMod);
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

    //#endregion

    getMax(list: any[], propertyName: string) {
        var max = -99999;
        var maxObj = list ? list.reduce(function (prev, current) { return (prev[propertyName] > current[propertyName]) ? prev : current }) : null;
        if (maxObj != null)
            if (max <= maxObj[propertyName])
                max = maxObj[propertyName];
        return max;
    }

    OpenExporterInvoiceItem() {


        if (!this.IsDisplayOnly) {
            var windowArgs: any = {};
            windowArgs.SupplierInvoiceItem = this.OriginalItemPM;

            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            var windowTitle = "נתונים נוספים ליצוא - שורת חשבון יצואן";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 400;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/ExporterInvoiceItemComponent');
        }


    }


    EditActualLines() {


        if (!this.IsDisplayOnly) {
            var windowArgs: any = {};
            windowArgs.SupplierInvoiceItemPM = this.OriginalItemPM;

            windowArgs.IsDisplayOnly = this.IsDisplayOnly;
            windowArgs.ActualInvoiceLines = this.ActualInvoiceLines;
            var windowTitle = "שורות חשבון בפועל";

            var logWindow = new LogitudeWindow();
            logWindow.Width = 700;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.ShowCloseButton = false;
            logWindow.WindowArgs = windowArgs;
            logWindow.WindowClosed.subscribe(($event: any) => this.UpdateActualInvoiceLines($event));

            logWindow.Show('./CustomsModules/CustomsDeclarationModules/DeclarationSupplierInvoice/Components/SupplierInvoices/SupplierInvoiceItem/AddEditActualLinesComponent');
        }


    }

    UpdateActualInvoiceLines(data: string) {
        if (data != "cancel") {
            this.ActualInvoiceLines = data;
        }
    }

    OnEscHotKeyPressed() {
        this.CancelButtonClicked();
    }

    OnCTRL_S_HotKeyPressed() {
        this.OkButtonClicked();
    }
}


export class AbachItemModel extends BaseComponent {

    public AbachPM: SuppInvoiceItemsAbachStatementPM = null;
    public ObjectTableName = "Customs.SuppInvoiceItemsAbachStatement";
    public DataContext = this;

    constructor(private _abachPM: SuppInvoiceItemsAbachStatementPM) {
        super();
        this.AbachPM = _abachPM;
    }

    get StatementTypeCode() { return this.AbachPM.StatementTypeCode; }
    set StatementTypeCode(value: string) {
        if (this.AbachPM.StatementTypeCode != value) {
            this.AbachPM.StatementTypeCode = value;

        }
    }

    get SequenceNumeric() { return this.AbachPM.SequenceNumeric; }
    set SequenceNumeric(value: number) {
        if (this.AbachPM.SequenceNumeric != value) {
            this.AbachPM.SequenceNumeric = value;

        }
    }


    get StatementTypeName() { return this.AbachPM.StatementTypeName; }
    set StatementTypeName(value: string) {
        if (this.AbachPM.StatementTypeName != value) {
            this.AbachPM.StatementTypeName = value;

        }
    }

    get IsStatementInd() { return this.AbachPM.IsStatementInd; }
    set IsStatementInd(value: boolean) {
        if (this.AbachPM.IsStatementInd != value) {
            this.AbachPM.IsStatementInd = value;

        }
    }



    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}


export class PriceItemModel extends BaseComponent {

    public PricePM: SupplierInvoiceItemsPricePM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsPrice";
    public DataContext = this;

    constructor(private _pricePM: SupplierInvoiceItemsPricePM) {
        super();
        this.PricePM = _pricePM;
    }

    get AdditionalPrice() { return this.PricePM.AdditionalPrice; }
    set AdditionalPrice(value: number) {
        if (this.PricePM.AdditionalPrice != value) {
            this.PricePM.AdditionalPrice = value;

        }
    }

    get LineNumber() { return this.PricePM.LineNumber; }
    set LineNumber(value: number) {
        if (this.PricePM.LineNumber != value) {
            this.PricePM.LineNumber = value;

        }
    }

    get AdditionalPriceTypeCode() { return this.PricePM.AdditionalPriceTypeCode; }
    set AdditionalPriceTypeCode(value: string) {
        if (this.PricePM.AdditionalPriceTypeCode != value) {
            this.PricePM.AdditionalPriceTypeCode = value;

        }
    }

    get AdditionalPriceTypeName() { return this.PricePM.AdditionalPriceTypeName; }
    set AdditionalPriceTypeName(value: string) {
        if (this.PricePM.AdditionalPriceTypeName != value) {
            this.PricePM.AdditionalPriceTypeName = value;

        }
    }



    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

// Details Tab
export class ModificationItemModel extends BaseComponent {
    public ModificationPM: SupplierInvoiceItemsModPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsMod";
    public DataContext = this;
    public CurrentSession = SessionLocator.SelectedSession;

    constructor(private modificationPM: SupplierInvoiceItemsModPM ,private supplierInvoiceSharedService:SupplierInvoiceSharedService) {
        super();
        this.ModificationPM = modificationPM;
    }



    //#region Properties

    get TypeCode() { return this.ModificationPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.ModificationPM.TypeCode != value) {
            this.ModificationPM.TypeCode = value;

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

        }
    }


    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
    OnTypeCodeLostFocus(){
        if(this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E" && this.Amount !=null){
            this.supplierInvoiceSharedService.DifferenceAndTotalForeignCurrency$.next()
        }

    }
     
    OnAmountOrCurrencyLostFocus(){
 
        this.CurrentSession.CurrentEditComponent.EntityPM.Direction == "E" ? this.supplierInvoiceSharedService.DifferenceAndTotalForeignCurrency$.next() : '';

    }
}
export class ProcessTypeItemModel extends BaseComponent {
    public ProcessTypePM: SupplierInvoiceItemProcesTypePM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemProcesType";
    public DataContext = this;

    constructor(private processTypePM: SupplierInvoiceItemProcesTypePM) {
        super();
        this.ProcessTypePM = processTypePM;
    }

    //#region Properties

    get ProcessTypeCode() { return this.ProcessTypePM.ProcessTypeCode; }
    set ProcessTypeCode(value: string) {
        if (this.ProcessTypePM.ProcessTypeCode != value) {
            this.ProcessTypePM.ProcessTypeCode = value;

        }
    }

    get ProcessTypeName() { return this.ProcessTypePM.ProcessTypeName; }
    set ProcessTypeName(value: string) {
        if (this.ProcessTypePM.ProcessTypeName != value) {
            this.ProcessTypePM.ProcessTypeName = value;

        }
    }

    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

// Declaration Tab
export class ConDeclarItemModel extends BaseComponent { // connected declaration
    public ConnDeclarPM: SupplierInvoiceItemsConDeclarPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsConDeclar";
    public DataContext = this;

    constructor(private connDeclar: SupplierInvoiceItemsConDeclarPM) {
        super();
        this.ConnDeclarPM = connDeclar;
    }

    //#region Properties

    get DeclarationTypeCode() { return this.ConnDeclarPM.DeclarationTypeCode; }
    set DeclarationTypeCode(value: string) {
        if (this.ConnDeclarPM.DeclarationTypeCode != value) {
            this.ConnDeclarPM.DeclarationTypeCode = value;

        }
    }


    get DeclarationTypeName() { return this.ConnDeclarPM.DeclarationTypeName; }
    set DeclarationTypeName(value: string) {
        if (this.ConnDeclarPM.DeclarationTypeName != value) {
            this.ConnDeclarPM.DeclarationTypeName = value;

        }
    }

    get DeclarationNumber() { return this.ConnDeclarPM.DeclarationNumber; }
    set DeclarationNumber(value: string) {
        if (this.ConnDeclarPM.DeclarationNumber != value) {
            this.ConnDeclarPM.DeclarationNumber = value;

        }
    }

    get InvoiceNumber() { return this.ConnDeclarPM.InvoiceNumber; }
    set InvoiceNumber(value: number) {
        if (this.ConnDeclarPM.InvoiceNumber != value) {
            this.ConnDeclarPM.InvoiceNumber = value;

        }
    }

    get ItemSequence() { return this.ConnDeclarPM.ItemSequence; }
    set ItemSequence(value: number) {
        if (this.ConnDeclarPM.ItemSequence != value) {
            this.ConnDeclarPM.ItemSequence = value;

        }
    }

    get Quantity() { return this.ConnDeclarPM.Quantity; }
    set Quantity(value: number) {
        if (this.ConnDeclarPM.Quantity != value) {
            this.ConnDeclarPM.Quantity = value;

        }
    }

    get QuantityTypeCode() { return this.ConnDeclarPM.QuantityTypeCode; }
    set QuantityTypeCode(value: string) {
        if (this.ConnDeclarPM.QuantityTypeCode != value) {
            this.ConnDeclarPM.QuantityTypeCode = value;

        }
    }

    get QuantityTypeName() { return this.ConnDeclarPM.QuantityTypeName; }
    set QuantityTypeName(value: string) {
        if (this.ConnDeclarPM.QuantityTypeName != value) {
            this.ConnDeclarPM.QuantityTypeName = value;

        }
    }


    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}

// Serial Numbers Tab
export class SerialNoItemModel extends BaseComponent {
    public SerialNoPM: SupplierInvoiceItemsSerialNumPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsSerialNum";
    public DataContext = this;

    constructor(private entity: SupplierInvoiceItemsSerialNumPM) {
        super();
        this.SerialNoPM = entity;
    }

    //#region Properties
    get TypeCode() { return this.SerialNoPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.SerialNoPM.TypeCode != value) {
            this.SerialNoPM.TypeCode = value;

        }
    }

    get TypeName() { return this.SerialNoPM.TypeName; }
    set TypeName(value: string) {
        if (this.SerialNoPM.TypeName != value) {
            this.SerialNoPM.TypeName = value;

        }
    }

    get SerialNumber() { return this.SerialNoPM.SerialNumber; }
    set SerialNumber(value: string) {
        if (this.SerialNoPM.SerialNumber != value) {
            this.SerialNoPM.SerialNumber = value;

        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

}
export class DescribtionItemModel extends BaseComponent {
    public DescribtionPM: SupplierInvoiceItemsDescriptPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsDescript";
    public DataContext = this;

    constructor(private entity: SupplierInvoiceItemsDescriptPM) {
        super();
        this.DescribtionPM = entity;
    }

    //#region Properties
    get TypeCode() { return this.DescribtionPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.DescribtionPM.TypeCode != value) {
            this.DescribtionPM.TypeCode = value;

        }
    }

    get TypeName() { return this.DescribtionPM.TypeName; }
    set TypeName(value: string) {
        if (this.DescribtionPM.TypeName != value) {
            this.DescribtionPM.TypeName = value;

        }
    }

    get Description() { return this.DescribtionPM.Description; }
    set Description(value: string) {
        if (this.DescribtionPM.Description != value) {
            this.DescribtionPM.Description = value;

        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }
}
export class ProdIdentItemModel extends BaseComponent {
    public ProdIdentPM: SupplierInvoiceItemsProdIdentPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsProdIdent";
    public DataContext = this;

    constructor(private entity: SupplierInvoiceItemsProdIdentPM) {
        super();
        this.ProdIdentPM = entity;
    }

    //#region Properties
    get TypeCode() { return this.ProdIdentPM.TypeCode; }
    set TypeCode(value: string) {
        if (this.ProdIdentPM.TypeCode != value) {
            this.ProdIdentPM.TypeCode = value;

        }
    }

    get TypeName() { return this.ProdIdentPM.TypeName; }
    set TypeName(value: string) {
        if (this.ProdIdentPM.TypeName != value) {
            this.ProdIdentPM.TypeName = value;

        }
    }

    get Identification() { return this.ProdIdentPM.Identification; }
    set Identification(value: string) {
        if (this.ProdIdentPM.Identification != value) {
            this.ProdIdentPM.Identification = value;

        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }

}

export class LevyItemModel extends BaseComponent {
    public LevyPM: SupplierInvoiceItemsLevyPM = null;
    public ObjectTableName = "Customs.SupplierInvoiceItemsLevy";
    public DataContext = this;

    constructor(private entity: SupplierInvoiceItemsLevyPM) {
        super();
        this.LevyPM = entity;
    }

    //#region Properties
    get TradeLevyExamptCode() { return this.LevyPM.TradeLevyExamptCode; }
    set TradeLevyExamptCode(value: string) {
        if (this.LevyPM.TradeLevyExamptCode != value) {
            this.LevyPM.TradeLevyExamptCode = value;

        }
    }

    get TradeLevyExamptName() { return this.LevyPM.TradeLevyExamptName; }
    set TradeLevyExamptName(value: string) {
        if (this.LevyPM.TradeLevyExamptName != value) {
            this.LevyPM.TradeLevyExamptName = value;

        }
    }

    get TradeLevyNumber() { return this.LevyPM.TradeLevyNumber; }
    set TradeLevyNumber(value: string) {
        if (this.LevyPM.TradeLevyNumber != value) {
            this.LevyPM.TradeLevyNumber = value;

        }
    }
    //#endregion

    SetLocalName(entity, fieldName) {
        if (!AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        } else {
            this[fieldName] = null;
        }

    }


}
