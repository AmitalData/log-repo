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
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var SessionInfo_1 = require("../../Infrastructure/Utilities/SessionInfo");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var CustomerPMService_1 = require("../../Common/Services/StandardPMs/CustomerPMService");
var TenantPMService_1 = require("../../Common/Services/StandardPMs/TenantPMService");
var SharedLogisticsService_1 = require("../Services/Others/SharedLogisticsService");
var DocumentTypeListService_1 = require("../../Common/Services/StandardLists/DocumentTypeListService");
var ApiQueryFilters_1 = require("../../Infrastructure/DataContracts/ApiQueryFilters");
var ObservableCollection_1 = require("../../Infrastructure/Utilities/ObservableCollection");
var CutsomerTenantAccessManagementComponent = /** @class */ (function () {
    function CutsomerTenantAccessManagementComponent(_sharedLogisticsService, _documentTypeListService) {
        this._sharedLogisticsService = _sharedLogisticsService;
        this._documentTypeListService = _documentTypeListService;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._customerPMService = new CustomerPMService_1.CustomerPMService();
        this.EnableAccess = true;
        this.TitleSettings = "Access Management";
        this.TitleStatus = "Requests Status";
        this.EAWBQueryGroupVisibility = true;
        this.MobileActivatedEnabled = false;
        this.WaitingCount = 0;
        this.InProgressCount = 0;
        this.AcceptedCount = 0;
        this.InactiveCount = 0;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ItemsSource = new ObservableCollection_1.ObservableCollection([]);
        if (this.tenantPMService == null) {
            this.tenantPMService = new TenantPMService_1.TenantPMService();
        }
    }
    CutsomerTenantAccessManagementComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("CustomerTenantAccess", 0).subscribe(function (response) {
            _this.LoadData();
        });
    };
    CutsomerTenantAccessManagementComponent.prototype.LoadData = function () {
        this.LoadCurrentTenant();
        this.LoadLastCustomerRequest();
    };
    CutsomerTenantAccessManagementComponent.prototype.LoadCurrentTenant = function () {
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
    CutsomerTenantAccessManagementComponent.prototype.LoadLastCustomerRequest = function () {
        var _this = this;
        this._sharedLogisticsService.GetLastCustomerRequest(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.LastCustomerRequestList = pmResponse.Result;
                _this.LastCustomerRequestList.forEach(function (item) {
                    _this.ItemsSource.Insert(item);
                });
            }
        });
    };
    CutsomerTenantAccessManagementComponent.prototype.OnRowSelected = function (itemComponent) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: itemComponent.Id, ObjectTableName: 'CustomerTenantAccess', BackButtonLabel: "Back" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.ItemsSource.Clear();
                _this.LoadData();
            });
        });
    };
    CutsomerTenantAccessManagementComponent.prototype.RefreshTenantScreenData = function () {
        if (this.myTenantPM != null && !this.myTenantPM.IsCustomerTenantShare) {
            this.EnableAccess = false;
        }
        this.loadCustomerRequestStatusData();
        //this.LoadSharedLogisticsSummary();
    };
    CutsomerTenantAccessManagementComponent.prototype.loadCustomerRequestStatusData = function () {
        var _this = this;
        this._sharedLogisticsService.getCustomerTenantAccessRequestStatusCount(SessionInfo_1.SessionInfo.LoggedUserTenant).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    var data = myResult;
                    if (data != null) {
                        _this.WaitingCount = data.WaitingCount;
                        _this.InProgressCount = data.InProgressCount;
                        _this.AcceptedCount = data.AcceptedCount;
                        _this.InactiveCount = data.InactiveCount;
                    }
                    _this.WaitingCountEnabled = _this.WaitingCount == 0 ? false : true;
                    _this.InProgressCountEnabled = _this.InProgressCount == 0 ? false : true;
                    _this.AcceptedCountEnabled = _this.AcceptedCount == 0 ? false : true;
                    _this.InactiveCountEnabled = _this.InactiveCount == 0 ? false : true;
                }
            }
        });
    };
    CutsomerTenantAccessManagementComponent.prototype.SettingsLinkClick = function () {
        var windowArgs = {};
        windowArgs = { EntityPM: this.myTenantPM, Parent: this };
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.Title = "LogBox Access Settings";
        logWindow.Show("./SharedLogistics/Components/TenantAccessSettingsComponent");
    };
    CutsomerTenantAccessManagementComponent.prototype.InviteLinkClick = function (code) {
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
            //this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(response => {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
            //});
        }
    };
    CutsomerTenantAccessManagementComponent.prototype.StatusZoomCommand = function (code) {
        var _this = this;
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        //this.filterAgrs.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, true, false, "string");
        //this.filterAgrs.addAdditionalFilter("PartnerTypeId", "CS", null, null, "Equals", false, true, false, "string");
        var backButtonTitle = "Shared Logistics";
        var queryCode = "";
        var displayTitle = "";
        var showViews = false;
        var objectTableName = "CustomerTenantAccess";
        var navigate = true;
        switch (code) {
            case "W":
                {
                    if (this.WaitingCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "W", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";
                    }
                    break;
                }
            case "IP":
                {
                    if (this.InProgressCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "IP", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";
                    }
                    break;
                }
            case "A":
                {
                    if (this.AcceptedCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "A", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";
                    }
                    break;
                }
            case "IA":
                {
                    if (this.InactiveCount == 0) {
                        navigate = false;
                    }
                    else {
                        this.filterAgrs.SortBy = "RequestDateTime";
                        this.filterAgrs.SortDirection = "Descending";
                        this.filterAgrs.addAdditionalFilter("Status", "IA", null, null, "Equals", false, true, false, "string");
                        displayTitle = "Request";
                        queryCode = "CustomerTenantAccesses";
                    }
                    break;
                }
            case "All":
                {
                    displayTitle = "Request";
                    this.filterAgrs.SortBy = "RequestDateTime";
                    this.filterAgrs.SortDirection = "Descending";
                    queryCode = "CustomerTenantAccesses";
                    showViews = true;
                    break;
                }
        }
        if (navigate) {
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            if (queryCode != "") {
                listArgs.QueryCode = queryCode;
            }
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.ShowViews = showViews;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    CutsomerTenantAccessManagementComponent.prototype.ActivityZoomLinkClick = function (m) {
        var windowArgs = {};
        windowArgs.TenantPM = this.myTenantPM;
        switch (m) {
            case "Today Customers":
                {
                    windowArgs.PartnerTypeId = "CS";
                    windowArgs.DateParameter = "T";
                    windowArgs.DataContext = this;
                    // model = new ActivityZoomViewModel("CS", "T", myPartnersContext);
                    //control.DataContext = model;
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
    CutsomerTenantAccessManagementComponent.prototype.ShowDetailsButtonclick = function (item) {
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
    CutsomerTenantAccessManagementComponent.prototype.ViewBlocedEntity = function (item) {
        var windowArgs = {};
        windowArgs.CustomerPM = item;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Title = "View Customer";
        logitudeWindow.Height = 500;
        logitudeWindow.Width = 800;
        logitudeWindow.Show("./SharedLogistics/Components/ViewBlocedCustomerComponent");
    };
    CutsomerTenantAccessManagementComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CutsomerTenantAccessManagementComponent.html',
            providers: [SharedLogisticsService_1.SharedLogisticsService, DocumentTypeListService_1.DocumentTypeListService],
        }),
        __metadata("design:paramtypes", [SharedLogisticsService_1.SharedLogisticsService, DocumentTypeListService_1.DocumentTypeListService])
    ], CutsomerTenantAccessManagementComponent);
    return CutsomerTenantAccessManagementComponent;
}());
exports.CutsomerTenantAccessManagementComponent = CutsomerTenantAccessManagementComponent;
//# sourceMappingURL=CutsomerTenantAccessManagementComponent.js.map