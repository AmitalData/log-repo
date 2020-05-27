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
/// <reference path="../../../infrastructure/Utilities/featurelocator.ts" />
var core_1 = require("@angular/core");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var OpportunityListService_1 = require("../../Services/StandardLists/OpportunityListService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DateTimeToDatePipe_1 = require("../../../Controls/Pipes/DateTimeToDatePipe");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_1 = require("../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var OpportunityWorkspaceComponent = /** @class */ (function (_super) {
    __extends(OpportunityWorkspaceComponent, _super);
    function OpportunityWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.QuickSearchItems = [];
        _this.ReloadUserQueries = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_RecordsType = "RecordsType";
        _this.filterName_CreatedByType = "CreatedByType";
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.OpportunitiesPageControl";
        _this.filterName_SalesFunnel = "SalesFunnel";
        _this.filterName_TopOpportunities = "TopOpportunities";
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
        //Funnel Filter
        _this.FunneFilterList = [];
        //Top Opportunities Filter
        _this.TopOpportunitiesFilterList = [];
        //Screen Visibilities
        _this.NoRecentVisibility = false;
        _this.NoTopVisibility = false;
        // Queries Features
        _this.OpenQueriesVisibility = false;
        _this.MyOpenQueryVisibility = false;
        _this.AllOpenQueryVisibility = false;
        _this.StageOpenQueryVisibility = false;
        _this.ClosedQueriesVisibility = false;
        _this.MyClosedQueryVisibility = false;
        _this.AllClosedQueryVisibility = false;
        _this.OtherQueriesVisibility = false;
        _this.AllQueryVisibility = false;
        _this.CancelledQueryVisibility = false;
        _this.MyViewsQueryVisibility = false;
        // Load Recent Data
        _this.RecentOpportunitiesCount = 0;
        _this.RecentOpportuntiesList = [];
        _this.FunnelDataFilterd = [];
        _this.SalesFunnelId = "SalesFunnel_" + _this.CurrentSession.GetNewId("SalesFunnel");
        _this.InitializeServices();
        _this.SetQueriesVisibility();
        _this.LoadNonFilteredQueries();
        _this.BuildFilters();
        _this.InitializeFilters();
        return _this;
    }
    OpportunityWorkspaceComponent.prototype.InitializeServices = function () {
        this.myDomainService = new CRMDomainService_1.CRMDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
    };
    OpportunityWorkspaceComponent.prototype.InitializeFilters = function () {
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
    OpportunityWorkspaceComponent.prototype.OnFiltersInitialized = function () {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedRecordsTypeFilter", {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedCreatedByTypeFilter", {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedBusinessUnitFilter", {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "ListOfValuesUserId", {
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
    OpportunityWorkspaceComponent.prototype.BuildFilters = function () {
        this.BuildFunnelFilters();
        this.BuildTopOpportunitiesFilters();
    };
    OpportunityWorkspaceComponent.prototype.BuildFunnelFilters = function () {
        this.FunneFilterList = [];
        this.FunneFilterList.push(new CodeNameClass_1.CodeNameClass("CNT", "Count"));
        this.FunneFilterList.push(new CodeNameClass_1.CodeNameClass("SHI", TextCodeTranslator_1.TextCodeTranslator.Translate("Opportunity.F.NumberOfShipments")));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_SalesFunnel);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "CNT";
        }
        this.selectedFunnelFilter = this.FunneFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedFunnelFilter", {
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
    OpportunityWorkspaceComponent.prototype.BuildTopOpportunitiesFilters = function () {
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
    Object.defineProperty(OpportunityWorkspaceComponent.prototype, "SelectedTopOpportunitiesFilter", {
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
    OpportunityWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    OpportunityWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    OpportunityWorkspaceComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    OpportunityWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    };
    OpportunityWorkspaceComponent.prototype.LoadFilteredQueries = function () {
        this.LoadFunnelData();
        this.LoadTopOpportunities();
        this.LoadQueriesCounts();
    };
    OpportunityWorkspaceComponent.prototype.LoadNonFilteredQueries = function () {
        this.ReloadUsersQuery();
        //this.LoadQueriesCounts();
        this.LoadRecentOpportunities();
    };
    OpportunityWorkspaceComponent.prototype.SetQueriesVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyOpenOpportunities")) {
            this.OpenQueriesVisibility = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpenOpportunities")) {
            this.OpenQueriesVisibility = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.OpenByStage")) {
            this.OpenQueriesVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyOpenOpportunities")) {
            this.MyOpenQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpenOpportunities")) {
            this.AllOpenQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.OpenByStage")) {
            this.StageOpenQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyClosedOpportunities")) {
            this.ClosedQueriesVisibility = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllClosedOpportunities")) {
            this.ClosedQueriesVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.MyClosedOpportunities")) {
            this.MyClosedQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllClosedOpportunities")) {
            this.AllClosedQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpportunities")) {
            this.OtherQueriesVisibility = true;
        }
        else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.CancelledOpportunities")) {
            this.OtherQueriesVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.AllOpportunities")) {
            this.AllQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "Opportunity.Q.CancelledOpportunities")) {
            this.CancelledQueryVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES")) {
            this.MyViewsQueryVisibility = true;
        }
    };
    OpportunityWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myDomainService.GetOpportunitiesSummary(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.MyOpenCount = myData.MyOpenDataCount;
                    _this.AllOpenCount = myData.AllOpenDataCount;
                    _this.OpenByStageCount = myData.OpenByStageCount;
                }
            }
        });
    };
    OpportunityWorkspaceComponent.prototype.LoadRecentOpportunities = function () {
        var _this = this;
        this.myDomainService.GetRecentOpportunities(null, null).subscribe(function (myResult) {
            if (myResult == null) {
                _this.RecentOpportuntiesList = [];
                _this.RecentOpportunitiesCount = 0;
                _this.NoRecentVisibility = true;
            }
            else {
                _this.RecentOpportuntiesList = myResult;
                _this.RecentOpportunitiesCount = _this.RecentOpportuntiesList.length;
                if (_this.RecentOpportunitiesCount == 0)
                    _this.NoRecentVisibility = true;
                else
                    _this.NoRecentVisibility = false;
            }
        });
    };
    OpportunityWorkspaceComponent.prototype.LoadTopOpportunities = function () {
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
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;
            if (myBusinessUnitId == "all" || myBusinessUnitId == "null") {
                myBusinessUnitId = null;
            }
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
        if (this.TopOpportunitiesList == null) {
            this.TopOpportunitiesList = new Array();
        }
        else {
            this.TopOpportunitiesList = [];
        }
        var myService = new OpportunityListService_1.OpportunityListService();
        myService.getByFilters(filters).subscribe(function (myResult) {
            if (myResult != null) {
                var myResponse = myResult;
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.FillTopOpportunitiesList(list);
                }
            }
        });
    };
    OpportunityWorkspaceComponent.prototype.FillTopOpportunitiesList = function (myList) {
        var _this = this;
        this.TopOpportunitiesList = [];
        switch (this.SelectedTopOpportunitiesFilter.Code) {
            case "STG":
                {
                    var tempList = myList.sort(function (a, b) { return b.RatingIndexOrder - a.RatingIndexOrder; });
                    tempList.sort(function (a, b) { return b.StageProbability - a.StageProbability; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "SHI":
                {
                    myList.sort(function (a, b) { return b.NumberOfShipments - a.NumberOfShipments; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "RAT":
                {
                    myList.sort(function (a, b) { return (a.StageName === b.StageName) ? 0 : (a.StageName < b.StageName) ? -1 : 1; });
                    myList.sort(function (a, b) { return b.RatingIndexOrder - a.RatingIndexOrder; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    //this.TopOpportunitiesList.reverse();
                    break;
                }
            case "EST":
                {
                    myList.sort(function (a, b) { return b.EstimatedClosingDate != null ? (b.EstimatedClosingDate.valueOf() - a.EstimatedClosingDate.valueOf()) : -1; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
            case "DUE":
                {
                    myList.sort(function (a, b) { return b.StageDueDate != null ? (b.StageDueDate.valueOf() - a.StageDueDate.valueOf()) : -1; }).forEach(function (item) {
                        var itemViewModel = new TopOpportunityItem(item, _this.SelectedTopOpportunitiesFilter);
                        _this.TopOpportunitiesList.push(itemViewModel);
                    });
                    break;
                }
        }
        this.NoTopVisibility = this.TopOpportunitiesList.length == 0 ? true : false;
        //this.TopOpportunitiesList.reverse();
    };
    OpportunityWorkspaceComponent.prototype.LoadFunnelData = function () {
        var _this = this;
        this.myDomainService.GetStageFunnelData(this.OwnerId, this.BusinessUnitId, this.SelectedFunnelFilter.Code, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            _this.FunnelData = myResult.Result;
            _this.fillFunnelData();
        });
    };
    OpportunityWorkspaceComponent.prototype.fillFunnelData = function () {
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
    OpportunityWorkspaceComponent.prototype.FunnelClick = function () {
        var _this = this;
        var item = FunnelClick();
        ResetItem();
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
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadAllScreenData(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    OpportunityWorkspaceComponent.prototype.EditOpportunity = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Opportunity', BackButtonLabel: "Opportunity" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
                //this.isWindowOpened = false;
            });
        });
    };
    OpportunityWorkspaceComponent.prototype.RunOpportunityWizard = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.Title = "New Opportunity";
        logWindow.Show('./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.LoadAllScreenData();
            }
        });
    };
    OpportunityWorkspaceComponent.prototype.ViewOpportunityQuery = function (code) {
        var _this = this;
        if (code != null) {
            var objectTableName = "Opportunity";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
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
                        queryCode = "My Open Opportunities";
                        break;
                    }
                case "Open:All":
                    {
                        queryCode = "All Open Opportunities";
                        break;
                    }
                case "Closed:My":
                    {
                        queryCode = "My Closed Opportunities";
                        break;
                    }
                case "Closed:All":
                    {
                        queryCode = "All Closed Opportunities";
                        break;
                    }
                case "OpenByStage":
                    {
                        queryCode = "Open By Stage";
                        break;
                    }
                case "All":
                    {
                        queryCode = "All Opportunities";
                        break;
                    }
                case "Cancelled":
                    {
                        queryCode = "Cancelled Opportunities";
                        break;
                    }
                default: {
                    break;
                }
            }
            displayTitle = queryCode;
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
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
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], OpportunityWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    OpportunityWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OpportunityWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], OpportunityWorkspaceComponent);
    return OpportunityWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.OpportunityWorkspaceComponent = OpportunityWorkspaceComponent;
var TopOpportunityItem = /** @class */ (function () {
    function TopOpportunityItem(entityList, filter) {
        this.entityList = entityList;
        this.filter = filter;
        this.ComputeFilterValue();
    }
    Object.defineProperty(TopOpportunityItem.prototype, "CustomerName", {
        get: function () { return this.entityList.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "LastActivityTypeName", {
        get: function () { return this.entityList.LastCompletedActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "StageName", {
        get: function () { return this.entityList.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "LastActivityDate", {
        get: function () { return this.entityList.LastCompletedActivityDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "Topic", {
        get: function () { return this.entityList.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "StageAge", {
        get: function () { return this.entityList.LastStageDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "CreateDate", {
        get: function () { return this.entityList.CreateDate; },
        enumerable: true,
        configurable: true
    });
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
    Object.defineProperty(TopOpportunityItem.prototype, "EstimatedClosingDate", {
        get: function () { return this.entityList.EstimatedClosingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "NumberOfShipments", {
        get: function () { return this.entityList.NumberOfShipments; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "BudgetAmount", {
        get: function () { return this.entityList.ValueField; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "OwnerName", {
        get: function () { return this.entityList.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopOpportunityItem.prototype, "StageAgeVisibility", {
        get: function () {
            var result = false;
            if (this.StageAge != null) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    TopOpportunityItem.prototype.ComputeFilterValue = function () {
        var myResult = null;
        switch (this.filter.Code) {
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
        return myResult;
    };
    return TopOpportunityItem;
}());
exports.TopOpportunityItem = TopOpportunityItem;
//# sourceMappingURL=OpportunityWorkspaceComponent.js.map