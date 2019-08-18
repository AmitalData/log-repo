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
var Validator_1 = require("../../../Infrastructure/Validators/Validator");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var RatesTablePMService_1 = require("../../../Infrastructure/Services/StandardPMs/RatesTablePMService");
var RatesTablePM_1 = require("../../../Infrastructure/EntityPMs/RatesTablePM");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var EditLastRateComponent = /** @class */ (function (_super) {
    __extends(EditLastRateComponent, _super);
    function EditLastRateComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "RatesTable";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.WarningsList = [];
        _this.IsEditingEnabled = false;
        _this.TodayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        _this.RatesTable = new RatesTablePM_1.RatesTablePM();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.OldRate = null;
        _this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        return _this;
    }
    EditLastRateComponent.prototype.CreateRatesTablePM = function () {
        this.RatesTable = new RatesTablePM_1.RatesTablePM();
        this.RatesTable.Tenant = this.TenantPM.Id;
        this.RatesTable.BaseCurrencyId = this.TenantPM.CurrencyId;
        this.RatesTable.ForeignCurrencyId = this.EntityPM.ForeignCurrencyId;
        this.RatesTable.LogDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
    };
    EditLastRateComponent.prototype.SetDataContext = function (dataContext) {
        this.IsEditingEnabled = dataContext.IsEditingEnabled;
        this.EntityPM = dataContext.LastRate;
        this.OldRate = this.EntityPM.Rate;
        this.CurrentRate = dataContext.CurrentRate;
        this.CurrentValueDate = dataContext.CurrentValueDate;
        this.CreateRatesTablePM();
    };
    Object.defineProperty(EditLastRateComponent.prototype, "Rate", {
        get: function () { return this.RatesTable.Rate; },
        set: function (value) {
            if (this.RatesTable.Rate != value) {
                this.RatesTable.Rate = Tools_1.AppTool.Round(value, 5);
                this.ValidateRateWarningMethod();
            }
        },
        enumerable: true,
        configurable: true
    });
    EditLastRateComponent.prototype.ValidateRateWarningMethod = function () {
        var warnings = [];
        if (this.Rate != null && this.OldRate != null) {
            var acceptRatio = 0.05;
            var rr = Math.abs(this.OldRate - this.Rate) / this.OldRate;
            if (rr > acceptRatio) {
                warnings.push("Difference between new and old value is more than 0.05");
            }
        }
        this.WarningsList = warnings;
    };
    Object.defineProperty(EditLastRateComponent.prototype, "ValueDate", {
        get: function () { return this.RatesTable.ValueDate; },
        set: function (value) {
            if (this.RatesTable.ValueDate != value) {
                this.RatesTable.ValueDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditLastRateComponent.prototype.SetAsToday = function () {
        this.ValueDate = Tools_1.DateTool.GetCurrentDateAsUtc();
    };
    EditLastRateComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    EditLastRateComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        Validator_1.Validator.TryValidateObject(this.RatesTable, this.ObjectTableName, errors);
        if (errors.length == 0) {
            if (this.ValueDate == null) {
                errors.push("Date is required");
            }
            else {
                var valueDate = new Date(this.ValueDate.valueOf()).valueOf();
                var today = Tools_1.DateTool.GetCurrentDateAsUtc().valueOf();
                if (valueDate > today) {
                    errors.push("Cant add future date rate");
                }
            }
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
            confirmWindow.Show("Please confirm changing the exchange rate to " + this.Rate);
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.SubmitChanges();
                }
            });
        }
    };
    EditLastRateComponent.prototype.SubmitChanges = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var myService = new RatesTablePMService_1.RatesTablePMService();
        myService.insert(this.RatesTable).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.CurrentSession.CloseCurrentWindowEmit("ok");
                }
                else {
                    _this.ValidationErrorsList = myResponse.ErrorsArray;
                    _this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    };
    EditLastRateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './EditLastRateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], EditLastRateComponent);
    return EditLastRateComponent;
}(BaseComponent_1.BaseComponent));
exports.EditLastRateComponent = EditLastRateComponent;
//# sourceMappingURL=EditLastRateComponent.js.map