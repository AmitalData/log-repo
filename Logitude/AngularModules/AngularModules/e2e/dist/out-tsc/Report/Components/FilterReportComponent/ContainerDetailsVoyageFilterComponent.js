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
var ContainerDetailsVoyageFilterComponent = /** @class */ (function (_super) {
    __extends(ContainerDetailsVoyageFilterComponent, _super);
    function ContainerDetailsVoyageFilterComponent() {
        var _this = _super.call(this) || this;
        _this.mySelectedDirectionFilter = "All";
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        return _this;
    }
    Object.defineProperty(ContainerDetailsVoyageFilterComponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (newValue == "All") {
                newValue = null;
            }
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ContainerDetailsVoyageFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
    };
    ContainerDetailsVoyageFilterComponent.prototype.ngOnInit = function () {
    };
    ContainerDetailsVoyageFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    ContainerDetailsVoyageFilterComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.MySelectedDirectionFilter == "All")
            this.MySelectedDirectionFilter = null;
        if (this.FromDate == null && this.ToDate == null && this.FromDateActual == null && this.ToDateActual == null) {
            this.ValidationErrorsList.push("At lease one date type range is mandatory");
        }
        else {
            if (this.FromDate != null && this.ToDate != null) {
                if (this.ToDate < this.FromDate) {
                    this.ValidationErrorsList.push("From date must be less than to date");
                }
            }
            if (this.FromDateActual != null && this.ToDateActual != null) {
                if (this.ToDateActual < this.FromDateActual) {
                    this.ValidationErrorsList.push("From date must be less than to date");
                }
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            if (this.CustomerId) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CustomerId";
                this.queryFilterItem.FieldValue = this.CustomerId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.mySelectedDirectionFilter) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "DirectionId";
                this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.VoyageNumber) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "VoyageNumber";
                this.queryFilterItem.FieldValue = this.VoyageNumber;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.FromDate) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateFromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.ToDate) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CreateToDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.FromDateActual) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalingFromDate";
                this.queryFilterItem.FieldValue = this.FromDateActual;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.ToDateActual) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalingToDate";
                this.queryFilterItem.FieldValue = this.ToDateActual;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
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
            this.ReportsPreview.CleanPartnersObslist();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            }
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    ContainerDetailsVoyageFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ContainerDetailsVoyageFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ContainerDetailsVoyageFilterComponent',
            templateUrl: './ContainerDetailsVoyageFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], ContainerDetailsVoyageFilterComponent);
    return ContainerDetailsVoyageFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ContainerDetailsVoyageFilterComponent = ContainerDetailsVoyageFilterComponent;
//# sourceMappingURL=ContainerDetailsVoyageFilterComponent.js.map