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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ReportsDomainService_1 = require("../../Services/ReportsDomainService");
var CodeNameClass_1 = require("./CodeNameClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var MonthlyConversionFilterComponent = /** @class */ (function (_super) {
    __extends(MonthlyConversionFilterComponent, _super);
    function MonthlyConversionFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.SelectedItem = "All";
        _this.CountryId = null;
        _this.IsByCreateDate = true;
        _this.IsStageDate = false;
        _this.IsCreateDate = true;
        _this.IsCreateDateId = "IsCreateDateId_";
        _this.IsStageDateId = "IsStageDateId";
        _this.ShipmentTypeRadio = "ShipmentTypeRadio_";
        _this.ResellerId = null;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.IsCRMTenant = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsStageDateId = _this.IsStageDateId + _this.CurrentSession.GetNewId(_this.IsStageDateId);
        _this.IsCreateDateId = _this.IsCreateDateId + _this.CurrentSession.GetNewId(_this.IsCreateDateId);
        _this.ShipmentTypeRadio = _this.ShipmentTypeRadio + _this.CurrentSession.GetNewId(_this.ShipmentTypeRadio);
        _this.reportDoaminService = new ReportsDomainService_1.ReportsDomainService();
        if (SessionLocator_1.SessionLocator.Tenant == 341) {
            _this.IsCRMTenant = true;
        }
        return _this;
    }
    MonthlyConversionFilterComponent.prototype.IsStageDateClicked = function () {
        this.IsByCreateDate = false;
    };
    MonthlyConversionFilterComponent.prototype.fillcombo = function (arr) {
        var _this = this;
        this.FilterdAdditionalService = [];
        arr.forEach(function (i) {
            if (!i.InActive) {
                var item = new CodeNameClass_1.CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                item.Checked = false;
                _this.FilterdAdditionalService.push(i);
            }
        });
        this.FilterdAdditionalService.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
    };
    MonthlyConversionFilterComponent.prototype.IsCreateDateClicked = function () {
        this.IsByCreateDate = true;
    };
    MonthlyConversionFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        var _this = this;
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        var fromDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        fromDate.setDate(1);
        var toDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var daysofmonth = this.daysInMonth(Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject);
        toDate.setDate(daysofmonth + 1);
        this.FromDate = fromDate;
        this.ToDate = toDate;
        this.reportDoaminService.GetLeadSourceLists(this.TenantPM.Id).subscribe(function (myResult) {
            _this.fillcombo(myResult);
        });
    };
    MonthlyConversionFilterComponent.prototype.SelectedViewByComboList = function (item) {
        this.SelectedViewItem = item;
    };
    MonthlyConversionFilterComponent.prototype.EditedItemSource = function (newSource) {
        this.FilterdAdditionalService = newSource;
    };
    MonthlyConversionFilterComponent.prototype.SelectedItemChanged = function (item) {
        this.SelectedItem = item;
    };
    MonthlyConversionFilterComponent.prototype.daysInMonth = function (aDate) {
        aDate.setMonth(aDate.getMonth() + 1);
        aDate.setDate(0);
        return (Tools_1.DateTool.GetDateParts(aDate).DateObject.getDate());
    };
    MonthlyConversionFilterComponent.prototype.RunReport = function (isloading) {
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
            var myAdditionalServices = "";
            if (this.SelectedItem == "All")
                myAdditionalServices = "All";
            else {
                this.FilterdAdditionalService.forEach(function (i) {
                    if (i.Checked) {
                        myAdditionalServices += i.Id + ",";
                    }
                });
            }
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LeadSources";
            this.queryFilterItem.FieldValue = myAdditionalServices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BusinessUnitId";
            this.queryFilterItem.FieldValue = this.BusinessUnitId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DataType";
            this.queryFilterItem.FieldValue = this.OpportunityTypeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "OwnerId";
            this.queryFilterItem.FieldValue = this.OwnerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CountryId";
            this.queryFilterItem.FieldValue = this.CountryId;
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
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    MonthlyConversionFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    MonthlyConversionFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MonthlyConversionFilterComponent',
            templateUrl: './MonthlyConversionFilterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MonthlyConversionFilterComponent);
    return MonthlyConversionFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.MonthlyConversionFilterComponent = MonthlyConversionFilterComponent;
//# sourceMappingURL=MonthlyConversionFilterComponent.js.map