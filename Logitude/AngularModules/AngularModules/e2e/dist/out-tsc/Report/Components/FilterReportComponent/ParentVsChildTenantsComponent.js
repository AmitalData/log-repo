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
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var GlobalDomainService_1 = require("../../../Common/Services/GlobalDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ParentVsChildTenantsComponent = /** @class */ (function (_super) {
    __extends(ParentVsChildTenantsComponent, _super);
    function ParentVsChildTenantsComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "Report";
        _this.DataContext = _this;
        return _this;
    }
    ParentVsChildTenantsComponent.prototype.ngOnInit = function () {
    };
    ParentVsChildTenantsComponent.prototype.InitializeComponent = function (myReportsPreview) {
        this.ReportsPreview = myReportsPreview;
        this.GetTenants();
    };
    ParentVsChildTenantsComponent.prototype.GetTenants = function () {
        var _this = this;
        var service = new GlobalDomainService_1.GlobalDomainService();
        service.GetParentTenants().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var myList = myResponse.Result;
                _this.BuildParentTenantList(myList);
            }
        });
    };
    ParentVsChildTenantsComponent.prototype.BuildParentTenantList = function (myList) {
        var _this = this;
        this.ParentTenantList = [];
        if (myList) {
            if (myList.length > 0) {
                myList.forEach(function (item) {
                    var newItem = new CodeNameClass_1.CodeNameClass();
                    newItem.Code = item.Id.toString();
                    newItem.Name = item.Name;
                    newItem.DisplyText = item.Name + " (" + item.Id + ")";
                    _this.ParentTenantList.push(newItem);
                });
            }
        }
    };
    Object.defineProperty(ParentVsChildTenantsComponent.prototype, "SelectedParentTenant", {
        get: function () { return this.selectedParentTenant; },
        set: function (value) {
            if (this.selectedParentTenant != value) {
                this.selectedParentTenant = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ParentVsChildTenantsComponent.prototype.RunReport = function (isloading) {
        this.ValidationErrorsList = [];
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array();
            if (this.SelectedParentTenant != null) {
                this.queryFilterItem = new QueryFilterItem_1.QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ParentTenantId";
                this.queryFilterItem.FieldValue = this.SelectedParentTenant.Code;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            var reportFliter = new ReportFliter_1.ReportFliter();
            reportFliter.Tenant = SessionLocator_1.SessionLocator.Tenant;
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(reportFliter, isloading);
        }
    };
    ParentVsChildTenantsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ParentVsChildTenantsComponent',
            templateUrl: './ParentVsChildTenantsComponent.html',
            inputs: ['ReportsPreview']
        }),
        __metadata("design:paramtypes", [])
    ], ParentVsChildTenantsComponent);
    return ParentVsChildTenantsComponent;
}(BaseComponent_1.BaseComponent));
exports.ParentVsChildTenantsComponent = ParentVsChildTenantsComponent;
//# sourceMappingURL=ParentVsChildTenantsComponent.js.map