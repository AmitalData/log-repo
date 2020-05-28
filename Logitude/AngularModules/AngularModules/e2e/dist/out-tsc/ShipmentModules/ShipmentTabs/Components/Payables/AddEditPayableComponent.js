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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var AddEditPayableComponent = /** @class */ (function () {
    function AddEditPayableComponent() {
        this.ObjectTableName = "ShipmentPayable";
        this.ShipmentLevelCode = null;
        this.ValidationErrorsList = [];
        this.IsOrangeInfoVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.MeasurementDependencyProperty1 = null;
        this.MeasurementDependencyProperty2 = null;
    }
    AddEditPayableComponent.prototype.SetDataContext = function (dataContext) {
        this.DataContext = dataContext;
        this.EntityPM = dataContext.EntityPM;
        this.ShipmentLevelCode = dataContext.ShipmentPM.ShipmentLevelCode;
        this.IsOrangeInfoVisible = Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentPayableParentId) ? false : true;
        this.SetDependencies();
        this.BuildQueryFilters();
        this.Clone();
    };
    AddEditPayableComponent.prototype.SetDependencies = function () {
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
    AddEditPayableComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
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
    };
    AddEditPayableComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditPayableComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        if (this.EntityPM.ShipmentPayableAmountTypeCode == "ACCU") {
            if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.MeasurementId)) {
                errors.push("Measurement field is Required");
            }
        }
        if (this.DataContext.IsByContainerType) {
            if (this.DataContext.ByContainersItemsSource.length == 0) {
                errors.push("This shipment doesn't contain any containers");
            }
            else {
                this.DataContext.ByContainersItemsSource.forEach(function (item) {
                    Validator_1.Validator.TryValidateObject(item, _this.ObjectTableName, errors);
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
    };
    AddEditPayableComponent.prototype.AddByContainerEntities = function () {
        var _this = this;
        if (this.DataContext.IsByContainerType) {
            var _Amount = null;
            var _AmountLocal = null;
            var _AmountProft = null;
            this.DataContext.ByContainersItemsSource.forEach(function (item) {
                item.ShipmentPayableLineStatusCode = (!Tools_1.AppTool.IsNullOrEmpty(item.UnitPrice) && !Tools_1.AppTool.IsNullOrEmpty(item.Quantity)) ? "OAMT" : "EMPT";
                item.PrepaidCollectId = _this.EntityPM.PrepaidCollectId;
                item.VendorId = _this.EntityPM.VendorId;
                item.VendorName = _this.EntityPM.VendorName;
                item.CurrencyId = _this.EntityPM.CurrencyId;
                item.CurrencyCode = _this.EntityPM.CurrencyCode;
                item.Rate = _this.EntityPM.Rate;
                item.ProfitCurrencyExchangeRate = _this.EntityPM.ProfitCurrencyExchangeRate;
                _Amount = item.UnitPrice * item.Quantity;
                _AmountLocal = _Amount * item.Rate;
                _AmountProft = _AmountLocal / item.ProfitCurrencyExchangeRate;
                item.ExpectedAmount = Tools_1.AppTool.Round(_Amount, 2);
                item.ExpectedAmountLocal = Tools_1.AppTool.Round(_AmountLocal, 2);
                item.ExpectedAmountInProfitCurrency = Tools_1.AppTool.Round(_AmountProft, 2);
                item.OpenAmount = item.ExpectedAmount;
                item.OpenAmountInLocalCurrency = item.ExpectedAmountLocal;
                item.OpenAmountInProfitCurrency = item.ExpectedAmountInProfitCurrency;
                item.AccountedAmount = 0;
                item.AccountedAmountInLocalCurrency = 0;
                item.AccountedAmountInProfitCurrency = 0;
                var exsistingEntity = _this.DataContext.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId; })[0];
                if (exsistingEntity == null) {
                    _this.DataContext.ShipmentPM.AddPayable(item);
                }
                else {
                    var acctEntity = _this.DataContext.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && (f.ShipmentPayableLineStatusCode == "ACCT" || f.ShipmentPayableLineStatusCode == "PACC"); })[0];
                    var openEntity = _this.DataContext.ShipmentPM.ShipmentPayables.filter(function (f) { return f.ChargesTypeId == item.ChargesTypeId && f.MeasurementId == item.MeasurementId && f.CurrencyId == item.CurrencyId && (f.ShipmentPayableLineStatusCode == "EMPT" || f.ShipmentPayableLineStatusCode == "OAMT"); })[0];
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
                            _this.DataContext.ShipmentPM.AddPayable(item);
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
    };
    AddEditPayableComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
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
    };
    AddEditPayableComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditPayableComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditPayableComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditPayableComponent);
    return AddEditPayableComponent;
}());
exports.AddEditPayableComponent = AddEditPayableComponent;
//# sourceMappingURL=AddEditPayableComponent.js.map