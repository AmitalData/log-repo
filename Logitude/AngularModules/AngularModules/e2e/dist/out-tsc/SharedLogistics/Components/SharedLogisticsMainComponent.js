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
var Args_1 = require("../../Infrastructure/Args");
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var CustomerPMService_1 = require("../../Common/Services/StandardPMs/CustomerPMService");
var TenantPMService_1 = require("../../Common/Services/StandardPMs/TenantPMService");
var SharedLogisticsService_1 = require("../Services/Others/SharedLogisticsService");
var DocumentTypeListService_1 = require("../../Common/Services/StandardLists/DocumentTypeListService");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var SharedLogisticsMainComponent = /** @class */ (function () {
    function SharedLogisticsMainComponent(_sharedLogisticsService, _documentTypeListService) {
        this._sharedLogisticsService = _sharedLogisticsService;
        this._documentTypeListService = _documentTypeListService;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._customerPMService = new CustomerPMService_1.CustomerPMService();
        this.IsDisplayAreaDocument = false;
        this.IsShowActivationWizardLink = false;
        this.IsShowSettingLink = false;
        this.SharedLogisticsActivatedEnabled = false;
        this.EAWBQueryGroupVisibility = true;
        this.MobileActivatedEnabled = false;
        this.InvitedCustomersCount = 0;
        this.NotInvitedCustomersCount = 0;
        this.ActivatedCustomersCount = 0;
        this.ActivatedCustomersForMobileCount = 0;
        this.InvitedAgentsCount = 0;
        this.NotInvitedAgentsCount = 0;
        this.ActivatedAgentsCount = 0;
        this.TodayCustomerLogsCount = "0";
        this.LastWeekCustomerLogsCount = "0";
        this.LastMonthCustomerLogsCount = "0";
        this.TodayAgentLogsCount = "0";
        this.LastWeekAgentLogsCount = "0";
        this.LastMonthAgentLogsCount = "0";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService_1.TenantPMService();
        }
    }
    SharedLogisticsMainComponent.prototype.ngOnInit = function () {
        this.LoadData();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
            this.TitleSettings = "Shared Logistics & Mobile Settings";
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE"))
            this.TitleSettings = "Mobile Settings";
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
            this.TitleSettings = "Shared Logistics Settings";
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
            this.TitleStatus = "Shared Logistics & Mobile Status";
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MOBILE"))
            this.TitleStatus = "Mobile Status";
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SHAREDLOGISTICS"))
            this.TitleStatus = "Shared Logistics Status";
    };
    SharedLogisticsMainComponent.prototype.LoadData = function () {
        this.LoadCurrentTenant();
        this.LoadLastLoginPartners();
    };
    SharedLogisticsMainComponent.prototype.LoadCurrentTenant = function () {
        var _this = this;
        this.tenantPMService.get(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.myTenantPM = myResult;
                    _this.RefreshTenantScreenData();
                }
            }
        });
    };
    SharedLogisticsMainComponent.prototype.RefreshTenantScreenData = function () {
        if (this.myTenantPM.IsSharedLogisticsActivated || this.myTenantPM.IsMobileActivated) {
            this.SharedLogisticsActivatedEnabled = true;
            this.IsShowActivationWizardLink = false;
            this.IsShowSettingLink = true;
        }
        else {
            this.SharedLogisticsActivatedEnabled = false;
            this.IsShowActivationWizardLink = true;
            this.IsShowSettingLink = false;
        }
        if (this.myTenantPM.IsMobileActivated) {
            this.MobileActivatedEnabled = true;
            this.IsShowActivationWizardLink = false;
            this.IsShowSettingLink = true;
        }
        else {
            this.MobileActivatedEnabled = false;
        }
        //this.MobileActivatedEnabled = false;
        this.LoadCardData();
        this.LoadSharedLogisticsSummary();
    };
    SharedLogisticsMainComponent.prototype.LoadCardData = function () {
        var _this = this;
        this._sharedLogisticsService.getSharedLogisticsStatistics(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var data = myResult;
                    if (data != null) {
                        _this.InvitedCustomersCount = data.InvitedCustomersCount;
                        _this.NotInvitedCustomersCount = data.NotInvitedCustomersCount;
                        _this.ActivatedCustomersCount = data.ActivatedCustomersCount;
                        _this.ActivatedCustomersForMobileCount = data.ActivatedCustomersForMobileCount;
                        _this.InvitedAgentsCount = data.InvitedAgentsCount;
                        _this.NotInvitedAgentsCount = data.NotInvitedAgentsCount;
                        _this.ActivatedAgentsCount = data.ActivatedAgentsCount;
                    }
                    if (_this.SharedLogisticsActivatedEnabled) {
                        _this.InvitedCustomersCountIsEnabled = _this.InvitedCustomersCount == 0 ? false : true;
                        _this.NotInvitedCustomersCountIsEnabled = _this.NotInvitedCustomersCount == 0 ? false : true;
                        _this.ActivatedCustomersCountIsEnabled = _this.ActivatedCustomersCount == 0 ? false : true;
                        _this.InvitedAgentsCountIsEnabled = _this.InvitedAgentsCount == 0 ? false : true;
                        _this.NotInvitedAgentsCountIsEnabled = _this.NotInvitedAgentsCount == 0 ? false : true;
                        _this.ActivatedAgentsCountIsEnabled = _this.ActivatedAgentsCount == 0 ? false : true;
                    }
                    else {
                        _this.InvitedCustomersCountIsEnabled = false;
                        _this.NotInvitedCustomersCountIsEnabled = false;
                        _this.ActivatedCustomersCountIsEnabled = false;
                        _this.InvitedAgentsCountIsEnabled = false;
                        _this.NotInvitedAgentsCountIsEnabled = false;
                        _this.ActivatedAgentsCountIsEnabled = false;
                    }
                    if (_this.MobileActivatedEnabled) {
                        _this.ActivatedCustomersForMobileCountIsEnabled = _this.ActivatedCustomersForMobileCount == 0 ? false : true;
                    }
                    else {
                        _this.ActivatedCustomersForMobileCountIsEnabled = false;
                    }
                }
            }
        });
    };
    SharedLogisticsMainComponent.prototype.LoadSharedLogisticsSummary = function () {
        var _this = this;
        this._sharedLogisticsService.getSharedLogisticsSummaryData(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.sharedLogisticsSummary = pmResponse.Result;
                if (_this.sharedLogisticsSummary != null) {
                    _this.TodayCustomerLogsCount = _this.sharedLogisticsSummary.TodayCustomersCount.toString();
                    _this.LastWeekCustomerLogsCount = _this.sharedLogisticsSummary.LastWeekCustomersCount.toString();
                    _this.LastMonthCustomerLogsCount = _this.sharedLogisticsSummary.LastMonthCustomersCount.toString();
                    _this.TodayAgentLogsCount = _this.sharedLogisticsSummary.TodayAgentsCount.toString();
                    _this.LastWeekAgentLogsCount = _this.sharedLogisticsSummary.LastWeekAgentsCount.toString();
                    _this.LastMonthAgentLogsCount = _this.sharedLogisticsSummary.LastMonthAgentsCount.toString();
                    if (_this.SharedLogisticsActivatedEnabled) {
                        _this.TodayCustomerIsEnabled = _this.TodayCustomerLogsCount == "0" ? false : true;
                        _this.LastWeekCustomerIsEnabled = _this.LastWeekCustomerLogsCount == "0" ? false : true;
                        _this.LastMonthCustomerIsEnabled = _this.LastMonthCustomerLogsCount == "0" ? false : true;
                        _this.TodayAgentIsEnabled = _this.TodayAgentLogsCount == "0" ? false : true;
                        _this.LastWeekAgentIsEnabled = _this.LastWeekAgentLogsCount == "0" ? false : true;
                        _this.LastMonthAgentIsEnabled = _this.LastMonthAgentLogsCount == "0" ? false : true;
                    }
                    else {
                        _this.TodayCustomerIsEnabled = false;
                        _this.LastWeekCustomerIsEnabled = false;
                        _this.LastMonthCustomerIsEnabled = false;
                        _this.TodayAgentIsEnabled = false;
                        _this.LastWeekAgentIsEnabled = false;
                        _this.LastMonthAgentIsEnabled = false;
                    }
                }
            }
        });
    };
    SharedLogisticsMainComponent.prototype.LoadLastLoginPartners = function () {
        var _this = this;
        this._sharedLogisticsService.getLastLoginPartners(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.LastPartnersList = pmResponse.Result;
                _this.LastPartnersList.forEach(function (item) {
                    item.IsEnabledShowDetailsButton = true;
                });
            }
        });
    };
    SharedLogisticsMainComponent.prototype.EventsPermissionsLinkClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Events Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsEventPermissiosComponent");
    };
    SharedLogisticsMainComponent.prototype.DocumentsPermissionsLinkClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Documents Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsDocumentPermissiosComponent");
    };
    SharedLogisticsMainComponent.prototype.MoneyPermissionsLinkClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 520;
        logWindow.Title = "Money Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsMoneyPermissiosComponent");
    };
    SharedLogisticsMainComponent.prototype.PartnersPermissionsLinkClick = function () {
        var windowArgs = {};
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 820;
        logWindow.Height = 570;
        logWindow.Title = "Partners Permissions";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsPartnersPermissiosComponent");
    };
    SharedLogisticsMainComponent.prototype.ActivationWizardLinkClick = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowArgs = {};
        windowArgs.TenantPM = this.myTenantPM;
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 910;
        logWindow.Height = 550;
        logWindow.Title = "Shared Logistics Wizard";
        logWindow.DataContext = this.myTenantPM;
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsWizardComponent");
    };
    SharedLogisticsMainComponent.prototype.SettingsLinkClick = function () {
        var windowArgs = {};
        windowArgs.TenantPM = this.myTenantPM;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 900;
        logWindow.Height = 550;
        logWindow.Title = "Shared Logistics Settings";
        logWindow.Show("./SharedLogistics/Components/SharedLogisticsSettingComponent");
    };
    SharedLogisticsMainComponent.prototype.InviteLinkClick = function (code) {
        var _this = this;
        var backButtonTitle = "Shared Logistics";
        var queryCode = "";
        var displayTitle = "";
        var objectTableName = "";
        if (code != null) {
            switch (code) {
                case "Customers":
                    {
                        displayTitle = "Customers";
                        objectTableName = "Customer";
                        queryCode = "Shared Logistics Customers";
                        break;
                    }
                case "Agents":
                    {
                        displayTitle = "Agents";
                        objectTableName = "Agent";
                        queryCode = "Shared Logistics Agents";
                        break;
                    }
                default: {
                    break;
                }
            }
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            //listArgs.ShowViews = false;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    SharedLogisticsMainComponent.prototype.CustomersZoomLinkClcik = function (code) {
        var _this = this;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
        this.filterAgrs.addAdditionalFilter("PartnerTypeId", "CS", null, null, "Equals", false, true, false, "string");
        var backButtonTitle = "Shared Logistics";
        var queryCode = "Shared Logistics Customers";
        var displayTitle = "";
        var objectTableName = "Customer";
        var navigate = true;
        switch (code) {
            case "invited":
                {
                    if (this.InvitedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "InvitationDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 2, null, null, "Equals", false, true, false, "string");
                        displayTitle = "Invited Customers";
                    }
                    break;
                }
            case "not invited":
                {
                    if (this.NotInvitedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "LastShipmentDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 1, null, null, "Equals", false, true, false, "string");
                        displayTitle = "Not Invited Customers";
                    }
                    break;
                }
            case "activated":
                {
                    if (this.ActivatedCustomersCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "LastLoginDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");
                        this.filterAgrs.addAdditionalFilter("IsActiveForMobile", false, null, null, "Equals", false, true, false, "boolen");
                        displayTitle = "Activated Customers";
                    }
                    break;
                }
            case "activatedmobile":
                {
                    if (this.ActivatedCustomersForMobileCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "LastLoginDate";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("SharedLogisticsInvitationStatusCode", 3, null, null, "Equals", false, true, false, "string");
                        this.filterAgrs.addAdditionalFilter("IsActiveForMobile", true, null, null, "Equals", false, true, false, "boolen");
                        displayTitle = "Activated Mobile Customers";
                    }
                    break;
                }
        }
        if (navigate) {
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.ShowViews = false;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    SharedLogisticsMainComponent.prototype.ActivityZoomLinkClick = function (m) {
        var windowArgs = {};
        windowArgs.TenantPM = this.myTenantPM;
        switch (m) {
            case "Today Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "T";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Week Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "W";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Month Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "M";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Today Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "T";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Week Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "W";
                    windowArgs.DataContext = this;
                    break;
                }
            case "Last Month Agents":
                {
                    windowArgs.PartnerTypeId = "AG";
                    windowArgs.DateParameter = "M";
                    windowArgs.DataContext = this;
                    break;
                }
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "Activity Log";
        logitudeWindow.Height = 600;
        logitudeWindow.Width = 1000;
        logitudeWindow.Show("./SharedLogistics/Components/ActivityZoomComponent");
    };
    SharedLogisticsMainComponent.prototype.ShowDetailsButtonclick = function (item) {
        var _this = this;
        if (item.PartnerTypeName == "Customer") {
            item.IsEnabledShowDetailsButton = false;
            this._customerPMService.get(item.CardId).subscribe(function (res) {
                var pmResponse = res;
                item.IsEnabledShowDetailsButton = true;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        if (!myResult.IsCustomerAllowed)
                            _this.ViewBlocedEntity(myResult);
                        else {
                            var logWindow = new LogitudeWindow_1.LogitudeWindow();
                            logWindow.Title = "Customer" + " Edit";
                            if (window.innerHeight > 700 && window.innerWidth > 1200) {
                                logWindow.Width = 1200;
                                logWindow.Height = 700;
                                logWindow.ShowEditComponent(myResult.Id, "Customer", null, false);
                            }
                            else
                                logWindow.ShowEditComponent(myResult.Id, "Customer");
                        }
                    }
                }
            });
        }
    };
    SharedLogisticsMainComponent.prototype.ViewBlocedEntity = function (item) {
        var windowArgs = {};
        windowArgs.CustomerPM = item;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "View Customer";
        logitudeWindow.Height = 500;
        logitudeWindow.Width = 800;
        logitudeWindow.Show("./SharedLogistics/Components/ViewBlocedCustomerComponent");
    };
    SharedLogisticsMainComponent.prototype.AgentsZoomLinkClcik = function (arg) {
    };
    SharedLogisticsMainComponent.prototype.ShowDocumentTypeEditButtonclick = function (documentcode) {
        var _this = this;
        var apiQueryFilters = new ApiQueryFilters_1.ApiQueryFilters();
        apiQueryFilters.GetAll = true;
        apiQueryFilters.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        this._documentTypeListService.getAllFromCache(apiQueryFilters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                var item = myResult.filter(function (d) { return d.Code == documentcode; })[0];
                if (item) {
                    _this.OpenDocumentTypeEdit(item.Id);
                }
            }
        });
    };
    SharedLogisticsMainComponent.prototype.OpenDocumentTypeEdit = function (documentId) {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Document Type" + " Edit";
        logWindow.ShowEditComponent(documentId, "DocumentType");
    };
    SharedLogisticsMainComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './SharedLogisticsMainComponent.html',
            providers: [SharedLogisticsService_1.SharedLogisticsService, DocumentTypeListService_1.DocumentTypeListService],
        }),
        __metadata("design:paramtypes", [SharedLogisticsService_1.SharedLogisticsService, DocumentTypeListService_1.DocumentTypeListService])
    ], SharedLogisticsMainComponent);
    return SharedLogisticsMainComponent;
}());
exports.SharedLogisticsMainComponent = SharedLogisticsMainComponent;
//# sourceMappingURL=SharedLogisticsMainComponent.js.map