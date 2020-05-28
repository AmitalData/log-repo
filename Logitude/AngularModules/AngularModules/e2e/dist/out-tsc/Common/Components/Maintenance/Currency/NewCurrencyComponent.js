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
var CommonDomainService_1 = require("../../../Services/CommonDomainService");
var CurrencyListService_1 = require("../../../Services/StandardLists/CurrencyListService");
var InfraSettings_1 = require("../../../../Infrastructure/Utilities/InfraSettings");
var Tools_1 = require("../../../../Infrastructure/Tools");
var NewCurrencyComponent = /** @class */ (function (_super) {
    __extends(NewCurrencyComponent, _super);
    function NewCurrencyComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.LabelColumnWidth = 135;
        _this.ControlColumnWidth = 230;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.CurrencyList = [];
        _this.rateDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        _this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        _this.LoadCurrencyListMethod();
        _this.SetUIProperties();
        return _this;
    }
    NewCurrencyComponent.prototype.ngAfterViewInit = function () {
        //this.SetUIProperties();
    };
    NewCurrencyComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("CurrencyId", null, Tools_1.AppTool.IsNullOrEmpty(this.CurrencyId));
        this.UIProperties.SetRequired("RateDate", null, Tools_1.AppTool.IsNullOrEmpty(this.RateDate));
        this.UIProperties.SetRequired("CurrencyRate", null, Tools_1.AppTool.IsNullOrEmpty(this.CurrencyRate));
    };
    NewCurrencyComponent.prototype.LoadCurrencyListMethod = function () {
        var _this = this;
        var myService = new CurrencyListService_1.CurrencyListService();
        myService.getAll().subscribe(function (myResult) {
            if (myResult) {
                _this.CurrencyList = myResult.Result;
            }
        });
    };
    Object.defineProperty(NewCurrencyComponent.prototype, "CurrencyId", {
        get: function () { return this.currencyId; },
        set: function (newValue) {
            if (this.currencyId != newValue) {
                this.currencyId = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCurrencyComponent.prototype, "CurrencyRate", {
        get: function () { return this.currencyRate; },
        set: function (newValue) {
            if (this.currencyRate != newValue) {
                this.currencyRate = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCurrencyComponent.prototype, "RateDate", {
        get: function () { return this.rateDate; },
        set: function (newValue) {
            if (this.rateDate != newValue) {
                this.rateDate = newValue;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewCurrencyComponent.prototype, "SeletedCurrency", {
        get: function () { return this.seletedCurrency; },
        set: function (newValue) {
            if (this.seletedCurrency != newValue) {
                this.seletedCurrency = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    //Commands 
    NewCurrencyComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewCurrencyComponent.prototype.OkButtonClicked = function () {
        var _this = this;
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.CurrencyId)) {
            errors.push("Currency Field is Required");
        }
        if (this.RateDate == null) {
            errors.push("Exchage Rate Date Field is Required");
        }
        if (this.CurrencyRate == null || this.CurrencyRate <= 0) {
            errors.push("Exchange Rate Field is Required");
        }
        var ratedate = Tools_1.DateTool.GetDateParts(this.RateDate).DateObject;
        ratedate = Tools_1.DateTool.TruncateTime(ratedate);
        var today = Tools_1.DateTool.GetCurrentDateAsUtc();
        if (ratedate.valueOf() > today.valueOf()) {
            errors.push("Can't create currency with future date");
        }
        if (this.SeletedCurrency != null) {
            var list = this.CurrencyList.filter(function (c) { return c.Code == _this.SeletedCurrency.Code && c.Tenant == _this.TenantPM.Id; })[0];
            if (list != null) {
                errors.push("This currency already exists");
            }
        }
        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitCreatingCurrency();
        }
    };
    NewCurrencyComponent.prototype.SubmitCreatingCurrency = function () {
        var _this = this;
        var myService = new CommonDomainService_1.CommonDomainService();
        var month = this.RateDate.getMonth() + 1;
        var year = this.RateDate.getFullYear();
        var day = this.RateDate.getDate();
        var hour = this.RateDate.getHours();
        var minute = this.RateDate.getMinutes();
        var sec = this.RateDate.getSeconds();
        var millsec = this.RateDate.getMilliseconds();
        var myDate = year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + sec + "." + millsec;
        myService.CopyCurrencyToTenant(this.CurrencyId, this.CurrencyRate, myDate).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var newCreatedCurrency = myResponse.Result;
                _this.CurrentSession.CloseCurrentWindowEmit(newCreatedCurrency.Id);
            }
            else {
                _this.ValidationErrorsList = myResponse.ErrorsArray;
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewCurrencyComponent = __decorate([
        core_1.Component({
            selector: 'NewCurrencyComponent',
            moduleId: module.id,
            templateUrl: './NewCurrencyComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], NewCurrencyComponent);
    return NewCurrencyComponent;
}(BaseComponent_1.BaseComponent));
exports.NewCurrencyComponent = NewCurrencyComponent;
//# sourceMappingURL=NewCurrencyComponent.js.map