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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var EmployeeTimeSheetFilterComponent = /** @class */ (function (_super) {
    __extends(EmployeeTimeSheetFilterComponent, _super);
    function EmployeeTimeSheetFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.AgentId = null;
        _this.ObjectTableName = "TMEmployeeTime";
        _this.dateOfWorkDateFormat = "";
        _this.DateOfWorkMinutes = 540;
        return _this;
    }
    ;
    Object.defineProperty(EmployeeTimeSheetFilterComponent.prototype, "DateOfWorkMinutes", {
        get: function () {
            return this.dateOfWorkMinutes;
        },
        set: function (value) {
            if (this.dateOfWorkMinutes != value) {
                this.dateOfWorkMinutes = value;
                this.DateOfWorkDateFormat = this.ApplyTimeFormat(value);
            }
        },
        enumerable: true,
        configurable: true
    });
    EmployeeTimeSheetFilterComponent.prototype.ApplyTimeFormat = function (minutes) {
        var formattedMinutes = "";
        var val = minutes;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            formattedMinutes = "";
        }
        else {
            formattedMinutes = result;
        }
        return formattedMinutes;
    };
    Object.defineProperty(EmployeeTimeSheetFilterComponent.prototype, "DateOfWorkDateFormat", {
        get: function () {
            return this.dateOfWorkDateFormat;
        },
        set: function (value) {
            if (this.dateOfWorkDateFormat != value) {
                this.dateOfWorkDateFormat = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    EmployeeTimeSheetFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.EmployeeUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.ReportsPreview = myReportsPreview;
    };
    EmployeeTimeSheetFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    EmployeeTimeSheetFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    EmployeeTimeSheetFilterComponent.prototype.RunReport = function () {
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
        if (this.EmployeeUserId == null) {
            this.ValidationErrorsList.push("Employee is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "EmployeeUserId";
            this.queryFilterItem.FieldValue = this.EmployeeUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TimeRequired";
            this.TimeRequired = this.DateOfWorkMinutes;
            this.queryFilterItem.FieldValue = this.TimeRequired;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.reportFliter = new ReportFliter_1.ReportFliter();
            //this.reportFliter.DateType = "CreateDate";
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
    EmployeeTimeSheetFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EmployeeTimeSheetFilterComponent',
            templateUrl: './EmployeeTimeSheetFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], EmployeeTimeSheetFilterComponent);
    return EmployeeTimeSheetFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.EmployeeTimeSheetFilterComponent = EmployeeTimeSheetFilterComponent;
//# sourceMappingURL=EmployeeTimeSheetFilterComponent.js.map