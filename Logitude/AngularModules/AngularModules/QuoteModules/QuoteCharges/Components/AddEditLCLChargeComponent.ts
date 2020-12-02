import { Component, OnDestroy} from '@angular/core';
import {AppTool,} from '../../../Infrastructure/Tools';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {QuoteChargeItem} from './LCLChargesComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteChargePM} from '../../../Quote/EntityPMs/QuoteChargePM';
import {QuotePriceStepsPM} from '../../../Quote/EntityPMs/QuotePriceStepsPM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {VatTypesValidator} from '../../../Infrastructure/Validators/VatTypesValidator';
import { QuoteValidator } from '../../../Quote/Validators/QuoteValidator';
import { PriceStepList } from '../../../Infrastructure/EntityLists/PriceStepList';
import { MeasurementList } from '../../../Common/EntityLists/MeasurementList';
import { CommonTool } from '../../../Common/Tools';

@Component({
    
    templateUrl: './AddEditLCLChargeComponent.html',
})

export class AddEditLCLChargeComponent extends BaseComponent implements OnDestroy {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;
    public DataContext: QuoteChargeItem;
    public DataContext2 = this;
    public Father: any;
    public IsAdhoc: boolean = false;
    public IsRoutingRate: boolean = false;
    public IsEditingEnabled: boolean = false;
    public ObjectTableName: string = "QuoteCharge";
    public QuotePriceObjectTableName: string = "QuotePriceSteps";
    public ItemsSource: ObservableCollection;
    public StepsItemsSource: ObservableCollection;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public MeasurementsQueryFilters: ApiQueryFilters;
    public IsVATVisible: boolean = false;
    public IsRegionalTaxVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    public CheckChargeTypeDuplicationFlag: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;    
    private IsHyprid: boolean;
    private ChargesTypeCode: string;
    private PropertyChangedEvent: any = null;

