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
var Tools_1 = require("../../../Infrastructure/Tools");
var InvoicesFilterComponent = /** @class */ (function (_super) {
    __extends(InvoicesFilterComponent, _super);
    function InvoicesFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.IncludeVoidInvoices = false;
        _this.IncludeWaiting = false;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.shipmentTypeRadio = "InvoiceDate";
        _this.IsInvoiceDate = true;
        _this.IsCreateDate = false;
        _this.IsLocalCurrency = true;
        _this.IsInvoiceCurrency = false;
        _this.BranchId = null;
        _this.IncludeStatusString = "";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.LocalCurrency = "LocalCurrency_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("LocalCurrency");
        _this.InvoiceCurrency = "InvoiceCurrency_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("InvoiceCurrency");
        _this.InvoiceDate = "InvoiceDate_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("InvoiceDate");
        _this.CreateDate = "CreateDate_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("CreateDate");
        _this.InvoiceTypeRadio = "InvoiceTypeRadio_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("InvoiceTypeRadio");
        _this.LocalCurencyRadio = "LocalCurencyRadio_" + _this.CurrentSession.SessionIndex + _this.CurrentSession.GetNewId("LocalCurencyRadio");
        return _this;
    }
    InvoicesFilterComponent.prototype.settingShipmentTypeCode = function (code) {
        this.ShipmentTypeRadio = code;
    };
    Object.defineProperty(InvoicesFilterComponent.prototype, "ShipmentTypeRadio", {
        get: function () {
            return this.shipmentTypeRadio;
        },
        set: function (code) {
            this.shipmentTypeRadio = code;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesFilterComponent.prototype.ChangeCurrency = function (value) {
        this.IsLocalCurrency = value;
    };
    InvoicesFilterComponent.prototype.IsLocalCurrencyClicked = function () {
        this.IsLocalCurrency = true;
        this.IsInvoiceCurrency = false;
    };
    InvoicesFilterComponent.prototype.IsInvoiceCurrencyClicked = function () {
        this.IsLocalCurrency = false;
        this.IsInvoiceCurrency = true;
    };
    InvoicesFilterComponent.prototype.Currency = function () {
    };
    Object.defineProperty(InvoicesFilterComponent.prototype, "InvoiceType", {
        get: function () {
            return this.ShipmentTypeRadio;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesFilterComponent.prototype.IsInvoiceDateClicked = function () {
        this.IsInvoiceDate = true;
        this.IsCreateDate = false;
    };
    InvoicesFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    };
    InvoicesFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.IsLocalCurrency = true;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        if (this.ReportsPreview.Report.Code == "RAPI") {
            this.IncludeStatusString = "Include Waiting For Approval Invoices";
        }
        else if (this.ReportsPreview.Report.Code == "RINV") {
            this.IncludeStatusString = "Include Draft Invoices";
        }
    };
    InvoicesFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    InvoicesFilterComponent.prototype.RunReport = function (isloading) {
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
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BranchId";
            this.queryFilterItem.FieldValue = this.BranchId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LocalCurrency";
            this.queryFilterItem.FieldValue = this.IsLocalCurrency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeVoidInvoices";
            this.queryFilterItem.FieldValue = this.IncludeVoidInvoices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeDraftInvoices";
            this.queryFilterItem.FieldValue = this.IncludeWaiting;
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
            this.ReportsPreview.CleanPartnersObslist();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
                this.ReportsPreview.AddPartner("Partner", this.CustomerId);
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    InvoicesFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    InvoicesFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'InvoicesFilterComponent',
            templateUrl: './InvoicesFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], InvoicesFilterComponent);
    return InvoicesFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.InvoicesFilterComponent = InvoicesFilterComponent;
//# sourceMappingURL=InvoicesFilterComponent.js.map