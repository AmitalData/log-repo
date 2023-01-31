import {Component, ViewChild, ViewContainerRef} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ShipmentReceivableItem} from './ReceivablesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { CommonTool } from '../../../../Common/Tools';
import { ShipmentPayablePM } from '../../../../Shipment/EntityPMs/ShipmentPayablePM';

@Component({
    
    templateUrl: './AddEditReceivableComponent.html',
})

export class AddEditReceivableComponent {
    public EntityPM: ShipmentReceivablePM;

    public DataContext: ShipmentReceivableItem;
    public ObjectTableName: string = "ShipmentReceivable";
    public ShipmentLevelCode: string = null;    
    public ValidationErrorsList: string[] = [];
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public MeasurementsQueryFilters: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;


    @ViewChild('AdditionalFieldsArea', { read: ViewContainerRef, static: false }) viewContainerRef: ViewContainerRef;
    @ViewChild('ByContainerAdditionalFieldsArea', { read: ViewContainerRef, static: false }) byContainerViewContainerRef: ViewContainerRef;

    constructor() {
        this.LoadAdditionalCustomFieldsArea();
    }

    public LoadAdditionalCustomFieldsArea() {

        if (!this.viewContainerRef) {
            this.RunComponentTimer("DefaultAdditionalCustomFields");
            return;
        }

        this.LoadChildComponent(this.viewContainerRef);
    }

    IsByContainerAdditionalFieldsAreaLoaded: boolean = false;
    public LoadByContainerAdditionalFieldsArea() {

        if (this.IsByContainerAdditionalFieldsAreaLoaded) return;

        this.Retries = 0;
        if (!this.byContainerViewContainerRef) {
            this.RunComponentTimer("ByContainerAdditionalCustomFields");
            return;
        }

        this.LoadChildComponent(this.byContainerViewContainerRef);
        this.IsByContainerAdditionalFieldsAreaLoaded = true;
    }


    private Retries: number = 0;
    private timerToken: any;
    private RunComponentTimer(componentName: String) {
        this.Retries++;

        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }

