"use strict";
/// <reference path="codenameclass.ts" />
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
var Tools_1 = require("../../../Infrastructure/Tools");
var InvoicesByPartnerFilterComponent = /** @class */ (function (_super) {
    __extends(InvoicesByPartnerFilterComponent, _super);
    function InvoicesByPartnerFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.ValidationErrorsList = [];
        _this.IncludeClosed = false;
        _this.selectedCurrency = _this.LocalCurrencyCode;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.IsInvoiceDate = true;
        _this.IsCreateDate = false;
        _this.shipmentTypeRadio = "InvoiceDate";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(InvoicesByPartnerFilterComponent.prototype, "SelectedCurrency", {
        get: function () {
            return this.selectedCurrency;
        },
        set: function (value) {
            this.selectedCurrency = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoicesByPartnerFilterComponent.prototype, "InvoiceType", {
        get: function () {
            return this.ShipmentTypeRadio;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesByPartnerFilterComponent.prototype.settingShipmentTypeCode = function (code) {
        this.ShipmentTypeRadio = code;
    };
    Object.defineProperty(InvoicesByPartnerFilterComponent.prototype, "ShipmentTypeRadio", {
        get: function () {
            return this.shipmentTypeRadio;
        },
        set: function (code) {
            this.shipmentTypeRadio = code;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesByPartnerFilterComponent.prototype.IsInvoiceDateClicked = function () {
        this.IsInvoiceDate = true;
        this.IsCreateDate = false;
    };
    InvoicesByPartnerFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    };
    InvoicesByPartnerFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
    };
    InvoicesByPartnerFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    InvoicesByPartnerFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }
        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");
        }
        if (this.FromDate > this.ToDate) {
            this.ValidationErrorsList.push("From Date cannot be greater than To Date");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            if (this.CustomerId) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "BillToId";
                this.queryFilterItem.FieldValue = this.CustomerId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            if (this.SelectedCurrency == this.LocalCurrencyCode)
                this.reportFliter.CurrentCurrencyCodeType = this.LocalCurrencyCode + ",local";
            else
                this.reportFliter.CurrentCurrencyCodeType = this.ProfitCurrencyCode + ",profit";
            this.reportFliter.DateType = this.InvoiceType;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
                this.ReportsPreview.AddPartner("Partner", this.CustomerId);
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    InvoicesByPartnerFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    InvoicesByPartnerFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'InvoicesByPartnerFilterComponent',
            templateUrl: './InvoicesByPartnerFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], InvoicesByPartnerFilterComponent);
    return InvoicesByPartnerFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.InvoicesByPartnerFilterComponent = InvoicesByPartnerFilterComponent;
//# sourceMappingURL=InvoicesByPartnerFilterComponent.js.map