    constructor() {
        super();

        this.ItemsSource = new ObservableCollection([]);
        this.StepsItemsSource = new ObservableCollection([]);
        this.CurrentSession.SessionEvent.subscribe((res) => {
            if (res == "AddDefaultPriceStep") {
                if (this.StepsItemsSource.Length == 0) {
                    this.AddStepItemMethod();
                }
            }

            if (res == "CostMeasurementIdChanged") {
                this.StepsItemsSource.Collection.forEach(item => {
                    item.SetWeightUnitCode();
                });
            }
        });
        this.IsHyprid = SessionLocator.TenantPM.IsHybrid;
    }
    private propertiesChanges = [];
    private ListenPropertyChanged() {

        if (this.PropertyChangedEvent) {
            AppTool.KillEventEmitter(this.PropertyChangedEvent);
            this.PropertyChangedEvent = null;
        }

        this.PropertyChangedEvent = this.EntityPM.PropertyChanged.subscribe(s => {
            if (s) {
                this.propertiesChanges.push(s.PropertyName);
            }
        });
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.PropertyChangedEvent);
    }

    SetDataContext(dataContext: QuoteChargeItem) {
        this.QuotePM = dataContext.QuotePM;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.Father = this.DataContext.fatherComponent;
        this.IsAdhoc = this.DataContext.fatherComponent.IsAdhoc;
        this.IsRoutingRate = this.DataContext.fatherComponent.IsRoutingRate;
        this.IsEditingEnabled = this.DataContext.fatherComponent.IsEditingEnabled;
        this.IsVATVisible = this.IsAdhoc && this.QuotePM.IsChargesByVAT ? true : false;
        this.IsRegionalTaxVisible = this.IsVATVisible && this.Father.IsRegionalTaxVisible ? true : false;
        this.ChargesTypeCode = this.EntityPM.ChargesTypeCode;

        this.SetUIProperties();
        this.DataContext.SetUIProperties();
        this.BuildItemsSource();
        this.BuildQueryFilters();
        this.BuildStepItemsSource();
        this.Clone();
        this.ListenPropertyChanged();
    }

    public IsAddBreaksEnabled: boolean = false;
    SetUIProperties() {
        var isAddBreaksEnabled = false;

        if (this.IsEditingEnabled) {
            if (!AppTool.IsNullOrEmpty(this.SelectedPriceStepId)) {
                isAddBreaksEnabled = true;
            }
        }

        this.IsAddBreaksEnabled = isAddBreaksEnabled;
    }

    BuildItemsSource() {
        this.ItemsSource.Insert(this.DataContext);
    }
    BuildQueryFilters() {
        this.MeasurementsQueryFilters = new ApiQueryFilters();
        this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "NotContains", false, false, false, "string", false, true, true);

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");

        switch (this.DataContext.QuotePM.TransportModeId) {
            case "A": {
                this.ChargeTypesQueryFilters.addAdditionalFilter("IsAir", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }

            case "O": {
                this.ChargeTypesQueryFilters.addAdditionalFilter("IsOcean", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }

            case "I": {
                this.ChargeTypesQueryFilters.addAdditionalFilter("IsInland", true, null, null, "Equals", false, false, false, "Boolean");
                break;
            }
        }
        CommonTool.FilterChargeTypesByDirection(this.ChargeTypesQueryFilters, this.DataContext.QuotePM.DirectionId); 
    }
    BuildStepItemsSource() {
        if (this.StepsItemsSource == null) {
            this.StepsItemsSource = new ObservableCollection([]);
        }
        else {
            this.StepsItemsSource.Collection.forEach(item => {
                this.StepsItemsSource.Clear();
            });
        }

        var itemsCollection: QuoteStepItem[] = [];

        this.EntityPM.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step }).forEach((item) => {
            itemsCollection.push(new QuoteStepItem(item, this, false));
        });

        this.StepsItemsSource.InsertCollection(itemsCollection);
    }

    public SelectedRow: QuoteChargeItem = null;
    OnRowSelected(itemComponent: QuoteChargeItem) {
        this.SelectedRow = itemComponent;
    }

    public SelectedStepItem: QuoteStepItem;
    SelectingPriceItem(item: QuoteStepItem) {
        this.SelectedStepItem = item;   
    }

    AddStepItemMethod() {
        var newItem: QuotePriceStepsPM = new QuotePriceStepsPM(null);
        newItem.Tenant = SessionLocator.Tenant;
        newItem.QuoteId = this.EntityPM.Id;
        newItem.MarkupValue = this.EntityPM.MarkUpValue;
        newItem.QuoteChargeId = this.EntityPM.Id;
        this.StepsItemsSource.Insert(new QuoteStepItem(newItem, this, true));
    }

    AddStepClicked() {
        this.AddStepItemMethod();
    }
    EditStepClicked() {
        if (this.SelectedStepItem != null) {
            this.RunAddEditStep(this.SelectedStepItem, "Edit Price Break");
        }
    }
    RunAddEditStep(itemComponent: QuoteStepItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 350;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditPriceStepComponent');
    }
    DeleteStepClicked(item: QuoteStepItem) {
        if (this.DataContext.EntityPM.QuoteChargePriceSteps.indexOf(item.EntityPM) != -1) {
            this.DataContext.EntityPM.RemoveQuotePriceStepsPM(item.EntityPM);
        }

        if (this.StepsItemsSource.Collection.indexOf(item) != -1) {
            this.StepsItemsSource.Remove(item);
        }
    }
    OnRowEnded($event) {
        var errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (($event) == this.StepsItemsSource.Length) {
            this.AddStepItemMethod();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    CheckChargeTypeDuplication() {
        if (!this.DataContext.IsNew && (this.ChargesTypeCode != this.EntityPM.ChargesTypeCode)) {
            this.CheckChargeTypeDuplicationFlag = true;
        }
        if (this.DataContext.IsNew) {
            this.CheckChargeTypeDuplicationFlag = true;
        }
    }

  OkButtonClicked() {
    var errors: string[] = [];
    Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

    var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

    this.CheckChargeTypeDuplication();

    if (this.IsHyprid && this.CheckChargeTypeDuplicationFlag) {
      var quoteValidator: QuoteValidator = new QuoteValidator();
      quoteValidator.CheckDuplicateInCharges(this.QuotePM, this.EntityPM, errors);
    }

    if (this.DataContext.IsChargeBySteps) {
      this.StepsItemsSource.Collection.forEach(priceStep => {

        Validator.TryValidateObject(priceStep, this.QuotePriceObjectTableName, errors);

        if (AppTool.IsNullOrEmpty(priceStep.SaleUnitPrice)) {
          errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("QuotePriceSteps.F.SaleUnitPrice")));
        }

        this.DataContext.EntityPM.QuoteChargePriceSteps.filter(d => d.Step != null && d.Step == priceStep.Step).forEach((item) => {
          if (item != priceStep.EntityPM) {
            errors.push("Price Steps list already contains Step: " + AppTool.Round(priceStep.Step, 2));
          }
        });


        var duplicates = this.StepsItemsSource.Collection.filter(d => d.Step != null && d.Step == priceStep.Step);
        if (duplicates && duplicates.length > 1) {
          errors.push("Price Steps list already contains Step: " + AppTool.Round(priceStep.Step, 2));
        }
      });
    }

    if (this.EntityPM.ChargesGroupCode == "FRT") {
      if (this.DataContext.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT" && d != this.EntityPM).length > 0) {
        errors.push("Freight Charge already added");
      }
    }

    if (this.QuotePM.IsChargesByVAT) {
      if (!AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {

        if (this.EntityPM.VatIsMultiPercentage) {
          if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
            errors.push(VatTypesValidator.GetError());
          }
        }

        else {
          if (AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
            var field = TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
            errors.push(msg.replace("%FieldName", field));
          }
        }
      }
    }

    var freightLineCostCurrencyId: string = "";
    var freightLineSaleCurrencyId: string = "";
    if (this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT" && d != this.EntityPM).length > 0) {
      var quoteCharge = this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT" && d.Id != this.EntityPM.Id)[0];
      if (quoteCharge) {
        freightLineCostCurrencyId = quoteCharge.CostCurrencyId;
        freightLineSaleCurrencyId = quoteCharge.SaleCurrencyId;
      }
    }

    if (!AppTool.IsNullOrEmpty(this.EntityPM.CostMeasurementCode)) {
        if (this.EntityPM.CostMeasurementCode == "PRFR" && !AppTool.IsNullOrEmpty(this.EntityPM.CostCurrencyId) && !AppTool.IsNullOrEmpty(freightLineCostCurrencyId)) {
            if (!AppTool.IsNullOrZero(this.EntityPM.CostTotalAmount)) {
                if (this.EntityPM.CostCurrencyId != freightLineCostCurrencyId) {
                    errors.push("Cost currency must be the same as the freight currency in the case of Percent of Freight");
                }
            }
      }
    }

    if (!AppTool.IsNullOrEmpty(this.EntityPM.SaleMeasurementCode)) {
        if (this.EntityPM.SaleMeasurementCode == "PRFR" && !AppTool.IsNullOrEmpty(this.EntityPM.SaleCurrencyId) && !AppTool.IsNullOrEmpty(freightLineSaleCurrencyId)) {
            if (!AppTool.IsNullOrZero(this.EntityPM.SaleTotalAmount)) {
                if (this.EntityPM.SaleCurrencyId != freightLineSaleCurrencyId) {
                    errors.push("Sale currency must be the same as the freight currency in the case of Percent of Freight");
                }
            }
      }
    }

    this.ValidationErrorsList = errors;

    if (errors.length == 0) {
      if (this.DataContext.CostMeasurementCode == "FIXD" && this.DataContext.SaleMeasurementCode == "FIXD" && this.DataContext.IsChargeBySteps) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Steps will be erased since the UOM is fixed");
        confirmWindow.WindowClosed.subscribe((event: any) => {
          if (confirmWindow.Yes) {
            this.DataContext.IsChargeBySteps = false;
            this.FinishOkButton();
          }
        });
      }

      else {
        this.FinishOkButton();
      }
    }
  }

    FinishOkButton() {
        this.StepsItemsSource.Collection.forEach(item => {
            if (item != null) {
                if (item.IsNew) {
                    if (this.DataContext.EntityPM.QuoteChargePriceSteps.indexOf(item.EntityPM) == -1) {
                        item.IsNewEntity = false;
                        this.DataContext.EntityPM.AddQuotePriceStepsPM(item.EntityPM);
                    }
                }
            }
        });

        if (this.DataContext.IsNew) {
            this.DataContext.QuotePM.AddQuoteChargePM(this.EntityPM);
            this.DataContext.fatherComponent.BuildItemsSource();
        }

        if (!this.DataContext.IsChargeBySteps) {
            if (this.EntityPM.QuoteChargePriceSteps.length > 0) {
                this.EntityPM.QuoteChargePriceSteps = [];
            }
        }


        if (!AppTool.IsNullOrEmpty(this.DataContext.TariffId) && this.EntityPM.IsDirty && !this.DataContext.IsNew) {
            var property = this.propertiesChanges.filter(a => a == "CostUnitPrice" || a == "CostTotalAmount" || a == "CostCurrencyId")[0];
            if (property) {
                this.ShowTariffDisconnectionWindow();
            }
            else {
                this.DataContext.BuildPriceBreaksTooltips();
                this.DataContext.fatherComponent.ComputeTotals();
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }

        else {
            this.DataContext.BuildPriceBreaksTooltips();
            this.DataContext.fatherComponent.ComputeTotals();
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    private myCloner: Cloner;
    private oldPriceSteps: QuotePriceStepsPM[] = [];
    private Clone() {
        this.EntityPM.QuoteChargePriceSteps.forEach((item) => {
            var stepItem: QuotePriceStepsPM = new QuotePriceStepsPM(null);
            stepItem.Id = item.Id;
            stepItem.QuoteId = this.DataContext.QuotePM.Id;
            stepItem.QuoteChargeId = this.EntityPM.Id;
            stepItem.Step = item.Step;
            stepItem.CostUnitPrice = item.CostUnitPrice;
            stepItem.SaleUnitPrice = item.SaleUnitPrice;
            stepItem.MarkupValue = item.MarkupValue;
            this.oldPriceSteps.push(stepItem);
        });

        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('VendorId');
        this.myCloner.AddField('CostMeasurementId');
        this.myCloner.AddField('SaleMeasurementId');
        this.myCloner.AddField('CostCurrencyId');
        this.myCloner.AddField('CostExchangeRate');
        this.myCloner.AddField('CostIsFixedRate');
        this.myCloner.AddField('IsChargeBySteps');
        this.myCloner.AddField('CostQuantity');
        this.myCloner.AddField('CostUnitPrice');
        this.myCloner.AddField('CostTotalAmount');
        this.myCloner.AddField('SaleQuantity');
        this.myCloner.AddField('SaleUnitPrice');
        this.myCloner.AddField('MarkUpValue');
        this.myCloner.AddField('SaleTotalAmount');
        this.myCloner.AddField('SaleTotalAmountLocal');
        this.myCloner.AddField('IsAllIN');
        this.myCloner.AddField('Notes');
        this.myCloner.AddField('VatTypeId');
        this.myCloner.AddField('VatPercentage');
        this.myCloner.AddField('CostMinAmount');
        this.myCloner.AddField('CostMaxAmount');
        this.myCloner.AddField('SaleMinAmount');
        this.myCloner.AddField('SaleMaxAmount');
        this.myCloner.AddField('TariffId');
        this.myCloner.AddField('IsRegionalTax');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuotePM);
    }
    private RejectChanges() {

        var addedItems: any[] = [];
        var removedItems: any[] = [];

        this.oldPriceSteps.forEach(item => {
            var existingItem = this.EntityPM.QuoteChargePriceSteps.filter(f => f == item)[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });

        this.EntityPM.QuoteChargePriceSteps.forEach(item => {
            var oldItem = this.oldPriceSteps.filter(f => f == item)[0];
            if (oldItem) {
                if (item.Step != oldItem.Step) {
                    item.Step = oldItem.Step;
                }

                if (item.MarkupValue != oldItem.MarkupValue) {
                    item.MarkupValue = oldItem.MarkupValue;
                }

                if (item.CostUnitPrice != oldItem.CostUnitPrice) {
                    item.CostUnitPrice = oldItem.CostUnitPrice;
                }

                if (item.SaleUnitPrice != oldItem.SaleUnitPrice) {
                    item.SaleUnitPrice = oldItem.SaleUnitPrice;
                }
            }

            else {
                addedItems.push(item);
            }
        });

        addedItems.forEach(item => {
            this.EntityPM.RemoveQuotePriceStepsPM(item);
        });

        removedItems.forEach(item => {
            this.EntityPM.AddQuotePriceStepsPM(item);
        });

        this.myCloner.RejectChanges();
    }

    private selectedPriceStepId: string = null;
    public get SelectedPriceStepId() { return this.selectedPriceStepId; }
    public set SelectedPriceStepId(value: string) {
        if (this.selectedPriceStepId != value) {
            this.selectedPriceStepId = value;
            this.SetUIProperties();
        }
    }

    private selectedPriceStepList: PriceStepList = null;
    public get SelectedPriceStepList() { return this.selectedPriceStepList; }
    public set SelectedPriceStepList(value: PriceStepList) {
        if (this.selectedPriceStepList != value) {
            this.selectedPriceStepList = value;
        }
    }

    AddBreaksClicked() {
        if (this.SelectedPriceStepList) {
            if (this.StepsItemsSource.Collection.filter(f => !AppTool.IsNullOrZero(f.CostUnitPrice) || !AppTool.IsNullOrZero(f.SaleUnitPrice)).length > 0) {
                var messageWindow = new MessageWindow();
                messageWindow.Show("Can't use default breaks when you have added breaks, please delete first");
            }

            else {
                this.StepsItemsSource.Clear();

                var steps: string[] = this.SelectedPriceStepList.Steps.split(',');

                steps.forEach((step: string) => {
                    var newItem: QuotePriceStepsPM = new QuotePriceStepsPM(null);
                    newItem.Tenant = SessionLocator.Tenant;
                    newItem.QuoteId = this.EntityPM.Id;
                    newItem.Step = +step;
                    newItem.QuoteChargeId = this.EntityPM.Id;
                    this.StepsItemsSource.Insert(new QuoteStepItem(newItem, this, true));
                });

                this.SelectedPriceStepId = null;
                this.SelectedPriceStepList = null;

                //var logitudeWindow = new LogitudeWindow();
                //logitudeWindow.Title = "Select Price Breaks";
                //logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/SelectBreaksComponent');

                //logitudeWindow.WindowClosed.subscribe(s => {
                //    if (s) {
                //        var steps: string[] = s.split(',');

                //        steps.forEach((step: string) => {
                //            var newItem: QuotePriceStepsPM = new QuotePriceStepsPM(null);
                //            newItem.Tenant = SessionLocator.Tenant;
                //            newItem.QuoteId = this.EntityPM.Id;
                //            newItem.Step = +step;
                //            newItem.QuoteChargeId = this.EntityPM.Id;
                //            this.StepsItemsSource.Insert(new QuoteStepItem(newItem, this, true));
                //        });
                //    }
                //});
            }
        }
    }

    ShowTariffDisconnectionWindow() {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Show("Editing this line will unlink it from the tariff it was generated from.");
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.DataContext.TariffId = null;
                this.DataContext.TariffNumber = null;
                //this.DataContext.SetUIProperties();
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            if (confirmWindow.No) {
                //nothing 
            }
        });
    }
}
export class QuoteStepItem extends BaseComponent {
    public EntityPM: QuotePriceStepsPM;
    public QuoteChargePM: QuoteChargePM;
    public ObjectTableName: string = "QuotePriceSteps";
    public IsNew: boolean = false;
    constructor(entity: QuotePriceStepsPM, public fatherComponent: AddEditLCLChargeComponent, isNew: boolean) {
        super();
        this.IsNew = isNew;
        this.EntityPM = entity;
        this.QuoteChargePM = fatherComponent.EntityPM;

        this.SetWeightUnitCode();
    }

