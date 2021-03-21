import { Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {Cloner} from '../../../Infrastructure/Utilities/Cloner';
import {FCLQuoteChargeItem} from './FCLChargesComponent';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {QuoteChargePM} from '../../../Quote/EntityPMs/QuoteChargePM';
import {QuotePM} from '../../../Quote/EntityPMs/QuotePM';
import {VatTypesValidator} from '../../../Infrastructure/Validators/VatTypesValidator';
import { QuoteValidator } from '../../../Quote/Validators/QuoteValidator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { CommonTool } from '../../../Common/Tools';

@Component({
    
    templateUrl: './AddEditFCLChargeComponent.html',
})

export class AddEditFCLChargeComponent implements OnDestroy {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;
    public DataContext: FCLQuoteChargeItem;
    public Father: any;
    public IsAdhoc: boolean = false;
    public IsEditingEnabled: boolean = false;
    public ObjectTableName: string = "QuoteCharge";
    public ItemsSource: ObservableCollection;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public MeasurementsQueryFilters: ApiQueryFilters;
    public IsVATVisible: boolean = false;
    public IsRegionalTaxVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    public CheckChargeTypeDuplicationFlag: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    public HideFCLAllIn: boolean = false;
    private IsHyprid: boolean;
    private ChargesTypeCode: string;
    private PropertyChangedEvent: any = null;

    constructor() {
        this.ItemsSource = new ObservableCollection([]);
        this.HideFCLAllIn = SessionLocator.TenantPM.HideFCLAllIn;
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

    SetDataContext(dataContext: FCLQuoteChargeItem) {
        this.QuotePM = dataContext.QuotePM;
        this.EntityPM = dataContext.EntityPM;
        this.DataContext = dataContext;
        this.Father = this.DataContext.fatherComponent;
        this.IsAdhoc = this.DataContext.fatherComponent.IsAdhoc;
        this.IsEditingEnabled = this.DataContext.fatherComponent.IsEditingEnabled;
        this.IsVATVisible = this.IsAdhoc && this.QuotePM.IsChargesByVAT ? true : false;
        this.IsRegionalTaxVisible = this.IsVATVisible && this.Father.IsRegionalTaxVisible ? true : false;
        this.ChargesTypeCode = this.EntityPM.ChargesTypeCode;
        this.DataContext.SetUIProperties();
        this.BuildItemsSource();
        this.BuildQueryFilters();
        this.Clone();
        this.ListenPropertyChanged();
    }
 
    BuildItemsSource() {
        this.ItemsSource.Clear();
        this.ItemsSource.Insert(this.DataContext);
    }
    BuildQueryFilters() {
        this.MeasurementsQueryFilters = new ApiQueryFilters();
        this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "NotContains", false, false, false, "string", false, true, true);

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");

        switch (this.QuotePM.TransportModeId) {
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
        CommonTool.FilterChargeTypesByDirection(this.ChargeTypesQueryFilters, this.QuotePM.DirectionId); 

    }

    public SelectedRow: FCLQuoteChargeItem = null;
    OnRowSelected(itemComponent: FCLQuoteChargeItem) {
        this.SelectedRow = itemComponent;
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
    private errors: string[] = [];
    OkButtonClicked() {
        this.errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        this.CheckChargeTypeDuplication();

        if (this.IsHyprid && this.CheckChargeTypeDuplicationFlag) {
            var quoteValidator: QuoteValidator = new QuoteValidator();
            quoteValidator.CheckDuplicateInCharges(this.QuotePM, this.EntityPM, this.errors);
        }

        if (this.EntityPM.ChargesGroupCode == "FRT") {
            if (this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT" && d != this.EntityPM).length > 0) {
                this.errors.push("Freight Charge already added");
            }
        }

        if (this.QuotePM.IsChargesByVAT) {
            if (!AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {

                if (this.EntityPM.VatIsMultiPercentage) {
                    if (!SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                        this.errors.push(VatTypesValidator.GetError());
                    }
                }

                else {
                    if (AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
                        var field = TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
                        this.errors.push(msg.replace("%FieldName", field));
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
                        this.errors.push("Cost currency must be the same as the freight currency in the case of Percent of Freight");
                    }
                }
            }
        }

        if (!AppTool.IsNullOrEmpty(this.EntityPM.SaleMeasurementCode)) {
            if (this.EntityPM.SaleMeasurementCode == "PRFR" && !AppTool.IsNullOrEmpty(this.EntityPM.SaleCurrencyId) && !AppTool.IsNullOrEmpty(freightLineSaleCurrencyId)) {
                if (!AppTool.IsNullOrZero(this.EntityPM.SaleTotalAmount)) {
                    if (this.EntityPM.SaleCurrencyId != freightLineSaleCurrencyId) {
                        this.errors.push("Sale currency must be the same as the freight currency in the case of Percent of Freight");
                    }
                }
            }
        }

        this.ValidationErrorsList = this.errors;

        this.ValidateAddingPFCLUOM();

        if (this.errors.length == 0) {
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
                var property = this.propertiesChanges.filter(a => a == "CostUnitPrice" || a == "CostTotalAmount" || a == "CostCurrencyId"
                    || a == "CostContainerType1UnitPrice" || a == "CostContainerType2UnitPrice" || a == "CostContainerType3UnitPrice"
                    || a == "CostContainerType4UnitPrice" || a == "CostContainerType5UnitPrice")[0];
                if (property) {
                    this.ShowTariffDisconnectionWindow();
                }
                else {
                    this.DataContext.fatherComponent.ComputeTotals();
                    this.CurrentSession.CloseCurrentWindowEmit("OK");
                }
            }

            else {
                this.DataContext.fatherComponent.ComputeTotals();
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        }
    }

    ValidateAddingPFCLUOM() {
        if (this.EntityPM.CostMeasurementCode == "PFCL" || this.EntityPM.SaleMeasurementCode == "PFCL") {
            if (this.QuotePM.QuoteCharges.filter(d => d.CostMeasurementCode == "PFCL" || d.SaleMeasurementCode == "PFCL" && d != this.EntityPM).length > 0) {
                this.errors.push("Charge with Percent of foreign charges local amounts UOM already added");
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


    private myCloner: Cloner;
    private Clone() {
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
        this.myCloner.AddField('SaleCurrencyId');
        this.myCloner.AddField('SaleExchangeRate');
        this.myCloner.AddField('SaleIsFixedRate');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuotePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
