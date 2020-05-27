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
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var OceanShipmentReportFilterComponent = /** @class */ (function (_super) {
    __extends(OceanShipmentReportFilterComponent, _super);
    function OceanShipmentReportFilterComponent() {
        var _this = _super.call(this) || this;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.ShipmentTypeRadioId = "";
        _this.IsDomestic = false;
        _this.IsExport = false;
        _this.IsImport = false;
        _this.IncludeClosed = false;
        _this.SelectedCurrency = "USD,profit";
        _this.InActive = false;
        _this.ShipmentTypeRadio = "All";
        _this.ShipmentTypeRadioId = Guid_1.Guid.newGuid();
        return _this;
    }
    OceanShipmentReportFilterComponent.prototype.SetShipmentTypeRadio = function (value) {
        if (this.ShipmentTypeRadio != value) {
            this.ShipmentTypeRadio = value;
        }
    };
    Object.defineProperty(OceanShipmentReportFilterComponent.prototype, "CurrentDirectionId", {
        get: function () {
            var s = "";
            if (this.IsImport) {
                s = s + "I";
            }
            if (this.IsExport) {
                s = s + ",E";
            }
            if (this.IsDomestic) {
                s = s + ",D";
            }
            return s;
        },
        enumerable: true,
        configurable: true
    });
    OceanShipmentReportFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    };
    OceanShipmentReportFilterComponent.prototype.ChangeCurrency = function (code) {
        if (code != this.SelectedCurrency) {
            this.SelectedCurrency = code;
        }
    };
    OceanShipmentReportFilterComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    OceanShipmentReportFilterComponent.prototype.RunReport = function (isloading) {
        this.queryFilterItems = new Array();
        if (this.FromDate) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.ToDate) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDateTime";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.ShipmentTypeRadio != null) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipmentTypeId";
            this.queryFilterItem.FieldValue = this.ShipmentTypeRadio == "All" ? "" : this.ShipmentTypeRadio;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.CurrentDirectionId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.CurrentDirectionId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        this.reportFliter = new ReportFliter_1.ReportFliter();
        this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.CurrentCurrencyCodeType = this.SelectedCurrency;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.reportFliter.CustomerId = this.CustomerId;
        this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;
        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        this.ReportsPreview.CleanPartnersObslist();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
            this.ReportsPreview.AddPartner("Partner", this.CustomerId);
    };
    OceanShipmentReportFilterComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    OceanShipmentReportFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OceanShipmentReportFilterComponent',
            templateUrl: './OceanShipmentReportFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], OceanShipmentReportFilterComponent);
    return OceanShipmentReportFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.OceanShipmentReportFilterComponent = OceanShipmentReportFilterComponent;
//# sourceMappingURL=OceanShipmentReportFilterComponent.js.map