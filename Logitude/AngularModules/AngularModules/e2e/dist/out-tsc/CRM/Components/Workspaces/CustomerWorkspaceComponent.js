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
var Tools_1 = require("../../../Infrastructure/Tools");
var Args_1 = require("../../../Infrastructure/Args");
var PartnersDomainService_1 = require("../../../Common/Services/PartnersDomainService");
var BusinessUnitListService_1 = require("../../../Common/Services/StandardLists/BusinessUnitListService");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CustomerWorkspaceComponent = /** @class */ (function (_super) {
    __extends(CustomerWorkspaceComponent, _super);
    function CustomerWorkspaceComponent() {
        var _this = _super.call(this) || this;
        _this.SearchBoxWatermark = "Search...";
        _this.DataContext = _this;
        _this.QuickSearchItemsCount = 0;
        _this.QuickSearchItems = [];
        _this.ReloadUserQueries = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.filterName_RecordsType = "RecordsType";
        _this.filterName_CreatedByType = "CreatedByType";
        _this.filterName_Owner = "Owner";
        _this.filterName_BusinessUnit = "BusinessUnit";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.CustomersPageControl";
        _this.filterName_ViewDecreasedDataType = "ViewDecreasedDataType";
        _this.filterName_ViewDecreasedTimeRange = "ViewDecreasedTimeRange";
        _this.searchText = null;
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
        // Decreased Shipment Filters
        _this.DecreasedShipmentsDataTypeList = [];
        _this.DecreasedShipmentsTimeRangeList = [];
        // Load Recent Data
        _this.RecentCustomersCount = 0;
        // Decreased Shipments
        _this.DecreasedQuantityWidth = 60;
        _this.SearchBoxWatermark = TextCodeTranslator_1.TextCodeTranslator.Translate("Card.F.SearchFields");
        _this.InitializeServices();
        _this.LoadNonFilteredQueries();
        _this.BuildDecreasedShipmentsFilters();
        _this.InitializeFilters();
        return _this;
    }
    CustomerWorkspaceComponent.prototype.InitializeServices = function () {
        this.myUserListService = new UserListService_1.UserListService();
        this.myPartnersDomainService = new PartnersDomainService_1.PartnersDomainService;
        this.myBusinessUnitListService = new BusinessUnitListService_1.BusinessUnitListService();
    };
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerWorkspaceComponent.prototype.InitializeFilters = function () {
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
    CustomerWorkspaceComponent.prototype.OnFiltersInitialized = function () {
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
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "SelectedRecordsTypeFilter", {
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
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "SelectedCreatedByTypeFilter", {
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
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "SelectedBusinessUnitFilter", {
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
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "ListOfValuesUserId", {
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
    CustomerWorkspaceComponent.prototype.BuildDecreasedShipmentsFilters = function () {
        // DataType
        this.DecreasedShipmentsDataTypeList = [];
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass_1.CodeNameClass("S", "Shipments"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass_1.CodeNameClass("T", "TEU"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass_1.CodeNameClass("R", "Revenue"));
        this.DecreasedShipmentsDataTypeList.push(new CodeNameClass_1.CodeNameClass("C", "Chargeable Weight"));
        // TimeRange
        var todayDateTime = Tools_1.DateTool.GetCurrentDateAsUtc();
        var last1MonthDateTime = Tools_1.DateTool.GetDateByMonth(-1);
        var last2MonthDateTime = Tools_1.DateTool.GetDateByMonth(-2);
        var date1Formats = Tools_1.DateTool.GetDateFormats(last1MonthDateTime);
        var date2Formats = Tools_1.DateTool.GetDateFormats(last2MonthDateTime);
        var last1MonthLabel = date1Formats.MonthName + " " + date1Formats.DateParts.Year;
        var last2MonthLabel = date2Formats.MonthName + " " + date2Formats.DateParts.Year;
        this.DecreasedShipmentsTimeRangeList = [];
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass_1.CodeNameClass("LM", last1MonthLabel + " vs. " + last2MonthLabel));
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass_1.CodeNameClass("AV.03", last1MonthLabel + " vs. Average of last 3 months"));
        this.DecreasedShipmentsTimeRangeList.push(new CodeNameClass_1.CodeNameClass("AV.12", last1MonthLabel + " vs. Average of last 12 months"));
        var defaultDateTypeFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewDecreasedDataType);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultDateTypeFilterCode)) {
            defaultDateTypeFilterCode = "S";
        }
        var defaultTimeRangeFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_ViewDecreasedTimeRange);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultTimeRangeFilterCode)) {
            defaultTimeRangeFilterCode = "LM";
        }
        this.decreasedShipmentsDataTypeSelectedItem = this.DecreasedShipmentsDataTypeList.filter(function (d) { return d.Code == defaultDateTypeFilterCode; })[0];
        this.decreasedShipmentsTimeRangeSelectedItem = this.DecreasedShipmentsTimeRangeList.filter(function (d) { return d.Code == defaultTimeRangeFilterCode; })[0];
    };
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "DecreasedShipmentsDataTypeSelectedItem", {
        get: function () { return this.decreasedShipmentsDataTypeSelectedItem; },
        set: function (value) {
            if (this.decreasedShipmentsDataTypeSelectedItem != value) {
                this.decreasedShipmentsDataTypeSelectedItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_ViewDecreasedDataType, (value == null ? null : value.Code));
                this.LoadDecreasedShipmentsData();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerWorkspaceComponent.prototype, "DecreasedShipmentsTimeRangeSelectedItem", {
        get: function () { return this.decreasedShipmentsTimeRangeSelectedItem; },
        set: function (value) {
            if (this.decreasedShipmentsTimeRangeSelectedItem != value) {
                this.decreasedShipmentsTimeRangeSelectedItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_ViewDecreasedTimeRange, (value == null ? null : value.Code));
                this.LoadDecreasedShipmentsData();
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomerWorkspaceComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    CustomerWorkspaceComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    CustomerWorkspaceComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    CustomerWorkspaceComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
        this.LoadNonFilteredQueries();
    };
    CustomerWorkspaceComponent.prototype.LoadFilteredQueries = function () {
        this.LoadQueriesCounts();
        this.LoadDecreasedShipmentsData();
    };
    CustomerWorkspaceComponent.prototype.LoadNonFilteredQueries = function () {
        this.ReloadUsersQuery();
        this.LoadRecentCustomers();
    };
    CustomerWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myPartnersDomainService.GetCustomersCounts(this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.Customers_My = myData.MyOpenDataCount;
                    _this.Customers_AccontManager = myData.MyOpenAsAccountManagerDataCount;
                    _this.Customers_Waiting = myData.Customers_Waiting;
                    _this.Customers_Potential = myData.Customers_Potential;
                    _this.Customers_Active = myData.Customers_Active;
                    _this.Customers_Inactive = myData.Customers_Inactive;
                }
            }
        });
    };
    CustomerWorkspaceComponent.prototype.LoadRecentCustomers = function () {
        var _this = this;
        this.myPartnersDomainService.GetRecentCustomers("all", "all").subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.RecentCustomersList = [];
                _this.RecentCustomersCount = 0;
                list.forEach(function (item) {
                    var itemViewModel = new RecentCustomerItem(item);
                    _this.RecentCustomersList.push(itemViewModel);
                });
                _this.RecentCustomersCount = _this.RecentCustomersList.length;
            }
        });
    };
    CustomerWorkspaceComponent.prototype.LoadDecreasedShipmentsData = function () {
        var _this = this;
        var dateTypeCode = "S";
        if (this.DecreasedShipmentsDataTypeSelectedItem != null) {
            dateTypeCode = this.DecreasedShipmentsDataTypeSelectedItem.Code;
        }
        var timeRange = "LM";
        if (this.DecreasedShipmentsTimeRangeSelectedItem != null) {
            timeRange = this.DecreasedShipmentsTimeRangeSelectedItem.Code;
        }
        var myStartDateTime = Tools_1.DateTool.GetDateByMonth(-1);
        this.myPartnersDomainService.GetCustomersDecreasedShipments(dateTypeCode, myStartDateTime, timeRange, this.OwnerId, this.BusinessUnitId, this.RecordsTypeFilterCode).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                _this.BuildDecreasedShipmentsList(myData);
            }
        });
    };
    CustomerWorkspaceComponent.prototype.BuildDecreasedShipmentsList = function (myList) {
        var _this = this;
        this.ShipmentsDataList = [];
        this.DecreasedQuantityWidth = 60;
        if (myList != null) {
            var data = [];
            myList.forEach(function (item) {
                data.push(new ShipmentDataItem(item, _this.DecreasedShipmentsDataTypeSelectedItem.Code));
            });
            data.filter(function (d) { return d.IsDecreased; }).sort(function (a, b) { return a.QuantityValue - b.QuantityValue; }).forEach(function (item) {
                if (_this.ShipmentsDataList.length < 20) {
                    var width = Tools_1.AppTool.GetTextWidth(item.Quantity + "") + 10;
                    if (width > _this.DecreasedQuantityWidth) {
                        _this.DecreasedQuantityWidth = width;
                    }
                    _this.ShipmentsDataList.push(item);
                }
            });
        }
    };
    CustomerWorkspaceComponent.prototype.NewCustomerClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "New Potential Customer";
        logWindow.Width = 990;
        logWindow.Height = 600;
        logWindow.Show("./CommonModules/CommonPartners/Components/NewEntity/NewPotentialCustomerComponent");
        logWindow.WindowClosed.subscribe(function (s) {
            if (s) {
                _this.LoadAllScreenData();
            }
        });
    };
    CustomerWorkspaceComponent.prototype.ViewCustomerQuery = function (code) {
        var _this = this;
        if (code != null) {
            var objectTableName = "Customer";
            var queryCode = null;
            var displayTitle = "";
            var backButtonTitle = "CRM";
            var isBusinessUnitFilterOn = true;
            switch (code) {
                case "My":
                    {
                        queryCode = "Customer.MyCustomers";
                        displayTitle = "My Customers (as Salesman)";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }
                case "All":
                    {
                        queryCode = "Customers";
                        displayTitle = "Customers";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }
                case "AccontManager":
                    {
                        queryCode = "Customer.Q.MyCustomersAccMngr";
                        displayTitle = "My Customers (as Account Manager)";
                        //isBusinessUnitFilterOn = false;
                        break;
                    }
                case "ReadyCustomers":
                    {
                        queryCode = "Customer.ReadyCustomers";
                        displayTitle = "Waiting for Activation";
                        break;
                    }
                case "PotentialCustomers":
                    {
                        queryCode = "Customer.PotentialCustomers";
                        displayTitle = "Potential Customers";
                        break;
                    }
                case "ActiveCustomers":
                    {
                        queryCode = "Customer.ActiveCustomers";
                        displayTitle = "Active Customers";
                        break;
                    }
                case "InactiveCustomers":
                    {
                        queryCode = "Customer.InactiveCustomers";
                        displayTitle = "Inactive Customers";
                        break;
                    }
                default: {
                    break;
                }
            }
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            if (isBusinessUnitFilterOn) {
                //var myOwnerId = null;
                //var myBusinessUnitId = null;
                //if (!AppTool.IsNullOrEmpty(this.OwnerId)) {
                //    myOwnerId = this.OwnerId;
                //}
                //if (!AppTool.IsNullOrEmpty(this.BusinessUnitId)) {
                //    myBusinessUnitId = this.BusinessUnitId;
                //}
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
                        filterAgrs.addAdditionalFilter("SalesmanBusinessUnitId", this.BusinessUnitId, null, null, "Equals", false, true, false, "string");
                    }
                }
            }
            //filterAgrs.Filter1Name = "SalesmanUserId";
            //filterAgrs.Filter1Value = myOwnerId;
            //filterAgrs.Filter1Operator = "Equals";
            //filterAgrs.Filter2Name = "SalesmanBusinessUnitId";
            //filterAgrs.Filter2Value = myBusinessUnitId;
            //filterAgrs.Filter2Operator = "Equals";
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
    CustomerWorkspaceComponent.prototype.EditCustomer = function (entityId) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: 'Customer', BackButtonLabel: "Customers" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadAllScreenData();
            });
        });
    };
    CustomerWorkspaceComponent.prototype.EditCustomerList = function (entity) {
        var _this = this;
        if (entity.IsBlockedQuickSearch) {
            var windowTitle = "View Customer";
            var windowArgs = {};
            windowArgs.CustomerList = entity;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Title = windowTitle;
            logWindow.WindowArgs = windowArgs;
            logWindow.Show('./CRMModules/CRMOthers/Components/BlockedCustomer/BlockedCustomerComponent');
        }
        else {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Customer', BackButtonLabel: "Customers" });
                cmpRef.instance.BackCompleted.subscribe(function ($event) {
                    _this.LoadAllScreenData();
                });
            });
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomerWorkspaceComponent.prototype, "ReloadUserQueries", void 0);
    CustomerWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], CustomerWorkspaceComponent);
    return CustomerWorkspaceComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomerWorkspaceComponent = CustomerWorkspaceComponent;
