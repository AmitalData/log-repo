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
var CompanyPerformanceComponent = /** @class */ (function (_super) {
    __extends(CompanyPerformanceComponent, _super);
    function CompanyPerformanceComponent() {
        var _this = _super.call(this) || this;
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.DashboardTabsControls.CompanyPerformanceControl";
        _this.filterName_CloseDate = "CloseDate";
        _this.fieldCode = "S";
        _this.DateFilterList = [];
        _this.DataContext = _this;
        _this.OpportunitiesWonLostRatioIdExistance = false;
        _this.OpportunitiesbyLeadSourcetypeIdExistance = false;
        _this.OpportunitiesByTypeIdExistance = false;
        _this.CompletedActivitiesIdExistance = false;
        _this.AcceptedDeclinedQuotesExistance = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UsersFilterList = [];
        _this.BusinessUnitFilterList = [];
        _this.ZoomedChartVisibility = false;
        _this.ActivityList = [];
        _this.QuoteList = [];
        _this.NewActivitiesYAxisFitlerd = [];
        _this.NewCustomersYAxisFitlerd = [];
        _this.OpportunityListWonLost = [];
        _this.OpportunitiesListByLeadSource = [];
        _this.OpportunityListByType = [];
        _this.InitializeIds();
        _this.InitializeServices();
        _this.BuildBusinessUnitFilter();
        _this.BuildDateFilters();
        return _this;
    }
    CompanyPerformanceComponent.prototype.InitializeServices = function () {
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
        this.crmDomainService = new CRMDomainService_1.CRMDomainService();
    };
    Object.defineProperty(CompanyPerformanceComponent.prototype, "ToDate", {
        get: function () { return this.toDate; },
        set: function (value) {
            if (value != this.toDate) {
                this.toDate = value;
                this.SelectedDateFilter = this.DateFilterList[10];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyPerformanceComponent.prototype, "FromDate", {
        get: function () { return this.fromDate; },
        set: function (value) {
            if (value != this.fromDate) {
                this.fromDate = value;
                this.SelectedDateFilter = this.DateFilterList[10];
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(value)));
            }
        },
        enumerable: true,
        configurable: true
    });
    CompanyPerformanceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadFilteredQueries();
    };
    CompanyPerformanceComponent.prototype.BuildDateFilters = function () {
        this.DateFilterList = CRMUtilities_1.CRMUtilities.GetClosingDateFilterList();
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_CloseDate);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-30";
        }
        this.selectedDateFilter = this.DateFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
        if (this.selectedDateFilter.Code == "-1_-1") {
            var ActiviytFromDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "CompanyPerformanceFromDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytFromDate)) {
                var ActivityDate = new Date();
                var ActivityFromDateString = ActiviytFromDate.split(':');
                ActivityDate.setFullYear(ActivityFromDateString[0], ActivityFromDateString[1] - 1, ActivityFromDateString[2]);
                this.fromDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
            var ActiviytToDate = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, "CompanyPerformanceToDate");
            if (!Tools_1.AppTool.IsNullOrEmpty(ActiviytToDate)) {
                var ActivityDate = new Date();
                var ActivityToDateString = ActiviytToDate.split(':');
                ActivityDate.setFullYear(ActivityToDateString[0], ActivityToDateString[1] - 1, ActivityToDateString[2]);
                this.toDate = Tools_1.DateTool.GetDateParts(ActivityDate).DateObject;
            }
        }
    };
    CompanyPerformanceComponent.prototype.InitializeIds = function () {
        this.OpportunitiesWonLostRatioId = "OpportunitiesWonLostRatioId_" + this.CurrentSession.GetNewId("OpportunitiesWonLostRatioId");
        this.OpportunitiesbyLeadSourcetypeId = "OpportunitiesbyLeadSourcetypeId_" + this.CurrentSession.GetNewId("OpportunitiesbyLeadSourcetypeId");
        this.OpportunitiesByTypeId = "OpportunitiesByTypeId_" + this.CurrentSession.GetNewId("OpportunitiesByTypeId");
        this.CompletedActivitiesId = "CompletedActivitiesId_" + this.CurrentSession.GetNewId("CompletedActivitiesId");
        this.AcceptedDeclinedQuotes = "AcceptedDeclinedQuotes_" + this.CurrentSession.GetNewId("AcceptedDeclinedQuotes");
        this.ZoomedChartId = "ZoomedChartId_" + this.CurrentSession.GetNewId("ZoomedChartId");
        this.legenddivId = "legenddiv_" + this.CurrentSession.GetNewId("legenddiv");
        this.OpportunityByWonLostLegendId = "OpportunityByWonLostLegendId_" + this.CurrentSession.GetNewId("OpportunityByWonLostLegendId");
        this.OpportunityByTypeLegendId = "OpportunityByTypeLegendId_" + this.CurrentSession.GetNewId("OpportunityByTypeLegendId");
        this.OpportunityByLeadSourceLegendId = "OpportunityByLeadSourceLegendId_" + this.CurrentSession.GetNewId("OpportunityByLeadSourceLegendId");
        this.ActivityChartLegendId = "ActivityChartLegendId_" + this.CurrentSession.GetNewId("ActivityChartLegendId");
        this.QuoteLegendId = "QuoteLegendId_" + this.CurrentSession.GetNewId("QuoteLegendId");
    };
    CompanyPerformanceComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
    };
    CompanyPerformanceComponent.prototype.RefreshTab = function () {
    };
    CompanyPerformanceComponent.prototype.BuildBusinessUnitFilter = function () {
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
    CompanyPerformanceComponent.prototype.BuildUsersFilters = function (isUpdatingFilter) {
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
    CompanyPerformanceComponent.prototype.GetSelectedBusinessUnitId = function () {
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
    CompanyPerformanceComponent.prototype.GetSelectedOwnerId = function () {
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
    CompanyPerformanceComponent.prototype.ViewZoomed = function (chartCode) {
        switch (chartCode) {
            case "QT":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedQuotesData();
                    break;
                }
            case "AC":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedActivitiesData();
                    break;
                }
            case "OP":
                {
                    this.ZoomedChartVisibility = true;
                    this.LoadZoomedOpportunitiesData();
                    break;
                }
        }
    };
    CompanyPerformanceComponent.prototype.LoadZoomedQuotesData = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetQuotesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedQueries(result.Result, "Q");
            });
        }
        else {
            this.crmDomainService.GetQuotesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedQueries(result.Result, "Q");
            });
        }
    };
    CompanyPerformanceComponent.prototype.LoadZoomedOpportunitiesData = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedQueries(result.Result, "O");
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedQueries(result.Result, "O");
            });
        }
    };
    CompanyPerformanceComponent.prototype.LoadZoomedActivitiesData = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetActivitiesGroupBySalesmanCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedActivitiesQueries(result.Result);
            });
        }
        else {
            this.crmDomainService.GetActivitiesGroupBySalesman(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, this.fieldCode, true).subscribe(function (result) {
                _this.FillZoomedActivitiesQueries(result.Result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillZoomedActivitiesQueries = function (List) {
        var barChartData = [{ data: [], label: '' }, { data: [], label: '' }, { data: [], label: '' }];
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        barChartData[0].data = [];
        barChartData[1].data = [];
        barChartData[2].data = [];
        var barChartLabels = [];
        var max = 0;
        var flag = "";
        var Col1Title = "Task";
        var Col2Title = "Phone Call";
        var Col3Title = "Appointment";
        var labelIndex = 0;
        var val1Index = 0;
        var val2Index = 0;
        var val3Index = 0;
        var list = List.sort(function (a, b) { return (a.StringProperty === b.StringProperty) ? 0 : (a.StringProperty < b.StringProperty) ? -1 : 1; });
        ;
        list.forEach(function (element) {
            var Accepted;
            var Declined;
            if (i == 0) {
                Graphs = [{
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-1" + i,
                        "title": Col1Title,
                        "type": "column",
                        "valueField": "col1",
                        "fillColors": ["#DA7B38", "#ecbd9b"],
                        "lineAlpha": 0,
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": Col2Title,
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col2",
                        "fillColors": ["#21782E", "#90bb96"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-3" + i,
                        "title": Col3Title,
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col3",
                        "fillColors": ["#487E9F", "#c8d8e2"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    }
                ];
            }
            var val1 = 0;
            var val2 = 0;
            var val3 = 0;
            switch (element.DataTypeCode) {
                case "TS":
                    {
                        val1 = element.IntegerProperty;
                        barChartData[0].label = "TS";
                        barChartData[0].data[val1Index] = val1;
                        val1Index++;
                        break;
                    }
                case "CL":
                    {
                        val2 = element.IntegerProperty;
                        barChartData[0].label = "CL";
                        barChartData[1].data[val2Index] = val2;
                        val2Index++;
                        break;
                    }
                case "AP":
                    {
                        val3 = element.IntegerProperty;
                        barChartData[0].label = "AP";
                        barChartData[2].data[val3Index] = val3;
                        val3Index++;
                        break;
                    }
            }
            if (val1 == null) {
                val1 = 0;
            }
            if (val2 == null) {
                val2 = 0;
            }
            if (val3 == null)
                val3 = 0;
            if (val1 != 0 || val2 != 0 || val3) {
                if (!barChartLabels.includes(element.StringProperty)) {
                    barChartLabels[labelIndex] = element.StringProperty;
                    labelIndex++;
                }
                i++;
                if (val1 > max)
                    max = val1;
                if (val2 > max)
                    max = val2;
                if (val3 > max)
                    max = val3;
            }
        });
        var i = 0;
        barChartLabels.forEach(function (item) {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i], "col3": barChartData[2].data[i] };
            i++;
        });
        if (max < 5)
            max = 5;
        makeAmBarChart(this.ZoomedChartId, Graphs, DataProvider, max, true, this.legenddivId);
    };
    CompanyPerformanceComponent.prototype.BackButtonClicked = function () {
        this.ZoomedChartVisibility = false;
        this.LoadFilteredQueries();
    };
    CompanyPerformanceComponent.prototype.FillZoomedQueries = function (List, Code) {
        var barChartData = [{ data: [], label: '' }, { data: [], label: '' }];
        var i = 0;
        var index = 0;
        var Graphs = [];
        var DataProvider = [];
        var objectArray = [];
        barChartData[0].data = [];
        barChartData[1].data = [];
        var barChartLabels = [];
        var max = 0;
        var flag;
        var Col1Title;
        var Col2Title;
        if (Code == "Q") {
            flag = "QTAC";
            Col1Title = "Accepted";
            Col2Title = "Declined";
        }
        else if (Code = "O") {
            flag = "CWN";
            Col1Title = "Won";
            Col2Title = "Lost";
        }
        var labelIndex = 0;
        var WonValueIndex = 0;
        var LostValueIndex = 0;
        var list = List.sort(function (a, b) { return (a.StringProperty === b.StringProperty) ? 0 : (a.StringProperty < b.StringProperty) ? -1 : 1; });
        ;
        list.forEach(function (element) {
            var Accepted;
            var Declined;
            if (i == 0) {
                Graphs = [{
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-1" + i,
                        "title": Col1Title,
                        "type": "column",
                        "valueField": "col1",
                        "fillColors": ["#0f7816", "#5ed967"],
                        "lineAlpha": 0,
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    },
                    {
                        "balloonText": Tools_1.FormatTool.FormatBigNumbersToExtension("[[value]]") + "",
                        "fillAlphas": 1,
                        "id": "AmGraph-2" + i,
                        "title": Col2Title,
                        "type": "column",
                        "lineAlpha": 0,
                        "valueField": "col2",
                        "fillColors": ["#c80d05", "#fb5851"],
                        "gradientOrientation": "horizontal",
                        "borderAlpha": 0,
                    }
                ];
            }
            var WonValue = 0;
            var LostValue = 0;
            switch (element.DataTypeCode) {
                case flag:
                    {
                        WonValue = element.IntegerProperty;
                        barChartData[0].label = "W";
                        barChartData[0].data[WonValueIndex] = WonValue;
                        WonValueIndex++;
                        break;
                    }
                default:
                    {
                        LostValue = element.IntegerProperty;
                        barChartData[1].label = "L";
                        barChartData[1].data[LostValueIndex] = LostValue;
                        LostValueIndex++;
                        break;
                    }
            }
            if (WonValue == null) {
                WonValue = 0;
            }
            if (LostValue == null) {
                LostValue = 0;
            }
            if (WonValue != 0 || LostValue != 0) {
                if (!barChartLabels.includes(element.StringProperty)) {
                    barChartLabels[labelIndex] = element.StringProperty;
                    labelIndex++;
                }
                i++;
                if (WonValue > max)
                    max = WonValue;
                if (LostValue > max)
                    max = LostValue;
            }
        });
        var i = 0;
        barChartLabels.forEach(function (item) {
            DataProvider[i] = { "category": barChartLabels[i], "col1": barChartData[0].data[i], "col2": barChartData[1].data[i] };
            i++;
        });
        if (max < 5)
            max = 5;
        makeAmBarChart(this.ZoomedChartId, Graphs, DataProvider, max, true, this.legenddivId);
    };
    Object.defineProperty(CompanyPerformanceComponent.prototype, "SelectedBusinessUnitFilter", {
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
    Object.defineProperty(CompanyPerformanceComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.selectedDateFilter; },
        set: function (value) {
            if (this.selectedDateFilter != value) {
                this.selectedDateFilter = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_CloseDate, (value == null ? null : value.Code));
                if (value.Code == "-1_-1") {
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceFromDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate)));
                    LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, "CompanyPerformanceToDate", (value == null ? null : ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate)));
                }
            }
            this.LoadFilteredQueries();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompanyPerformanceComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(CompanyPerformanceComponent.prototype, "ListOfValuesUserId", {
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
    CompanyPerformanceComponent.prototype.ComputeDays = function () {
        var days;
        var Todate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        var FromDate = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedDateFilter.Code == "0") {
            this.toDate = Todate;
            this.fromDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-1") {
            FromDate.setDate(Todate.getDate() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-7") {
            FromDate.setDate(Todate.getDate() - 6);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-30") {
            FromDate.setMonth(Todate.getMonth() - 1);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-90") {
            FromDate.setMonth(Todate.getMonth() - 3);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code == "-365") {
            FromDate.setMonth(Todate.getMonth() - 12);
            this.fromDate = FromDate;
            this.toDate = Todate;
        }
        else if (this.SelectedDateFilter.Code != "-1_-1") {
            FromDate = this.SelectedDateFilter.FromDate;
            this.fromDate = FromDate;
            Todate = this.SelectedDateFilter.ToDate;
            this.toDate = Todate;
        }
    };
    CompanyPerformanceComponent.prototype.LoadFilteredQueries = function () {
        if (this.SelectedDateFilter != null) {
            this.ComputeDays();
            this.LoadOpportunitiesByWonLost();
            this.LoadOpportunitiesByType();
            this.LoadOpportunitiesByLeadSource();
            this.LoadActivities();
            this.LoadQuotes();
        }
    };
    CompanyPerformanceComponent.prototype.ActivityClicking = function () {
        if (PieClick() != null) {
            this.OnActivityClick(PieClick());
            ResetItemPie();
        }
    };
    CompanyPerformanceComponent.prototype.QuoteClicking = function () {
        if (PieClick() != null) {
            this.OnQuoteClick(PieClick());
            ResetItemPie();
        }
    };
    CompanyPerformanceComponent.prototype.OnOpportunityClick = function (e, code) {
        var _this = this;
        var item = null;
        var myQueryCode = "All Opportunities";
        var myTableName = "Opportunity";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (code == "A") {
            item = this.OpportunityListWonLost[e.index];
            filterAgrs.addAdditionalFilter("StageId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }
        else if (code == "B") {
            item = this.OpportunityListByType[e.index];
            filterAgrs.addAdditionalFilter("OpportunityTypeId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }
        else {
            item = this.OpportunitiesListByLeadSource[e.index];
            filterAgrs.addAdditionalFilter("LeadSourceId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        }
        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartActualClosingDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("IsClosed", true, null, null, "Equals", false, false, false, "boolean");
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
    CompanyPerformanceComponent.prototype.OnQuoteClick = function (e) {
        var _this = this;
        var item = this.QuoteList[e.index];
        var myQueryCode = "All Quotes";
        var myTableName = "Quote";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("SalesmanUserId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("StageId", item.GroupedId, null, null, "Equals", false, false, false, "String");
        if (item.DataTypeCode == "QTAC") {
            filterAgrs.addAdditionalFilter("ChartAcceptedDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        }
        else if (item.DataTypeCode == "QTDC") {
            filterAgrs.addAdditionalFilter("ChartDeclinedDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
        }
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
    CompanyPerformanceComponent.prototype.OnActivityClick = function (e) {
        var _this = this;
        var item = this.ActivityList[e.index];
        var myQueryCode = "All Activities";
        var myTableName = "Activity";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var Key = item.DataTypeCode;
        var typeName = "";
        var typeCode = "";
        switch (Key + "") {
            case "TS": {
                typeName = "Tasks";
                typeCode = "TS";
                break;
            }
            case "CL": {
                typeName = "Phone Calls";
                typeCode = "CL";
                break;
            }
            case "AP": {
                typeName = "Appointments";
                typeCode = "AP";
                break;
            }
            case "EO": {
                typeName = "Emails Out";
                typeCode = "EO";
                break;
            }
            case "EI": {
                typeName = "Emails In";
                typeCode = "EI";
                break;
            }
        }
        filterAgrs.addAdditionalFilter("IsOpen", false, null, null, "Equals", false, false, false, "boolean");
        filterAgrs.addAdditionalFilter("ActivityStatusCode", "C", null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("BusinessUnitId", item.BusinessUnitId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ActivityTypeCode", item.DataTypeCode, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartCompleteDateFilter", ServiceHelper_1.ServiceHelper.GetDateString(this.FromDate), ServiceHelper_1.ServiceHelper.GetDateString(this.ToDate), null, "Equals", true, false, false, "String");
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
    CompanyPerformanceComponent.prototype.FillQuotes = function (result) {
        try {
            if (this.CurrentQuoteChart != null) {
                this.CurrentQuoteChart.clear();
                this.CurrentQuoteChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.AcceptedDeclinedQuotesExistance = false;
        }
        else {
            this.FillQuotesList(result.Result);
            this.AcceptedDeclinedQuotesExistance = true;
        }
    };
    CompanyPerformanceComponent.prototype.LoadQuotes = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetQuotesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "").subscribe(function (result) {
                _this.FillQuotes(result);
            });
        }
        else {
            this.crmDomainService.GetQuotesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "").subscribe(function (result) {
                _this.FillQuotes(result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillQuotesList = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.QuoteList = List;
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
            this.CurrentQuoteChart = makePieChart(this.AcceptedDeclinedQuotes, fullData, false, true, this.QuoteLegendId, 150);
        }
    };
    CompanyPerformanceComponent.prototype.FillActivities = function (result) {
        if (this.CurrentActivityChart != null) {
            this.CurrentActivityChart.clear();
            this.CurrentActivityChart = null;
        }
        if (result.Result.length == 0) {
            this.CompletedActivitiesIdExistance = false;
        }
        else {
            this.CompletedActivitiesIdExistance = true;
            this.FillActivitiesList(result.Result);
        }
    };
    CompanyPerformanceComponent.prototype.LoadActivities = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetActivitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "").subscribe(function (result) {
                _this.FillActivities(result);
            });
        }
        else {
            this.crmDomainService.GetActivitiesChartData(this.SelectedDateFilter.Code, this.OwnerId, this.BusinessUnitId, "").subscribe(function (result) {
                _this.FillActivities(result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillActivitiesList = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.ActivityList = List;
        List.forEach(function (element) {
            var label = element.StringProperty;
            label == "Call" ? label = "Phone Call" : label = label;
            fullData.push({ label: label, data: element.IntegerProperty });
            pieChartLabels.push(label);
            pieChartData.push(element.IntegerProperty);
        });
        var flagEmpty = true;
        pieChartData.forEach(function (p) {
            if (p != "0")
                flagEmpty = false;
        });
        if (!flagEmpty) {
            this.CurrentActivityChart = makePieChart(this.CompletedActivitiesId, fullData, false, true, this.ActivityChartLegendId, 150);
        }
    };
    CompanyPerformanceComponent.prototype.FillOpportunitiesByLeadSource = function (result) {
        if (this.CurrentOpportunityByLeadSourceChart != null) {
            this.CurrentOpportunityByLeadSourceChart.clear();
            this.CurrentOpportunityByLeadSourceChart = null;
        }
        if (result.Result.length == 0) {
            this.OpportunitiesbyLeadSourcetypeIdExistance = false;
            try {
            }
            catch (er) { }
        }
        else {
            this.OpportunitiesbyLeadSourcetypeIdExistance = true;
            this.FillOpportunitiesListByLeadSource(result.Result);
        }
    };
    CompanyPerformanceComponent.prototype.LoadOpportunitiesByLeadSource = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "LS").subscribe(function (result) {
                _this.FillOpportunitiesByLeadSource(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "LS").subscribe(function (result) {
                _this.FillOpportunitiesByLeadSource(result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillOppotuniriesByType = function (result) {
        try {
            if (this.CurrentOpportunityByTypeChart != null) {
                this.CurrentOpportunityByTypeChart.clear();
                this.CurrentOpportunityByTypeChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.OpportunitiesByTypeIdExistance = false;
        }
        else {
            this.FillOpportunitiesListBySalesman(result.Result);
            this.OpportunitiesByTypeIdExistance = true;
        }
    };
    CompanyPerformanceComponent.prototype.LoadOpportunitiesByType = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "T").subscribe(function (result) {
                _this.FillOppotuniriesByType(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "T").subscribe(function (result) {
                _this.FillOppotuniriesByType(result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillOpportunitiesByWonLost = function (result) {
        try {
            if (this.CurrentOpportunityByWonLostChart != null) {
                this.CurrentOpportunityByWonLostChart.clear();
                this.CurrentOpportunityByWonLostChart = null;
            }
        }
        catch (er) { }
        if (result.Result.length == 0) {
            this.OpportunitiesWonLostRatioIdExistance = false;
        }
        else {
            this.FillOpportunitiesListWonLost(result.Result);
            this.OpportunitiesWonLostRatioIdExistance = true;
        }
    };
    CompanyPerformanceComponent.prototype.LoadOpportunitiesByWonLost = function () {
        var _this = this;
        if (this.SelectedDateFilter.Code == "-1_-1") {
            this.crmDomainService.GetOpportunitiesChartDataCustom(this.FromDate, this.ToDate, this.OwnerId, this.BusinessUnitId, "WL").subscribe(function (result) {
                _this.FillOpportunitiesByWonLost(result);
            });
        }
        else {
            this.crmDomainService.GetOpportunitiesChartData(this.SelectedDateFilter.Code + "", this.OwnerId, this.BusinessUnitId, "WL").subscribe(function (result) {
                _this.FillOpportunitiesByWonLost(result);
            });
        }
    };
    CompanyPerformanceComponent.prototype.FillOpportunitiesListWonLost = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunityListWonLost = List;
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
            this.CurrentOpportunityByWonLostChart = makePieChart(this.OpportunitiesWonLostRatioId, fullData, false, true, this.OpportunityByWonLostLegendId, 150);
        }
    };
    CompanyPerformanceComponent.prototype.OpportunityClicking = function (code) {
        if (PieClick() != null) {
            this.OnOpportunityClick(PieClick(), code);
            ResetItemPie();
        }
    };
    CompanyPerformanceComponent.prototype.FillOpportunitiesListBySalesman = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunityListByType = List;
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
            this.CurrentOpportunityByTypeChart = makePieChart(this.OpportunitiesByTypeId, fullData, false, true, this.OpportunityByTypeLegendId, 150);
        }
    };
    CompanyPerformanceComponent.prototype.FillOpportunitiesListByLeadSource = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.OpportunitiesListByLeadSource = List;
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
            this.CurrentOpportunityByLeadSourceChart = makePieChart(this.OpportunitiesbyLeadSourcetypeId, fullData, false, true, this.OpportunityByLeadSourceLegendId, 150);
        }
    };
    CompanyPerformanceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CompanyPerformanceComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [])
    ], CompanyPerformanceComponent);
    return CompanyPerformanceComponent;
}(BaseComponent_1.BaseComponent));
exports.CompanyPerformanceComponent = CompanyPerformanceComponent;
//# sourceMappingURL=CompanyPerformanceComponent.js.map