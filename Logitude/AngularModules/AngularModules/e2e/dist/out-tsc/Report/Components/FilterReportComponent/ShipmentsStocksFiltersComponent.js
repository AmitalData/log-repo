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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ShipmentsStocksFiltersComponent = /** @class */ (function (_super) {
    __extends(ShipmentsStocksFiltersComponent, _super);
    function ShipmentsStocksFiltersComponent(fb) {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        _this.includeShipmentsDetails = false;
        _this.myForm = fb.group({});
        return _this;
    }
    ShipmentsStocksFiltersComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
        this.RunReport(false);
    };
    ShipmentsStocksFiltersComponent.prototype.ngOnInit = function () {
    };
    ShipmentsStocksFiltersComponent.prototype.daysInMonth = function (aDate) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    };
    Object.defineProperty(ShipmentsStocksFiltersComponent.prototype, "IncludeShipmentsDetails", {
        get: function () { return this.includeShipmentsDetails; },
        set: function (value) {
            if (this.includeShipmentsDetails != value) {
                this.includeShipmentsDetails = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ShipmentsStocksFiltersComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.ToDate == null) {
            var FIELD_IS_REQUIERD = null;
            FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDate");
            if (!fieldName)
                fieldName = "To Date";
            var s = FIELD_IS_REQUIERD.replace("%FieldName", fieldName);
            this.ValidationErrorsList.push(s);
        }
        if (this.FromDate == null) {
            var FIELD_IS_REQUIERD = null;
            FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var fieldName = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FromDate");
            if (!fieldName)
                fieldName = "From Date";
            var s = FIELD_IS_REQUIERD.replace("%FieldName", fieldName);
            this.ValidationErrorsList.push(s);
        }
        if (this.ToDate < this.FromDate) {
            var messageError = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.ToDateMustGreaterFromDate");
            if (!messageError)
                messageError = "to date must be greater than from date";
            this.ValidationErrorsList.push(messageError);
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("IncludeShipmentsDetails", this.IncludeShipmentsDetails));
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
    ShipmentsStocksFiltersComponent.prototype.SetDate = function (year, month, day) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    };
    ShipmentsStocksFiltersComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ShipmentsStocksFiltersComponent',
            templateUrl: './ShipmentsStocksFiltersComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], ShipmentsStocksFiltersComponent);
    return ShipmentsStocksFiltersComponent;
}(BaseComponent_1.BaseComponent));
exports.ShipmentsStocksFiltersComponent = ShipmentsStocksFiltersComponent;
//# sourceMappingURL=ShipmentsStocksFiltersComponent.js.map