    public WeightUnitCode: string;
    public SetWeightUnitCode() {
        var code: string;

        switch (this.QuoteChargePM.CostMeasurementCode) {
            case "GRWT":  { code = this.fatherComponent.DataContext.QuotePM.GrossWeightUnitCode; break; }
            case "CHWT": case "PDCW":{ code = this.fatherComponent.DataContext.QuotePM.ChargeableWeightUnitCode; break; }
            case "VOLU": { code = this.fatherComponent.DataContext.QuotePM.VolumeUnitCode; break; }
            case "BTEU": { code = "TEU"; break; }
            case "PRVL": { code = "Value of Goods" ; break; }
            case "PRFR": { code = "Freight Value"; break; }
            case "GWTN": { code = "Ton"; break; }
            case "QTY": { code = "Pieces"; break; }
            case "CWKG": { code = "KG"; break; }
            case "GWKG": { code = "KG"; break; }
            case "VCBM": { code = "CBM"; break; }
        }

        this.WeightUnitCode = code;
    }
    
    get MarkUpType() {
        var myResult = "";

        if (this.QuoteChargePM.MarkUpTypeCode == "P") {
            myResult = "Percentage(%)";
        }

        else {
            myResult = "Fixed";
            if (!AppTool.IsNullOrEmpty(this.QuoteChargePM.CostCurrencyCode)) {
                myResult = myResult + " (" + this.QuoteChargePM.CostCurrencyCode + ")";
            }
        }

        return myResult;
    }

