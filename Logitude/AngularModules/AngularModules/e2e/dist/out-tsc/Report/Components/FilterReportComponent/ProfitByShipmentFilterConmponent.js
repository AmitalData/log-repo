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
var ProfitByShipmentFilterConmponent = /** @class */ (function (_super) {
    __extends(ProfitByShipmentFilterConmponent, _super);
    function ProfitByShipmentFilterConmponent() {
        var _this = _super.call(this) || this;
        _this.IncludeClosed = false;
        _this.IncludeAccounting = false;
        _this.IncludeAccountedOnly = false;
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode;
        _this.selectedCurrency = _this.LocalCurrencyCode;
        _this.mySelectedDirectionFilter = "All";
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsByCreateDate = false;
        _this.IsCreateDate = false;
        _this.IsOperationalDate = true;
        _this.IsCreateDateId = "IsCreateDateId_";
        _this.IsOperationalDateId = "IsOperationalDateId";
        _this.DateRadio = "DateRadio_";
        _this.IsOperationalDateId = _this.IsOperationalDateId + _this.CurrentSession.GetNewId(_this.IsOperationalDateId);
        _this.IsCreateDateId = _this.IsCreateDateId + _this.CurrentSession.GetNewId(_this.IsCreateDateId);
        _this.DateRadio = _this.DateRadio + _this.CurrentSession.GetNewId(_this.DateRadio);
        return _this;
    }
    Object.defineProperty(ProfitByShipmentFilterConmponent.prototype, "SelectedCurrency", {
        get: function () {
            return this.selectedCurrency;
        },
        set: function (value) {
            this.selectedCurrency = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProfitByShipmentFilterConmponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ProfitByShipmentFilterConmponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    };
    ProfitByShipmentFilterConmponent.prototype.ngOnInit = function () {
    };
    ProfitByShipmentFilterConmponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    ProfitByShipmentFilterConmponent.prototype.RunReport = function (isloading) {
        this.queryFilterItems = new Array();
        if (this.FromDate) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.ToDate) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.AgentId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AgentId";
            this.queryFilterItem.FieldValue = this.AgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.MySelectedDirectionFilter) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.IncludeAccounting) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AccountingClosed";
            this.queryFilterItem.FieldValue = this.IncludeAccounting;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.IncludeAccountedOnly) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeAccountedOnly";
            this.queryFilterItem.FieldValue = this.IncludeAccountedOnly;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.CustomerId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.SalesmanUserId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "SalesmanUserId";
            this.queryFilterItem.FieldValue = this.SalesmanUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.DepartmentId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DepartmentId";
            this.queryFilterItem.FieldValue = this.DepartmentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.CarrierId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CarrierId";
            this.queryFilterItem.FieldValue = this.CarrierId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "IsByCreateDate";
        this.queryFilterItem.FieldValue = this.IsByCreateDate;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        this.reportFliter = new ReportFliter_1.ReportFliter();
        this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        if (this.SelectedCurrency == this.LocalCurrencyCode)
            this.reportFliter.CurrentCurrencyCodeType = this.LocalCurrencyCode + ",local";
        else
            this.reportFliter.CurrentCurrencyCodeType = this.ProfitCurrencyCode + ",profit";
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;
        this.ReportsPreview.CleanPartnersObslist();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentId))
            this.ReportsPreview.AddPartner("Agent", this.AgentId);
        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    };
    ProfitByShipmentFilterConmponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ProfitByShipmentFilterConmponent.prototype.IsOperationalDateClicked = function () {
        this.IsByCreateDate = false;
    };
    ProfitByShipmentFilterConmponent.prototype.IsCreateDateClicked = function () {
        this.IsByCreateDate = true;
    };
    ProfitByShipmentFilterConmponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ProfitByShipmentFilterConmponent',
            templateUrl: './ProfitByShipmentFilterConmponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], ProfitByShipmentFilterConmponent);
    return ProfitByShipmentFilterConmponent;
}(BaseComponent_1.BaseComponent));
exports.ProfitByShipmentFilterConmponent = ProfitByShipmentFilterConmponent;
//# sourceMappingURL=ProfitByShipmentFilterConmponent.js.map