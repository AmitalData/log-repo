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
var ARInvoicesDepositReportFilterComponent = /** @class */ (function (_super) {
    __extends(ARInvoicesDepositReportFilterComponent, _super);
    function ARInvoicesDepositReportFilterComponent() {
        var _this = _super.call(this) || this;
        _this.LocalCurrency = null;
        _this.IncludeVoidInvoices = false;
        _this.IncludeWaiting = false;
        _this.ObjectTableName = "Report";
        _this.shipmentTypeRadio = "InvoiceDate";
        _this.IsInvoiceDate = true;
        _this.IsCreateDate = false;
        _this.IsLocalCurrency = true;
        _this.BranchId = null;
        _this.CustomerId = null;
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (_this.CurrentSession == null) {
            _this.LocalCurrency = "Local_-1_-1";
            _this.InvoiceCurrency = "Invoice_-1_-1";
        }
        else {
            var idIndex = _this.CurrentSession.GetNewId("RadioButton");
            _this.LocalCurrency = "Local_" + idIndex;
            _this.InvoiceCurrency = "Invoice_" + idIndex;
        }
        return _this;
    }
    ARInvoicesDepositReportFilterComponent.prototype.settingShipmentTypeCode = function (code) {
        this.ShipmentTypeRadio = code;
    };
    Object.defineProperty(ARInvoicesDepositReportFilterComponent.prototype, "ShipmentTypeRadio", {
        get: function () {
            return this.shipmentTypeRadio;
        },
        set: function (code) {
            this.shipmentTypeRadio = code;
        },
        enumerable: true,
        configurable: true
    });
    ARInvoicesDepositReportFilterComponent.prototype.setLocalCurrency = function (code) {
        if (code == "true")
            this.IsLocalCurrency = true;
        else if (code == "false")
            this.IsLocalCurrency = false;
    };
    ARInvoicesDepositReportFilterComponent.prototype.IsLocalCurrencyClicked = function () {
        this.IsLocalCurrency = true;
    };
    ARInvoicesDepositReportFilterComponent.prototype.Currency = function () {
    };
    Object.defineProperty(ARInvoicesDepositReportFilterComponent.prototype, "InvoiceType", {
        get: function () {
            return this.ShipmentTypeRadio;
        },
        enumerable: true,
        configurable: true
    });
    ARInvoicesDepositReportFilterComponent.prototype.IsInvoiceDateClicked = function () {
        this.IsInvoiceDate = true;
        this.IsCreateDate = false;
    };
    ARInvoicesDepositReportFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    };
    ARInvoicesDepositReportFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.IsLocalCurrency = true;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    };
    ARInvoicesDepositReportFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    ARInvoicesDepositReportFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is Required");
        }
        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is Required");
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
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LocalCurrency";
            this.queryFilterItem.FieldValue = this.IsLocalCurrency;
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
                this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    ARInvoicesDepositReportFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ARInvoicesDepositReportFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ARInvoicesDepositReportFilterComponent',
            templateUrl: './ARInvoicesDepositReportFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], ARInvoicesDepositReportFilterComponent);
    return ARInvoicesDepositReportFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ARInvoicesDepositReportFilterComponent = ARInvoicesDepositReportFilterComponent;
//# sourceMappingURL=ARInvoicesDepositReportFilterComponent.js.map