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
var ReportsDomainService_1 = require("../../Services/ReportsDomainService");
var CodeNameClass_1 = require("./CodeNameClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var EAWBFilterComponent = /** @class */ (function (_super) {
    __extends(EAWBFilterComponent, _super);
    function EAWBFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        return _this;
    }
    EAWBFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
        this.GetTenants();
    };
    EAWBFilterComponent.prototype.ngOnInit = function () {
    };
    EAWBFilterComponent.prototype.GetTenants = function () {
        var _this = this;
        var mySerivce = new ReportsDomainService_1.ReportsDomainService();
        mySerivce.GetActivityStatus(this.TenantPM.Id).subscribe(function (result) {
            _this.UpdateTenantsList(result);
        });
    };
    EAWBFilterComponent.prototype.UpdateTenantsList = function (result) {
        var _this = this;
        this.TenantsComboList = [];
        result.sort(function (a, b) { return (a.ForwarderTenantName === b.ForwarderTenantName) ? 0 : (a.ForwarderTenantName > b.ForwarderTenantName) ? -1 : 1; });
        result.forEach(function (item) {
            var obj = new CodeNameClass_1.CodeNameClass();
            obj.Code = item.ForwarderTenant.toString();
            obj.Name = item.ForwarderTenantName;
            _this.TenantsComboList.push(obj);
        });
    };
    EAWBFilterComponent.prototype.RunReport = function (isloading) {
        this.queryFilterItems = new Array();
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
        if (this.MainCarriageFromPortId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFromPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFromPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.MainCarriageFinalDestinationPortId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFinalDestinationPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFinalDestinationPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.SelectedItemComboBox) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.SelectedItemComboBox;
            this.queryFilterItem.Operator = "CustomerId";
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
        this.reportFliter.IncludeOperationalyClosed = false;
        this.ReportsPreview.CleanPartnersObslist();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedItemComboBox)) {
            this.ReportsPreview.AddPartner("Participant", this.SelectedItemComboBox);
        }
        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    };
    EAWBFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    EAWBFilterComponent.prototype.DaysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    };
    EAWBFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'EAWBFilterComponent',
            templateUrl: './EAWBFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], EAWBFilterComponent);
    return EAWBFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.EAWBFilterComponent = EAWBFilterComponent;
//# sourceMappingURL=EAWBFilterComponent.js.map