var RecentCustomerItem = /** @class */ (function () {
    function RecentCustomerItem(entityList) {
        this.entityList = entityList;
        this.ComputeRank();
    }
    Object.defineProperty(RecentCustomerItem.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "LastActivityTypeName", {
        get: function () { return this.entityList.LastActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "LastActivityDate", {
        get: function () { return this.entityList.LastActivityDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "EnglishName", {
        get: function () { return this.entityList.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "CustomerStatusCode", {
        get: function () { return this.entityList.CustomerStatusCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "CustomerStatusName", {
        get: function () { return this.entityList.CustomerStatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "SalesmanUserEnglishName", {
        get: function () { return this.entityList.SalesmanUserEnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RecentCustomerItem.prototype, "AccountManagerUserEnglishName", {
        get: function () { return this.entityList.AccountManagerUserEnglishName; },
        enumerable: true,
        configurable: true
    });
    RecentCustomerItem.prototype.ComputeRank = function () {
        this.RankName = this.entityList.RankName;
        if (this.RankName != null) {
            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    //this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "gold": {
                    //this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "platinum": {
                    //this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    //this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    };
    return RecentCustomerItem;
}());
exports.RecentCustomerItem = RecentCustomerItem;
var ShipmentDataItem = /** @class */ (function () {
    function ShipmentDataItem(entity, filterCode) {
        this.Value1 = 0;
        this.Value2 = 0;
        this.IsDecreased = false;
        this.Quantity = "";
        this.QuantityValue = 0;
        this.Percent = "";
        this.PercentValue = 0;
        this.PercentForeground = "#282E30";
        this.entity = entity;
        this.filterCode = filterCode;
        this.CustomerId = entity.Id;
        this.Id = entity.Id;
        this.ComputeRank();
        this.ComputeValues();
    }
    Object.defineProperty(ShipmentDataItem.prototype, "CustomerName", {
        get: function () { return this.entity.EntityName; },
        enumerable: true,
        configurable: true
    });
    ShipmentDataItem.prototype.ComputeValues = function () {
        var value1 = 0;
        var value2 = 0;
        var isDecreased = false;
        var quantity = "";
        var quantityValue = 0;
        var percent = "";
        var percentValue = 0;
        var percentForeground = "#282E30";
        switch (this.filterCode) {
            case "T": {
                value1 = Tools_1.AppTool.Round(this.entity.TEU_Old, 2);
                value2 = Tools_1.AppTool.Round(this.entity.TEU_New, 2);
                break;
            }
            case "R": {
                value1 = Tools_1.AppTool.Round(this.entity.Revenue_Old, 2);
                value2 = Tools_1.AppTool.Round(this.entity.Revenue_New, 2);
                break;
            }
            case "S": {
                value1 = this.entity.NumberOfShipments_Old;
                value2 = this.entity.NumberOfShipments_New;
                break;
            }
            case "C": {
                value1 = Tools_1.AppTool.Round(this.entity.ChargeableWeight_Old, 2);
                value2 = Tools_1.AppTool.Round(this.entity.ChargeableWeight_New, 2);
                break;
            }
        }
        quantityValue = value2 - value1;
        quantity = Tools_1.AppTool.Round(quantityValue, 2).toString();
        if (value1 != 0 || value2 != 0) {
            if (value1 == 0) {
                percentValue = 100;
                percent = "100 %";
            }
            else if (value2 == 0) {
                percentValue = 100;
                percent = "-100 %";
            }
            else {
                var def = value2 - value1;
                var rat = def / value1 * 100;
                if (rat != null) {
                    percentValue = rat;
                    percent = Tools_1.AppTool.Round(rat, 2) + " %";
                }
            }
        }
        var value = Math.abs(percentValue);
        if (value >= 80) {
            percentForeground = "#FF0000";
        }
        else if (value >= 40 && percentValue < 80) {
            percentForeground = "#FF5F0F";
        }
        else if (value < 40) {
            percentForeground = "#E6A000";
        }
        if (value2 < value1) {
            isDecreased = true;
        }
        this.Value1 = value1;
        this.Value2 = value2;
        this.IsDecreased = isDecreased;
        this.Quantity = quantity;
        this.QuantityValue = quantityValue;
        this.Percent = percent;
        this.PercentValue = percentValue;
        this.PercentForeground = percentForeground;
    };
    ShipmentDataItem.prototype.ComputeRank = function () {
        this.RankName = this.entity.RankName;
        if (this.RankName != null) {
            switch (this.RankName.toLowerCase()) {
                case "silver": {
                    //this.RankCode = "1";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "gold": {
                    //this.RankCode = "2";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                    break;
                }
                case "platinum": {
                    //this.RankCode = "3";
                    this.RankSource1 = "./Images/Icons/StarOrange.png";
                    this.RankSource2 = "./Images/Icons/StarOrange.png";
                    this.RankSource3 = "./Images/Icons/StarOrange.png";
                    break;
                }
                default: {
                    //this.RankCode = "0";
                    this.RankSource1 = "./Images/Icons/StarGray.png";
                    this.RankSource2 = "./Images/Icons/StarGray.png";
                    this.RankSource3 = "./Images/Icons/StarGray.png";
                }
            }
        }
    };
    return ShipmentDataItem;
}());
exports.ShipmentDataItem = ShipmentDataItem;
//# sourceMappingURL=CustomerWorkspaceComponent.js.map