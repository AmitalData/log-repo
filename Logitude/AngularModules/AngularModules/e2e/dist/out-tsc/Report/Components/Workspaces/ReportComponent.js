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
var Tools_1 = require("../../../Infrastructure/Tools");
var ReportsTemplateListExtendedService_1 = require("../../../Common/Services/ExtendedLists/ReportsTemplateListExtendedService");
var ReportComponent = /** @class */ (function () {
    function ReportComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ItemsSource = [];
        this.ItemsSourceTemp = [];
        this.IsViewReport = false;
        this.showLocal = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsLoadSettingWorkerRoleRuning = false;
        this.IsLoadReportsTemplateListRuning = false;
        this.ReportTemplates = [];
        this.ReportsRunUsingWR = false;
        this.mySearchText = null;
        this.reportsTemplateListExtendedService = new ReportsTemplateListExtendedService_1.ReportsTemplateListExtendedService();
        this.LoadData();
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
    }
    ReportComponent.prototype.InitComponent = function () {
    };
    ReportComponent.prototype.LoadData = function () {
        var _this = this;
        this.groupList = [];
        this.reportList = [];
        var groupService = new ReportGroupService_1.ReportGroupService();
        var reportService = new ReportService_1.ReportService();
        groupService.getReportGroupLists(0).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.groupList = myResponse.Result;
                _this.groupList = _this.groupList.sort(function (a, b) { return a.OrderNumber - b.OrderNumber; });
                _this.groupList.forEach(function (item) {
                    reportService.GetReportListsByGroupId(item.Id, SessionLocator_1.SessionLocator.Tenant).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var myResult = myResponse.Result;
                            myResult.forEach(function (item) {
                                if (item.Code == "AREX") {
                                    if (SessionLocator_1.SessionLocator.Tenant == 1212 || FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
                                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            _this.reportList.push(item);
                                        }
                                    }
                                }
                                else if (item.Code == "DSCA") {
                                    if (SessionLocator_1.SessionLocator.Tenant != 1212 || FeatureLocator_1.FeatureLocator.IsPackage_DVMT()) {
                                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            _this.reportList.push(item);
                                        }
                                    }
                                }
                                else if (item.Code == "VDK") {
                                    if (SessionLocator_1.SessionLocator.Tenant == 1495 || SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode == "DVMT") {
                                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            _this.reportList.push(item);
                                        }
                                    }
                                }
                                else if (item.Code == "UNER") {
                                    var FeatureToggle = SessionLocator_1.SessionLocator.FeatureToggles.filter(function (d) { return d.ToggleCode == "URT" && d.TenantNumber == SessionLocator_1.SessionLocator.Tenant; })[0];
                                    if (FeatureToggle) {
                                        _this.reportList.push(item);
                                    }
                                }
                                else if (item.Code == "SHID") {
                                    if (SessionLocator_1.SessionLocator.Tenant == 1526 || SessionLocator_1.SessionLocator.Tenant == 1525 || SessionLocator_1.SessionLocator.Tenant == 1524 || SessionLocator_1.SessionLocator.Tenant == 1523 || SessionLocator_1.SessionLocator.Tenant == 1608 || SessionLocator_1.SessionLocator.Tenant == 1609 || SessionLocator_1.SessionLocator.Tenant == 1684 || SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode == "DVMT") {
                                        _this.reportList.push(item);
                                    }
                                    else {
                                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            _this.reportList.push(item);
                                        }
                                    }
                                }
                                else if (item.Code == "SHEL") {
                                    if (SessionLocator_1.SessionLocator.TenantManagementJS.PackageCode == "DVMT" || SessionLocator_1.SessionLocator.Tenant == 1609 || SessionLocator_1.SessionLocator.Tenant == 1608 || SessionLocator_1.SessionLocator.Tenant == 1523 || SessionLocator_1.SessionLocator.Tenant == 1524 || SessionLocator_1.SessionLocator.Tenant == 1525 || SessionLocator_1.SessionLocator.Tenant == 1526) {
                                        if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                            _this.reportList.push(item);
                                        }
                                    }
                                }
                                else {
                                    if (item.FeatureCode && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", item.FeatureCode)) {
                                        _this.reportList.push(item);
                                    }
                                }
                            });
                            _this.FillTempItemsSource();
                        }
                    });
                });
            }
        });
    };
    ReportComponent.prototype.FillTempItemsSource = function () {
        var _this = this;
        this.ItemsSourceTemp = [];
        this.groupList.forEach(function (item) {
            var myItem = new ReportsGrpupClass(item, _this);
            _this.ItemsSourceTemp.push(myItem);
        });
    };
    ReportComponent.prototype.BuildItemsSource = function () {
        this.ItemsSource = [];
        if (this.ItemsSourceTemp.filter(function (f) { return f.IsDataLoaded == false; }).length == 0) {
            this.ItemsSource = this.ItemsSourceTemp.filter(function (f) { return f.ItemsSource.length > 0; });
        }
    };
    ReportComponent.prototype.ViewReportClicked = function (GroupList, ReportList) {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Reports", "Report View");
        var isLoadingResources = false;
        var entityResourceName = "";
        switch (ReportList.FilterControlName) {
            case "QuotesFilterControl":
                {
                    isLoadingResources = true;
                    entityResourceName = "Quote";
                    break;
                }
        }
        if (isLoadingResources) {
            this.entityResourceService.getEntityResourceByTableName(entityResourceName, 0).subscribe(function (p) {
                _this.ViewReport(GroupList, ReportList);
            });
        }
        else {
            this.ViewReport(GroupList, ReportList);
        }
    };
    ReportComponent.prototype.ViewReport = function (groupList, reportList) {
        if (!this.IsViewReport) {
            this.IsViewReport = true;
            this.IsLoadSettingWorkerRoleRuning = true;
            this.IsLoadReportsTemplateListRuning = true;
            this.LoadReportTemplate(groupList, reportList);
            this.LoadReportsRunUsingWR(groupList, reportList);
        }
    };
    ReportComponent.prototype.LoadReportTemplate = function (groupList, reportList) {
        var _this = this;
        this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(reportList.Id, "R").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.ReportTemplates = myResponse.Result;
            }
            _this.IsLoadReportsTemplateListRuning = false;
            _this.LoadComplete(groupList, reportList);
        });
    };
    ReportComponent.prototype.LoadReportsRunUsingWR = function (groupList, reportList) {
        var _this = this;
        var myService = new ReportService_1.ReportService();
        myService.GetCheckIfReportsRunUsingWR().subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.ReportsRunUsingWR = pmResponse.Result;
            }
            _this.IsLoadSettingWorkerRoleRuning = false;
            _this.LoadComplete(groupList, reportList);
        });
    };
    ReportComponent.prototype.LoadComplete = function (groupList, reportList) {
        var _this = this;
        if (!this.IsLoadSettingWorkerRoleRuning && !this.IsLoadReportsTemplateListRuning) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load("./Report/Components/ReportsPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.ReportsPreview(groupList, reportList, _this.ReportTemplates, _this.ReportsRunUsingWR);
            });
            this.IsViewReport = false;
        }
    };
    ReportComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.FillTempItemsSource();
    };
    ReportComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/Workspaces/',
            templateUrl: 'ReportComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], ReportComponent);
    return ReportComponent;
}());
exports.ReportComponent = ReportComponent;
var ReportsGrpupClass = /** @class */ (function () {
    function ReportsGrpupClass(list, fatherComponent) {
        this.list = list;
        this.fatherComponent = fatherComponent;
        this.ItemsSource = [];
        this.IsDataLoaded = false;
        this.GroupList = list;
        this.Name = list.EnglishName;
        this.FillData();
    }
    ReportsGrpupClass.prototype.FillData = function () {
        var _this = this;
        this.ItemsSource = [];
        var myReports = this.fatherComponent.reportList.filter(function (d) { return d.ReportGroupId == _this.GroupList.Id; });
        if (Tools_1.AppTool.IsNullOrEmpty(this.fatherComponent.mySearchText)) {
            myReports.forEach(function (item) {
                _this.ItemsSource.push(item);
            });
        }
        else {
            myReports.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(_this.fatherComponent.mySearchText.toUpperCase()) > -1
                    ||
                        !Tools_1.AppTool.IsNullOrEmpty(item.LocalName) && item.LocalName.toUpperCase().indexOf(_this.fatherComponent.mySearchText.toUpperCase()) > -1) {
                    _this.ItemsSource.push(item);
                }
            });
        }
        this.IsDataLoaded = true;
        this.fatherComponent.BuildItemsSource();
    };
    return ReportsGrpupClass;
}());
exports.ReportsGrpupClass = ReportsGrpupClass;
//# sourceMappingURL=ReportComponent.js.map