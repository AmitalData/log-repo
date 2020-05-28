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
var Tools_1 = require("../../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ReportFliter_1 = require("../../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../../Components/Filters/QueryFilterItem");
var ShipmentProfitVSQuoteEstimateComponent = /** @class */ (function (_super) {
    __extends(ShipmentProfitVSQuoteEstimateComponent, _super);
    function ShipmentProfitVSQuoteEstimateComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        // Filters
        _this.customerId = null;
        _this.salesmanUserId = null;
        _this.fromDate = null;
        _this.toDate = null;
        _this.selectedCurrencyCode = null;
        _this.shipmentsTypeCode = "C";
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.SelectedCurrencyCode = _this.LocalCurrencyCode;
        return _this;
    }
    ShipmentProfitVSQuoteEstimateComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    ShipmentProfitVSQuoteEstimateComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("FromDate", null, Tools_1.AppTool.IsNullOrEmpty(this.FromDate) ? true : false);
    };
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "CustomerId", {
        get: function () { return this.customerId; },
        set: function (value) {
            if (this.customerId != value) {
                this.customerId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "SalesmanUserId", {
        get: function () { return this.salesmanUserId; },
        set: function (value) {
            if (this.salesmanUserId != value) {
                this.salesmanUserId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (this.fromDate != value) {
                this.fromDate = value;
                this.SetUIProperties();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "SelectedCurrencyCode", {
        get: function () { return this.selectedCurrencyCode; },
        set: function (value) {
            if (this.selectedCurrencyCode != value) {
                this.selectedCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ShipmentProfitVSQuoteEstimateComponent.prototype, "ShipmentsTypeCode", {
        get: function () { return this.shipmentsTypeCode; },
        set: function (value) {
            if (this.shipmentsTypeCode != value) {
                this.shipmentsTypeCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentProfitVSQuoteEstimateComponent.prototype.RunButtonClicked = function () {
        this.SetUIProperties();
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            errors.push("From Date is required");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var myFilterItems = [];
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CustomerId", this.CustomerId));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SalesmanUserId", this.SalesmanUserId));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("FromDate", this.FromDate, "Date"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ToDate", this.ToDate, "Date"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ShipmentsTypeCode", this.ShipmentsTypeCode));
            var myReportFliter = new ReportFliter_1.ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;
            this.RunReportEvent.emit(myReportFliter);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ShipmentProfitVSQuoteEstimateComponent.prototype, "RunReportEvent", void 0);
    ShipmentProfitVSQuoteEstimateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentProfitVSQuoteEstimateComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ShipmentProfitVSQuoteEstimateComponent);
    return ShipmentProfitVSQuoteEstimateComponent;
}(BaseComponent_1.BaseComponent));
exports.ShipmentProfitVSQuoteEstimateComponent = ShipmentProfitVSQuoteEstimateComponent;
//# sourceMappingURL=ShipmentProfitVSQuoteEstimateComponent.js.map