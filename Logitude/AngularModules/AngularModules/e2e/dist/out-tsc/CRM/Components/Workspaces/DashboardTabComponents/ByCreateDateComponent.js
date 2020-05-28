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
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var CodeNameClass_1 = require("../../../../Infrastructure/DataContracts/CodeNameClass");
var BusinessUnitListService_1 = require("../../../../Common/Services/StandardLists/BusinessUnitListService");
var LastFilterClass_1 = require("../../../../Infrastructure/Utilities/LastFilterClass");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var CRMUtilities_1 = require("../../../CRMUtilities");
var CRMDomainService_1 = require("../../../Services/CRMDomainService");
var Args_1 = require("../../../../Infrastructure/Args");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var ByCreateDateComponent = /** @class */ (function (_super) {
    __extends(ByCreateDateComponent, _super);
    function ByCreateDateComponent() {
        var _this = _super.call(this) || this;
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.DashboardTabsControls.ByCreateDateControl";
        _this.filterName_CreateDate = "CreateDate";
        _this.DateFilterList = [];
        _this.DataContext = _this;
        _this.fieldCode = "C";
        _this.NewCustomerDashboardListExistance = false;
        _this.NewOpportunitiesDashboardIdExistance = false;
        _this.NewQuotesDashboardIdExistance = false;
        _this.NewActivitiesDashboardIdExistance = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UsersFilterList = [];
        _this.BusinessUnitFilterList = [];
        _this.NewQuoteList = [];
        _this.NewActivitiesYAxisFitlerd = [];
        _this.NewCustomersYAxisFitlerd = [];
        _this.NewOpportunityList = [];
        _this.InitializeIds();
        _this.InitializeServices();
        _this.BuildBusinessUnitFilter();
        _this.BuildDateFilters();
        return _this;
    }
    ByCreateDateComponent.prototype.InitializeServices = function () {
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
        this.crmDomainService = new CRMDomainService_1.CRMDomainService();
    };
    ByCreateDateComponent.prototype.RefreshButtonClicked = function () {
        this.LoadFilteredQueries();
    };
    ByCreateDateComponent.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        var FromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedDateFilter.Code == "0") {
            days = 0;
            this.toDate = Todate;
            this.fromDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-1") {
            days = -1;
            FromDate.setDate(Todate.getDate() - 1);
            this.fromDate = FromDate;
            this.toDate = FromDate;
        }
        else if (this.SelectedDateFilter.Code == "-7") {
            days = -7;
            FromDate.setDate(Todate.getDate() - 6);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-30") {
            days = -30;
            FromDate.setMonth(Todate.getMonth() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-90") {
            days = -90;
            FromDate.setMonth(Todate.getMonth() - 3);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-365") {
            days = -365;
            FromDate.setMonth(Todate.getMonth() - 12);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        return days;
    };
    ByCreateDateComponent.prototype.BuildDateFilters = function () {
        this.DateFilterList = CRMUtilities_1.CRMUtilities.GetDateFilterList();
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CreateDate);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-30";
        }
        this.selectedDateFilter = this.DateFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
        if (this.selectedDateFilter.Code == "-2") {
            var ActiviytFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ByCreateFromDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.fromDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
            var ActiviytToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "ByCreateToDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.toDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
        }
    };
    ByCreateDateComponent.prototype.InitializeIds = function () {
        this.NewCustomersDashboardId = "NewCustomersDashboardId_" + this.CurrentSession.GetNewId("NewCustomerDashboard");
        this.NewOpportunitiesDashboardId = "NewOpportunitiesDashboardId_" + this.CurrentSession.GetNewId("NewOpportunitiesDashboard");
        this.NewQuotesDashboardId = "NewQuotesDashboardId_" + this.CurrentSession.GetNewId("NewQuotesDashboard");
        this.NewActivitiesDashboardId = "NewActivitiesDashboardId_" + this.CurrentSession.GetNewId("NewActivitiesDashboard");
        this.NewOpportunityBySalesmanLegendId = "NewOpportunityBySalesmanLegendId_" + this.CurrentSession.GetNewId("NewOpportunityBySalesmanLegendId");
        this.NewActivitiesTDId = "NewActivitiesTDId_" + this.CurrentSession.GetNewId("NewActivitiesTDId");
        this.NewQuotesBySalesmanLegendId = "NewQuotesBySalesmanLegendId_" + this.CurrentSession.GetNewId("NewQuotesBySalesmanLegendId");
    };
    ByCreateDateComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
    };
    ByCreateDateComponent.prototype.RefreshTab = function () {
    };
    Object.defineProperty(ByCreateDateComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (value != this.toDate) {
                this.toDate = value;
                this.SelectedDateFilter = this.DateFilterList[6];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByCreateDateComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (value != this.fromDate) {
                this.fromDate = value;
                this.SelectedDateFilter = this.DateFilterList[6];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    ByCreateDateComponent.prototype.BuildBusinessUnitFilter = function () {
        var _this = this;
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
                var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(_this.filterControlNameSpace, _this.filterName_BusinessUnit);
                if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
                    defaultFilterCode = "M";
                }
                _this.selectedBusinessUnitFilter = _this.BusinessUnitFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
                _this.GetSelectedBusinessUnitId();
                _this.BuildUsersFilters(false);
            }
        });
    };
    ByCreateDateComponent.prototype.BuildUsersFilters = function (isUpdatingFilter) {
        var _this = this;
        this.UsersFilterList = [];
        if (this.SelectedBusinessUnitFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();
            if (isUpdatingFilter) {
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            }
            this.LoadFilteredQueries();
        }
        else {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M":
                    {
                        var item = new CodeNameClass_1.CodeNameClass(SessionLocator_1.SessionLocator.LoggedUserId, SessionLocator_1.SessionLocator.LoggedUserPM.EnglishName);
                        this.UsersFilterList.push(item);
                        this.selectedUserFilter = item;
                        this.GetSelectedOwnerId();
                        if (isUpdatingFilter) {
                            LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }
                        this.LoadFilteredQueries();
                        break;
                    }
                case "A":
                    {
                        // On Screen will be LOV
                        var item = new CodeNameClass_1.CodeNameClass("A", "All Owners");
                        this.UsersFilterList.push(item);
                        if (isUpdatingFilter) {
                            this.OwnerId = null;
                            this.listOfValuesUserId = null;
                            this.selectedUserFilter = null;
                            LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                        }
                        else {
                            this.selectedUserFilter = item;
                            this.GetSelectedOwnerId();
                        }
                        this.LoadFilteredQueries();
                        break;
                    }
                default:
                    {
                        var item = new CodeNameClass_1.CodeNameClass("A", "All " + this.SelectedBusinessUnitFilter.Name + " Owners");
                        this.UsersFilterList.push(item);
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
                                        _this.UsersFilterList.push(new CodeNameClass_1.CodeNameClass(list.Id, list.EnglishName));
                                    });
                                }
                            }
                            if (isUpdatingFilter) {
                                _this.OwnerId = null;
                                _this.listOfValuesUserId = null;
                                _this.selectedUserFilter = item;
                                LastFilterClass_1.LastFilterClass.UpdateFilter(_this.filterControlNameSpace, _this.filterName_Owner, _this.OwnerId);
                            }
                            else {
                                var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(_this.filterControlNameSpace, _this.filterName_Owner);
                                if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
                                    defaultFilterCode = null;
                                }
                                _this.OwnerId = defaultFilterCode;
                                _this.listOfValuesUserId = _this.OwnerId;
                                if (!Tools_1.AppTool.IsNullOrEmpty(_this.OwnerId)) {
                                    _this.selectedUserFilter = _this.UsersFilterList.filter(function (d) { return d.Code == _this.OwnerId; })[0];
                                }
                                if (_this.selectedUserFilter == null) {
                                    _this.selectedUserFilter = _this.UsersFilterList[0];
                                }
                            }
                            _this.LoadFilteredQueries();
                        });
                        break;
                    }
            }
        }
    };
    ByCreateDateComponent.prototype.GetSelectedBusinessUnitId = function () {
        var myResult = null;
        if (this.SelectedBusinessUnitFilter) {
            switch (this.SelectedBusinessUnitFilter.Code) {
                case "M": {
                    myResult = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
                    break;
                }
                case "A": {
                    myResult = null;
                    break;
                }
                default: {
                    myResult = this.SelectedBusinessUnitFilter.Code;
                    break;
                }
            }
        }
        this.BusinessUnitId = myResult;
    };
    ByCreateDateComponent.prototype.GetSelectedOwnerId = function () {
        var myResult = null;
        this.listOfValuesUserId = null;
        if (this.SelectedUserFilter) {
            switch (this.SelectedUserFilter.Code) {
                case "A": {
                    var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_Owner);
                    if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
                        defaultFilterCode = null;
                    }
                    myResult = defaultFilterCode;
                    this.listOfValuesUserId = myResult;
                    break;
                }
                default: {
                    myResult = this.SelectedUserFilter.Code;
                    this.listOfValuesUserId = myResult;
                    break;
                }
            }
        }
        this.OwnerId = myResult;
    };
    Object.defineProperty(ByCreateDateComponent.prototype, "SelectedBusinessUnitFilter", {
        get: function () { return this.selectedBusinessUnitFilter; },
        set: function (value) {
            if (this.selectedBusinessUnitFilter != value) {
                this.selectedBusinessUnitFilter = value;
                this.GetSelectedBusinessUnitId();
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_BusinessUnit, (value == null ? null : value.Code));
                this.BuildUsersFilters(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByCreateDateComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.selectedDateFilter; },
        set: function (value) {
            if (this.selectedDateFilter != value) {
                this.selectedDateFilter = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CreateDate, (value == null ? null : value.Code));
                if (value.Code == "-2") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "ByCreateToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate)));
                }
            }
            this.LoadFilteredQueries();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByCreateDateComponent.prototype, "SelectedUserFilter", {
        get: function () { return this.selectedUserFilter; },
        set: function (value) {
            if (this.selectedUserFilter != value) {
                this.selectedUserFilter = value;
                this.GetSelectedOwnerId();
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.LoadFilteredQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByCreateDateComponent.prototype, "ListOfValuesUserId", {
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
    ByCreateDateComponent.prototype.LoadFilteredQueries = function () {
        if (this.SelectedDateFilter != null) {
            var days = parseInt(this.SelectedDateFilter.Code);
            this.ComputeDays();
            this.LoadQuotesData(days);
            this.LoadCustomersData(days);
            this.LoadActivitiesData(days);
            this.LoadOpportunitiesData(days);
        }
    };
    ByCreateDateComponent.prototype.NewActivityClicking = function () {
        if (BarClick() != null) {
            this.OnNewActivityClick(BarClick());
            ResetItem();
        }
    };
    ByCreateDateComponent.prototype.NewOpportunityClicking = function () {
        if (PieClick() != null) {
            this.OnNewOpportunityClick(PieClick());
            ResetItemPie();
        }
    };
    ByCreateDateComponent.prototype.NewQuotesClicking = function () {
        if (PieClick() != null) {
            this.OnNewQuoteClick(PieClick());
            ResetItemPie();
        }
    };
    ByCreateDateComponent.prototype.NewCustomerClicking = function () {
        if (BarClick() != null) {
            this.OnNewCustomerClick(BarClick());
            ResetItem();
        }
    };
    ByCreateDateComponent.prototype.OnNewOpportunityClick = function (e) {
        var _this = this;
        var item = this.NewOpportunityList[e.index];
        var myQueryCode = "All Opportunities";
        var myTableName = "Opportunity";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Opportunities";
        listArgs.BackButtonTitle = "CRM";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ByCreateDateComponent.prototype.OnNewQuoteClick = function (e) {
        var _this = this;
        var item = this.NewQuoteList[e.index];
        var myQueryCode = "All Quotes";
        var myTableName = "Quote";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("SalesmanUserId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Quotes";
        listArgs.BackButtonTitle = "CRM";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ByCreateDateComponent.prototype.OnNewCustomerClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "ShippersAndConsignees";
        var myTableName = "Customer";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var myCode = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NewCustomersYAxisFitlerd[e.target.columnIndex].label))
            myCode = this.NewCustomersYAxisFitlerd[e.target.columnIndex].label;
        filterAgrs.addAdditionalFilter("SalesmanUserId", this.NewCustomersYAxisFitlerd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("SalesmanBusinessUnitId", this.NewCustomersYAxisFitlerd[e.target.columnIndex].BusinessUnitId[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("CRMChartFilter", true, null, null, "Equals", true, false, false, "boolean");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Customers";
        listArgs.BackButtonTitle = "CRM";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ByCreateDateComponent.prototype.OnNewActivityClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "All Activities";
        var myTableName = "Activity";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
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
                    typeName = "Appointment";
                    typeCode = "AP";
                    break;
                }
            }
        }
        var myCode = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.NewActivitiesYAxisFitlerd[e.target.columnIndex].label))
            myCode = this.NewActivitiesYAxisFitlerd[e.target.columnIndex].label;
        filterAgrs.addAdditionalFilter("OwnerId", this.NewActivitiesYAxisFitlerd[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCreateDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("ActivityTypeCode", typeCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", this.NewActivitiesYAxisFitlerd[e.target.columnIndex].BusinessUnitId[item.index], null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = typeName;
        listArgs.BackButtonTitle = "CRM";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ByCreateDateComponent.prototype.FillQuotesData = function (result) {
        try {
            if (this.CurrentQuotesBySalesmanChart != null) {
                this.CurrentQuotesBySalesmanChart.clear();
                this.CurrentQuotesBySalesmanChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.NewQuotesDashboardIdExistance = false;
        }
        else {
            this.FillQuotesList(result.Result);
            this.NewQuotesDashboardIdExistance = true;
        }
    };
    ByCreateDateComponent.prototype.LoadQuotesData = function (days) {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-2") {
            this.crmDomainService.GetQuotesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillQuotesData(result);
            });
        }
        else {
            this.crmDomainService.GetQuotesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillQuotesData(result);
            });
        }
    };
    ByCreateDateComponent.prototype.FillQuotesList = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.NewQuoteList = List;
        List.forEach(function (element) {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty });
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);
        });
        var flagEmpty = true;
        pieChartData.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            this.CurrentQuotesBySalesmanChart = makePieChart(this.NewQuotesDashboardId, fullData, false, true, this.NewQuotesBySalesmanLegendId);
        }
    };
    ByCreateDateComponent.prototype.FillCustomerData = function (result) {
        if (result.Result.length == 0) {
            this.NewCustomerDashboardListExistance = false;
            try {
                var elm = document.getElementById(this.NewCustomersDashboardId);
            }
            catch (er) { }
            elm.innerHTML = "";
        }
        else {
            this.NewCustomerDashboardListExistance = true;
            this.FillCustomerList(result.Result);
        }
    };
    ByCreateDateComponent.prototype.LoadCustomersData = function (days) {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-2") {
            this.crmDomainService.GetCustomersGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillCustomerData(result);
            });
        }
        else {
            this.crmDomainService.GetCustomersGroupBySalesman(days, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillCustomerData(result);
            });
        }
    };
    ByCreateDateComponent.prototype.FillCustomerList = function (List) {
        var _this = this;
        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        List.sort(function (a, b) { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1; });
        var StringArr = new Array();
        var j = 0;
        List.forEach(function (element) {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });
        var Graphs = [];
        var index = 0;
        List.forEach(function (element) {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(1);
                    NewCustomerYAxis[i].data[0] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[0] = element.StringProperty;
                    NewCustomerYAxis[i].BusinessUnitId[0] = element.BusinessUnitId;
                    NewCustomerYAxis[i].DateTime[0] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[0] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.StringProperty);
                    }
                }
            }
        });
        this.NewCustomersYAxisFitlerd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.NewCustomersYAxisFitlerd[i] == null) {
                    _this.NewCustomersYAxisFitlerd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.NewCustomersYAxisFitlerd[i].data.push(element.data[i]);
                _this.NewCustomersYAxisFitlerd[i].BindingElement.push(element.BindingElement[i]);
                _this.NewCustomersYAxisFitlerd[i].OwnerIds.push(element.OwnerIds[i]);
                _this.NewCustomersYAxisFitlerd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                _this.NewCustomersYAxisFitlerd[i].DateTime.push(element.DateTime[i]);
                _this.NewCustomersYAxisFitlerd[i].label = element.label;
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
                        "fillColors": ["#DA7B38", "#ecbd9b"],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    };
                }
                objectArray[i] = (element.data[i]);
            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (NewCustomerXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.NewCustomersDashboardId, Graphs, DataProvider, maximum, null, null, 0);
            }
        }
        catch (e) {
        }
    };
    ByCreateDateComponent.prototype.FillActivitiesData = function (result) {
        if (result.Result.length == 0) {
            this.NewActivitiesDashboardIdExistance = false;
            try {
                var elm = document.getElementById(this.NewActivitiesDashboardId);
            }
            catch (er) { }
            elm.innerHTML = "";
        }
        else {
            this.NewActivitiesDashboardIdExistance = true;
            this.FillActivitiesList(result.Result);
        }
    };
    ByCreateDateComponent.prototype.LoadActivitiesData = function (days) {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-2") {
            this.crmDomainService.GetActivitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillActivitiesData(result);
            });
        }
        else {
            this.crmDomainService.GetActivitiesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillActivitiesData(result);
            });
        }
    };
    ByCreateDateComponent.prototype.FillActivitiesList = function (List) {
        var _this = this;
        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        //   List.sort((a, b) => { return (a.DateTimeProperty === b.DateTimeProperty) ? 0 : (a.DateTimeProperty < b.DateTimeProperty) ? -1 : 1 });
        var StringArr = new Array();
        var j = 0;
        List = List.filter(function (element) { return element.DataTypeCode == "TS" || element.DataTypeCode == "CL" || element.DataTypeCode == "AP"; });
        List.forEach(function (element) {
            if (!StringArr.includes(element.StringProperty)) {
                StringArr.push(element.StringProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });
        var Graphs = [];
        var index = 0;
        List.forEach(function (element) {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.StringProperty == StringArr[i]) {
                    var k = 0;
                    if (element.DataTypeCode == "CL")
                        k = 1;
                    else if (element.DataTypeCode == "AP")
                        k = 2;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(3);
                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].BusinessUnitId[k] = element.BusinessUnitId;
                    NewCustomerYAxis[i].DateTime[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.StringProperty) && element.StringProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.StringProperty);
                    }
                }
            }
        });
        var barChartColors = [
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
        this.NewActivitiesYAxisFitlerd = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.NewActivitiesYAxisFitlerd[i] == null) {
                    _this.NewActivitiesYAxisFitlerd[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], BusinessUnitId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.NewActivitiesYAxisFitlerd[i].data.push(element.data[i]);
                _this.NewActivitiesYAxisFitlerd[i].BindingElement.push(element.BindingElement[i]);
                _this.NewActivitiesYAxisFitlerd[i].OwnerIds.push(element.OwnerIds[i]);
                _this.NewActivitiesYAxisFitlerd[i].BusinessUnitId.push(element.BusinessUnitId[i]);
                _this.NewActivitiesYAxisFitlerd[i].DateTime.push(element.DateTime[i]);
                _this.NewActivitiesYAxisFitlerd[i].label = element.label;
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
                        "fillColors": [barChartColors[i].backgroundColor1 + "", barChartColors[i].backgroundColor2 + ""],
                        //  "fillColors": ["#ff0000", "#00ff00"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                        "showHandOnHover": true,
                    };
                }
                objectArray[i] = (element.data[i]);
            }
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1], "col3": objectArray[2] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (NewCustomerXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.NewActivitiesDashboardId, Graphs, DataProvider, maximum, null, null, 0);
            }
        }
        catch (e) {
        }
    };
    ByCreateDateComponent.prototype.FillOpportunitiesData = function (result) {
        try {
            if (this.CurrentOpportunityBySalesmanChart != null) {
                this.CurrentOpportunityBySalesmanChart.clear();
                this.CurrentOpportunityBySalesmanChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.NewOpportunitiesDashboardIdExistance = false;
        }
        else {
            this.FillOpportunitiesList(result.Result);
            this.NewOpportunitiesDashboardIdExistance = true;
        }
    };
    ByCreateDateComponent.prototype.LoadOpportunitiesData = function (days) {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-2") {
            this.crmDomainService.GetOpportunitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillOpportunitiesData(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesGroupBySalesman(days + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, false).subscribe(function (result) {
                _this.FillOpportunitiesData(result);
            });
        }
    };
    ByCreateDateComponent.prototype.FillOpportunitiesList = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.NewOpportunityList = List;
        List.forEach(function (element) {
            fullData.push({ label: element.StringProperty, data: element.IntegerProperty });
            pieChartLabels.push(element.StringProperty);
            pieChartData.push(element.IntegerProperty);
        });
        var flagEmpty = true;
        pieChartData.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            this.CurrentOpportunityBySalesmanChart = makePieChart(this.NewOpportunitiesDashboardId, fullData, false, true, this.NewOpportunityBySalesmanLegendId);
        }
    };
    ByCreateDateComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ByCreateDateComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [])
    ], ByCreateDateComponent);
    return ByCreateDateComponent;
}(BaseComponent_1.BaseComponent));
exports.ByCreateDateComponent = ByCreateDateComponent;
//# sourceMappingURL=ByCreateDateComponent.js.map