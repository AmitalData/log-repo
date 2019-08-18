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
var ReportFliter_1 = require("../../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../../Components/Filters/QueryFilterItem");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var InvoicesRoutingsFilterComponent = /** @class */ (function (_super) {
    __extends(InvoicesRoutingsFilterComponent, _super);
    function InvoicesRoutingsFilterComponent() {
        var _this = _super.call(this) || this;
        _this.IncludeVoidInvoices = false;
        _this.IncludeWaiting = false;
        _this.InvoiceStatusComboList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        _this.ObjectTableName = "Report";
        _this.CustomerId = null;
        _this.mySelectedTransportFilter = "All";
        _this.mySelectedDirectionFilter = "All";
        _this.IsInvoiceDate = true;
        _this.IsCreateDate = false;
        _this.IsLocalCurrency = true;
        _this.IsInvoiceCurrency = false;
        _this.IncludeStatusString = "";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.FillInvoiceStatus();
        _this.InitilizeIds();
        return _this;
    }
    InvoicesRoutingsFilterComponent.prototype.InitilizeIds = function () {
        this.InvoiceDateId = "InvoiceDateId_" + this.CurrentSession.GetNewId("InvoiceDateId");
        this.CreateDateId = "CreateDateId_" + this.CurrentSession.GetNewId("CreateDateId");
        this.ShipmentTypeRadioName = "ShipmentTypeRadioName_" + this.CurrentSession.GetNewId("ShipmentTypeRadioName");
        this.LocalCurrencyId = "LocalCurrencyId_" + this.CurrentSession.GetNewId("LocalCurrencyId");
        this.LocalCurencyRadio = "LocalCurencyRadio_" + this.CurrentSession.GetNewId("LocalCurencyRadio");
        this.InvoiceCurrencyId = "InvoiceCurrencyId_" + this.CurrentSession.GetNewId("InvoiceCurrencyId");
        this.ShipmentTypeRadio = "InvoiceDate";
    };
    Object.defineProperty(InvoicesRoutingsFilterComponent.prototype, "MySelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InvoicesRoutingsFilterComponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    InvoicesRoutingsFilterComponent.prototype.InvoiceStatusChange = function (event) {
        this.InvoiceStatusSelectedItem = event;
    };
    InvoicesRoutingsFilterComponent.prototype.settingShipmentTypeCode = function (code) {
        this.ShipmentTypeRadio = code;
    };
    Object.defineProperty(InvoicesRoutingsFilterComponent.prototype, "ShipmentTypeRadio", {
        get: function () {
            return this.shipmentTypeRadio;
        },
        set: function (code) {
            this.shipmentTypeRadio = code;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesRoutingsFilterComponent.prototype.ChangeCurrency = function (value) {
        this.IsLocalCurrency = value;
    };
    InvoicesRoutingsFilterComponent.prototype.FillInvoiceStatus = function () {
        this.InvoiceStatusComboList.push(new CodeNameClass_1.CodeNameClass("", "All"));
        this.InvoiceStatusComboList.push(new CodeNameClass_1.CodeNameClass("PD", "Paid"));
        this.InvoiceStatusComboList.push(new CodeNameClass_1.CodeNameClass("AD", "Unpaid"));
        this.InvoiceStatusSelectedItem = this.InvoiceStatusComboList.filter(function (d) { return d.Code == ""; })[0];
    };
    InvoicesRoutingsFilterComponent.prototype.IsLocalCurrencyClicked = function () {
        this.IsLocalCurrency = true;
        this.IsInvoiceCurrency = false;
    };
    InvoicesRoutingsFilterComponent.prototype.IsInvoiceCurrencyClicked = function () {
        this.IsLocalCurrency = false;
        this.IsInvoiceCurrency = true;
    };
    InvoicesRoutingsFilterComponent.prototype.Currency = function () {
    };
    Object.defineProperty(InvoicesRoutingsFilterComponent.prototype, "InvoiceType", {
        get: function () {
            return this.ShipmentTypeRadio;
        },
        enumerable: true,
        configurable: true
    });
    InvoicesRoutingsFilterComponent.prototype.IsInvoiceDateClicked = function () {
        this.IsInvoiceDate = true;
        this.IsCreateDate = false;
    };
    InvoicesRoutingsFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    };
    InvoicesRoutingsFilterComponent.prototype.InitializeComponent = function () {
        this.IsLocalCurrency = true;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    };
    InvoicesRoutingsFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    InvoicesRoutingsFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.ToDate < this.FromDate) {
            this.ValidationErrorsList.push("From date must be less than to date");
        }
        if (!this.InvoiceStatusSelectedItem) {
            this.ValidationErrorsList.push("Invoice status field is required");
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
            this.queryFilterItem.FieldName = "IsLocalCurrency";
            this.queryFilterItem.FieldValue = this.IsLocalCurrency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsByInvoiceDate";
            this.queryFilterItem.FieldValue = this.ShipmentTypeRadio == "InvoiceDate" ? true : false;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            var DirectionFilter = null;
            if (this.MySelectedDirectionFilter != "All")
                DirectionFilter = this.MySelectedDirectionFilter;
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionCode";
            this.queryFilterItem.FieldValue = DirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            var TransportMode = null;
            if (this.MySelectedTransportFilter != "All")
                TransportMode = this.MySelectedTransportFilter;
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TransportModeCode";
            this.queryFilterItem.FieldValue = TransportMode;
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
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "InvoiceStatusCode";
            this.queryFilterItem.FieldValue = this.InvoiceStatusSelectedItem.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            var reportFliter = new ReportFliter_1.ReportFliter();
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.RunReportEvent.emit(reportFliter);
        }
    };
    InvoicesRoutingsFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], InvoicesRoutingsFilterComponent.prototype, "RunReportEvent", void 0);
    InvoicesRoutingsFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'InvoicesRoutingsFilterComponent',
            templateUrl: './InvoicesRoutingsFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], InvoicesRoutingsFilterComponent);
    return InvoicesRoutingsFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.InvoicesRoutingsFilterComponent = InvoicesRoutingsFilterComponent;
//# sourceMappingURL=InvoicesRoutingsFilterComponent.js.map