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
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeListBoxComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent");
var ContainerTruckingFilterComponent = /** @class */ (function (_super) {
    __extends(ContainerTruckingFilterComponent, _super);
    function ContainerTruckingFilterComponent(fb) {
        var _this = _super.call(this) || this;
        _this.AgentId = null;
        _this.CustomerId = null;
        _this.DataContext = _this;
        _this.ObjectTableName = "FlightsSchedulesResponse";
        _this.mySelectedDirectionFilter = "All";
        _this.myForm = fb.group({});
        return _this;
    }
    Object.defineProperty(ContainerTruckingFilterComponent.prototype, "MySelectedDirectionFilter", {
        get: function () { return this.mySelectedDirectionFilter; },
        set: function (newValue) {
            if (this.mySelectedDirectionFilter != newValue) {
                this.mySelectedDirectionFilter = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    ContainerTruckingFilterComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
    };
    ContainerTruckingFilterComponent.prototype.ngOnInit = function () {
        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(65);
        //}
    };
    ContainerTruckingFilterComponent.prototype.onSelectedItemChanged = function (item) {
    };
    ContainerTruckingFilterComponent.prototype.onSelectedItemShowChanged = function (item) {
    };
    ContainerTruckingFilterComponent.prototype.RunReport = function () {
        this.queryFilterItems = new Array();
        if (this.MySelectedDirectionFilter != "All") {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.AgentId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AgentId";
            this.queryFilterItem.FieldValue = this.AgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.CustomerId) {
            this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (!this.DateType) {
            this.DateType = "CreateDate";
        }
        this.reportFliter = new ReportFliter_1.ReportFliter();
        this.reportFliter.DateType = this.DateType;
        this.reportFliter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.ReportsPreview.CleanPartnersObslist();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomerId))
            this.ReportsPreview.AddPartner("Customer", this.CustomerId);
        if (!Tools_1.AppTool.IsNullOrEmpty(this.AgentId))
            this.ReportsPreview.AddPartner("Agent", this.AgentId);
        this.ReportsPreview.GenerateReport(this.reportFliter, true);
    };
    ContainerTruckingFilterComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ContainerTruckingFilterComponent',
            templateUrl: './ContainerTruckingFilterComponent.html',
            inputs: ['ReportsPreview'],
            entryComponents: [LogitudeListBoxComponent_1.LogitudeListBoxComponent]
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], ContainerTruckingFilterComponent);
    return ContainerTruckingFilterComponent;
}(BaseComponent_1.BaseComponent));
exports.ContainerTruckingFilterComponent = ContainerTruckingFilterComponent;
//# sourceMappingURL=ContainerTruckingFilterComponent.js.map