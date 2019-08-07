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
var StatisticsByCustomerFilterComponent = /** @class */ (function (_super) {
    __extends(StatisticsByCustomerFilterComponent, _super);
    function StatisticsByCustomerFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.IncludeClosed = false;
        _this.CustomerId = null;
        _this.IsCreateDateId = "IsCreateDateId";
        _this.IsOperationalDateId = "IsOperationalDateId";
        _this.DateRadio = "DateRadio_";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsByCreateDate = true;
        _this.IsOperationalDate = false;
        _this.IsCreateDate = true;
        _this.selectedCurrency = _this.LocalCurrencyCode;
        _this.mySelectedTransportFilter = "All";
        _this.mySelectedDirectionFilter = "All";
        _this.LevelCodeSelectedValue = "All";
        _this.IsOperationalDateId = _this.IsOperationalDateId + _this.CurrentSession.GetNewId(_this.IsOperationalDateId);
        _this.IsCreateDateId = _this.IsCreateDateId + _this.CurrentSession.GetNewId(_this.IsCreateDateId);
        _this.DateRadio = _this.DateRadio + _this.CurrentSession.GetNewId(_this.DateRadio);
        return _this;
    }
    StatisticsByCustomerFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    };
    StatisticsByCustomerFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsByCreateDate = true;
    };
    StatisticsByCustomerFilterComponent.prototype.IsOperationalDateClicked = function () {
        this.IsByCreateDate = false;
    };
    Object.defineProperty(StatisticsByCustomerFilterComponent.prototype, "SelectedCurrency", {
        get: function () {
            return this.selectedCurrency;
        },
        set: function (value) {
            this.selectedCurrency = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatisticsByCustomerFilterComponent.prototype, "SelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (value) {
            if (this.mySelectedTransportFilter != value) {
                this.mySelectedTransportFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(StatisticsByCustomerFilterComponent.prototype, "SelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (value) {
            if (this.mySelectedDirectionFilter != value) {
                this.mySelectedDirectionFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    StatisticsByCustomerFilterComponent.prototype.LevelCodeitemClicked = function (itemValue) {
        if (this.LevelCodeSelectedValue != itemValue) {
            this.LevelCodeSelectedValue = itemValue;
        }
    };
    StatisticsByCustomerFilterComponent.prototype.LevelCodeMouseOver = function (itemValue) {
        if (this.LevelCodeSelectedValue != itemValue) {
        }
    };
    StatisticsByCustomerFilterComponent.prototype.LevelCodeMouseLeave = function (itemValue) {
        if (this.LevelCodeSelectedValue != itemValue) {
        }
    };
    StatisticsByCustomerFilterComponent.prototype.RunReport = function (isloading) {
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
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsByCreateDate";
            this.queryFilterItem.FieldValue = this.IsByCreateDate;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
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
            if (this.SelectedDirectionFilter != "All") {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "Direction";
                this.queryFilterItem.FieldValue = this.SelectedDirectionFilter;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.SelectedTransportFilter != "All") {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "TransportMode";
                this.queryFilterItem.FieldValue = this.SelectedTransportFilter;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.LevelCodeSelectedValue != "All") {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ShipmentLevel";
                this.queryFilterItem.FieldValue = this.LevelCodeSelectedValue;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            var currency;
            if (this.SelectedCurrency == this.LocalCurrencyCode) {
                currency = this.LocalCurrencyCode + ",local";
            }
            else {
                currency = this.ProfitCurrencyCode + ",profit";
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "Currency";
            this.queryFilterItem.FieldValue = currency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeOperationalClosed";
            this.queryFilterItem.FieldValue = this.IncludeClosed;
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
            //this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;
            this.ReportsPreview.CleanPartnersObslist();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            }
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    StatisticsByCustomerFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    StatisticsByCustomerFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    StatisticsByCustomerFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'StatisticsByCustomerFilterComponent',
            templateUrl: './StatisticsByCustomerFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], StatisticsByCustomerFilterComponent);
    return StatisticsByCustomerFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.StatisticsByCustomerFilterComponent = StatisticsByCustomerFilterComponent;
//# sourceMappingURL=StatisticsByCustomerFilterComponent.js.map