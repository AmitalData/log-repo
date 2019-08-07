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
var forms_1 = require("@angular/forms");
var Tools_1 = require("../../../Infrastructure/Tools");
var LastFilter_1 = require("../../../Infrastructure/Utilities/LastFilter");
var CodeNameClass_1 = require("./CodeNameClass");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeListBoxComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent");
var ActivityStatusDashboardFilterComponent = /** @class */ (function (_super) {
    __extends(ActivityStatusDashboardFilterComponent, _super);
    function ActivityStatusDashboardFilterComponent(fb) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "FlightsSchedulesResponse";
        _this.mySelectedTransportFilter = "All";
        _this.mySelectedDirectionFilter = "All";
        _this.myForm = fb.group({});
        return _this;
    }
    Object.defineProperty(ActivityStatusDashboardFilterComponent.prototype, "TimeRangeComboList", {
        get: function () { return this.timeRangeComboList; },
        set: function (value) { this.timeRangeComboList = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDashboardFilterComponent.prototype, "MySelectedTransportFilter", {
        get: function () { return this.mySelectedTransportFilter; },
        set: function (newValue) {
            if (this.mySelectedTransportFilter != newValue) {
                this.mySelectedTransportFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDashboardFilterComponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityStatusDashboardFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.timeRangeComboList = LastFilter_1.LastFilter.myList();
        var lastFilter = new LastFilter_1.LastFilter();
        lastFilter.lastTitle = "Custom";
        lastFilter.LastDays = 0;
        lastFilter.Lastmonths = 0;
        this.timeRangeComboList.push(lastFilter);
        this.SelectedItem = this.timeRangeComboList[0];
        this.ComputeDays();
        this.BuildShowTypesFilters();
    };
    ActivityStatusDashboardFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, aDate.getDate() + 1)).getDate();
    };
    ActivityStatusDashboardFilterComponent.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedItem.LastDays == -7) {
            days = -7;
            Todate.setDate(Todate.getDate() - 6);
            this.activityToDate = Todate;
        }
        else if (this.SelectedItem.LastDays == -30) {
            days = -30;
            Todate.setMonth(Todate.getMonth() - 1);
            this.activityToDate = Todate;
        }
        else if (this.SelectedItem.LastDays == -90) {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }
        else if (this.SelectedItem.LastDays == -365) {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }
        return days;
    };
    ActivityStatusDashboardFilterComponent.prototype.ngOnInit = function () {
        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(80);
        //}
        //this.timeRangeComboList = LastFilter.myList();
        //this.SelectedItem = this.timeRangeComboList[0];
        //this.BuildShowTypesFilters();
    };
    Object.defineProperty(ActivityStatusDashboardFilterComponent.prototype, "ActivityToDate", {
        get: function () { return this.activityToDate; },
        set: function (value) {
            if (value != this.activityToDate) {
                this.activityToDate = value;
                this.SelectedItem = this.TimeRangeComboList[4];
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityStatusDashboardFilterComponent.prototype, "ActivityFromDate", {
        get: function () { return this.activityFromDate; },
        set: function (value) {
            if (value != this.activityFromDate) {
                this.activityFromDate = value;
                this.SelectedItem = this.TimeRangeComboList[4];
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityStatusDashboardFilterComponent.prototype.onSelectedItemChanged = function (item) {
        this.SelectedItem = item;
        this.ComputeDays();
    };
    ActivityStatusDashboardFilterComponent.prototype.onSelectedItemShowChanged = function (item) {
        this.SelectedItemShow = item;
    };
    ActivityStatusDashboardFilterComponent.prototype.BuildShowTypesFilters = function () {
        this.showComboList = [];
        var item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "SHI";
        item1.Name = "Shipments";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "CHW";
        item1.Name = "ChargeWeight";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "GSW";
        item1.Name = "GrossWeight";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "PAC";
        item1.Name = "Profit " + "(" + SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode + ")";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "PPT";
        item1.Name = "Profit " + "(" + SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode + ")";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "RAC";
        //Receivables
        item1.Name = "Receivables " + "(" + SessionLocator_1.SessionLocator.TenantPM.AccountingCurrencyCode + ")";
        this.showComboList.push(item1);
        item1 = new CodeNameClass_1.CodeNameClass();
        item1.Code = "RPT";
        item1.Name = "Receivables " + "(" + SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode + ")";
        this.showComboList.push(item1);
        this.SelectedItemShow = this.showComboList[0];
    };
    ActivityStatusDashboardFilterComponent.prototype.RunReport = function () {
        this.ValidationErrorsList = [];
        if (!this.SelectedItem) {
            this.ValidationErrorsList.push("Time Range field is required");
        }
        if (!this.SelectedItemShow) {
            this.ValidationErrorsList.push("Show field is required");
        }
        if (this.SelectedItem.lastTitle == "Custom") {
            if (this.ActivityFromDate == null)
                this.ValidationErrorsList.push("From Date field is required");
            if (this.ActivityToDate == null)
                this.ValidationErrorsList.push("To Date field is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            var showIndex;
            switch (this.SelectedItemShow.Code) {
                case "SHI":
                    {
                        showIndex = 0;
                        break;
                    }
                case "CHW":
                    {
                        showIndex = 1;
                        break;
                    }
                case "GSW":
                    {
                        showIndex = 2;
                        break;
                    }
                case "PAC":
                    {
                        showIndex = 3;
                        break;
                    }
                case "PPT":
                    {
                        showIndex = 4;
                        break;
                    }
                case "RAC":
                    {
                        showIndex = 5;
                        break;
                    }
                case "RPT":
                    {
                        showIndex = 6;
                        break;
                    }
            }
            if (this.MySelectedDirectionFilter == "All")
                this.MySelectedDirectionFilter = "";
            if (this.MySelectedTransportFilter == "All")
                this.MySelectedTransportFilter = "";
            this.queryFilterItems = [];
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LastDays";
            this.queryFilterItem.FieldValue = this.SelectedItem.LastDays;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LastMonths";
            this.queryFilterItem.FieldValue = 0 + "";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShowIndex";
            this.queryFilterItem.FieldValue = showIndex;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TransportModeId";
            this.queryFilterItem.FieldValue = this.MySelectedTransportFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.ActivityToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ActivityFromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TimeRange";
            this.queryFilterItem.FieldValue = this.SelectedItem.lastTitle;
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
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    };
    ActivityStatusDashboardFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ActivityStatusDashboardFilterComponent',
            templateUrl: './ActivityStatusDashboardFilterComponent.html',
            inputs: ['ReportsPreview'],
            entryComponents: [LogitudeListBoxComponent_1.LogitudeListBoxComponent]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], ActivityStatusDashboardFilterComponent);
    return ActivityStatusDashboardFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ActivityStatusDashboardFilterComponent = ActivityStatusDashboardFilterComponent;
//# sourceMappingURL=ActivityStatusDashboardFilterComponent.js.map