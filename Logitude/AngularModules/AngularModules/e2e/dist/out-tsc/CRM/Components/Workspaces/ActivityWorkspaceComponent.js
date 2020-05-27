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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_1 = require("../../../Infrastructure/Args");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var UpcomingActivityItem_1 = require("../../Components/Workspaces/UpcomingActivityItem");
var Args_2 = require("../../Args");
var Tools_2 = require("../../Tools");
var ActivityWorkspaceComponent = /** @class */ (function (_super) {
    __extends(ActivityWorkspaceComponent, _super);
    function ActivityWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this.ReloadUserQueries = new core_1.EventEmitter();
        _this.DataContext = _this;
        _this.QuickSearchItems = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_RecordsType = "RecordsType";
        _this.filterName_CreatedByType = "CreatedByType";
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.ActivitiesPageControl";
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
        _this.selectedActivityFilter = "All";
        // Queries Features
        _this.IsQueryVisible_OpenGroup = false;
        _this.IsQueryVisible_MyOpen = false;
        _this.IsQueryVisible_AllOpen = false;
        _this.IsQueryVisible_ClosedGroup = false;
        _this.IsQueryVisible_MyClosed = false;
        _this.IsQueryVisible_AllClosed = false;
        _this.IsQueryVisible_Meetings = false;
        _this.IsQueryVisible_OthersGroup = false;
        _this.IsQueryVisible_All = false;
        _this.IsQueryVisible_Cancelled = false;
        _this.IsQueryVisible_MyViewsGroup = false;
        //Upcoming Activities
        _this.UpcomingActivitiesCount = 0;
        _this.barChartColors = [
            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',
                borderWidth: 0
            },
            {
                backgroundColor1: '#21782E',
                backgroundColor2: '#90bb96',
                borderWidth: 0,
            },
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',
                borderWidth: 0,
            },
        ];
        //Dashboard Propereties
        _this.InProgressBookingYAxis = [];
        _this.InProgressBookingYAxisFilterd = [];
        _this.InProgressBookingXAxis = [];
        _this.ChartID = null;
        _this.InProgressBookingId = "InProgressBookingId_";
        _this.isResizing = false;
        _this.ChartLeft = 0;
        _this.lastDownX = 0;
        _this.lastDownY = 0;
        _this.ChartID = "ChartID_" + _this.CurrentSession.GetChartId();
        _this.InProgressBookingId = _this.InProgressBookingId + _this.CurrentSession.GetChartId();
        _this.InitializeServices();
        _this.SetQueriesVisibility();
        _this.LoadNonFilteredQueries();
        _this.InitializeFilters();
        return _this;
    }
    ActivityWorkspaceComponent.prototype.InitializeServices = function () {
        this.myDomainService = new CRMDomainService_1.CRMDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
    };
    ActivityWorkspaceComponent.prototype.InitializeFilters = function () {
        var _this = this;
        // Records Types
        this.RecordsTypesFilterList = [];
        this.RecordsTypesFilterList.push(new CodeNameClass_1.CodeNameClass("S", "Salesman Records"));
        this.RecordsTypesFilterList.push(new CodeNameClass_1.CodeNameClass("C", "Created By Records"));
        this.RecordsTypeFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_RecordsType);
        if (Tools_1.AppTool.IsNullOrEmpty(this.RecordsTypeFilterCode)) {
            this.RecordsTypeFilterCode = "S";
        }
        this.selectedRecordsTypeFilter = this.RecordsTypesFilterList.filter(function (d) { return d.Code == _this.RecordsTypeFilterCode; })[0];
        // CreatedBy Types
        this.CreatedByTypesFilterList = [];
        this.CreatedByTypesFilterList.push(new CodeNameClass_1.CodeNameClass("M", "Created By Me"));
        this.CreatedByTypesFilterList.push(new CodeNameClass_1.CodeNameClass("All", "Created By"));
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
    ActivityWorkspaceComponent.prototype.OnFiltersInitialized = function () {
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
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "SelectedRecordsTypeFilter", {
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
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "SelectedCreatedByTypeFilter", {
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
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "SelectedBusinessUnitFilter", {
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
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "ListOfValuesUserId", {
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
    ActivityWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    ActivityWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    ActivityWorkspaceComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    Object.defineProperty(ActivityWorkspaceComponent.prototype, "SelectedActivityFilter", {
        get: function () { return this.selectedActivityFilter; },
        set: function (value) {
            if (this.selectedActivityFilter != value) {
                this.selectedActivityFilter = value;
                this.LoadAllScreenData();
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    };
    ActivityWorkspaceComponent.prototype.LoadFilteredQueries = function () {
        this.LoadChartData();
        this.LoadDataCounts();
        this.LoadUpcomingEntities();
    };
    ActivityWorkspaceComponent.prototype.LoadNonFilteredQueries = function () {
        this.ReloadUsersQuery();
        //this.LoadDataCounts();
    };
    ActivityWorkspaceComponent.prototype.SetQueriesVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyOpenActivities")) {
            this.IsQueryVisible_OpenGroup = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllOpenActivities")) {
            this.IsQueryVisible_OpenGroup = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyOpenActivities")) {
            this.IsQueryVisible_MyOpen = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllOpenActivities")) {
            this.IsQueryVisible_AllOpen = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyClosedActivities")) {
            this.IsQueryVisible_ClosedGroup = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllClosedActivities")) {
            this.IsQueryVisible_ClosedGroup = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MeetingsSummary")) {
            this.IsQueryVisible_ClosedGroup = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MyClosedActivities")) {
            this.IsQueryVisible_MyClosed = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllClosedActivities")) {
            this.IsQueryVisible_AllClosed = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.MeetingsSummary")) {
            this.IsQueryVisible_Meetings = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllActivities")) {
            this.IsQueryVisible_OthersGroup = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.CancelledActivities")) {
            this.IsQueryVisible_OthersGroup = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.AllActivities")) {
            this.IsQueryVisible_All = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "Activity.Q.CancelledActivities")) {
            this.IsQueryVisible_Cancelled = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.IsQueryVisible_MyViewsGroup = true;
        }
    };
    ActivityWorkspaceComponent.prototype.LoadDataCounts = function () {
        var _this = this;
        this.myDomainService.GetActivitiesSummary(this.SelectedActivityFilter, this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.MyOpenCount = myData.MyOpenDataCount;
                    _this.AllOpenCount = myData.AllOpenDataCount;
                }
            }
        });
    };
    ActivityWorkspaceComponent.prototype.LoadUpcomingEntities = function () {
        var _this = this;
        this.myDomainService.GetUpcomigActivities(this.OwnerId, this.BusinessUnitId, this.selectedActivityFilter, this.RecordsTypeFilterCode).subscribe(function (myResult) {
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
    ActivityWorkspaceComponent.prototype.FillUpcomingActivitiesList = function (myList) {
        var _this = this;
        this.UpcomingActivitiesList = [];
        myList.forEach(function (item) {
            var itemViewModel = new UpcomingActivityItem_1.UpcomingActivityItem(item, _this, null);
            _this.UpcomingActivitiesList.push(itemViewModel);
        });
        this.UpcomingActivitiesCount = this.UpcomingActivitiesList.length;
        //myList.sort((a, b) => {
        //    return (DateTool.GetDateFromDate(a.SortByDate).valueOf() === DateTool.GetDateFromDate(b.SortByDate).valueOf()) ? 0 : (DateTool.GetDateFromDate(a.SortByDate).valueOf() < DateTool.GetDateFromDate(b.SortByDate).valueOf()) ? -1 : 1
        //}).forEach((item) => {
        //    var itemViewModel: UpcomingActivityItem = new UpcomingActivityItem(item, this, null);
        //    this.UpcomingActivitiesList.push(itemViewModel);
        //});
    };
    // Chart
    ActivityWorkspaceComponent.prototype.LoadChartData = function () {
        var _this = this;
        this.myDomainService.GetActivitiesDashBoard(this.OwnerId, this.BusinessUnitId, this.selectedActivityFilter, this.RecordsTypeFilterCode).subscribe(function (result) {
            _this.InProgressBookingDashboard = result.Result;
            _this.FillInProgressBookingDashboardData();
        });
    };
    ActivityWorkspaceComponent.prototype.BarClicking = function () {
        if (BarClick() != null) {
            this.OnBarClick(BarClick());
            ResetItem();
        }
    };
    ActivityWorkspaceComponent.prototype.OnBarClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "All Activities";
        var displayName = "";
        var myTableName = "Activity";
        this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (flag) {
            var typeName = "";
            var typeCode = "";
            switch (Key + "") {
                case "0": {
                    typeName = "Tasks";
                    typeCode = "TS";
                    break;
                }
                case "1": {
                    typeName = "Phone Calls";
                    typeCode = "CL";
                    break;
                }
                case "2": {
                    typeName = "Appointments";
                    typeCode = "AP";
                    break;
                }
            }
            if ((item.category + "").toLowerCase() == "no date") {
                displayName = "No Date " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", "no date", null, null, "Equals", true, false, false, "string");
            }
            else if ((item.category + "").toLowerCase() == "old") {
                displayName = "Old " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", "old", null, null, "Equals", true, false, false, "string");
            }
            else {
                var date = this.InProgressBookingYAxisFilterd[Key].DateTime[item.index];
                var myFormats = Tools_1.DateTool.GetDateFormats(Tools_1.DateTool.GetDateParts(date).DateObject);
                myFormats.DateParts.DateObject.setHours(0, 0, 0, 0);
                var dayName = myFormats.DayName;
                displayName = dayName + " " + typeName;
                this.filterAgrs.addAdditionalFilter("ActivityNext7DaysCustomFilter", myFormats.DateParts.DateObject, null, null, "Equals", true, false, false, "date");
            }
        }
        this.filterAgrs.addAdditionalFilter("IsOpen", true, null, null, "Equals", false, false, false, "boolean");
        this.filterAgrs.addAdditionalFilter("ActivityTypeCode", typeCode, null, null, "Equals", false, false, false, "String");
        if (this.RecordsTypeFilterCode == "C") {
            this.filterAgrs.addAdditionalFilter("CreatedByUserId", this.InProgressBookingYAxisFilterd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        }
        else {
            this.filterAgrs.addAdditionalFilter("OwnerId", this.InProgressBookingYAxisFilterd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
            this.filterAgrs.addAdditionalFilter("BusinessUnitId", this.InProgressBookingYAxisFilterd[Key].BusinessUnitId[item.index], null, null, "Equals", false, false, false, "String");
        }
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = this.filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayName;
        listArgs.BackButtonTitle = "CRM";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ActivityWorkspaceComponent.prototype.FillInProgressBookingDashboardData = function () {
        var _this = this;
        var index = 0;
        this.InProgressBookingXAxis = [];
        this.InProgressBookingDashboard.sort(function (a, b) { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1; });
        var StringArr = new Array();
        var j = 0;
        this.InProgressBookingDashboard.forEach(function (element) {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                _this.InProgressBookingYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                _this.InProgressBookingYAxis[j].data = [];
                j++;
            }
        });
        var Graphs = [];
        var index = 0;
        this.InProgressBookingDashboard = this.InProgressBookingDashboard.filter(function (element) { return element.StringProperty == "TS" || element.StringProperty == "CL" || element.StringProperty == "AP"; });
        this.InProgressBookingDashboard.forEach(function (element) {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.LabelProperty == StringArr[i]) {
                    var k = 0;
                    if (element.StringProperty == "CL")
                        k = 1;
                    else if (element.StringProperty == "AP")
                        k = 2;
                    if (_this.InProgressBookingYAxis[i].data.length == 0)
                        _this.InProgressBookingYAxis[i].data = new Array(3);
                    _this.InProgressBookingYAxis[i].data[k] = element.IntegerProperty;
                    _this.InProgressBookingYAxis[i].label = element.StringProperty;
                    _this.InProgressBookingYAxis[i].BindingElement[k] = element.StringProperty;
                    _this.InProgressBookingYAxis[i].BusinessUnitId[k] = element.BusinessUnitId;
                    _this.InProgressBookingYAxis[i].DateTime[k] = element.DateTimeProperty;
                    _this.InProgressBookingYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!_this.InProgressBookingXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (_this.InProgressBookingXAxis[i] == null)
                            _this.InProgressBookingXAxis[i] = (element.LabelProperty);
                    }
                }
            }
        });
        this.InProgressBookingYAxisFilterd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (this.InProgressBookingYAxis.length > 0)
            maximum = this.InProgressBookingYAxis[0].data[0];
        this.InProgressBookingYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.InProgressBookingYAxisFilterd[i] == null) {
                    _this.InProgressBookingYAxisFilterd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.InProgressBookingYAxisFilterd[i].data.push(element.data[i]);
                _this.InProgressBookingYAxisFilterd[i].BindingElement.push(element.BindingElement[i]);
                _this.InProgressBookingYAxisFilterd[i].OwnerIds.push(element.OwnerIds[i]);
                _this.InProgressBookingYAxisFilterd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                _this.InProgressBookingYAxisFilterd[i].DateTime.push(element.DateTime[i]);
                if (index == 0) {
                    Graphs[i] = {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "lineAlpha": 0,
                        "id": "AmGraph-1" + i,
                        "title": element.BindingElement[i] + "",
                        "type": "column",
                        "valueField": "col" + (i + 1),
                        // "bulletBorderColor": "#FFFFFF",
                        "fillColors": [_this.barChartColors[i].backgroundColor1 + "", _this.barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    };
                }
                objectArray[i] = (element.data[i]);
            }
            DataProvider[index] = { "category": _this.InProgressBookingXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (this.InProgressBookingXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.InProgressBookingId, Graphs, DataProvider, maximum);
            }
        }
        catch (e) {
        }
    };
    ActivityWorkspaceComponent.prototype.OnMyMouseDown = function ($event, arg) {
        this.isResizing = true;
        var grid = document.getElementById(this.ChartID);
        var rec = grid.getBoundingClientRect();
        this.ChartLeft = rec.left;
        this.lastDownY = ($event.clientY - rec.bottom);
        this.lastDownX = ($event.clientX - this.ChartLeft);
    };
    ActivityWorkspaceComponent.prototype.ViewActivityQuery = function (code) {
        var _this = this;
        if (code != null) {
            var objectTableName = "Activity";
            var queryCode = null;
            var backButtonTitle = "CRM";
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedActivityFilter)) {
                if (this.SelectedActivityFilter != "All") {
                    filterAgrs.addAdditionalFilter("ActivityTypeCode", this.SelectedActivityFilter, null, null, "Equals", false, false, false, "String");
                }
            }
            if (this.RecordsTypeFilterCode == "C") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("CreatedByUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("OwnerId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                    filterAgrs.addAdditionalFilter("BusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                }
            }
            switch (code) {
                case "Open:My":
                    {
                        queryCode = "My Open Activities";
                        break;
                    }
                case "Open:All":
                    {
                        queryCode = "All Open Activities";
                        break;
                    }
                case "Closed:My":
                    {
                        queryCode = "My Closed Activities";
                        break;
                    }
                case "Closed:All":
                    {
                        queryCode = "All Closed Activities";
                        break;
                    }
                case "All":
                    {
                        queryCode = "All Activities";
                        break;
                    }
                case "Meetings":
                    {
                        queryCode = "Meetings Summary";
                        break;
                    }
                case "Cancelled":
                    {
                        queryCode = "Cancelled Activities";
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
            ;
        }
    };
    ActivityWorkspaceComponent.prototype.NewActivityClicked = function (typeCode) {
        var _this = this;
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        switch (typeCode) {
            case "TS":
                {
                    windowTitle = "New Task";
                    break;
                }
            case "CL": {
                windowTitle = "New Phone Call";
                break;
            }
            case "AP": {
                windowTitle = "New Appointment";
                logWindow.Width = 800;
                logWindow.Height = 600;
                break;
            }
            default: {
                break;
            }
        }
        windowTitleIcon = Tools_2.CRMTool.GetActivityImageSrc(typeCode);
        var windowArgs = new Args_2.ActivityInputArgs();
        windowArgs.TypeCode = typeCode;
        windowArgs.IsAddCustomerAllowed = true;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.LoadAllScreenData();
            }
        });
    };
    ActivityWorkspaceComponent.prototype.EditActivity = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Activity', BackButtonLabel: "Activities" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
            });
        });
    };
    ActivityWorkspaceComponent.prototype.Updated = function (arg) {
        if (arg) {
            this.LoadUpcomingEntities();
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ActivityWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    ActivityWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ActivityWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ActivityWorkspaceComponent);
    return ActivityWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.ActivityWorkspaceComponent = ActivityWorkspaceComponent;
//# sourceMappingURL=ActivityWorkspaceComponent.js.map