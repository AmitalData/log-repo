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
var Tools_1 = require("../../../../Infrastructure/Tools");
var Validator_1 = require("../../../../Infrastructure/Validators/Validator");
var RatesTablePM_1 = require("../../../../Infrastructure/EntityPMs/RatesTablePM");
var RatesTablePMService_1 = require("../../../../Infrastructure/Services/StandardPMs/RatesTablePMService");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CurrencyRatesService_1 = require("../../../../Common/Services/CurrencyRatesService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConfirmWindow_1 = require("../../../../Controls/Windows/ConfirmWindow");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var UpdateCurrencyRateComponent = /** @class */ (function (_super) {
    __extends(UpdateCurrencyRateComponent, _super);
    function UpdateCurrencyRateComponent(entityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityResourceService = entityResourceService;
        _this.WarningErrorsList = [];
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "RatesTable";
        _this.DataContext = _this;
        _this.RatesList = [];
        _this.IsResourcesReady = false;
        _this.isRTL = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrencyId = null;
        _this.CurrencyCode = null;
        _this.OldRate = null;
        _this.LoadDate = null;
        _this.IsEditingEnabled = true;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        _this.EntityPM = new RatesTablePM_1.RatesTablePM();
        _this.EntityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        _this.EntityPM.BaseCurrencyId = SessionLocator_1.SessionLocator.TenantPM.CurrencyId;
        _this.EntityPM.LogDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        _this.EntityPM.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.myService = new RatesTablePMService_1.RatesTablePMService();
        return _this;
    }
    UpdateCurrencyRateComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(function (res) {
            _this.IsResourcesReady = true;
            _this.OldRate = args['Rate'];
            _this.LoadDate = args['Date'];
            _this.CurrencyId = args['CurrencyId'];
            _this.CurrencyCode = args['CurrencyCode'];
            _this.EntityPM.ForeignCurrencyId = _this.CurrencyId;
            _this.EntityPM.ForeignCurrencyCode = _this.CurrencyCode;
            _this.EntityPM.Rate = _this.OldRate;
            _this.SetIsEditingEnabled();
        });
    };
    UpdateCurrencyRateComponent.prototype.SetIsEditingEnabled = function () {
        var isEditingEnabled = true;
        if (SessionLocator_1.SessionLocator.Tenant == 65) {
            isEditingEnabled = false;
            if (SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = true;
            }
        }
        this.IsEditingEnabled = isEditingEnabled;
    };
    Object.defineProperty(UpdateCurrencyRateComponent.prototype, "RateDate", {
        get: function () { return this.EntityPM.ValueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateCurrencyRateComponent.prototype, "LogDateTime", {
        get: function () { return this.EntityPM.LogDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(UpdateCurrencyRateComponent.prototype, "Rate", {
        get: function () { return this.EntityPM.Rate; },
        set: function (newValue) {
            if (this.EntityPM.Rate != newValue) {
                this.EntityPM.Rate = Tools_1.AppTool.Round(newValue, 5);
                this.ValidateRateWarningMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    UpdateCurrencyRateComponent.prototype.ValidateRateWarningMethod = function () {
        var warnings = [];
        if (this.Rate != null && this.OldRate != null) {
            var acceptRatio = 0.05;
            var rr = Math.abs(this.OldRate - this.Rate) / this.OldRate;
            if (rr > acceptRatio) {
                warnings.push(TextCodeTranslator_1.TextCodeTranslator.Translate("RatesTable.M.DefirenceIsMoreThan"));
            }
        }
        this.WarningErrorsList = warnings;
    };
    UpdateCurrencyRateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    UpdateCurrencyRateComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("RatesTable.M.ChangingTheExchangeRate") + this.Rate);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.SubmitChanges();
                }
            });
        }
    };
    UpdateCurrencyRateComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.myService.insert(this.EntityPM).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.LoadRates();
            }
        });
    };
    UpdateCurrencyRateComponent.prototype.LoadRates = function () {
        var _this = this;
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService_1.CurrencyRatesService();
        }
        var loadingDate = this.LoadDate;
        if (loadingDate == null) {
            loadingDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        }
        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator_1.SessionLocator.AccountingCurrencyId, loadingDate).subscribe(function (myResponse) {
            if (myResponse.HasError) {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                _this.RatesList = myResponse.Result;
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        });
    };
    UpdateCurrencyRateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './UpdateCurrencyRateComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], UpdateCurrencyRateComponent);
    return UpdateCurrencyRateComponent;
}(BaseComponent_1.BaseComponent));
exports.UpdateCurrencyRateComponent = UpdateCurrencyRateComponent;
//# sourceMappingURL=UpdateCurrencyRateComponent.js.map