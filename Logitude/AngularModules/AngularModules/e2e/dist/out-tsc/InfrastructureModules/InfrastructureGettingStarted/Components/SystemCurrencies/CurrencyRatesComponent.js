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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var CurrencyRatesComponent = /** @class */ (function (_super) {
    __extends(CurrencyRatesComponent, _super);
    function CurrencyRatesComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Tenant";
        _this.ItemsSource = [];
        _this.CurrencyFieldIsEnabled = true;
        _this.RatesListVisibility = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        return _this;
    }
    Object.defineProperty(CurrencyRatesComponent.prototype, "CurrencyId", {
        get: function () { return this.EntityPM.CurrencyId; },
        set: function (value) {
            if (this.EntityPM.CurrencyId != value) {
                this.EntityPM.CurrencyId = value;
                this.RatesListVisibility = true;
                this.BuildRatesList();
            }
        },
        enumerable: true,
        configurable: true
    });
    CurrencyRatesComponent.prototype.SetWindowArgs = function (args) {
        this.EntityPM = args["EntityPM"];
        var entitiesCount = args["ShipmentsQuotesCount"];
        if (entitiesCount > 0) {
            this.CurrencyFieldIsEnabled = false;
            this.UIProperties.SetEnabled("CurrencyId", "Tenant", this.CurrencyFieldIsEnabled);
        }
        this.Clone();
    };
    CurrencyRatesComponent.prototype.BuildRatesList = function () {
        var _this = this;
        var myService = new CurrencyRatesService_1.CurrencyRatesService();
        myService.GetRatesByValueDate(this.CurrencyId, Tools_1.DateTool.GetCurrentDateAsUtc()).subscribe(function (myResponse) {
            _this.ItemsSource = [];
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
            }
            else {
                myResponse.Result.forEach(function (item) {
                    _this.ItemsSource.push(new CurrencyRatesModelData(item, _this.CurrencyFieldIsEnabled));
                });
            }
        });
    };
    CurrencyRatesComponent.prototype.CancelButtonClicked = function () {
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    };
    CurrencyRatesComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }
        else {
            var errors = [];
            this.ItemsSource.forEach(function (item) {
                if (item.Rate == null) {
                    errors.push("Rate field for currency " + item.Code + " is required");
                }
            });
            this.ValidationErrorsList = errors;
            if (this.ValidationErrorsList.length == 0) {
                this.CurrentSession.StartBusyIndicator("Saving...");
                var lastRates = [];
                this.ItemsSource.forEach(function (item) {
                    var lastRateItem = new CurrencyRatesService_1.LastRate();
                    lastRateItem.Tenant = _this.EntityPM.Id;
                    lastRateItem.BaseCurrencyId = _this.EntityPM.CurrencyId;
                    lastRateItem.ForeignCurrencyId = item.ForeignCurrencyId;
                    lastRateItem.LogDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    lastRateItem.ValueDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                    lastRateItem.Rate = item.Rate;
                    lastRates.push(lastRateItem);
                });
                var myService = new CurrencyRatesService_1.CurrencyRatesService();
                var myHelper = new CurrencyRatesService_1.AccountingCurrencyHelper();
                myHelper.TenantPM = this.EntityPM;
                myHelper.LastRates = lastRates;
                myService.UpdateAccountingCurrency(myHelper).subscribe(function (myResponse) {
                    _this.CurrentSession.StopBusyIndicator();
                    if (!myResponse.HasError) {
                        _this.EntityPM = myResponse.Result;
                        InfraSettings_1.InfraSettings.TenantPM = myResponse.Result;
                        _this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }
                    else {
                        _this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    };
    CurrencyRatesComponent.prototype.Clone = function () {
        this.myCloner = new Cloner_1.Cloner(this.EntityPM);
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddEntity(this.EntityPM);
    };
    CurrencyRatesComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    CurrencyRatesComponent = __decorate([
        core_1.Component({
            selector: 'CurrencyRatesComponent',
            moduleId: module.id,
            templateUrl: './CurrencyRatesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CurrencyRatesComponent);
    return CurrencyRatesComponent;
}(BaseComponent_1.BaseComponent));
exports.CurrencyRatesComponent = CurrencyRatesComponent;
var CurrencyRatesModelData = /** @class */ (function (_super) {
    __extends(CurrencyRatesModelData, _super);
    function CurrencyRatesModelData(entityPM, RateIsEnabled) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "RatesTable";
        _this.EntityPM = entityPM;
        _this.UIProperties.SetEnabled("Rate", _this.ObjectTableName, RateIsEnabled);
        return _this;
    }
    Object.defineProperty(CurrencyRatesModelData.prototype, "ForeignCurrencyId", {
        get: function () { return this.EntityPM.ForeignCurrencyId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CurrencyRatesModelData.prototype, "Code", {
        get: function () { return this.EntityPM.ForeignCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CurrencyRatesModelData.prototype, "ValueDate", {
        get: function () { return this.EntityPM.ValueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CurrencyRatesModelData.prototype, "Rate", {
        get: function () { return this.EntityPM.Rate; },
        set: function (value) {
            if (this.EntityPM.Rate != value) {
                this.EntityPM.Rate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    return CurrencyRatesModelData;
}(BaseComponent_1.BaseComponent));
exports.CurrencyRatesModelData = CurrencyRatesModelData;
//# sourceMappingURL=CurrencyRatesComponent.js.map