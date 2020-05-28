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
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DateTimeToDatePipe_1 = require("../../../Controls/Pipes/DateTimeToDatePipe");
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_1 = require("../../../Infrastructure/Args");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var OpportunityListService_1 = require("../../Services/StandardLists/OpportunityListService");
var UpcomingActivityItem_1 = require("../../Components/Workspaces/UpcomingActivityItem");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_2 = require("../../Args");
var ContactListService_1 = require("../../../Common/Services/StandardLists/ContactListService");
var ServiceLocator_1 = require("../../../Infrastructure/Locators/ServiceLocator");
var OverviewWorkspaceComponent = /** @class */ (function (_super) {
    __extends(OverviewWorkspaceComponent, _super);
    function OverviewWorkspaceComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.SalesFunnelId = "SalesFunnelId_";
        _this.DataContext = _this;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_RecordsType = "RecordsType";
        _this.filterName_CreatedByType = "CreatedByType";
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterName_SalesFunnel = "SalesFunnel";
        _this.filterName_TopOpportunities = "TopOpportunities";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.OverviewPageControl";
        _this.OwnerId = null;
        _this.BusinessUnitId = null;
        _this.RecordsTypeFilterCode = null;
        _this.CreatedByTypeFilterCode = null;
        _this.BusinessUnitFilterCode = null;
        _this.RecordsTypesFilterList = [];
        _this.CreatedByTypesFilterList = [];
        _this.BusinessUnitFilterList = [];
        _this.BusinessUnitUsersFilterList = [];
        _this.IsBusinessUnitUsers = false;
        // Upcoming Activities
        _this.UpcomingActivitiesCount = 0;
        _this.Quotes_TodayIsEnabled = false;
        _this.Quotes_YesterdayIsEnabled = false;
        _this.Quotes_LastWeekIsEnabled = false;
        _this.Potentials_TodayIsEnabled = false;
        _this.Potentials_YesterdayIsEnabled = false;
        _this.Potentials_LastWeekIsEnabled = false;
        _this.Customers_TodayIsEnabled = false;
        _this.Customers_YesterdayIsEnabled = false;
        _this.Customers_LastWeekIsEnabled = false;
        _this.Activities_TodayIsEnabled = false;
        _this.Activities_YesterdayIsEnabled = false;
        _this.Activities_LastWeekIsEnabled = false;
        _this.Opportunities_TodayIsEnabled = false;
        _this.Opportunities_YesterdayIsEnabled = false;
        _this.Opportunities_LastWeekIsEnabled = false;
        _this.Quotes_Today = "0";
        _this.Quotes_Yesterday = "0";
        _this.Quotes_LastWeek = "0";
        _this.Potentials_Today = "0";
        _this.Potentials_Yesterday = "0";
        _this.Potentials_LastWeek = "0";
        _this.Customers_Today = "0";
        _this.Customers_Yesterday = "0";
        _this.Customers_LastWeek = "0";
        _this.Activities_Today = "0";
        _this.Activities_Yesterday = "0";
        _this.Activities_LastWeek = "0";
        _this.Opportunities_Today = "0";
        _this.Opportunities_Yesterday = "0";
        _this.Opportunities_LastWeek = "0";
        _this.FunnelDataFilterd = [];
        _this.SalesFunnelId = "SalesFunnel_" + _this.CurrentSession.GetNewId("SalesFunnel");
        _this.InitializeServices();
        _this.LoadNonFilteredQueries();
        _this.BuildFilters();
        _this.InitializeFilters();
        return _this;
    }
    OverviewWorkspaceComponent.prototype.InitComponent = function (father) {
        this.FatherComp = father;
    };
    OverviewWorkspaceComponent.prototype.InitializeServices = function () {
        this.myDomainService = new CRMDomainService_1.CRMDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
    };
    OverviewWorkspaceComponent.prototype.InitializeFilters = function () {
        var _this = this;
        this.RecordsTypeFilterCode = "S";
        // Records Types
        //this.RecordsTypesFilterList = [];
        //this.RecordsTypesFilterList.push(new CodeNameClass("S", "Salesmen Records"));
        //this.RecordsTypesFilterList.push(new CodeNameClass("C", "Created By Records"));
        //this.RecordsTypeFilterCode = LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_RecordsType);
        //if (AppTool.IsNullOrEmpty(this.RecordsTypeFilterCode)) {
        //    this.RecordsTypeFilterCode = "S";
        //}
        //this.selectedRecordsTypeFilter = this.RecordsTypesFilterList.filter(d => d.Code == this.RecordsTypeFilterCode)[0];
        // CreatedBy Types
        //this.CreatedByTypesFilterList = [];
        //this.CreatedByTypesFilterList.push(new CodeNameClass("M", "Created By Me"));
        //this.CreatedByTypesFilterList.push(new CodeNameClass("All", "Created By"));
        // Business Units
        this.myBusinessUnitListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.BusinessUnitFilterList = [];
                _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass("M", "My Records"));
                if (list) {
                    list.filter(function (d) { return d.Id != SessionLocator_1.SessionLocator.Tenant.toString(); }).forEach(function (item) {
                        _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass(item.Id, item.Name));
                    });
                }
                _this.BusinessUnitFilterList.push(new CodeNameClass_1.CodeNameClass("A", "All Records"));
            }
            _this.OnFiltersInitialized();
        });
    };
    OverviewWorkspaceComponent.prototype.OnFiltersInitialized = function () {
        var _this = this;
        if (this.RecordsTypeFilterCode == "C") {
            this.CreatedByTypeFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreatedByType);
            if (Tools_1.AppTool.IsNullOrEmpty(this.CreatedByTypeFilterCode)) {
                this.CreatedByTypeFilterCode = "M";
            }
            this.selectedCreatedByTypeFilter = this.CreatedByTypesFilterList.filter(function (d) { return d.Code == _this.CreatedByTypeFilterCode; })[0];
            if (this.CreatedByTypeFilterCode == "M") {
                this.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
            }
            else {
                this.OwnerId = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
            }
            this.LoadFilteredQueries();
        }
        else {
            this.BusinessUnitFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_BusinessUnit);
            if (Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitFilterCode)) {
                this.BusinessUnitFilterCode = "M";
            }
            this.selectedBusinessUnitFilter = this.BusinessUnitFilterList.filter(function (d) { return d.Code == _this.BusinessUnitFilterCode; })[0];
            switch (this.BusinessUnitFilterCode) {
                case "M": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
                    this.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                    this.LoadFilteredQueries();
                    break;
                }
                case "A": {
                    this.IsBusinessUnitUsers = false;
                    this.BusinessUnitId = null;
                    this.OwnerId = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    this.listOfValuesUserId = this.OwnerId;
                    this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                    this.LoadFilteredQueries();
                    break;
                }
                default: {
                    this.IsBusinessUnitUsers = true;
                    this.BusinessUnitId = this.BusinessUnitFilterCode;
                    var item = new CodeNameClass_1.CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                    this.BusinessUnitUsersFilterList.push(item);
                    var filters = new ApiQueryFilters_1.ApiQueryFilters();
                    filters.PageIndex = 0;
                    filters.PageSize = 100;
                    filters.Filter1Name = "BusinessUnitId";
                    filters.Filter1Value = this.BusinessUnitId;
                    filters.Filter1Operator = "Equals";
                    this.myUserListService.getAllFromCache(filters).subscribe(function (myResponse) {
                        if (!myResponse.HasError) {
                            var loadedUsers = myResponse.Result;
                            if (loadedUsers != null) {
                                loadedUsers.forEach(function (list) {
                                    _this.BusinessUnitUsersFilterList.push(new CodeNameClass_1.CodeNameClass(list.Id, list.EnglishName));
                                });
                            }
                        }
                        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(_this.filterControlNameSpace, _this.filterName_Owner);
                        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
                            defaultFilterCode = null;
                        }
                        _this.OwnerId = defaultFilterCode;
                        _this.listOfValuesUserId = _this.OwnerId;
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.OwnerId)) {
                            _this.selectedUserFilter = _this.BusinessUnitUsersFilterList.filter(function (d) { return d.Code == _this.OwnerId; })[0];
                        }
                        if (_this.selectedUserFilter == null) {
                            _this.selectedUserFilter = _this.BusinessUnitUsersFilterList[0];
                        }
                        _this.LoadFilteredQueries();
                    });
                    break;
                }
            }
        }
    };
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedRecordsTypeFilter", {
        get: function () { return this.selectedRecordsTypeFilter; },
        set: function (value) {
            if (this.selectedRecordsTypeFilter != value) {
                this.selectedRecordsTypeFilter = value;
                this.RecordsTypeFilterCode = value == null ? "S" : value.Code;
                this.BusinessUnitFilterCode = "M";
                this.CreatedByTypeFilterCode = "M";
                this.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
                this.BusinessUnitId = this.RecordsTypeFilterCode == "S" ? SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId : null;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_RecordsType, this.RecordsTypeFilterCode);
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.OnFiltersInitialized();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedCreatedByTypeFilter", {
        get: function () { return this.selectedCreatedByTypeFilter; },
        set: function (value) {
            if (this.selectedCreatedByTypeFilter != value) {
                this.selectedCreatedByTypeFilter = value;
                this.CreatedByTypeFilterCode = value == null ? "M" : value.Code;
                this.OwnerId = this.CreatedByTypeFilterCode == "M" ? SessionLocator_1.SessionLocator.LoggedUserId : null;
                this.listOfValuesUserId = this.OwnerId;
                this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', this.CreatedByTypeFilterCode == "M" ? false : true);
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreatedByType, this.CreatedByTypeFilterCode);
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.LoadFilteredQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedBusinessUnitFilter", {
        get: function () { return this.selectedBusinessUnitFilter; },
        set: function (value) {
            var _this = this;
            if (this.selectedBusinessUnitFilter != value) {
                this.selectedBusinessUnitFilter = value;
                this.BusinessUnitFilterCode = value == null ? "M" : value.Code;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, this.BusinessUnitFilterCode);
                switch (this.BusinessUnitFilterCode) {
                    case "M": {
                        this.IsBusinessUnitUsers = false;
                        this.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
                        this.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
                        this.listOfValuesUserId = this.OwnerId;
                        this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', false);
                        LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        this.LoadFilteredQueries();
                        break;
                    }
                    case "A": {
                        this.IsBusinessUnitUsers = false;
                        this.BusinessUnitId = null;
                        this.OwnerId = null;
                        this.listOfValuesUserId = this.OwnerId;
                        this.UIProperties.SetEnabled("ListOfValuesUserId", 'CRM_UsersFilter', true);
                        LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        this.LoadFilteredQueries();
                        break;
                    }
                    default: {
                        this.IsBusinessUnitUsers = true;
                        this.BusinessUnitId = this.BusinessUnitFilterCode;
                        this.OwnerId = null;
                        this.listOfValuesUserId = this.OwnerId;
                        this.BusinessUnitUsersFilterList = [];
                        var item = new CodeNameClass_1.CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                        this.BusinessUnitUsersFilterList.push(item);
                        var filters = new ApiQueryFilters_1.ApiQueryFilters();
                        filters.PageIndex = 0;
                        filters.PageSize = 100;
                        filters.Filter1Name = "BusinessUnitId";
                        filters.Filter1Value = this.BusinessUnitId;
                        filters.Filter1Operator = "Equals";
                        this.myUserListService.getAllFromCache(filters).subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                var loadedUsers = myResponse.Result;
                                if (loadedUsers != null) {
                                    loadedUsers.forEach(function (list) {
                                        _this.BusinessUnitUsersFilterList.push(new CodeNameClass_1.CodeNameClass(list.Id, list.EnglishName));
                                    });
                                }
                            }
                            _this.selectedUserFilter = _this.BusinessUnitUsersFilterList[0];
                            _this.LoadFilteredQueries();
                        });
                        break;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedUserFilter", {
        get: function () { return this.selectedUserFilter; },
        set: function (value) {
            if (this.selectedUserFilter != value) {
                this.selectedUserFilter = value;
                var myCode = value == null ? "A" : value.Code;
                this.OwnerId = myCode == "A" ? null : myCode;
                this.listOfValuesUserId = this.OwnerId;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.LoadFilteredQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "ListOfValuesUserId", {
        get: function () { return this.listOfValuesUserId; },
        set: function (value) {
            if (this.listOfValuesUserId != value) {
                this.OwnerId = value;
                this.listOfValuesUserId = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.LoadFilteredQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    OverviewWorkspaceComponent.prototype.BuildFilters = function () {
        this.BuildFunnelFilters();
        this.BuildTopOpportunitiesFilters();
    };
    OverviewWorkspaceComponent.prototype.BuildFunnelFilters = function () {
        this.FunneFilterList = [];
        this.FunneFilterList.push(new CodeNameClass_1.CodeNameClass("CNT", "Count"));
        this.FunneFilterList.push(new CodeNameClass_1.CodeNameClass("SHI", TextCodeTranslator_1.TextCodeTranslator.Translate("Opportunity.F.NumberOfShipments")));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SalesFunnel);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CNT";
        }
        this.selectedFunnelFilter = this.FunneFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedFunnelFilter", {
        get: function () { return this.selectedFunnelFilter; },
        set: function (value) {
            if (this.selectedFunnelFilter != value) {
                this.selectedFunnelFilter = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_SalesFunnel, (value == null ? null : value.Code));
                this.LoadFunnelData();
            }
        },
        enumerable: true,
        configurable: true
    });
    OverviewWorkspaceComponent.prototype.BuildTopOpportunitiesFilters = function () {
        this.TopOpportunitiesFilterList = [];
        this.TopOpportunitiesFilterList.push(new CodeNameClass_1.CodeNameClass("SHI", TextCodeTranslator_1.TextCodeTranslator.Translate("Opportunity.F.NumberOfShipments")));
        this.TopOpportunitiesFilterList.push(new CodeNameClass_1.CodeNameClass("STG", "Stage"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass_1.CodeNameClass("RAT", "Rating"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass_1.CodeNameClass("EST", "Est. Closing Date"));
        this.TopOpportunitiesFilterList.push(new CodeNameClass_1.CodeNameClass("DUE", "Stage Due Date"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopOpportunities);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "STG";
        }
        this.selectedTopOpportunitiesFilter = this.TopOpportunitiesFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    Object.defineProperty(OverviewWorkspaceComponent.prototype, "SelectedTopOpportunitiesFilter", {
        get: function () { return this.selectedTopOpportunitiesFilter; },
        set: function (value) {
            if (this.selectedTopOpportunitiesFilter != value) {
                this.selectedTopOpportunitiesFilter = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopOpportunities, (value == null ? null : value.Code));
                this.LoadTopOpportunities();
            }
        },
        enumerable: true,
        configurable: true
    });
    OverviewWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    OverviewWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    };
    OverviewWorkspaceComponent.prototype.LoadFilteredQueries = function () {
        this.LoadActivitiesSummary();
        this.LoadTopOpportunities();
        this.LoadFunnelData();
        this.LoadSpotLightData();
    };
    OverviewWorkspaceComponent.prototype.LoadNonFilteredQueries = function () {
        this.RunUpcomingBirthdaysFilter();
    };
    OverviewWorkspaceComponent.prototype.LoadActivitiesSummary = function () {
        var _this = this;
        this.myDomainService.GetUpcomigActivities(this.OwnerId, this.BusinessUnitId, null, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            if (myResult == null) {
                _this.UpcomingActivitiesList = [];
                _this.UpcomingActivitiesCount = 0;
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.FillUpcomingActivitiesList(list);
                }
            }
        });
    };
    OverviewWorkspaceComponent.prototype.FillUpcomingActivitiesList = function (myList) {
        var _this = this;
        this.UpcomingActivitiesList = [];
        myList.forEach(function (item) {
            var itemViewModel = new UpcomingActivityItem_1.UpcomingActivityItem(item, null, _this);
            _this.UpcomingActivitiesList.push(itemViewModel);
        });
        this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
        //if (this.UpcomingActivitiesList == null) {
        //    this.UpcomingActivitiesList = new Array<UpcomingActivityItem>();
        //}
        //else {
        //    this.UpcomingActivitiesList = [];
        //}
        //myList.sort((a, b) => {
        //    return (DateTool.GetDateFromDate(a.SortByDate) === DateTool.GetDateFromDate(b.SortByDate)) ? 0 : (DateTool.GetDateFromDate(a.SortByDate) < DateTool.GetDateFromDate(b.SortByDate)) ? -1 : 1
        //}).forEach((item) => {
        //    var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, null, this);
        //    this.UpcomingActivitiesList.push(itemViewModel);
        //});
        //this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
    };
    OverviewWorkspaceComponent.prototype.NewActivityClicked = function (code) {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var windowTitle = "";
        var windowTitleIcon = "";
        var args = new Args_2.ActivityInputArgs();
        args.Activity = this.EntityPM;
        args.TypeCode = code.toUpperCase();
        logWindow.WindowArgs = args;
        switch (code.toUpperCase()) {
            case "TS":
                {
                    windowTitle = "New Task";
                    windowTitleIcon = "./Images/Activities/TS.png";
                    break;
                }
            case "CL": {
                windowTitle = "New Phone Call";
                windowTitleIcon = "./Images/Activities/CL.png";
                break;
            }
            case "AP": {
                windowTitle = "New Appointment";
                windowTitleIcon = "./Images/Activities/AP.png";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: {
                break;
            }
        }
        var windowArgs = new Args_2.ActivityInputArgs();
        windowArgs.TypeCode = code;
        windowArgs.IsAddCustomerAllowed = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s == "ok") {
                _this.LoadAllScreenData();
            }
        });
    };
    OverviewWorkspaceComponent.prototype.Updated = function (arg) {
        if (arg) {
            this.LoadActivitiesSummary();
        }
    };
    OverviewWorkspaceComponent.prototype.EditActivity = function (entity) {
        var _this = this;
        if (entity != null) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Activity', BackButtonLabel: "CRM" });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.LoadActivitiesSummary();
                    });
                });
            });
        }
    };
    OverviewWorkspaceComponent.prototype.LoadSpotLightData = function () {
        var _this = this;
        this.myDomainService.GetCRMDailySpotlightCounts(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.SpotlightData = myResponse.Result;
                if (_this.SpotlightData != null) {
                    _this.SetNumbersProperties();
                    _this.SetEnabledProperties();
                }
            }
        });
    };
    OverviewWorkspaceComponent.prototype.SetEnabledProperties = function () {
        this.Quotes_TodayIsEnabled = this.Quotes_Today != "0";
        this.Quotes_YesterdayIsEnabled = this.Quotes_Yesterday != "0";
        this.Quotes_LastWeekIsEnabled = this.Quotes_LastWeek != "0";
        this.Potentials_TodayIsEnabled = this.Potentials_Today != "0";
        this.Potentials_YesterdayIsEnabled = this.Potentials_Yesterday != "0";
        this.Potentials_LastWeekIsEnabled = this.Potentials_LastWeek != "0";
        this.Customers_TodayIsEnabled = this.Customers_Today != "0";
        this.Customers_YesterdayIsEnabled = this.Customers_Yesterday != "0";
        this.Customers_LastWeekIsEnabled = this.Customers_LastWeek != "0";
        this.Activities_TodayIsEnabled = this.Activities_Today != "0";
        this.Activities_YesterdayIsEnabled = this.Activities_Yesterday != "0";
        this.Activities_LastWeekIsEnabled = this.Activities_LastWeek != "0";
        this.Opportunities_TodayIsEnabled = this.Opportunities_Today != "0";
        this.Opportunities_YesterdayIsEnabled = this.Opportunities_Yesterday != "0";
        this.Opportunities_LastWeekIsEnabled = this.Opportunities_LastWeek != "0";
    };
    OverviewWorkspaceComponent.prototype.SetNumbersProperties = function () {
        this.Quotes_Today = this.SpotlightData.Quotes_Today.toString();
        this.Quotes_Yesterday = this.SpotlightData.Quotes_Yesterday.toString();
        this.Quotes_LastWeek = this.SpotlightData.Quotes_LastWeek.toString();
        this.Potentials_Today = this.SpotlightData.PotentialCustomers_Today.toString();
        this.Potentials_Yesterday = this.SpotlightData.PotentialCustomers_Yesterday.toString();
        this.Potentials_LastWeek = this.SpotlightData.PotentialCustomers_LastWeek.toString();
        this.Customers_Today = this.SpotlightData.Customers_Today.toString();
        this.Customers_Yesterday = this.SpotlightData.Customers_Yesterday.toString();
        this.Customers_LastWeek = this.SpotlightData.Customers_LastWeek.toString();
        this.Activities_Today = this.SpotlightData.Activities_Today.toString();
        this.Activities_Yesterday = this.SpotlightData.Activities_Yesterday.toString();
        this.Activities_LastWeek = this.SpotlightData.Activities_LastWeek.toString();
        this.Opportunities_Today = this.SpotlightData.Opportunities_Today.toString();
        this.Opportunities_Yesterday = this.SpotlightData.Opportunities_Yesterday.toString();
        this.Opportunities_LastWeek = this.SpotlightData.Opportunities_LastWeek.toString();
    };
    OverviewWorkspaceComponent.prototype.DailySpotLightClicked = function (queryCode) {
        var _this = this;
        var myQueryCode = "";
        var displayName = "";
        var myTableName = "";
        var myOwnerId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }
        var myBusinessUnitId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        switch (queryCode) {
            case "QT_TD":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Today Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "QT_YS":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Yesterday Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "QT_LW":
                {
                    myTableName = "Quote";
                    myQueryCode = "All Quotes";
                    displayName = "Last Week Quotes";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Quotes Zoom");
                    filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "PO_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Today Potential Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "PO_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Yesterday Potential Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "PO_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "ShippersAndConsignees";
                    displayName = "Last Week Potential Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Potential Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "POT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "CS_TD":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Today Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "CS_YS":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Yesterday Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "CS_LW":
                {
                    myTableName = "Customer";
                    myQueryCode = "Customers";
                    displayName = "Last Week Customers";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Customers Zoom");
                    filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("IsCustomer", true, null, null, "Equals", false, false, false, "boolean");
                    filters.addAdditionalFilter("CustomerStatusCode", "ACT", null, null, "Equals", false, false, false, "string");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("CustomersBusinessUnitFilter", myOwnerId == null ? "null" : myOwnerId, myBusinessUnitId, null, "Equals", true, false, false, "string");
                    }
                    break;
                }
            case "AC_TD":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Today Activities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "AC_YS":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Yesterday Activities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "AC_LW":
                {
                    myTableName = "Activity";
                    myQueryCode = "All Activities";
                    displayName = "Last Week Activities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Activities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "OP_TD":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Today Opportunities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "OP_YS":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Yesterday Opportunities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
            case "OP_LW":
                {
                    myTableName = "Opportunity";
                    myQueryCode = "All Opportunities";
                    displayName = "Last Week Opportunities";
                    ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Opportunities Zoom");
                    filters.addAdditionalFilter("DailySpotlightFilter", queryCode, null, null, "Equals", true, false, false, "string");
                    if (this.RecordsTypeFilterCode == "C") {
                        filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
                    }
                    else {
                        filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
                        filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
                    }
                    break;
                }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(myQueryCode)) {
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filters;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = displayName;
            listArgs.BackButtonTitle = "CRM";
            listArgs.ShowViews = false;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    OverviewWorkspaceComponent.prototype.EditOpportunity = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.entityList.Id, ObjectTableName: 'Opportunity', BackButtonLabel: "CRM" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
                //this.isWindowOpened = false;
            });
        });
    };
    OverviewWorkspaceComponent.prototype.LoadFunnelData = function () {
        var _this = this;
        this.myDomainService.GetStageFunnelData(this.OwnerId, this.BusinessUnitId, this.SelectedFunnelFilter.Code, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            _this.FunnelData = myResult.Result;
            _this.fillFunnelData();
        });
    };
    OverviewWorkspaceComponent.prototype.fillFunnelData = function () {
        var _this = this;
        try {
            var labelArr = [];
            var dataArr = [];
            this.FunnelDataFilterd = [];
            var i = 0;
            var sum = 0;
            this.FunnelData.forEach(function (p) {
                _this.FunnelDataFilterd[i] = { title: p.LabelProperty, value: p.DecimalProperty };
                i++;
                sum += p.DecimalProperty;
            });
            makeChart(this.SalesFunnelId, this.FunnelDataFilterd, sum);
        }
        catch (e) { }
    };
    OverviewWorkspaceComponent.prototype.FunnelClick = function () {
        var _this = this;
        var item = FunnelClick();
        ResetItemFunnel();
        if (item != null) {
            console.log(item);
            var objectTableName = "Opportunity";
            var queryCode = "All Open Opportunities";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Opportunities";
            var backButtonTitle = "CRM";
            if (this.FunnelData[item.index].DataTypeCode == "SHI") {
                displayTitle += " (" + this.FunnelData[item.index].DecimalProperty + " Shipments)";
            }
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var listArgs = new Args_1.ListComponentArgs();
            var myOwnerId = null;
            var myBusinessUnitId = null;
            var myFilterCode = null;
            if (this.FunnelData[item.index].OwnerId != null && this.FunnelData[item.index].OwnerId != "") {
                myOwnerId = this.FunnelData[item.index].OwnerId;
            }
            if (this.FunnelData[item.index].BusinessUnitId != null && this.FunnelData[item.index].BusinessUnitId != "") {
                myBusinessUnitId = this.FunnelData[item.index].BusinessUnitId;
            }
            if (this.FunnelData[item.index].DataTypeCode != null && this.FunnelData[item.index].DataTypeCode != "") {
                myFilterCode = this.FunnelData[item.index].DataTypeCode;
            }
            filterAgrs.addAdditionalFilter("StageId", this.FunnelData[item.index].GroupedId, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", true, false, false, "Boolean");
            filterAgrs.addAdditionalFilter("FunnelFilterCode", myFilterCode, null, null, "Equals", true, false, false, "string");
            if (this.RecordsTypeFilterCode == "C") {
                filterAgrs.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", true, false, false, "Boolean");
            }
            else {
                filterAgrs.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", true, false, false, "Boolean");
                filterAgrs.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", true, false, false, "string");
            }
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run(listArgs);
                    cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
    };
    OverviewWorkspaceComponent.prototype.LoadTopOpportunities = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";
        var myOwnerId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }
        var myBusinessUnitId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;
        }
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "boolean");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        if (this.RecordsTypeFilterCode == "C") {
            filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }
        else {
            filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
        }
        switch (this.SelectedTopOpportunitiesFilter.Code) {
            case "STG":
                {
                    filters.SortBy = "StageProbability";
                    break;
                }
            case "SHI":
                {
                    filters.SortBy = "NumberOfShipments";
                    break;
                }
            case "RAT":
                {
                    filters.SortBy = "RatingCode";
                    break;
                }
            case "EST":
                {
                    filters.SortBy = "EstimatedClosingDate";
                    break;
                }
            case "DUE":
                {
                    filters.SortBy = "StageDueDate";
                    break;
                }
        }
        var myService = new OpportunityListService_1.OpportunityListService();
        myService.getByFilters(filters).subscribe(function (myResult) {
            if (myResult == null) {
                _this.TopOpportunitiesList = [];
            }
            else {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.FillTopOpportunitiesList(list);
                }
            }
        });
    };
    OverviewWorkspaceComponent.prototype.FillTopOpportunitiesList = function (myList) {
        var _this = this;
        if (this.TopOpportunitiesList == null) {
            this.TopOpportunitiesList = new Array();
        }
        else {
            this.TopOpportunitiesList = [];
        }
        switch (this.SelectedTopOpportunitiesFilter.Code) {
            case "STG":
                {
                    var tempList = myList.sort(function (a, b) { return b.RatingIndexOrder - a.RatingIndexOrder; });
                    tempList.sort(function (a, b) { return b.StageProbability - a.StageProbability; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter.Code);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "SHI":
                {
                    myList.sort(function (a, b) { return b.NumberOfShipments - a.NumberOfShipments; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter.Code);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "RAT":
                {
                    var tempList = myList; //.sort((a, b) => { return b.StageName - a.StageName });
                    tempList.sort(function (a, b) { return b.RatingIndexOrder - a.RatingIndexOrder; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter.Code);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "EST":
                {
                    //.sort((a, b) => { return b.EstimatedClosingDate.valueOf() - a.EstimatedClosingDate.valueOf() })
                    myList.forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter.Code);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "DUE":
                {
                    //.sort((a, b) => { return b.StageDueDate.valueOf() - a.StageDueDate.valueOf() })
                    myList.forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter.Code);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
        }
    };
    // LoadUpcomingBirthdays
    OverviewWorkspaceComponent.prototype.RunUpcomingBirthdaysFilter = function () {
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue("Logitude.CRM.Views.CRMPages.ContactsPageControl", "ViewUpcomingBirthdays");
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "W05";
        }
        switch (defaultFilterCode) {
            case "W05":
                {
                    this.LoadUpcomingBirthdays(0, 5);
                    break;
                }
            case "W10":
                {
                    this.LoadUpcomingBirthdays(0, 10);
                    break;
                }
            case "W30":
                {
                    this.LoadUpcomingBirthdays(0, 30);
                    break;
                }
            case "TOD":
                {
                    this.LoadUpcomingBirthdays(0, 0);
                    break;
                }
        }
    };
    OverviewWorkspaceComponent.prototype.LoadUpcomingBirthdays = function (start, end) {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.addAdditionalFilter("UpcomingBirthdaysFilter", start, end, null, "Equals", true, true, false, "number");
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.GetCount = true;
        var service = new ContactListService_1.ContactListService();
        service.getByFilters(filters).subscribe(function (myResult) {
            if (myResult != null) {
                _this.FatherComp.UpcomingCount = myResult.Count;
                if (myResult.Count != 0) {
                    _this.FatherComp.UpcomingCountVisibility = true;
                }
                else {
                    _this.FatherComp.UpcomingCountVisibility = false;
                }
            }
        });
    };
    OverviewWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OverviewWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], OverviewWorkspaceComponent);
    return OverviewWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.OverviewWorkspaceComponent = OverviewWorkspaceComponent;
var TopOpportunityItem = /** @class */ (function () {
    function TopOpportunityItem(entityList, filterCode) {
        this.entityList = entityList;
        this.filterCode = filterCode;
        this.ComputeFilterValue();
    }
    Object.defineProperty(TopOpportunityItem.prototype, "RatingCode", {
        get: function () { return this.entityList.RatingCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "RatingName", {
        get: function () { return this.entityList.RatingName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "Topic", {
        get: function () { return this.entityList.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "CustomerName", {
        get: function () { return this.entityList.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "StageName", {
        get: function () { return this.entityList.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "StageAge", {
        get: function () { return this.entityList.LastStageDate; },
        enumerable: true,
        configurable: true
    });
    TopOpportunityItem.prototype.ComputeFilterValue = function () {
        var myResult = null;
        switch (this.filterCode) {
            case "CRD":
                {
                    myResult = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.entityList.CreateDate);
                    break;
                }
            case "EST":
                {
                    myResult = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.entityList.EstimatedClosingDate);
                    break;
                }
            case "DUE":
                {
                    myResult = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.entityList.StageDueDate);
                    break;
                }
            case "SHI":
                {
                    myResult = this.entityList.NumberOfShipments + "";
                    break;
                }
            case "RAT":
                {
                    myResult = this.entityList.RatingName;
                    break;
                }
        }
        this.FilterValue = myResult;
    };
    return TopOpportunityItem;
}());
exports.TopOpportunityItem = TopOpportunityItem;
//# sourceMappingURL=OverviewWorkspaceComponent.js.map