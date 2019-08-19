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
var QuoteDomainService_1 = require("../../Services/QuoteDomainService");
var QuoteListService_1 = require("../../Services/StandardLists/QuoteListService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var DateTimeToDatePipe_1 = require("../../../Controls/Pipes/DateTimeToDatePipe");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Args_1 = require("../../Args");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var Args_2 = require("../../../Infrastructure/Args");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var QuotesComponent = /** @class */ (function (_super) {
    __extends(QuotesComponent, _super);
    function QuotesComponent(_entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.DataContext = _this;
        _this.SalesFunnelId = "SalesFunnelId_";
        _this.IsResourcesReady = false;
        _this.ReloadUserQueries = new core_1.EventEmitter();
        _this.QuickSearchItems = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_RecordsType = "RecordsType";
        _this.filterName_CreatedByType = "CreatedByType";
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Simplog.QuoteLib.Views.QuotesMainMenu.QuotesMainControl";
        _this.filterName_TopQuotes = "TopQuotes";
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
        //Top Quotes Filter
        _this.TopQuotesFilterList = [];
        //Direction and TransportMode filters
        _this.currentDirectionId = "";
        _this.currentTransportModeId = "";
        // Load Recent Data
        _this.RecentQuotesList = [];
        _this.IsNoDataVisible_RecentQuotes = false;
        // Load Top Data
        _this.TopQuotesList = [];
        _this.IsNoDataVisible_TopQuotes = false;
        _this.FunnelDataFilterd = [];
        _this.SalesFunnelId = "SalesFunnel_" + _this.CurrentSession.GetNewId("SalesFunnel");
        _this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.InitializeServices();
            _this.LoadNonFilteredQueries();
            _this.BuildTopQuotesFilters();
            _this.InitializeFilters();
        });
        return _this;
    }
    QuotesComponent.prototype.InitializeServices = function () {
        this.myDomainService = new QuoteDomainService_1.QuoteDomainService();
        this.myUserListService = new UserListService_1.UserListService();
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
    };
    QuotesComponent.prototype.InitializeFilters = function () {
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
    QuotesComponent.prototype.OnFiltersInitialized = function () {
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
    Object.defineProperty(QuotesComponent.prototype, "SelectedRecordsTypeFilter", {
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
    Object.defineProperty(QuotesComponent.prototype, "SelectedCreatedByTypeFilter", {
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
    Object.defineProperty(QuotesComponent.prototype, "SelectedBusinessUnitFilter", {
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
    Object.defineProperty(QuotesComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(QuotesComponent.prototype, "ListOfValuesUserId", {
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
    QuotesComponent.prototype.BuildTopQuotesFilters = function () {
        this.TopQuotesFilterList = [];
        this.TopQuotesFilterList.push(new CodeNameClass_1.CodeNameClass("EX", "Expiration Date"));
        this.TopQuotesFilterList.push(new CodeNameClass_1.CodeNameClass("SD", "Stage Due Date"));
        this.TopQuotesFilterList.push(new CodeNameClass_1.CodeNameClass("RT", "Rating"));
        this.TopQuotesFilterList.push(new CodeNameClass_1.CodeNameClass("ST", "Stage"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopQuotes);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "EX";
        }
        this.selectedTopQuotesItem = this.TopQuotesFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    Object.defineProperty(QuotesComponent.prototype, "SelectedTopQuotesItem", {
        get: function () { return this.selectedTopQuotesItem; },
        set: function (value) {
            if (this.selectedTopQuotesItem != value) {
                this.selectedTopQuotesItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopQuotes, (value == null ? null : value.Code));
                this.LoadTopQuotes();
            }
        },
        enumerable: true,
        configurable: true
    });
    QuotesComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    QuotesComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    QuotesComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    QuotesComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    };
    QuotesComponent.prototype.LoadFilteredQueries = function () {
        this.LoadQueriesCounts();
        this.LoadTopQuotes();
        this.LoadFunnelData();
    };
    QuotesComponent.prototype.LoadNonFilteredQueries = function () {
        this.LoadRecentQuotes();
        this.ReloadUsersQuery();
    };
    QuotesComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myDomainService.GetQuotesCounts(this.OwnerId, this.BusinessUnitId, this.currentDirectionId, this.currentTransportModeId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.Quotes_Created = myData.Quotes_Created;
                    _this.Quotes_Draft = myData.Quotes_Draft;
                    _this.Quotes_Expired = myData.Quotes_Expired;
                    _this.Quotes_Accepted = myData.Quotes_Accepted;
                    _this.Quotes_AcceptedNOShip = myData.Quotes_AcceptedNOShip;
                    _this.Quotes_Cancelled = myData.Quotes_Cancelled;
                    _this.Quotes_Sent = myData.Quotes_Sent;
                    _this.Quotes_AllFollowups = myData.Quotes_AllFollowups;
                    _this.Quotes_MyFollowups = myData.Quotes_MyFollowups;
                    _this.Quotes_All = myData.Quotes_All;
                    _this.Quotes_My = myData.Quotes_My;
                }
            }
        });
    };
    QuotesComponent.prototype.LoadRecentQuotes = function () {
        var _this = this;
        this.RecentQuotesList = [];
        this.IsNoDataVisible_RecentQuotes = false;
        this.myDomainService.GetRecentQuotes("all", "all").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                _this.RecentQuotesList = myResponse.Result;
                if (_this.RecentQuotesList.length == 0) {
                    _this.IsNoDataVisible_RecentQuotes = true;
                }
            }
        });
    };
    QuotesComponent.prototype.LoadTopQuotes = function () {
        var _this = this;
        this.TopQuotesList = [];
        this.IsNoDataVisible_TopQuotes = false;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortDirection = "Descending";
        switch (this.SelectedTopQuotesItem.Code) {
            case "EX":
                {
                    filters.SortBy = "ExpirationDate";
                    break;
                }
            case "SD":
                {
                    filters.SortBy = "StageDueDate";
                    break;
                }
            case "RT":
                {
                    filters.SortBy = "RatingIndexOrder";
                    break;
                }
            case "ST":
                {
                    filters.SortBy = "StageMaxDays";
                    break;
                }
        }
        var myOwnerId = null;
        var myBusinessUnitId = null;
        var myDirectionId = null;
        var myTransportModeId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
            if (myOwnerId == "all" || myOwnerId == "null") {
                myOwnerId = null;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
            myBusinessUnitId = this.BusinessUnitId;
            if (myBusinessUnitId == "all" || myBusinessUnitId == "null") {
                myBusinessUnitId = null;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.currentDirectionId)) {
            myDirectionId = this.currentDirectionId;
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.currentTransportModeId)) {
            myTransportModeId = this.currentTransportModeId;
        }
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("DirectionId", myDirectionId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TransportModeId", myTransportModeId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("TopQuotes", true, null, null, "Equals", true, false, false, "Boolean");
        if (this.RecordsTypeFilterCode == "C") {
            filters.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }
        else {
            filters.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", false, false, false, "string");
            filters.addAdditionalFilter("BusinessUnitId", myBusinessUnitId, null, null, "Equals", false, false, false, "string");
        }
        var myService = new QuoteListService_1.QuoteListService();
        myService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.FillTopQuotesList(list);
            }
        });
    };
    QuotesComponent.prototype.FillTopQuotesList = function (myList) {
        var _this = this;
        switch (this.SelectedTopQuotesItem.Code) {
            case "EX":
                {
                    myList.sort(function (a, b) { return a.ExpirationDate.valueOf() - b.ExpirationDate.valueOf(); }).forEach(function (item) {
                        var itemViewModel = new TopQuoteItem(item, _this.SelectedTopQuotesItem);
                        _this.TopQuotesList.push(itemViewModel);
                    });
                    break;
                }
            case "SD":
                {
                    myList.sort(function (a, b) { return a.StageDueDate.valueOf() - b.StageDueDate.valueOf(); }).forEach(function (item) {
                        var itemViewModel = new TopQuoteItem(item, _this.SelectedTopQuotesItem);
                        _this.TopQuotesList.push(itemViewModel);
                    });
                    break;
                }
            case "RT":
                {
                    myList.sort(function (a, b) { return b.RatingIndexOrder - a.RatingIndexOrder; }).forEach(function (item) {
                        var itemViewModel = new TopQuoteItem(item, _this.SelectedTopQuotesItem);
                        _this.TopQuotesList.push(itemViewModel);
                    });
                    break;
                }
            case "ST":
                {
                    myList.sort(function (a, b) { return a.StageMaxDays - b.StageMaxDays; }).forEach(function (item) {
                        var itemViewModel = new TopQuoteItem(item, _this.SelectedTopQuotesItem);
                        _this.TopQuotesList.push(itemViewModel);
                    });
                    break;
                }
        }
        if (this.TopQuotesList.length == 0) {
            this.IsNoDataVisible_TopQuotes = true;
        }
    };
    QuotesComponent.prototype.LoadFunnelData = function () {
        var _this = this;
        this.myDomainService.GetStageFunnelData(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            _this.FunnelData = myResult;
            _this.fillFunnelData();
        });
    };
    QuotesComponent.prototype.fillFunnelData = function () {
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
            var els = document.getElementsByTagName('a');
            els[1].remove();
        }
        catch (e) { }
    };
    QuotesComponent.prototype.FunnelClick = function () {
        var _this = this;
        var item = FunnelClick();
        ResetItemFunnel();
        if (item != null) {
            var objectTableName = "Quote";
            var queryCode = "Open Quotes";
            var displayTitle = this.FunnelData[item.index].LabelProperty + " Quotes";
            var backButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Quotes");
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var listArgs = new Args_2.ListComponentArgs();
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
            filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", true, false, false, "Boolean");
            if (this.RecordsTypeFilterCode == "C") {
                filterAgrs.addAdditionalFilter("CreatedByUserId", myOwnerId, null, null, "Equals", true, false, false, "string");
            }
            else {
                filterAgrs.addAdditionalFilter("SalesmanUserId", myOwnerId, null, null, "Equals", true, false, false, "string");
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
    QuotesComponent.prototype.EditQuote = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Quote', BackButtonLabel: TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Quotes") });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
                //this.isWindowOpened = false;
            });
        });
    };
    QuotesComponent.prototype.RunQuoteWizard = function () {
        var _this = this;
        var args = new Args_1.NewQuoteComponentArgs();
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Quote.S.NewQuote.CreateNewQuote");
        logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.LoadAllScreenData();
            }
        });
    };
    QuotesComponent.prototype.ViewQuoteQuery = function (code) {
        var _this = this;
        if (code != null) {
            var objectTableName = "Quote";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Quotes");
            var MethodName = null;
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            filterAgrs.SortBy = "OpenDate";
            filterAgrs.SortDirection = "Descending";
            if (this.RecordsTypeFilterCode == "C") {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("CreatedByUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                    filterAgrs.addAdditionalFilter("SalesmanUserId", this.OwnerId, null, null, "Equals", false, true, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                    filterAgrs.addAdditionalFilter("BusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                }
            }
            switch (code) {
                case "CRT":
                    {
                        queryCode = "Created Quotes";
                        break;
                    }
                case "DRF":
                    {
                        queryCode = "Draft Quotes";
                        break;
                    }
                case "SNT":
                    {
                        queryCode = "Sent Quotes";
                        break;
                    }
                case "ACW":
                    {
                        queryCode = "Accepted Without Shipments";
                        break;
                    }
                case "ACP":
                    {
                        queryCode = "Accepted Quotes";
                        break;
                    }
                case "CNC":
                    {
                        queryCode = "Cancelled Quotes";
                        break;
                    }
                case "MY":
                    {
                        queryCode = "My Quotes";
                        break;
                    }
                case "EXP":
                    {
                        queryCode = "Expired Quotes";
                        break;
                    }
                case "AllF":
                    {
                        queryCode = "All Follow Ups";
                        MethodName = "QuoteFollowUp";
                        break;
                    }
                case "MYF":
                    {
                        queryCode = "My Follow Ups";
                        MethodName = "QuoteFollowUp";
                        break;
                    }
                case "ALL":
                    {
                        queryCode = "All Quotes";
                        break;
                    }
                default: {
                    break;
                }
            }
            var listArgs = new Args_2.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.BackButtonTitle = backButtonTitle;
            listArgs.MethodName = MethodName;
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
    QuotesComponent.prototype.OnImageError = function (item, field) {
        if (item && field) {
            item[field] = "--";
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QuotesComponent.prototype, "ReloadUserQueries", void 0);
    QuotesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './QuotesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], QuotesComponent);
    return QuotesComponent;
}(BaseComponent_1.BaseComponent));
exports.QuotesComponent = QuotesComponent;
var TopQuoteItem = /** @class */ (function () {
    function TopQuoteItem(entityList, filter) {
        this.entityList = entityList;
        this.filter = filter;
        this.ComputeFilterValue();
    }
    Object.defineProperty(TopQuoteItem.prototype, "CustomerName", {
        get: function () { return this.entityList.CustomerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "Salesman", {
        get: function () { return this.entityList.Salesman; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "QuoteNumber", {
        get: function () { return this.entityList.QuoteNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "StageName", {
        get: function () { return this.entityList.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "DirectionId", {
        get: function () { return this.entityList.DirectionId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "DirectionName", {
        get: function () { return this.entityList.DirectionName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "TransportModeId", {
        get: function () { return this.entityList.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "TransportModeName", {
        get: function () { return this.entityList.TransportModeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "RatingCode", {
        get: function () { return this.entityList.RatingCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "RatingName", {
        get: function () { return this.entityList.RatingName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopQuoteItem.prototype, "LastStageDate", {
        get: function () { return this.entityList.LastStageDate; },
        enumerable: true,
        configurable: true
    });
    TopQuoteItem.prototype.ComputeFilterValue = function () {
        var myResult = null;
        switch (this.filter.Code) {
            case "EX":
                {
                    myResult = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.entityList.ExpirationDate);
                    break;
                }
            case "SD":
                {
                    myResult = DateTimeToDatePipe_1.DateTimeToDatePipe.Pipe(this.entityList.StageDueDate);
                    break;
                }
            case "RT":
                {
                    myResult = this.entityList.RatingName;
                    break;
                }
            case "ST":
                {
                    myResult = this.entityList.StageName;
                    break;
                }
        }
        this.FilterValue = myResult;
    };
    return TopQuoteItem;
}());
exports.TopQuoteItem = TopQuoteItem;
//# sourceMappingURL=QuotesComponent.js.map