    get Step() { return this.EntityPM.Step; }
    set Step(newValue: number) {
        var setValue = AppTool.Round(newValue, 2);

        if (this.EntityPM.Step != setValue) {
            this.EntityPM.Step = setValue;
        }
    }

    get CostUnitPrice() { return this.EntityPM.CostUnitPrice; }
    set CostUnitPrice(newValue: number) {
        var setValue = AppTool.Round(newValue, 3);

        if (this.EntityPM.CostUnitPrice != setValue) {
            this.EntityPM.CostUnitPrice = setValue;

            this.ComputeSaleUnitPrice();
        }
    }

    get MarkupValue() { return this.EntityPM.MarkupValue; }
    set MarkupValue(newValue: number) {
        var setValue = AppTool.Round(newValue, 3);

        if (this.EntityPM.MarkupValue != setValue) {
            this.EntityPM.MarkupValue = setValue;

            this.ComputeSaleUnitPrice();
        }
    }

    get SaleUnitPrice() { return this.EntityPM.SaleUnitPrice; }
    set SaleUnitPrice(newValue: number) {
        var setValue = AppTool.Round(newValue, 3);

        if (this.EntityPM.SaleUnitPrice != setValue) {
            this.EntityPM.SaleUnitPrice = setValue;

            this.ComputeMarkUp();
        }
    }

    private ComputeSaleUnitPrice() {
        var result = this.EntityPM.SaleUnitPrice;
        var markup = this.EntityPM.MarkupValue == null ? 0 : this.EntityPM.MarkupValue;

        if (this.EntityPM.CostUnitPrice != null) {
            if (this.QuoteChargePM.MarkUpTypeCode == "P") {
                result = this.EntityPM.CostUnitPrice + (this.EntityPM.CostUnitPrice * (markup / 100));
            }

            else {
                result = this.EntityPM.CostUnitPrice + markup;
            }
        }

        this.SaleUnitPrice = result;
    }

    private ComputeMarkUp() {
        var result = this.EntityPM.MarkupValue;

        if (this.EntityPM.CostUnitPrice != null && this.EntityPM.SaleUnitPrice != null) {
            if (this.QuoteChargePM.MarkUpTypeCode == "P") {
                result = ((this.EntityPM.SaleUnitPrice - this.EntityPM.CostUnitPrice) * 100) / this.EntityPM.CostUnitPrice;
            }

            else {
                result = this.EntityPM.SaleUnitPrice - this.EntityPM.CostUnitPrice;
            }
        }

        this.MarkupValue = result;
    }
}
