import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentPayablePM} from '../../../../Shipment/EntityPMs/ShipmentPayablePM';
import {ShipmentPayableItem} from './PayablesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    moduleId: module.id,
    templateUrl: './AddEditPayableComponent.html',
})

export class AddEditPayableComponent {
    public EntityPM: ShipmentPayablePM;
    public DataContext: ShipmentPayableItem;
    public ObjectTableName: string = "ShipmentPayable";
    public ShipmentLevelCode: string = null;
    public ValidationErrorsList: string[] = [];
    public ChargeTypesQueryFilters: ApiQueryFilters;
    public IsOrangeInfoVisible: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {

    }

    SetDataContext(dataContext: ShipmentPayableItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ShipmentLevelCode = dataContext.ShipmentPM.ShipmentLevelCode;
        this.IsOrangeInfoVisible = AppTool.IsNullOrEmpty(this.EntityPM.ShipmentPayableParentId) ? false : true;
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

        this.ChargeTypesQueryFilters = new ApiQueryFilters();
        this.ChargeTypesQueryFilters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "Boolean");
        this.ChargeTypesQueryFilters.addAdditionalFilter("IsPayable", true, null, null, "Equals", false, false, false, "Boolean");

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
    }

    CancelButtonClicked() {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {

        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (this.EntityPM.ShipmentPayableAmountTypeCode == "ACCU") {
            if (AppTool.IsNullOrEmpty(this.EntityPM.MeasurementId)) {
                errors.push("Measurement field is Required");
            }
        }

        if (this.DataContext.IsByContainerType) {
            if (this.DataContext.ByContainersItemsSource.length == 0) {
                errors.push("This shipment doesn't contain any containers");
            }

            else {
                this.DataContext.ByContainersItemsSource.forEach(item => {
                    Validator.TryValidateObject(item, this.ObjectTableName, errors);
                });
            }
        }

        // Back To Back Check
        

        this.ValidationErrorsList = errors;

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
                this.DataContext.ShipmentPM.AddPayable(this.EntityPM);
                this.DataContext.fatherComponent.BuildItemsSource();

                if (this.DataContext.ChargesGroupCode == "FRT") {
                    this.DataContext.fatherComponent.OnFreightAmountChanged();
                }

                this.DataContext.fatherComponent.ComputeShipmentFields();
            }

            this.DataContext.IsNewEntity = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");            
        }
    }

    AddByContainerEntities() {
        if (this.DataContext.IsByContainerType) {
            var _Amount: number = null;
            var _AmountLocal: number = null;
            var _AmountProft: number = null;

            this.DataContext.ByContainersItemsSource.forEach(item => {
                item.ShipmentPayableLineStatusCode = (!AppTool.IsNullOrEmpty(item.UnitPrice) && !AppTool.IsNullOrEmpty(item.Quantity)) ? "OAMT" : "EMPT";
                item.PrepaidCollectId = this.EntityPM.PrepaidCollectId;
                item.VendorId = this.EntityPM.VendorId;
                item.VendorName = this.EntityPM.VendorName;
                item.CurrencyId = this.EntityPM.CurrencyId;
                item.CurrencyCode = this.EntityPM.CurrencyCode;
                item.Rate = this.EntityPM.Rate;
                item.ProfitCurrencyExchangeRate = this.EntityPM.ProfitCurrencyExchangeRate;

                _Amount = item.UnitPrice * item.Quantity;
                _AmountLocal = _Amount * item.Rate;
                _AmountProft = _AmountLocal / item.ProfitCurrencyExchangeRate;
                item.ExpectedAmount = AppTool.Round(_Amount, 2);
                item.ExpectedAmountLocal = AppTool.Round(_AmountLocal, 2);
                item.ExpectedAmountInProfitCurrency = AppTool.Round(_AmountProft, 2);
                item.OpenAmount = item.ExpectedAmount;
                item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                item.AccountedAmount = 0;
                item.AccountedAmountInLocalCurrency = 0;
                item.AccountedAmountInProfitCurrency = 0;

                var exsistingEntity = this.DataContext.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId)[0];
                if (exsistingEntity == null) {
                    this.DataContext.ShipmentPM.AddPayable(item);
                }

                else {
                    var acctEntity = this.DataContext.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && (f.ShipmentPayableLineStatusCode == "ACCT" || f.ShipmentPayableLineStatusCode == "PACC"))[0];
                    var openEntity = this.DataContext.ShipmentPM.ShipmentPayables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && (f.ShipmentPayableLineStatusCode == "EMPT" || f.ShipmentPayableLineStatusCode == "OAMT"))[0];

                    if (acctEntity == null) {
                        openEntity.Quantity = item.Quantity;
                        openEntity.UnitPrice = item.UnitPrice;
                        openEntity.ExpectedAmount = item.ExpectedAmount;
                        openEntity.ExpectedAmountLocal = item.ExpectedAmountLocal;
                        openEntity.ExpectedAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                        openEntity.OpenAmount = item.OpenAmount;
                        openEntity.OpenAmountInLocalCurrency = item.OpenAmountInLocalCurrency;
                        openEntity.OpenAmountInProfitCurrency = item.OpenAmountInProfitCurrency;
                        openEntity.AccountedAmount = item.AccountedAmount;
                        openEntity.AccountedAmountInLocalCurrency = item.AccountedAmountInLocalCurrency;
                        openEntity.AccountedAmountInProfitCurrency = item.AccountedAmountInProfitCurrency;
                        openEntity.ShipmentPayableLineStatusCode = item.ShipmentPayableLineStatusCode;
                    }

                    else if (item.Quantity > acctEntity.Quantity) {
                        if (openEntity == null) {
                            this.DataContext.ShipmentPM.AddPayable(item);
                        }

                        else {
                            openEntity.Quantity = item.Quantity;
                            openEntity.UnitPrice = item.UnitPrice;
                            openEntity.ExpectedAmount = item.ExpectedAmount;
                            openEntity.ExpectedAmountLocal = item.ExpectedAmountLocal;
                            openEntity.ExpectedAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                            openEntity.OpenAmount = item.OpenAmount;
                            openEntity.OpenAmountInLocalCurrency = item.OpenAmountInLocalCurrency;
                            openEntity.OpenAmountInProfitCurrency = item.OpenAmountInProfitCurrency;
                            openEntity.AccountedAmount = item.AccountedAmount;
                            openEntity.AccountedAmountInLocalCurrency = item.AccountedAmountInLocalCurrency;
                            openEntity.AccountedAmountInProfitCurrency = item.AccountedAmountInProfitCurrency;
                            openEntity.ShipmentPayableLineStatusCode = item.ShipmentPayableLineStatusCode;
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
        this.myCloner.AddField('ExpectedAmount');
        this.myCloner.AddField('ExpectedAmountLocal');
        this.myCloner.AddField('AmountInProfitCurrency');
        this.myCloner.AddField('MeasurementId');
        this.myCloner.AddField('PrepaidCollectId');
        this.myCloner.AddField('VendorId');
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(this.EntityPM);
        this.myCloner.AddEntity(this.DataContext.ShipmentPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}
