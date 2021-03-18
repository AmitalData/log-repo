import {Component} from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ShipmentReceivablePM} from '../../../../Shipment/EntityPMs/ShipmentReceivablePM';
import {ShipmentReceivableItem} from './ReceivablesTabComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { CommonTool } from '../../../../Common/Tools';

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
    constructor() {
               
    }

    SetDataContext(dataContext: ShipmentReceivableItem) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
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
        this.MeasurementsQueryFilters.addAdditionalFilter("Code", "STFE", null, null, "NotContains", false, false, false, "string", false, true, true);

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
            }

            this.DataContext.IsNewEntity = false;
            this.CurrentSession.CloseCurrentWindowEmit("OK");
        }
    }

    ValidateAddingPFCLUOM() {
        if (this.DataContext.ShipmentPM.ShipmentReceivables.filter(d => d.MeasurementCode == "PFCL").length == 1 && this.EntityPM.MeasurementCode == "PFCL") {
            this.errors.push("Charge with Percent of foreign charges local amounts UOM already added");
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
