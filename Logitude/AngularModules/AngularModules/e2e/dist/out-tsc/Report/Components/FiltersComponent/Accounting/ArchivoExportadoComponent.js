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
var ArchivoExportadoComponent = /** @class */ (function (_super) {
    __extends(ArchivoExportadoComponent, _super);
    function ArchivoExportadoComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.RunReportEvent = new core_1.EventEmitter();
        // Filters
        _this.fromDate = null;
        _this.toDate = null;
        _this.selectedCurrencyCode = null;
        _this.includeDraftInvoices = false;
        _this.includeEstimations = false;
        _this.splitByCharges = false;
        _this.LocalCurrencyCode = SessionLocator_1.SessionLocator.LocalCurrencyCode;
        _this.ProfitCurrencyCode = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyCode;
        _this.SelectedCurrencyCode = _this.LocalCurrencyCode;
        return _this;
    }
    ArchivoExportadoComponent.prototype.ngOnInit = function () {
        this.SetUIProperties();
    };
    ArchivoExportadoComponent.prototype.SetUIProperties = function () {
        this.UIProperties.SetRequired("FromDate", null, Tools_1.AppTool.IsNullOrEmpty(this.FromDate) ? true : false);
    };
    Object.defineProperty(ArchivoExportadoComponent.prototype, "FromDate", {
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
    Object.defineProperty(ArchivoExportadoComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (this.toDate != value) {
                this.toDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArchivoExportadoComponent.prototype, "SelectedCurrencyCode", {
        get: function () { return this.selectedCurrencyCode; },
        set: function (value) {
            if (this.selectedCurrencyCode != value) {
                this.selectedCurrencyCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArchivoExportadoComponent.prototype, "IncludeDraftInvoices", {
        get: function () { return this.includeDraftInvoices; },
        set: function (value) {
            if (this.includeDraftInvoices != value) {
                this.includeDraftInvoices = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArchivoExportadoComponent.prototype, "IncludeEstimations", {
        get: function () { return this.includeEstimations; },
        set: function (value) {
            if (this.includeEstimations != value) {
                this.includeEstimations = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ArchivoExportadoComponent.prototype, "SplitByCharges", {
        get: function () { return this.splitByCharges; },
        set: function (value) {
            if (this.splitByCharges != value) {
                this.splitByCharges = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ArchivoExportadoComponent.prototype.RunButtonClicked = function () {
        this.SetUIProperties();
        var errors = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.FromDate)) {
            errors.push("From Date is required");
        }
        this.ValidationErrorsList = errors;
        if (errors.length == 0) {
            var myFilterItems = [];
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("FromDate", this.FromDate, "Date"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("ToDate", this.ToDate, "Date"));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SelectedCurrencyCode", this.SelectedCurrencyCode));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeEstimations", this.IncludeEstimations));
            myFilterItems.push(new QueryFilterItem_1.QueryFilterItem("SplitByCharges", this.SplitByCharges));
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
    ], ArchivoExportadoComponent.prototype, "RunReportEvent", void 0);
    ArchivoExportadoComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ArchivoExportadoComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ArchivoExportadoComponent);
    return ArchivoExportadoComponent;
}(BaseComponent_1.BaseComponent));
exports.ArchivoExportadoComponent = ArchivoExportadoComponent;
//# sourceMappingURL=ArchivoExportadoComponent.js.map