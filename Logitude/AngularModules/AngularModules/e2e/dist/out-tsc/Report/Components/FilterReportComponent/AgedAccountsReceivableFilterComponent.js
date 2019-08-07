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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var core_1 = require("@angular/core");
var AgedAccountsReceivableFilterComponent = /** @class */ (function (_super) {
    __extends(AgedAccountsReceivableFilterComponent, _super);
    function AgedAccountsReceivableFilterComponent() {
        var _this = _super.call(this) || this;
        _this.IsAr = false;
        _this.IsAp = false;
        _this.IsAll = true;
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.shipmentTypeRadio = "All";
        _this.selectedCurrency = _this.LocalCurrencyCode;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        return _this;
    }
    AgedAccountsReceivableFilterComponent.prototype.settingShipmentTypeCode = function (code) {
        this.ShipmentTypeRadio = code;
    };
    Object.defineProperty(AgedAccountsReceivableFilterComponent.prototype, "ShipmentTypeRadio", {
        get: function () {
            return this.shipmentTypeRadio;
        },
        set: function (code) {
            this.shipmentTypeRadio = code;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgedAccountsReceivableFilterComponent.prototype, "InvoiceType", {
        get: function () {
            return this.ShipmentTypeRadio;
        },
        enumerable: true,
        configurable: true
    });
    AgedAccountsReceivableFilterComponent.prototype.allClicked = function () {
        this.IsAll = true;
        this.IsAp = false;
        this.IsAr = false;
    };
    AgedAccountsReceivableFilterComponent.prototype.IsApClicked = function () {
        this.IsAll = false;
        this.IsAp = true;
        this.IsAr = false;
    };
    AgedAccountsReceivableFilterComponent.prototype.IsArClicked = function () {
        this.IsAll = false;
        this.IsAp = false;
        this.IsAr = true;
    };
    Object.defineProperty(AgedAccountsReceivableFilterComponent.prototype, "SelectedCurrency", {
        get: function () {
            return this.selectedCurrency;
        },
        set: function (value) {
            this.selectedCurrency = value;
        },
        enumerable: true,
        configurable: true
    });
    AgedAccountsReceivableFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.RunReport(false);
    };
    AgedAccountsReceivableFilterComponent.prototype.ngOnInit = function () {
        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(65);
        //}
        //else {
        //}
        //this.RunReport(false);
    };
    AgedAccountsReceivableFilterComponent.prototype.RunReport = function (isloading) {
        this.queryFilterItems = new Array();
        this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "InvoiceType";
        this.queryFilterItem.FieldValue = this.InvoiceType;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        var currencyType;
        if (this.SelectedCurrency == this.LocalCurrencyCode)
            currencyType = "local";
        else
            currencyType = "profit";
        this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CurrencyType";
        this.queryFilterItem.FieldValue = currencyType;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        this.reportFliter = new ReportFliter_1.ReportFliter();
        this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    };
    AgedAccountsReceivableFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AgedAccountsReceivableFilterComponent',
            templateUrl: './AgedAccountsReceivableFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], AgedAccountsReceivableFilterComponent);
    return AgedAccountsReceivableFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.AgedAccountsReceivableFilterComponent = AgedAccountsReceivableFilterComponent;
//# sourceMappingURL=AgedAccountsReceivableFilterComponent.js.map