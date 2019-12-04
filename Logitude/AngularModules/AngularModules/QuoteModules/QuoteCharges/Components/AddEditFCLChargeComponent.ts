import {Component} from '@angular/core';
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

@Component({
    moduleId: module.id,
    templateUrl: './AddEditFCLChargeComponent.html',
})

export class AddEditFCLChargeComponent {
    public QuotePM: QuotePM;
    public EntityPM: QuoteChargePM;
    public DataContext: FCLQuoteChargeItem;
    public Father: any;
    public IsAdhoc: boolean = false;
    public IsRoutingRate: boolean = false;
    public IsEditingEnabled: boolean = false;
    public ObjectTableName: string = "QuoteCharge";
    public ItemsSource: ObservableCollection;
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public IsVATVisible: boolean = false;
    public ValidationErrorsList: string[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    private IsHyprid: boolean;
    constructor() {
        this.ItemsSource = new ObservableCollection([]);
        this.IsHyprid = SessionLocator.TenantPM.IsHybrid;
    }

    SetDataContext(dataContext: FCLQuoteChargeItem) {
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
        this.Clone();
    }
 
    BuildItemsSource() {
        this.ItemsSource.Clear();
        this.ItemsSource.Insert(this.DataContext);
    }
    BuildQueryFilters() {
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
    }

    public SelectedRow: FCLQuoteChargeItem = null;
    OnRowSelected(itemComponent: FCLQuoteChargeItem) {
        this.SelectedRow = itemComponent;
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");

        if (this.IsHyprid) {
            var quoteValidator: QuoteValidator = new QuoteValidator();
            quoteValidator.CheckDuplicateInCharges(this.QuotePM, this.EntityPM, errors);
        }

        if (this.EntityPM.ChargesGroupCode == "FRT") {
            if (this.QuotePM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT" && d != this.EntityPM).length > 0) {
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
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.QuotePM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
