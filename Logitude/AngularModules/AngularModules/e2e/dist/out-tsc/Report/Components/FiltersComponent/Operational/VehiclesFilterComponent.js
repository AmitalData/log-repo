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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../../Components/Filters/QueryFilterItem");
var core_1 = require("@angular/core");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var DashboardFilters_1 = require("../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters");
var VehiclesFilterComponent = /** @class */ (function (_super) {
    __extends(VehiclesFilterComponent, _super);
    function VehiclesFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.SupplierValues = "CS";
        _this.selectedTransportFilterCountries = "All";
        _this.selectedDirectionFilterCountries = "All";
        _this.DateTypeFilterList = [];
        _this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Create Date", "CreateDate"));
        _this.DateTypeFilterList.push(new DashboardFilters_1.DashBoardFilters("Operational Date", "OperationalDate"));
        _this.selectedDateTypeItem = _this.DateTypeFilterList.filter(function (d) { return d.Index == "CreateDate"; })[0];
        if (SessionLocator_1.SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            _this.SupplierValues = "CS,AG";
        }
        else {
            _this.SupplierValues = "CS";
        }
        return _this;
    }
    Object.defineProperty(VehiclesFilterComponent.prototype, "ShipmentCustomerTypeCode", {
        get: function () { return this.shipmentCustomerTypeCode; },
        set: function (newValue) {
            if (this.shipmentCustomerTypeCode != newValue) {
                this.shipmentCustomerTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehiclesFilterComponent.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (newValue) {
            if (this.customerId != newValue) {
                this.customerId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehiclesFilterComponent.prototype, "PackageTypeId", {
        get: function () { return this.packageTypeId; },
        set: function (newValue) {
            if (this.packageTypeId != newValue) {
                this.packageTypeId = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehiclesFilterComponent.prototype, "SelectedDateTypeItem", {
        get: function () { return this.selectedDateTypeItem; },
        set: function (value) {
            if (this.selectedDateTypeItem != value) {
                this.selectedDateTypeItem = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehiclesFilterComponent.prototype, "SelectedTransportFilterCountries", {
        get: function () { return this.selectedTransportFilterCountries; },
        set: function (newValue) {
            if (this.selectedTransportFilterCountries != newValue) {
                this.selectedTransportFilterCountries = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(VehiclesFilterComponent.prototype, "SelectedDirectionFilterCountries", {
        get: function () { return this.selectedDirectionFilterCountries; },
        set: function (newValue) {
            if (this.selectedDirectionFilterCountries != newValue) {
                this.selectedDirectionFilterCountries = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    VehiclesFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.FromDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.FromDate.setMonth(this.FromDate.getMonth() - 1);
        this.ToDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        this.RunReport(false);
    };
    VehiclesFilterComponent.prototype.RunReport = function (isloading) {
        if (isloading) {
            this.ValidationErrorsList = [];
            if (this.FromDate == null) {
                this.ValidationErrorsList.push("From Date is required");
            }
            if (this.ToDate != null) {
                if (this.FromDate > this.ToDate) {
                    this.ValidationErrorsList.push("From Date cannot be greater than To Date");
                }
            }
            if (this.ValidationErrorsList.length == 0) {
                this.queryFilterItems = new Array();
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
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Packagetype", this.PackageTypeId, "String"));
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("BillToId", this.CustomerId, "String"));
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("TransportMode", this.SelectedTransportFilterCountries, "String"));
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Direction", this.SelectedDirectionFilterCountries, "String"));
                if (this.SelectedDateTypeItem.Index == "CreateDate") {
                    this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsByCreateDate", true, "boolean"));
                }
                else {
                    this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsByCreateDate", false, "boolean"));
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
                this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
            }
        }
    };
    VehiclesFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'VehiclesFilterComponent',
            templateUrl: './VehiclesFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], VehiclesFilterComponent);
    return VehiclesFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.VehiclesFilterComponent = VehiclesFilterComponent;
//# sourceMappingURL=VehiclesFilterComponent.js.map