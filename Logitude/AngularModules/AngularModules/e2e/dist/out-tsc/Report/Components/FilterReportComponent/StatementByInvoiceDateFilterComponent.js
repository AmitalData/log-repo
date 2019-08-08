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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var ReportFliter_1 = require("../../Components/Filters/ReportFliter");
var QueryFilterItem_1 = require("../../Components/Filters/QueryFilterItem");
var Tools_1 = require("../../../Infrastructure/Tools");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var StatementByInvoiceDateFilterComponent = /** @class */ (function (_super) {
    __extends(StatementByInvoiceDateFilterComponent, _super);
    function StatementByInvoiceDateFilterComponent() {
        var _this = _super.call(this) || this;
        _this.CustomerId = null;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        return _this;
    }
    StatementByInvoiceDateFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
    };
    StatementByInvoiceDateFilterComponent.prototype.RunReport = function (isloading) {
        this.queryFilterItems = [];
        this.queryFilterItems = new Array();
        if (Tools_1.AppTool.IsNullOrEmpty(this.CustomerId)) {
            var window = new MessageWindow_1.MessageWindow();
            window.Title = "Message";
            window.Show("Please select a partner then press Run Report");
        }
        else {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
            this.reportFliter = new ReportFliter_1.ReportFliter();
            this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
                this.ReportsPreview.AddPartner("Partner", this.CustomerId);
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    };
    StatementByInvoiceDateFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'StatementByInvoiceDateFilterComponent',
            templateUrl: './StatementByInvoiceDateFilterComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], StatementByInvoiceDateFilterComponent);
    return StatementByInvoiceDateFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.StatementByInvoiceDateFilterComponent = StatementByInvoiceDateFilterComponent;
//# sourceMappingURL=StatementByInvoiceDateFilterComponent.js.map