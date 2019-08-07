"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditReceivableComponent = /** @class */ (function () {
    function AddEditReceivableComponent() {
        this.ObjectTableName = "ShipmentReceivable";
        this.ShipmentLevelCode = null;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.MeasurementDependencyProperty1 = null;
        this.MeasurementDependencyProperty2 = null;
    }
    AddEditReceivableComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ShipmentLevelCode = dataContext.ShipmentPM.ShipmentLevelCode;
        this.SetDependencies();
        this.BuildQueryFilters();
        this.Clone();
    };
    AddEditReceivableComponent.prototype.SetDependencies = function () {
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
    };
    AddEditReceivableComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
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
    };
    AddEditReceivableComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditReceivableComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.DataContext.IsByContainerType) {
            if (this.DataContext.ByContainersItemsSource.length == 0) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Shipment.M.Receivables.ShipmentDoesntContainContainers"));
            }
            else {
                this.DataContext.ByContainersItemsSource.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, _this.ObjectTableName, errors);
                });
            }
        }
        if (this.EntityPM.TotalAmount != null && this.EntityPM.UnitPrice != null && this.EntityPM.Quantity != null) {
            if (this.EntityPM.Rate == null) {
                errors.push(TextCodeTranslator_1.TextCodeTranslator.Translate("ShipmentReceivable.M.ExchangeRateIsRequired"));
            }
        }
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
    };
    AddEditReceivableComponent.prototype.AddByContainerEntities = function () {
        var _this = this;
        if (this.DataContext.IsByContainerType) {
            var _Amount = null;
            var _AmountLocal = null;
            var _AmountProft = null;
            this.DataContext.ByContainersItemsSource.forEach(function (item) {
                item.ShipmentReceivableLineStatusCode = (!Tools_1.AppTool.IsNullOrEmpty(item.UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) ? "OAMT" : "EMPT";
                item.PrepaidCollectId = _this.EntityPM.PrepaidCollectId;
                item.CurrencyId = _this.EntityPM.CurrencyId;
                item.CurrencyCode = _this.EntityPM.CurrencyCode;
                item.Rate = _this.EntityPM.Rate;
                item.ProfitCurrencyExchangeRate = _this.EntityPM.ProfitCurrencyExchangeRate;
                _Amount = item.UnitPrice * item.Quantity;
                _AmountLocal = _Amount * item.Rate;
                _AmountProft = _AmountLocal / item.ProfitCurrencyExchangeRate;
                item.TotalAmount = Tools_1.AppTool.Round(_Amount, 2);
                item.TotalAmountLocal = Tools_1.AppTool.Round(_AmountLocal, 2);
                item.AmountInProfitCurrency = Tools_1.AppTool.Round(_AmountProft, 2);
                var exsistingEntity = _this.DataContext.ShipmentPM.ShipmentReceivables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId; })[0];
                if (exsistingEntity == null) {
                    _this.DataContext.ShipmentPM.AddReceivable(item);
                }
                else {
                    //var acctEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && (f.ShipmentReceivableLineStatusCode == "ACCT" || f.ShipmentReceivableLineStatusCode == "DRFT"))[0];
                    //var openEntity = this.DataContext.ShipmentPM.ShipmentReceivables.filter(f => f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && (f.ShipmentReceivableLineStatusCode == "EMPT" || f.ShipmentReceivableLineStatusCode == "OAMT"))[0];
                    var acctEntity = _this.DataContext.ShipmentPM.ShipmentReceivables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && f.ShipmentReceivableLineStatusCode == "ACCT"; })[0];
                    var openEntity = _this.DataContext.ShipmentPM.ShipmentReceivables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && f.ShipmentReceivableLineStatusCode != "ACCT"; })[0];
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
                            _this.DataContext.ShipmentPM.AddReceivable(item);
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
    };
    AddEditReceivableComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
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
    };
    AddEditReceivableComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditReceivableComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditReceivableComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditReceivableComponent);
    return AddEditReceivableComponent;
}());
exports.AddEditReceivableComponent = AddEditReceivableComponent;
//# sourceMappingURL=AddEditReceivableComponent.js.map