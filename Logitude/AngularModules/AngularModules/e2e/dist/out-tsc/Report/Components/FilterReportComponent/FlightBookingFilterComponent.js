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
var FlightBookingFilterComponent = /** @class */ (function (_super) {
    __extends(FlightBookingFilterComponent, _super);
    function FlightBookingFilterComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "FlightsSchedulesResponse";
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = _this.daysInMonth(new Date());
        _this.FlightDate = _this.SetDate(Year, month, 1);
        return _this;
    }
    Object.defineProperty(FlightBookingFilterComponent.prototype, "FlightDate", {
        get: function () { return this.flightDate; },
        set: function (value) { if (this.flightDate != value)
            this.flightDate = value; },
        enumerable: true,
        configurable: true
    });
    FlightBookingFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    FlightBookingFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
    };
    FlightBookingFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, aDate.getDate() + 1)).getDate();
    };
    FlightBookingFilterComponent.prototype.ngOnInit = function () {
    };
    FlightBookingFilterComponent.prototype.RunReport = function () {
        this.ValidationErrorsList = [];
        if (this.FlightNumber == null || this.FlightNumber.trim() == '') {
            this.ValidationErrorsList.push("Flight Number is required");
        }
        if (this.FlightDate == null) {
            this.ValidationErrorsList.push("Flight Date is required");
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FlightDate";
            this.queryFilterItem.FieldValue = this.FlightDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FlightNumber";
            this.queryFilterItem.FieldValue = this.FlightNumber;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomAgentId";
            this.queryFilterItem.FieldValue = this.CustomAgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            if (!this.DateType) {
                this.DateType = "CreateDate";
            }
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.DateType = this.DateType;
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
    FlightBookingFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'FlightBookingFilterComponent',
            templateUrl: './FlightBookingFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], FlightBookingFilterComponent);
    return FlightBookingFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.FlightBookingFilterComponent = FlightBookingFilterComponent;
//# sourceMappingURL=FlightBookingFilterComponent.js.map