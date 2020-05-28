"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var Cloner_1 = require("../../../Infrastructure/Utilities/Cloner");
var ObservableCollection_1 = require("../../../Infrastructure/Utilities/ObservableCollection");
var QuotePriceStepsPM_1 = require("../../../Quote/EntityPMs/QuotePriceStepsPM");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var VatTypesValidator_1 = require("../../../Infrastructure/Validators/VatTypesValidator");
var AddEditLCLChargeComponent = /** @class */ (function () {
    function AddEditLCLChargeComponent() {
        this.IsAdhoc = false;
        this.IsRoutingRate = false;
        this.IsEditingEnabled = false;
        this.ObjectTableName = "QuoteCharge";
        this.StepsItemsSource = [];
        this.IsVATVisible = false;
        this.ValidationErrorsList = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SelectedRow = null;
        this.oldPriceSteps = [];
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
    }
    AddEditLCLChargeComponent.prototype.SetDataContext = function (dataContext) {
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
    };
    AddEditLCLChargeComponent.prototype.BuildItemsSource = function () {
        this.ItemsSource.Insert(this.DataContext);
    };
    AddEditLCLChargeComponent.prototype.BuildQueryFilters = function () {
        this.ChargeTypesQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
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
    };
    AddEditLCLChargeComponent.prototype.BuildStepItemsSource = function () {
        var _this = this;
        this.StepsItemsSource = [];
        this.EntityPM.QuoteChargePriceSteps.sort(function (a, b) { return a.Step - b.Step; }).forEach(function (item) {
            _this.StepsItemsSource.push(new QuoteStepItem(item, _this, false));
        });
    };
    AddEditLCLChargeComponent.prototype.OnRowSelected = function (itemComponent) {
        this.SelectedRow = itemComponent;
    };
    AddEditLCLChargeComponent.prototype.SelectingPriceItem = function (item) {
        this.SelectedStepItem = item;
    };
    AddEditLCLChargeComponent.prototype.AddStepClicked = function () {
        var newItem = new QuotePriceStepsPM_1.QuotePriceStepsPM(null);
        newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
        newItem.QuoteId = this.EntityPM.Id;
        newItem.MarkupValue = this.EntityPM.MarkUpValue;
        newItem.QuoteChargeId = this.EntityPM.Id;
        var itemComponent = new QuoteStepItem(newItem, this, true);
        this.RunAddEditStep(itemComponent, "Add Price Break");
    };
    AddEditLCLChargeComponent.prototype.EditStepClicked = function () {
        if (this.SelectedStepItem != null) {
            this.RunAddEditStep(this.SelectedStepItem, "Edit Price Break");
        }
    };
    AddEditLCLChargeComponent.prototype.RunAddEditStep = function (itemComponent, windowTitle) {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = windowTitle;
        logitudeWindow.Width = 400;
        logitudeWindow.Height = 350;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./QuoteModules/QuoteCharges/Components/AddEditPriceStepComponent');
    };
    AddEditLCLChargeComponent.prototype.DeleteStepClicked = function () {
        if (this.SelectedStepItem) {
            this.EntityPM.RemoveQuotePriceStepsPM(this.SelectedStepItem.EntityPM);
            this.BuildStepItemsSource();
        }
    };
    AddEditLCLChargeComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    AddEditLCLChargeComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (this.EntityPM.ChargesGroupCode == "FRT") {
            if (this.DataContext.QuotePM.QuoteCharges.filter(function (d) { return d.ChargesGroupCode == "FRT" && d != _this.EntityPM; }).length > 0) {
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
    AddEditLCLChargeComponent.prototype.Clone = function () {
        var _this = this;
        this.EntityPM.QuoteChargePriceSteps.forEach(function (item) {
            var stepItem = new QuotePriceStepsPM_1.QuotePriceStepsPM(null);
            stepItem.Id = item.Id;
            stepItem.QuoteId = _this.DataContext.QuotePM.Id;
            stepItem.QuoteChargeId = _this.EntityPM.Id;
            stepItem.Step = item.Step;
            stepItem.CostUnitPrice = item.CostUnitPrice;
            stepItem.SaleUnitPrice = item.SaleUnitPrice;
            stepItem.MarkupValue = item.MarkupValue;
            _this.oldPriceSteps.push(stepItem);
        });
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
    AddEditLCLChargeComponent.prototype.RejectChanges = function () {
        var _this = this;
        var addedItems = [];
        var removedItems = [];
        this.oldPriceSteps.forEach(function (item) {
            var existingItem = _this.EntityPM.QuoteChargePriceSteps.filter(function (f) { return f == item; })[0];
            if (!existingItem) {
                removedItems.push(item);
            }
        });
        this.EntityPM.QuoteChargePriceSteps.forEach(function (item) {
            var oldItem = _this.oldPriceSteps.filter(function (f) { return f == item; })[0];
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
        addedItems.forEach(function (item) {
            _this.EntityPM.RemoveQuotePriceStepsPM(item);
        });
        removedItems.forEach(function (item) {
            _this.EntityPM.AddQuotePriceStepsPM(item);
        });
        this.myCloner.RejectChanges();
    };
    AddEditLCLChargeComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './AddEditLCLChargeComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddEditLCLChargeComponent);
    return AddEditLCLChargeComponent;
}());
exports.AddEditLCLChargeComponent = AddEditLCLChargeComponent;
var QuoteStepItem = /** @class */ (function (_super) {
    __extends(QuoteStepItem, _super);
    function QuoteStepItem(entity, fatherComponent, isNew) {
        var _this = _super.call(this) || this;
        _this.fatherComponent = fatherComponent;
        _this.ObjectTableName = "QuotePriceSteps";
        _this.IsNew = false;
        _this.IsNew = isNew;
        _this.EntityPM = entity;
        _this.QuoteChargePM = fatherComponent.EntityPM;
        return _this;
    }
    Object.defineProperty(QuoteStepItem.prototype, "WeightUnitCode", {
        get: function () { return this.fatherComponent.DataContext.QuotePM.GrossWeightUnitCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteStepItem.prototype, "MarkUpType", {
        get: function () {
            var myResult = "";
            if (this.QuoteChargePM.MarkUpTypeCode == "P") {
                myResult = "Percentage(%)";
            }
            else {
                myResult = "Fixed";
                if (!Tools_1.AppTool.IsNullOrEmpty(this.QuoteChargePM.CostCurrencyCode)) {
                    myResult = myResult + " (" + this.QuoteChargePM.CostCurrencyCode + ")";
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteStepItem.prototype, "Step", {
        get: function () { return this.EntityPM.Step; },
        set: function (newValue) {
            var setValue = Tools_1.AppTool.Round(newValue, 2);
            if (this.EntityPM.Step != setValue) {
                this.EntityPM.Step = setValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteStepItem.prototype, "CostUnitPrice", {
        get: function () { return this.EntityPM.CostUnitPrice; },
        set: function (newValue) {
            var setValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.CostUnitPrice != setValue) {
                this.EntityPM.CostUnitPrice = setValue;
                this.ComputeSaleUnitPrice();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteStepItem.prototype, "MarkupValue", {
        get: function () { return this.EntityPM.MarkupValue; },
        set: function (newValue) {
            var setValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.MarkupValue != setValue) {
                this.EntityPM.MarkupValue = setValue;
                this.ComputeSaleUnitPrice();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteStepItem.prototype, "SaleUnitPrice", {
        get: function () { return this.EntityPM.SaleUnitPrice; },
        set: function (newValue) {
            var setValue = Tools_1.AppTool.Round(newValue, 3);
            if (this.EntityPM.SaleUnitPrice != setValue) {
                this.EntityPM.SaleUnitPrice = setValue;
                this.ComputeMarkUp();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuoteStepItem.prototype.ComputeSaleUnitPrice = function () {
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
    };
    QuoteStepItem.prototype.ComputeMarkUp = function () {
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
    };
    return QuoteStepItem;
}(BaseComponent_1.BaseComponent));
exports.QuoteStepItem = QuoteStepItem;
//# sourceMappingURL=AddEditLCLChargeComponent.js.map