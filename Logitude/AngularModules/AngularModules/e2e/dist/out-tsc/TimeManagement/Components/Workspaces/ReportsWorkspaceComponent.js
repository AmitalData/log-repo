"use strict";
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ReportService_1 = require("../../../Common/Services/ExtendedLists/ReportService");
var ReportGroupService_1 = require("../../../Common/Services/ExtendedLists/ReportGroupService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var ReportsTemplateListExtendedService_1 = require("../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService");
var ReportsWorkspaceComponent = /** @class */ (function () {
    function ReportsWorkspaceComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ItemsSource = [];
        this.ItemsSourceTemp = [];
        this.ReportTemplates = [];
        this.IsLoadReportsTemplateListRuning = false;
        this.IsLoadSettingWorkerRoleRuning = false;
        this.IsViewReport = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService_1.ReportsTemplateListExtendedService();
    }
    ReportsWorkspaceComponent.prototype.InitComponent = function () {
        this.LoadData();
    };
    ReportsWorkspaceComponent.prototype.RefreshTab = function () {
        this.LoadData();
    };
    ReportsWorkspaceComponent.prototype.LoadData = function () {
        var _this = this;
        var myService = new ReportGroupService_1.ReportGroupService();
        myService.getReportGroupListByCode('RTFS').subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ItemsSourceTemp = [];
                var myResult = myResponse.Result;
                _this.ItemsSourceTemp.push(new ReportsGrpupClass(myResult, _this));
            }
        });
    };
    ReportsWorkspaceComponent.prototype.BuildItemsSource = function () {
        if (this.ItemsSourceTemp.filter(function (f) { return f.IsDataLoaded == false; }).length == 0) {
            this.ItemsSource = this.ItemsSourceTemp.filter(function (f) { return f.ItemsSource.length > 0; });
        }
    };
    ReportsWorkspaceComponent.prototype.ViewReportClicked = function (GroupList, ReportList) {
        if (!this.IsViewReport) {
            this.IsViewReport = true;
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Reports", "Report View");
            this.LoadReportTemplate(GroupList, ReportList);
        }
    };
    ReportsWorkspaceComponent.prototype.LoadReportTemplate = function (groupList, reportList) {
        var _this = this;
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id, "R").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ReportTemplates = myResponse.Result;
            }
            _this.IsLoadReportsTemplateListRuning = false;
            _this.ViewReport(groupList, reportList);
        });
    };
    ReportsWorkspaceComponent.prototype.ViewReport = function (GroupList, ReportList) {
        var _this = this;
        if (!this.IsLoadSettingWorkerRoleRuning && !this.IsLoadReportsTemplateListRuning) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.ReportsPreview(GroupList, ReportList, _this.ReportTemplates);
            });
            this.IsViewReport = false;
        }
    };
    ReportsWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'ReportsWorkspaceComponent',
            moduleId: module.id,
            templateUrl: './ReportsWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ReportsWorkspaceComponent);
    return ReportsWorkspaceComponent;
}());
exports.ReportsWorkspaceComponent = ReportsWorkspaceComponent;
var ReportsGrpupClass = /** @class */ (function () {
    function ReportsGrpupClass(list, fatherComponent) {
        this.list = list;
        this.fatherComponent = fatherComponent;
        this.ItemsSource = [];
        this.IsDataLoaded = false;
        this.GroupList = list;
        this.Name = list.EnglishName;
        this.LoadData();
    }
    ReportsGrpupClass.prototype.LoadData = function () {
        var _this = this;
        var myService = new ReportService_1.ReportService();
        myService.GetReportListsByGroupId(this.list.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ItemsSource = [];
                var myResult = myResponse.Result;
                myResult.forEach(function (item) {
                    if (SessionLocator_1.SessionLocator.Tenant == 1526 || SessionLocator_1.SessionLocator.Tenant == 1525 || SessionLocator_1.SessionLocator.Tenant == 1524 || SessionLocator_1.SessionLocator.Tenant == 1523 || SessionLocator_1.SessionLocator.Tenant == 1608) {
                        if (item.Code == "SHID") {
                            _this.ItemsSource.push(item);
                        }
                    }
                    else {
                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                            _this.ItemsSource.push(item);
                        }
                    }
                });
                _this.IsDataLoaded = true;
                _this.fatherComponent.BuildItemsSource();
            }
        });
    };
    return ReportsGrpupClass;
}());
exports.ReportsGrpupClass = ReportsGrpupClass;
//# sourceMappingURL=ReportsWorkspaceComponent.js.map