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
var core_1 = require("@angular/core");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var RevenueExpenseFilterComponent = /** @class */ (function (_super) {
    __extends(RevenueExpenseFilterComponent, _super);
    function RevenueExpenseFilterComponent() {
        var _this = _super.call(this) || this;
        _this.ValidationErrorsList = [];
        _this.Level = "GLAccount";
        _this.ToDate = new Date();
        _this.DataContext = _this;
        _this.FilterSelectedValue = 'GLAccount';
        _this.chartofaccounttypeHtmlinputId = Guid_1.Guid.newGuid();
        _this.GLAccountHtmlinputId = Guid_1.Guid.newGuid();
        _this.ChartofaccountHtmlinputId = Guid_1.Guid.newGuid();
        _this.showlocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        return _this;
    }
    RevenueExpenseFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.BuildFilterList();
    };
    RevenueExpenseFilterComponent.prototype.BuildFilterList = function () {
        this.BalanceOptionsFilterList = [];
        this.BalanceOptionsFilterList.push(new CodeNameClass_1.CodeNameClass("WITHOUT", "Without Transactions", "בלי תנועות"));
        this.BalanceOptionsFilterList.push(new CodeNameClass_1.CodeNameClass("WITH", "With Transactions", "עם תנועות"));
        this.SelectedBalanceOptionFilter = this.BalanceOptionsFilterList.filter(function (d) { return d.Code == "WITHOUT"; })[0];
    };
    Object.defineProperty(RevenueExpenseFilterComponent.prototype, "SelectedBalanceOptionFilter", {
        get: function () { return this.selectedBalanceOptionFilter; },
        set: function (value) {
            if (this.selectedBalanceOptionFilter != value) {
                this.selectedBalanceOptionFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RevenueExpenseFilterComponent.prototype, "UseBalanceFilter", {
        get: function () { return this.useBalanceFilter; },
        set: function (value) {
            if (this.useBalanceFilter != value) {
                this.useBalanceFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    RevenueExpenseFilterComponent.prototype.FilterItemClicked = function (itemValue) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;
            this.Level = itemValue;
        }
    };
    RevenueExpenseFilterComponent.prototype.RunReport = function () {
        this.ValidationErrorsList = [];
        if (this.ToDate > new Date()) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.FutureDate"));
        }
        if (this.ToDate == null) {
            var FIELD_IS_REQUIERD = null;
            FIELD_IS_REQUIERD = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
            var s = FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.General.O.Date"));
            this.ValidationErrorsList.push(s);
        }
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
            if (!this.Level)
                this.Level = "GLAccount";
            this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("Level", this.Level));
            if (!this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CardFilter", "0"));
            }
            else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITHOUT") {
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CardFilter", "2"));
            }
            else if (this.UseBalanceFilter && this.SelectedBalanceOptionFilter.Code == "WITH") {
                this.queryFilterItems.push(new QueryFilterItem_1.QueryFilterItem("CardFilter", "1"));
            }
            this.reportFliter = new ReportFliter_1.ReportFliter();
            // this.reportFliter.Level = this.Level;
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    };
    RevenueExpenseFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'RevenueExpenseFilterComponent',
            templateUrl: './RevenueExpenseFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], RevenueExpenseFilterComponent);
    return RevenueExpenseFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.RevenueExpenseFilterComponent = RevenueExpenseFilterComponent;
//# sourceMappingURL=RevenueExpenseFilterComponent.js.map