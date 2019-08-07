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
var Tools_1 = require("../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var VatTypesValidator_1 = require("../../../Infrastructure/Validators/VatTypesValidator");
var AddEditFCLChargeComponent = /** @class */ (function () {
    function AddEditFCLChargeComponent() {
        this.IsAdhoc = false;
        this.IsRoutingRate = false;
        this.IsEditingEnabled = false;
        this.ObjectTableName = "QuoteCharge";
        this.IsVATVisible = false;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SelectedRow = null;
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
    }
    AddEditFCLChargeComponent.prototype.SetDataContext = function (dataContext) {
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
    };
    AddEditFCLChargeComponent.prototype.BuildItemsSource = function () {
        this.ItemsSource.Clear();
        this.ItemsSource.Insert(this.DataContext);
    };
    AddEditFCLChargeComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
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
    };
    AddEditFCLChargeComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    AddEditFCLChargeComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditFCLChargeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.EntityPM.ChargesGroupCode == "FRT") {
            if (this.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT" && d != _this.EntityPM; }).length > 0) {
                errors.push("Freight Charge already added");
            }
        }
        if (this.QuotePM.IsChargesByVAT) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatTypeId)) {
                if (this.EntityPM.VatIsMultiPercentage) {
                    if (!SessionLocator_1.SessionLocator.AccountingSettingPM.EnableMultiPercentageVATTypes) {
                        errors.push(VatTypesValidator_1.VatTypesValidator.GetError());
                    }
                }
                else {
                    if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.VatPercentage)) {
                        var field = TextCodeTranslator_1.TextCodeTranslator.Translate("QuoteCharge.F.VatPercentage");
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
    };
    AddEditFCLChargeComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.DataContext);
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
    };
    AddEditFCLChargeComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    AddEditFCLChargeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditFCLChargeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditFCLChargeComponent);
    return AddEditFCLChargeComponent;
}());
exports.AddEditFCLChargeComponent = AddEditFCLChargeComponent;
//# sourceMappingURL=AddEditFCLChargeComponent.js.map