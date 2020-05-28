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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var core_1 = require("@angular/core");
var CodeNameClass_1 = require("./CodeNameClass");
var ExpectedIncomeFilterComponent = /** @class */ (function (_super) {
    __extends(ExpectedIncomeFilterComponent, _super);
    function ExpectedIncomeFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.mySelectedDirectionFilter = "All";
        _this.IsCRMTenant = false;
        _this.RecurringComboList = [];
        _this.PaymentChannelCode = null;
        _this.CountryId = null;
        _this.ResellerId = null;
        if (SessionLocator_1.SessionLocator.Tenant == 341) {
            _this.IsCRMTenant = true;
        }
        return _this;
    }
    ExpectedIncomeFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        //this.FromDate = this.SetDate(Year, month - 1, 1);
        //this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
        this.BuildRecurringComboList();
    };
    ExpectedIncomeFilterComponent.prototype.BuildRecurringComboList = function () {
        this.RecurringComboList = [];
        this.RecurringComboList.push(new CodeNameClass_1.CodeNameClass("A", "All"));
        this.RecurringComboList.push(new CodeNameClass_1.CodeNameClass("Y", "Yes"));
        this.RecurringComboList.push(new CodeNameClass_1.CodeNameClass("N", "No"));
        this.selectedItemComboBox = this.RecurringComboList.filter(function (d) { return d.Code == "A"; })[0];
    };
    Object.defineProperty(ExpectedIncomeFilterComponent.prototype, "SelectedItemComboBox", {
        get: function () { return this.selectedItemComboBox; },
        set: function (value) {
            if (this.selectedItemComboBox != value) {
                this.selectedItemComboBox = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ExpectedIncomeFilterComponent.prototype.RunReport = function () {
        this.queryFilterItems = new Array();
        this.ValidationErrorsList = [];
        if (!this.SelectedItemComboBox) {
            this.ValidationErrorsList.push("Recurring field is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            if (this.FromDate != null) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "FromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.ToDate != null) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ToDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "Recurring";
            this.queryFilterItem.FieldValue = this.SelectedItemComboBox.Code;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            if (this.SalesmanId) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalesmanId";
                this.queryFilterItem.FieldValue = this.SalesmanId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.PaymentChannelCode) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "PaymentChannelCode";
                this.queryFilterItem.FieldValue = this.PaymentChannelCode;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.CountryId) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CountryId";
                this.queryFilterItem.FieldValue = this.CountryId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.IsCRMTenant) {
                if (this.ResellerId) {
                    this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                    this.queryFilterItem.DisplayInList = false;
                    this.queryFilterItem.FieldName = "ResellerId";
                    this.queryFilterItem.FieldValue = this.ResellerId;
                    this.queryFilterItem.Operator = "Equals";
                    this.queryFilterItems.push(this.queryFilterItem);
                }
            }
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    };
    ExpectedIncomeFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ExpectedIncomeFilterComponent.prototype.DaysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    ExpectedIncomeFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ExpectedIncomeFilterComponent',
            templateUrl: './ExpectedIncomeFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExpectedIncomeFilterComponent);
    return ExpectedIncomeFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ExpectedIncomeFilterComponent = ExpectedIncomeFilterComponent;
//# sourceMappingURL=ExpectedIncomeFilterComponent.js.map