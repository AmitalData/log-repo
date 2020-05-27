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
var LastFilterClass_1 = require("../../../../Infrastructure/Utilities/LastFilterClass");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var UserListService_1 = require("../../../../Common/Services/StandardLists/UserListService");
var CRMUtilities_1 = require("../../../CRMUtilities");
var CRMDomainService_1 = require("../../../Services/CRMDomainService");
var Args_1 = require("../../../../Infrastructure/Args");
var ByFirstResolveTicketComponent = /** @class */ (function (_super) {
    __extends(ByFirstResolveTicketComponent, _super);
    function ByFirstResolveTicketComponent() {
        var _this = _super.call(this) || this;
        _this.filterName_Owner = "Owner";
        _this.filterName_EmployeeGroup = "EmployeeGroup";
        _this.filterControlNameSpace = "Logitude.CRM.Views.TicketMainMenu.TicketDashboardTabs.ByFirstResolveDateControl";
        _this.filterName_FirstResolveDate = "FirstResolveDate";
        _this.EmployeeGroupFilterList = [];
        _this.EmployeeGroupFilterListPM = [];
        _this.DateFilterList = [];
        _this.DataContext = _this;
        _this.fieldCode = "S";
        _this.NewOpenedTicketsFiltered = [];
        _this.NewSLATicketsFiltered = [];
        _this.ClosedTicketsByClassificationIdExistance = false;
        _this.ClosedTicketsBySeverityIdExistance = false;
        _this.ClosedTicketsByTicketTypeIdExistance = false;
        _this.ClosedSLAViolationIdExistance = false;
        _this.ClosedSolvedTicketsIdExistance = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.UsersFilterList = [];
        _this.OwnerId = "";
        _this.EmployeeGroupId = "";
        _this.selectedUserId = "";
        _this.selectedEmployeeGroupId = "";
        _this.ActivityList = [];
        _this.QuoteList = [];
        _this.selectedDateIndex = 0;
        _this.TicketsGroupByClassification = [];
        _this.TicketsGroupBySeverity = [];
        _this.TicketsGroupByOwner = [];
        _this.InitializeIds();
        _this.InitializeServices();
        _this.BuildEmployeeGroupFilterList();
        _this.BuildDateFilters();
        return _this;
    }
    ByFirstResolveTicketComponent.prototype.InitializeServices = function () {
        this.myUserListService = new UserListService_1.UserListService();
        this.crmDomainService = new CRMDomainService_1.CRMDomainService();
    };
    ByFirstResolveTicketComponent.prototype.RefreshButtonClicked = function () {
        this.LoadFilteredQueries();
    };
    ByFirstResolveTicketComponent.prototype.BuildDateFilters = function () {
        this.DateFilterList = CRMUtilities_1.CRMUtilities.GetClosingDateFilterList();
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_FirstResolveDate);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "-7";
        }
        this.selectedDateFilter = this.DateFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    ByFirstResolveTicketComponent.prototype.InitializeIds = function () {
        this.ClosedTicketsByClassificationId = "TicketsByClassificationId_" + this.CurrentSession.GetNewId("TicketsByClassificationId");
        this.ClosedTicketsBySeverityId = "TicketsBySeverityId_" + this.CurrentSession.GetNewId("TicketsBySeverityId");
        this.ClosedTicketsByTicketTypeId = "TicketsByTicketOwnerId_" + this.CurrentSession.GetNewId("TicketsByTicketOwnerId");
        this.ClosedSLAViolationId = "SLAViolationId_" + this.CurrentSession.GetNewId("SLAViolationId");
        this.ClosedSolvedTicketsId = "OpenTicketsId_" + this.CurrentSession.GetNewId("OpenTicketsId");
        this.ClosedTicketsByTicketsTypeLegendId = "ClosedTicketsByTicketsTypeLegendId_" + this.CurrentSession.GetNewId("ClosedTicketsByTicketsTypeLegendId");
        this.ClosedTicketsBySeverityLegendId = "ClosedTicketsBySeverityLegendId_" + this.CurrentSession.GetNewId("ClosedTicketsBySeverityLegendId");
        this.ClosedTicketsByClassificationLegendId = "ClosedTicketsByClassificationLegendId_" + this.CurrentSession.GetNewId("ClosedTicketsByClassificationLegendId");
    };
    ByFirstResolveTicketComponent.prototype.InitTab = function (wizard) {
        this.Wizard = wizard;
    };
    ByFirstResolveTicketComponent.prototype.RefreshTab = function () {
        this.RefreshButtonClicked();
    };
    ByFirstResolveTicketComponent.prototype.BuildEmployeeGroupFilterList = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetEmployeeGroupsPMList().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var list = myResponse.Result;
                _this.EmployeeGroupFilterListPM = list;
                _this.EmployeeGroupFilterList = [];
                _this.EmployeeGroupFilterList.push(new CodeNameClass_1.CodeNameClass("M", "My Records"));
                if (list) {
                    list.filter(function (d) { return d.Id != SessionLocator_1.SessionLocator.Tenant.toString(); }).forEach(function (item) {
                        _this.EmployeeGroupFilterList.push(new CodeNameClass_1.CodeNameClass(item.Id, item.Name));
                    });
                }
                _this.EmployeeGroupFilterList.push(new CodeNameClass_1.CodeNameClass("A", "All Records"));
                var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(_this.filterControlNameSpace, _this.filterName_EmployeeGroup);
                if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
                    defaultFilterCode = "M";
                }
                _this.selectedEmployeeGroupFilter = _this.EmployeeGroupFilterList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
                _this.GetSelectedEmployeeGroup();
                _this.BuildUsersFilters(false);
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.BuildUsersFilters = function (isUpdatingFilter) {
        var _this = this;
        this.UsersFilterList = [];
        if (this.SelectedEmployeeGroupFilter == null) {
            this.selectedUserFilter = null;
            this.GetSelectedOwnerId();
            if (isUpdatingFilter) {
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
            }
            this.LoadFilteredQueries();
        }
        else {
            switch (this.SelectedEmployeeGroupFilter.Code) {
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
                        var item = new CodeNameClass_1.CodeNameClass("A", "All " + this.SelectedEmployeeGroupFilter.Name + " Owners");
                        this.UsersFilterList.push(item);
                        var selectedEmployee = this.EmployeeGroupFilterListPM.filter(function (a) { return a.Id == _this.SelectedEmployeeGroupFilter.Code; })[0];
                        var employeeGroupLinesdIds = [];
                        selectedEmployee.EmployeeGroupLines.forEach(function (item) {
                            employeeGroupLinesdIds.push(item.UserId);
                        });
                        var myIds = this.GetIdsString(employeeGroupLinesdIds);
                        var loadedUsersList = [];
                        var service = new CRMDomainService_1.CRMDomainService();
                        if (employeeGroupLinesdIds != null && employeeGroupLinesdIds.length > 0) {
                            service.GetUsersByEmployeeGroupIds(myIds).subscribe(function (myResult) {
                                var myResponse = myResult;
                                if (!myResponse.HasError) {
                                    loadedUsersList = myResponse.Result.sort(function (a, b) { return (a.EnglishName === b.EnglishName) ? 0 : (a.EnglishName < b.EnglishName) ? -1 : 1; });
                                    if (loadedUsersList != null) {
                                        loadedUsersList.forEach(function (item) {
                                            var record = new CodeNameClass_1.CodeNameClass();
                                            record.Code = item.Id;
                                            record.Name = item.EnglishName;
                                            _this.UsersFilterList.push(record);
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
                        }
                        break;
                    }
            }
        }
    };
    ByFirstResolveTicketComponent.prototype.GetSelectedEmployeeGroup = function () {
        var myResult = null;
        if (this.SelectedEmployeeGroupFilter) {
            switch (this.SelectedEmployeeGroupFilter.Code) {
                case "M": {
                    myResult = null;
                    break;
                }
                case "A": {
                    myResult = null;
                    break;
                }
                default: {
                    myResult = this.SelectedEmployeeGroupFilter.Code;
                    break;
                }
            }
        }
        this.EmployeeGroupId = myResult;
    };
    ByFirstResolveTicketComponent.prototype.GetSelectedOwnerId = function () {
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
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "SelectedEmployeeGroupFilter", {
        get: function () {
            return this.selectedEmployeeGroupFilter;
        },
        set: function (newValue) {
            if (this.selectedEmployeeGroupFilter != newValue) {
                this.selectedEmployeeGroupFilter = newValue;
                this.GetSelectedEmployeeGroup();
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_EmployeeGroup, (newValue == null ? null : newValue.Code));
                this.BuildUsersFilters(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "SelectedUserFilter", {
        get: function () {
            return this.selectedUserFilter;
        },
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
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "ListOfValuesUserId", {
        get: function () {
            return this.listOfValuesUserId;
        },
        set: function (value) {
            if (this.listOfValuesUserId != value) {
                this.OwnerId = value;
                this.listOfValuesUserId = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_Owner, this.OwnerId);
                this.BuildUsersFilters(true);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "IsUsersFilterEnabled", {
        get: function () {
            var myResult = false;
            if (this.SelectedEmployeeGroupFilter != null) {
                if (this.SelectedEmployeeGroupFilter.Code != "M" && this.SelectedEmployeeGroupFilter.Code != "A") {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "IsListOfValuesVisible", {
        get: function () {
            var myResult = false;
            if (this.SelectedEmployeeGroupFilter != null) {
                if (this.SelectedEmployeeGroupFilter.Code == "A") {
                    myResult = true;
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    ByFirstResolveTicketComponent.prototype.GetIdsString = function (ids) {
        var myResult = "";
        ids.forEach(function (Id) {
            if (Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                myResult = Id;
            }
            else {
                myResult += ":" + Id;
            }
        });
        return myResult;
    };
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "SelectedDateFilter", {
        get: function () { return this.selectedDateFilter; },
        set: function (value) {
            if (this.selectedDateFilter != value) {
                this.selectedDateFilter = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_FirstResolveDate, (value == null ? null : value.Code));
                this.LoadFilteredQueries();
            }
        },
        enumerable: true,
        configurable: true
    });
    ByFirstResolveTicketComponent.prototype.LoadFilteredQueries = function () {
        if (this.SelectedDateFilter != null) {
            this.LoadClosedTicketsClassificationData();
            this.LoadClosedTicketsSeverityData();
            this.LoadClosedTicketsTypeData();
            this.LoadClosedSLAViolationData();
            this.LoadClosedSolvedTicketData();
        }
    };
    ByFirstResolveTicketComponent.prototype.SLAClicking = function () {
        if (BarClick() != null) {
            this.OnSLAClick(BarClick());
            ResetItemPie();
        }
    };
    ByFirstResolveTicketComponent.prototype.SolvedClosedTicketsClicking = function () {
        if (Lineclick() != null) {
            this.SolvedClosedTicketsClick(Lineclick());
            ResetLineclick();
        }
    };
    ByFirstResolveTicketComponent.prototype.OnTicketClick = function (e, code) {
        var _this = this;
        var item = null;
        var myQueryCode = "All Tickets";
        var myTableName = "Ticket";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        if (code == "M") {
            item = this.TicketsGroupByClassification[e.index];
            filterAgrs.addAdditionalFilter("MainClassificationId", item.ClassificationId, null, null, "Equals", false, false, false, "String");
        }
        else if (code == "S") {
            item = this.TicketsGroupBySeverity[e.index];
            filterAgrs.addAdditionalFilter("SeverityId", item.SeverityId, null, null, "Equals", false, false, false, "String");
        }
        else {
            item = this.TicketsGroupByOwner[e.index];
            filterAgrs.addAdditionalFilter("TicketTypeId", item.TicketTypeId, null, null, "Equals", false, false, false, "String");
        }
        filterAgrs.addAdditionalFilter("OwnerId", item.OwnerId, null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("ChartFirstResolveTicketFilter", item.Code, null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("EmployeeGroupId", item.EmployeeGroupId, null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = "Ticket";
        listArgs.BackButtonTitle = "Ticket";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    ByFirstResolveTicketComponent.prototype.SolvedClosedTicketsClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        if (item.values.value != 0) {
            var myQueryCode = "All Tickets";
            var myTableName = "Ticket";
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var typeName = "Ticket";
            var myDateFilter = this.NewOpenedTicketsFiltered[item.index].DateTimeProperty + "?" + this.NewOpenedTicketsFiltered[item.index].GroupByCode + "?" + this.NewOpenedTicketsFiltered[item.index].Code;
            filterAgrs.addAdditionalFilter("OwnerId", this.NewOpenedTicketsFiltered[item.index].OwnerId, null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("SolvedClosedTicketsFilter", myDateFilter, this.NewOpenedTicketsFiltered[item.index].DateTimeProperty, null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewOpenedTicketsFiltered[item.index].EmployeeGroupId, null, null, "Equals", false, false, false, "String");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = typeName;
            listArgs.BackButtonTitle = "Ticket";
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    ByFirstResolveTicketComponent.prototype.OnSLAClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        if (item.values.value != 0) {
            var myQueryCode = "All Tickets";
            var myTableName = "Ticket";
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            var typeName = "Ticket";
            var myDateFilter = this.NewSLATicketsFiltered[e.target.index].DateTimeProperty[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].GroupByCode[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].DataTypeCode[item.index] + "?" + this.NewSLATicketsFiltered[e.target.index].Code[item.index];
            filterAgrs.addAdditionalFilter("OwnerId", this.NewSLATicketsFiltered[e.target.index].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
            filterAgrs.addAdditionalFilter("SLAEscalationsClosedTicketsFilter", myDateFilter, this.NewSLATicketsFiltered[e.target.index].DateTimeProperty[item.index], null, "Equals", true, false, false, "String");
            filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewSLATicketsFiltered[e.target.index].EmployeeGroupId[item.index], null, null, "Equals", false, false, false, "String");
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = myQueryCode;
            listArgs.ObjectTableName = myTableName;
            listArgs.DisplayTitle = typeName;
            listArgs.BackButtonTitle = "Ticket";
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        }
    };
    ByFirstResolveTicketComponent.prototype.LoadClosedSolvedTicketData = function () {
        var _this = this;
        this.crmDomainService.GetClosedTicketsBySolvedStage(this.SelectedDateIndex, this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(function (result) {
            try {
                var elm = document.getElementById(_this.ClosedSolvedTicketsId);
                elm.innerHTML = "";
            }
            catch (er) { }
            if (result.Result.length == 0) {
                _this.ClosedSolvedTicketsIdExistance = false;
            }
            else {
                _this.NewOpenedTicketsFiltered = result.Result;
                _this.FillTicketsByTicketOwner(result.Result);
                _this.ClosedSolvedTicketsIdExistance = true;
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.FillTicketsByTicketOwner = function (List) {
        var _this = this;
        var lineData = this.FillLine(List);
        lineData.forEach(function (p) {
            if (p.visits != "0")
                _this.ClosedSolvedTicketsIdExistance = false;
        });
        try {
            var els = document.getElementById(this.ClosedSolvedTicketsId);
            if (!this.ClosedSolvedTicketsIdExistance) {
                makeAMLineChart(this.ClosedSolvedTicketsId, lineData);
                els.hidden = false;
            }
            else {
                els.hidden = true;
            }
        }
        catch (Ex) { }
    };
    ByFirstResolveTicketComponent.prototype.FillLine = function (data) {
        var index = 0;
        var lineChartData = [{ data: [], label: '' }];
        var AmLineChartTest = [];
        lineChartData = [{
                scales: {
                    xAxes: [{
                            gridThickness: 0,
                        }]
                },
                xAxes: {
                    gridThickness: 0,
                },
                offsetGridLines: false,
                scaleShowVerticalLines: false,
                data: [], label: 'Total', tension: 0, scaleShowHorizontalLines: false, scaleStepWidth: 0
            }];
        var lineChartLabels = [];
        data.forEach(function (element) {
            lineChartData[0].data[index] = element.IntegerProperty + "";
            lineChartLabels.push(element.LabelProperty != null ? element.LabelProperty : "");
            index++;
            AmLineChartTest.push({
                date: element.LabelProperty != null ? element.LabelProperty : "",
                visits: element.IntegerProperty + ""
            });
        });
        return AmLineChartTest;
    };
    Object.defineProperty(ByFirstResolveTicketComponent.prototype, "SelectedDateIndex", {
        get: function () {
            return this.selectedDateIndex;
        },
        set: function (value) {
            if (value != this.selectedDateIndex)
                this.SelectedDateIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    ByFirstResolveTicketComponent.prototype.LoadClosedSLAViolationData = function () {
        var _this = this;
        this.crmDomainService.GetClosedTicketsBySLAViolation(this.SelectedDateIndex, this.SelectedDateFilter.Code, this.OwnerId, this.EmployeeGroupId).subscribe(function (result) {
            if (result.Result.length == 0) {
                _this.ClosedSLAViolationIdExistance = false;
                try {
                    var elm = document.getElementById(_this.ClosedSLAViolationId);
                }
                catch (er) { }
                elm.innerHTML = "";
            }
            else {
                _this.ClosedSLAViolationIdExistance = true;
                _this.FillSLAViolationList(result.Result);
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.FillSLAViolationList = function (List) {
        var _this = this;
        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr = new Array();
        var j = 0;
        List = List.filter(function (element) { return element.DataTypeCode == "FR" || element.DataTypeCode == "RW"; });
        List.forEach(function (element) {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], EmployeeGroupId: [], DateTimeProperty: [], GroupByCode: [], DataTypeCode: [], Code: [] };
                NewCustomerYAxis[j].data = [];
                j++;
            }
        });
        var Graphs = [];
        var index = 0;
        List.forEach(function (element) {
            for (var i = 0; i < StringArr.length; i++) {
                if (element.LabelProperty == StringArr[i]) {
                    var k = 0;
                    if (element.DataTypeCode == "RW")
                        k = 1;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(2);
                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].EmployeeGroupId[k] = element.EmployeeGroupId;
                    NewCustomerYAxis[i].DateTimeProperty[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].GroupByCode[k] = element.GroupByCode;
                    NewCustomerYAxis[i].DataTypeCode[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].Code[k] = element.Code;
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    if (!NewCustomerXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.LabelProperty);
                    }
                }
            }
        });
        var barChartColors = [
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',
                borderWidth: 0
            },
            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',
                borderWidth: 0,
            },
        ];
        this.NewSLATicketsFiltered = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        NewCustomerYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.NewSLATicketsFiltered[i] == null) {
                    _this.NewSLATicketsFiltered[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], DateTime: [], EmployeeGroupId: [], DateTimeProperty: [], GroupByCode: [], DataTypeCode: [], Code: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.NewSLATicketsFiltered[i].data.push(element.data[i]);
                _this.NewSLATicketsFiltered[i].BindingElement.push(element.BindingElement[i]);
                _this.NewSLATicketsFiltered[i].OwnerIds.push(element.OwnerIds[i]);
                _this.NewSLATicketsFiltered[i].EmployeeGroupId.push(element.EmployeeGroupId[i]);
                _this.NewSLATicketsFiltered[i].DateTimeProperty.push(element.DateTimeProperty[i]);
                _this.NewSLATicketsFiltered[i].GroupByCode.push(element.GroupByCode[i]);
                _this.NewSLATicketsFiltered[i].DataTypeCode.push(element.DataTypeCode[i]);
                _this.NewSLATicketsFiltered[i].Code.push(element.Code[i]);
                _this.NewSLATicketsFiltered[i].OwnerIds.push(element.OwnerIds[i]);
                _this.NewSLATicketsFiltered[i].label = element.label;
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
            DataProvider[index] = { "category": NewCustomerXAxis[index], "col1": objectArray[0], "col2": objectArray[1] };
            index++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (NewCustomerXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.ClosedSLAViolationId, Graphs, DataProvider, maximum, null, null, 0, true);
            }
        }
        catch (e) {
        }
    };
    ByFirstResolveTicketComponent.prototype.LoadClosedTicketsTypeData = function () {
        var _this = this;
        this.crmDomainService.GetClosedTicketsGroupByType(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(function (result) {
            if (_this.CurrentTicketByTicketTypeChart != null) {
                _this.CurrentTicketByTicketTypeChart.clear();
                _this.CurrentTicketByTicketTypeChart = null;
            }
            if (result.Result.length == 0) {
                _this.ClosedTicketsByTicketTypeIdExistance = false;
                try {
                    //   var elm = document.getElementById(this.ClosedTicketsByTicketTypeId);
                }
                catch (er) { }
                //  elm.innerHTML = "";
            }
            else {
                _this.ClosedTicketsByTicketTypeIdExistance = true;
                _this.FillTicketsGroupByType(result.Result);
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.LoadClosedTicketsSeverityData = function () {
        var _this = this;
        this.crmDomainService.GetClosedTicketsGroupBySeverity(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(function (result) {
            try {
                if (_this.CurrentTicketBySeverityChart != null) {
                    _this.CurrentTicketBySeverityChart.clear();
                    _this.CurrentTicketBySeverityChart = null;
                }
            }
            catch (er) { }
            if (result.Result.length == 0) {
                _this.ClosedTicketsBySeverityIdExistance = false;
            }
            else {
                _this.FillTicketsGroupBySeverity(result.Result);
                _this.ClosedTicketsBySeverityIdExistance = true;
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.LoadClosedTicketsClassificationData = function () {
        var _this = this;
        this.crmDomainService.GetClosedTicketsGroupByClassification(this.SelectedDateFilter.Code + "", this.OwnerId, this.EmployeeGroupId).subscribe(function (result) {
            try {
                if (_this.CurrentTicketByClassificationChart != null) {
                    _this.CurrentTicketByClassificationChart.clear();
                    _this.CurrentTicketByClassificationChart = null;
                }
            }
            catch (er) { }
            if (result.Result.length == 0) {
                _this.ClosedTicketsByClassificationIdExistance = false;
            }
            else {
                _this.FillTicketsGroupByClassifications(result.Result);
                _this.ClosedTicketsByClassificationIdExistance = true;
            }
        });
    };
    ByFirstResolveTicketComponent.prototype.FillTicketsGroupByClassifications = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupByClassification = List;
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
            this.CurrentTicketByClassificationChart = makePieChart(this.ClosedTicketsByClassificationId, fullData, false, true, this.ClosedTicketsByClassificationLegendId);
        }
    };
    ByFirstResolveTicketComponent.prototype.TicketClicking = function (code) {
        if (PieClick() != null) {
            this.OnTicketClick(PieClick(), code);
            ResetItemPie();
        }
    };
    ByFirstResolveTicketComponent.prototype.FillTicketsGroupBySeverity = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupBySeverity = List;
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
            this.CurrentTicketBySeverityChart = makePieChart(this.ClosedTicketsBySeverityId, fullData, false, true, this.ClosedTicketsBySeverityLegendId);
        }
    };
    ByFirstResolveTicketComponent.prototype.FillTicketsGroupByType = function (List) {
        var pieChartLabels = [];
        var pieChartData = [];
        var fullData = [];
        this.TicketsGroupByOwner = List;
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
            this.CurrentTicketByTicketTypeChart = makePieChart(this.ClosedTicketsByTicketTypeId, fullData, false, true, this.ClosedTicketsByTicketsTypeLegendId);
        }
    };
    ByFirstResolveTicketComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ByFirstResolveTicketComponent.html',
            encapsulation: core_1.ViewEncapsulation.None,
        }),
        __metadata("design:paramtypes", [])
    ], ByFirstResolveTicketComponent);
    return ByFirstResolveTicketComponent;
}(BaseComponent_1.BaseComponent));
exports.ByFirstResolveTicketComponent = ByFirstResolveTicketComponent;
//# sourceMappingURL=ByFirstResolveTicketComponent.js.map