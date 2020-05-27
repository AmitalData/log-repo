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
var core_1 = require("@angular/core");
var Tools_1 = require("../../../Infrastructure/Tools");
var CarrierStatisticFilterComponent = /** @class */ (function (_super) {
    __extends(CarrierStatisticFilterComponent, _super);
    function CarrierStatisticFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.DataContext = _this;
        _this.ObjectTableName = "Report";
        _this.IncludeClosed = false;
        _this.mySelectedDirectionFilter = "All";
        return _this;
    }
    CarrierStatisticFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    };
    CarrierStatisticFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    CarrierStatisticFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    Object.defineProperty(CarrierStatisticFilterComponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    CarrierStatisticFilterComponent.prototype.RunReport = function (isloading) {
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
            if (this.MySelectedDirectionFilter == "All") {
                this.MySelectedDirectionFilter = null;
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsOperationalClosed";
            this.queryFilterItem.FieldValue = this.IncludeClosed;
            this.queryFilterItem.FieldDataType = "Boolean";
            this.queryFilterItem.Operator = "Equal";
            this.queryFilterItems.push(this.queryFilterItem);
            if (!Tools_1.AppTool.IsNullOrEmpty(this.MySelectedDirectionFilter)) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "Direction";
                this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    CarrierStatisticFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CarrierStatisticFilterComponent',
            templateUrl: './CarrierStatisticFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], CarrierStatisticFilterComponent);
    return CarrierStatisticFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.CarrierStatisticFilterComponent = CarrierStatisticFilterComponent;
//# sourceMappingURL=CarrierStatisticFilterComponent.js.map