        if (this.Retries < 20) {

            this.timerToken = componentName == "ByContainerAdditionalCustomFields" ? setTimeout(() => this.LoadByContainerAdditionalFieldsArea(), 1) : setTimeout(() => this.LoadAdditionalCustomFieldsArea(), 1);
        }
    }


    LoadChildComponent(viewContainerRef) {
        let screenCode: string = "ShipmentReceivable.AdditionalFields";
        SessionLocator.DynamicLoader.Load('./Infrastructure/GenericComponents/GeneratedComponent', viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.HideLastColumn = true;
                cmpRef.instance.LabelWidth = 120;
                cmpRef.instance.Run(this.EntityPM, this.ObjectTableName, screenCode);

            });
    }



    SetDataContext(dataContext: ShipmentReceivableItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        dataContext.AddEditReceivableComponent = this;
        this.ShipmentLevelCode = dataContext.ShipmentPM.ShipmentLevelCode;
        this.SetDependencies();
        this.BuildQueryFilters(); 
        this.Clone();
    }

    public MeasurementDependencyProperty1: any = null;
    public MeasurementDependencyProperty2: any = null;
    SetDependencies() {
        var myMeasurementDependencyProperty1 = null;
        var myMeasurementDependencyProperty2 = null;

        if (this.DataContext.fatherComponent.IsLCLEntity) {
            myMeasurementDependencyProperty1 = false;
            myMeasurementDependencyProperty2 = false;
        }

        else if (!this.DataContext.IsNewEntity) {
            myMeasurementDependencyProperty1 = false;
        }

        this.MeasurementDependencyProperty1 = myMeasurementDependencyProperty1;
        this.MeasurementDependencyProperty2 = myMeasurementDependencyProperty2;
    }

    private BuildQueryFilters() {
        this.MeasurementsQueryFilters = new ApiQueryFilters();
        this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "Exclude", false, false, false, "string", false, true, true);

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsReceivable", true, null, null, "Equals", false, false, false, "Boolean");

        switch (this.DataContext.ShipmentPM.TransportModeId) {
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

        CommonTool.FilterChargeTypesByDirection(this.ChargeTypesQueryFilters, this.DataContext.ShipmentPM.DirectionId); 
    }


    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    private errors: string[] = [];
    OkButtonClicked() {
        this.errors = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, this.errors);

        if (this.DataContext.IsByContainerType) {
            if (this.DataContext.ByContainersItemsSource.length == 0) {
                this.errors.push(TextCodeTranslator.Translate("Shipment.M.Receivables.ShipmentDoesntContainContainers"));
            }

            else {
                this.DataContext.ByContainersItemsSource.forEach(item => {
                    Validator.TryValidateObject(item, this.ObjectTableName, this.errors);
                });
            }
        }

        if (this.EntityPM.TotalAmount != null && this.EntityPM.UnitPrice != null && this.EntityPM.Quantity != null) {
            if (this.EntityPM.Rate == null) {
                this.errors.push(TextCodeTranslator.Translate("ShipmentReceivable.M.ExchangeRateIsRequired"));
            }
        }

        this.ValidateAddingPFCLUOM();

        this.ValidationErrorsList = this.errors;

        if (this.ValidationErrorsList.length == 0) {

            if (this.DataContext.IsByContainerType) {
                this.AddByContainerEntities();
                this.DataContext.fatherComponent.BuildItemsSource();

                if (this.DataContext.ChargesGroupCode == "FRT") {
                    this.DataContext.fatherComponent.OnFreightAmountChanged();
                }

                this.DataContext.fatherComponent.ComputeShipmentFields();
            }

            else if (this.DataContext.IsNewEntity) {
                this.DataContext.ShipmentPM.AddReceivable(this.EntityPM);
                this.DataContext.fatherComponent.BuildItemsSource();

                if (this.DataContext.ChargesGroupCode == "FRT") {
                    this.DataContext.fatherComponent.OnFreightAmountChanged();
                }

                this.DataContext.fatherComponent.ComputeShipmentFields();
                this.AddExpensePayable();
            }

            this.DataContext.fatherComponent.OnPercentForeignAmountChanged();

            this.DataContext.IsNewEntity = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }
    private AddExpensePayable() {
        if (this.EntityPM.IsExpense && this.DataContext.IsPayableCharge && SessionLocator.TenantPM.CountryCode == "MX" && SessionLocator.SATInterfaceSettings.TransferExpenseCharges) {
            var expensePayable: ShipmentPayablePM = new ShipmentPayablePM(this.DataContext.ShipmentPM);
            expensePayable.ChargesTypeId = this.DataContext.ChargesTypeId;
            expensePayable.IsExpenseCharge = this.DataContext.EntityPM.IsExpenseCharge;
            expensePayable.CurrencyId = this.DataContext.CurrencyId;
            expensePayable.CurrencyCode = this.DataContext.CurrencyCode;
            expensePayable.Rate = this.DataContext.Rate;
            expensePayable.MeasurementId = this.DataContext.MeasurementId;
            expensePayable.MeasurementCode = this.DataContext.MeasurementCode;
            expensePayable.VendorId = this.DataContext.PayableVendorId;
            expensePayable.VendorName = this.DataContext.PayableVendorName;
            expensePayable.ChargesTypeCode = this.DataContext.EntityPM.ChargesTypeCode;
            expensePayable.ChargesTypeName = this.DataContext.ChargesTypeName;
            expensePayable.ChargesGroupCode = this.DataContext.ChargesGroupCode;
            expensePayable.DueTypeCode = this.DataContext.EntityPM.DueTypeCode;
            expensePayable.DueTypeName = this.DataContext.EntityPM.DueTypeName;
            expensePayable.VatTypeId = this.DataContext.VatTypeId;
            expensePayable.IATACodeId = this.DataContext.EntityPM.IATACodeId;
            expensePayable.Tenant = SessionLocator.Tenant;
            expensePayable.ShipmentId = this.EntityPM.Id;
            expensePayable.ShipmentNumber = this.EntityPM.ShipmentNumber;
            expensePayable.CreateDate = DateTool.GetCurrentDateAsUtc();
            expensePayable.UpdateDate = DateTool.GetCurrentDateAsUtc();
            expensePayable.CreatedByUserId = SessionLocator.LoggedUserId;
            expensePayable.UpdateByUserId = SessionLocator.LoggedUserId;
            expensePayable.CreatedByUserName = SessionLocator.LoggedUserPM.EnglishName;
            expensePayable.UpdateByUserName = SessionLocator.LoggedUserPM.EnglishName;
            expensePayable.ShipmentPayableLineStatusCode = "EMPT";
            expensePayable.ShipmentPayableAmountTypeCode = "ACCU";
            expensePayable.ShipmentPayableAmountTypeName = "Accrual";
            expensePayable.ProfitCurrencyExchangeRate = this.DataContext.ProfitCurrencyExchangeRate;
            expensePayable.PrepaidCollectId = this.DataContext.PrepaidCollectId;
            this.DataContext.ShipmentPM.AddPayable(expensePayable);
            this.CurrentSession.FireEvent("ExpensePayableAdded");
        }
    }

    ValidateAddingPFCLUOM() {
        if (this.EntityPM.MeasurementCode == "PFCL") {
            if (this.DataContext.ShipmentPM.ShipmentReceivables.filter(d => d.MeasurementCode == "PFCL" && d.Id != this.EntityPM.Id).length > 0) {
                this.errors.push("Charge with Percent of foreign charges local amounts UOM already added");
            }
        }
    }

    AddByContainerEntities() {
        if (this.DataContext.IsByContainerType) {
            var _Amount: number = null;
            var _AmountLocal: number = null;
            var _AmountProft: number = null;

            this.DataContext.ByContainersItemsSource.forEach(item => {
                item.ShipmentReceivableLineStatusCode = (!AppTool.IsNullOrEmpty(item.UnitPrice) && !AppTool.IsNullOrEmpty(item.Quantity)) ? "OAMT" : "EMPT";
                item.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
                item.CurrencyId = this.EntityPM.CurrencyId;
                item.CurrencyCode = this.EntityPM.CurrencyCode;
                item.Rate = this.EntityPM.Rate;
                item.ProfitCurrencyExchangeRate = this.EntityPM.ProfitCurrencyExchangeRate;

                _Amount = item.UnitPrice * item.Quantity;
                _AmountLocal = _Amount * item.Rate;
                _AmountProft = _AmountLocal / item.ProfitCurrencyExchangeRate;
                item.TotalAmount = AppTool.Round(_Amount, 2);
                item.TotalAmountLocal = AppTool.Round(_AmountLocal, 2);
                item.AmountInProfitCurrency = AppTool.Round(_AmountProft, 2);

                var exsistingEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId)[0];
                if (exsistingEntity == null) {
                    this.DataContext.ShipmentPM.AddReceivable(item);
                }

                else {
                    //var acctEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && (f.ShipmentReceivableLineStatusCode == "ACCT" || f.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                    //var openEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && (f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"))[0];
                    var acctEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && f.ShipmentReceivableLineStatusCode == "ACCT")[0];
                    var openEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && f.ShipmentReceivableLineStatusCode != "ACCT")[0];

                    if (acctEntity == null) {
                        openEntity.Quantity = item.Quantity;
                        openEntity.UnitPrice = item.UnitPrice;
                        openEntity.TotalAmount = item.TotalAmount;
                        openEntity.TotalAmountLocal = item.TotalAmountLocal;
                        openEntity.AmountInProfitCurrency = item.AmountInProfitCurrency;
                        openEntity.ShipmentReceivableLineStatusCode = item.ShipmentReceivableLineStatusCode;
                    }

                    else if (item.Quantity > acctEntity.Quantity) {
                        if (openEntity == null) {
                            this.DataContext.ShipmentPM.AddReceivable(item);
                        }

                        else {
                            openEntity.Quantity = item.Quantity;
                            openEntity.UnitPrice = item.UnitPrice;
                            openEntity.TotalAmount = item.TotalAmount;
                            openEntity.TotalAmountLocal = item.TotalAmountLocal;
                            openEntity.AmountInProfitCurrency = item.AmountInProfitCurrency;
                            openEntity.ShipmentReceivableLineStatusCode = item.ShipmentReceivableLineStatusCode;
                        }
                    }
                }
            });
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.DataContext);
        this.myCloner.AddField('ChargesTypeId');
        this.myCloner.AddField('Quantity');
        this.myCloner.AddField('UnitPrice');
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddField('Rate');
        this.myCloner.AddField('IsExchangeRateFixed');
        this.myCloner.AddField('TotalAmount');
        this.myCloner.AddField('TotalAmountLocal');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
