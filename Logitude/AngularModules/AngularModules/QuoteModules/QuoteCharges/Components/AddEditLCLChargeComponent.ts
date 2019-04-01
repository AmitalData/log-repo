import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {QuoteChargeItem} from './LCLChargesComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import {QuoteChargePM} from '../../../Quote/EntityPMs/QuoteChargePM';
import {QuotePriceStepsPM} from '../../../Quote/EntityPMs/QuotePriceStepsPM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {VatTypesValidator} from '../../../Infrastructure/Validators/VatTypesValidator';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditLCLChargeComponent.html',
})

export class AddEditLCLChargeComponent {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;
    public DataContext: QuoteChargeItem;
    public Father: any;
    public IsAdhoc: boolean = false;
    public IsRoutingRate: boolean = false;
    public IsEditingEnabled: boolean = false;
    public ObjectTableName: string = "QuoteCharge";
    public ItemsSource: ObservableCollection;
    public StepsItemsSource: QuoteStepItem[] = [];
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public IsVATVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        this.ItemsSource = new ObservableCollection([]);
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
        this.DataContext.SetUIProperties();
        this.BuildItemsSource();
        this.BuildQueryFilters();
        this.BuildStepItemsSource();
        this.Clone();
    }

    BuildItemsSource() {
        this.ItemsSource.Insert(this.DataContext);
    }
    BuildQueryFilters() {
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
    }
    BuildStepItemsSource() {
        this.StepsItemsSource = [];

        this.EntityPM.QuoteChargePriceSteps.sort((a, b) => { return a.Step - b.Step }).forEach((item) => {
            this.StepsItemsSource.push(new QuoteStepItem(item, this, false));
        });
    }

    public SelectedRow: QuoteChargeItem = null;
    OnRowSelected(itemComponent: QuoteChargeItem) {
        this.SelectedRow = itemComponent;
    }

    public SelectedStepItem: QuoteStepItem;
    SelectingPriceItem(item: QuoteStepItem) {
        this.SelectedStepItem = item;   
    }

    AddStepClicked() {
        var newItem = new QuotePriceStepsPM(null);
        newItem.Tenant = SessionLocator.Tenant;
        newItem.QuoteId = this.EntityPM.Id;
        newItem.MarkupValue = this.EntityPM.MarkUpValue;
        newItem.QuoteChargeId = this.EntityPM.Id;
        var itemComponent = new QuoteStepItem(newItem, this, true);
        this.RunAddEditStep(itemComponent, "Add Price Break");
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
    DeleteStepClicked() {
        if (this.SelectedStepItem) {
            this.EntityPM.RemoveQuotePriceStepsPM(this.SelectedStepItem.EntityPM);
            this.BuildStepItemsSource();
        }
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

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

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {

            if (this.DataContext.IsNew) {
                this.DataContext.QuotePM.AddQuoteChargePM(this.EntityPM);
                this.DataContext.fatherComponent.BuildItemsSource();
            }

            if (!this.DataContext.IsChargeBySteps) {
                if (this.EntityPM.QuoteChargePriceSteps.length > 0) {
                    this.EntityPM.QuoteChargePriceSteps = [];
                }
            }

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
    }

    get WeightUnitCode() { return this.fatherComponent.DataContext.QuotePM.GrossWeightUnitCode; }

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
