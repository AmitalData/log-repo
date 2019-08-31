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
var TicketListService_1 = require("../../Services/StandardLists/TicketListService");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../Infrastructure/Args");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CardListService_1 = require("../../../Common/Services/StandardLists/CardListService");
var LastFilterClass_1 = require("../../../Infrastructure/Utilities/LastFilterClass");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var CodeNameClass_1 = require("../../../Infrastructure/DataContracts/CodeNameClass");
var TicketsComponent = /** @class */ (function (_super) {
    __extends(TicketsComponent, _super);
    function TicketsComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        //private ShipmentNumber: string = null;
        _this.ShipmentId = null;
        _this.CompanyId = null;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.IsVisible = false;
        _this.ReloadUserQueries = new core_1.EventEmitter();
        _this.QuickSearchItems = [];
        _this.OpenTicketsDueTimeExistance = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsReady = false;
        // Queries Features
        _this.OpenQueriesVisibility = false;
        _this.SolvedQueriesVisibility = false;
        _this.OthersQueriesVisibility = false;
        _this.UnassignedTicketQueryVisibility = false;
        _this.RecentlyUpdatedVisibility = false;
        _this.SLAFailureVisibility = false;
        _this.AllOpenQueryVisibility = false;
        _this.SolvedSLAFailureQueryVisibility = false;
        _this.SolvedTicketsQueryVisibility = false;
        _this.AllCancelledQueryVisibility = false;
        _this.AllTicketsQueryVisibility = false;
        _this.MyViewsQueryVisibility = false;
        // Recent Tickets
        _this.RecentTicketsCount = 0;
        _this.RecentTicketsList = [];
        // Top Tickets 
        _this.TopTicketsCount = 0;
        _this.TopTicketsList = [];
        _this.SeverityVisibility = false;
        _this.RankVisibility = false;
        _this.ActivityWatchVisibility = false;
        _this.UpdateDateVisibility = false;
        _this.CreateDateVisibility = false;
        // Filters 
        _this.EmployeeGroupFilterList = [];
        _this.EmployeeGroupFilterListPM = [];
        _this.UsersFilterList = [];
        _this.filterName_Owner = "Owner";
        _this.filterControlNameSpace = "Logitude.CRM.Views.CRMPages.TicketsPageControl";
        _this.filterName_EmployeeGroup = "EmployeeGroup";
        _this.filterName_TopTickets = "TopTickets";
        _this.OwnerId = "";
        _this.EmployeeGroupId = "";
        _this.selectedUserId = "";
        _this.selectedEmployeeGroupId = "";
        _this.InitializeIds();
        return _this;
    }
    TicketsComponent.prototype.InitializeIds = function () {
        this.OpenTicketsDueTimeId = "OpenTicketsDueTimeId_" + this.CurrentSession.GetNewId("OpenTicketsDueTimeId");
    };
    TicketsComponent.prototype.OnOpenTicketsClick = function (e) {
        var _this = this;
        var flag = false;
        var item;
        if (e.item != null && e.target != null)
            flag = true;
        item = e.item;
        var Key = e.target.columnIndex;
        var myQueryCode = "All Open Tickets";
        var myTableName = "Ticket";
        var displayTitle = "";
        var typeName = "Ticket";
        var labelString = String.prototype.toLowerCase.apply(this.NewOpenTicketsDueTime[e.target.columnIndex].LabelProperty[item.index] + "");
        if (labelString == "overdue" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " Overdue (First Response) ";
        }
        if (labelString == "overdue" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " Overdue (Resolve) ";
        }
        if (labelString == "due < 1h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 1h (First Response) ";
        }
        if (labelString == "due < 1h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 1h (Resolve) ";
        }
        if (labelString == "due < 2h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 2h (First Response) ";
        }
        if (labelString == "due < 2h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 2h (Resolve) ";
        }
        if (labelString == "due < 4h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 4h (First Response) ";
        }
        if (labelString == "due < 4h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " due < 4h (Resolve) ";
        }
        if (labelString == "due < 8h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "FR") {
            displayTitle = typeName + " due < 8h (First Response) ";
        }
        if (labelString == "due < 8h" && this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] == "RE") {
            displayTitle = typeName + " (Resolve) due < 8h ";
        }
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("OpenTicketByDueTimeCustomFilter", (this.NewOpenTicketsDueTime[e.target.columnIndex].LabelProperty[item.index] + "").toLowerCase() + (this.NewOpenTicketsDueTime[e.target.columnIndex].BindingElement[item.index] + "").toLowerCase(), null, null, "Equals", true, false, false, "String");
        filterAgrs.addAdditionalFilter("OwnerId", this.NewOpenTicketsDueTime[e.target.columnIndex].OwnerIds[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("EmployeeGroupId", this.NewOpenTicketsDueTime[e.target.columnIndex].EmployeeGroupId[item.index], null, null, "Equals", false, false, false, "String");
        filterAgrs.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        filterAgrs.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = myQueryCode;
        listArgs.ObjectTableName = myTableName;
        listArgs.DisplayTitle = displayTitle;
        listArgs.BackButtonTitle = "Tickets";
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadFilteredQueries(); });
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    TicketsComponent.prototype.OpenTicketsClick = function () {
        if (BarClick() != null) {
            this.OnOpenTicketsClick(BarClick());
            ResetItem();
        }
    };
    TicketsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(function (response) {
            _this.IsVisible = true;
            _this.LoadAllScreenData();
            _this.BuildEmployeeGroupFilterList();
        });
    };
    TicketsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            if (SessionLocator_1.SessionLocator.ExternalParams) {
                if (SessionLocator_1.SessionLocator.ExternalParams.Menu.toLocaleLowerCase() == "tickets") {
                    if (SessionLocator_1.SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "new") {
                        this.RunNewTicketWizard();
                    }
                    if (SessionLocator_1.SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "query") {
                        SessionLocator_1.SessionLocator.ExternalParams.Args.forEach(function (arg) {
                            if (arg.FieldName == 'ShipmentId') {
                                _this.ShipmentId = arg.FieldValue;
                                //if (arg.FieldValue) {
                                //    this.GetShipmentById(arg.FieldValue);
                                //}
                                //else {
                                //    this.IsShipmentFinished = true;
                                //}
                            }
                            if (arg.FieldName.toLocaleLowerCase() == "companycode") {
                                if (arg.FieldValue) {
                                    _this.GetCardFroCode(arg.FieldValue);
                                }
                                else {
                                    _this.IsReady = true;
                                }
                            }
                        });
                        this.RunTicketListByShipment();
                    }
                    //if (SessionLocator.ExternalParams.Action.toLocaleLowerCase() == "view") {
                    //    var entityId :any;
                    //    SessionLocator.ExternalParams.Args.forEach(arg => {
                    //        if (arg.FieldName == 'entityId') {
                    //            entityId = arg.FieldValue;
                    //        }
                    //    });
                    //    if (!AppTool.IsNullOrEmpty(entityId)) {
                    //        this.EditTicket(entityId);
                    //    }
                    //}
                    //SessionLocator.ClearExternalParams();
                }
            }
        }
    };
    TicketsComponent.prototype.GetCardFroCode = function (code) {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 10;
        var myService = new CardListService_1.CardListService();
        if (!Tools_1.AppTool.IsNullOrEmpty(code)) {
            filters.addAdditionalFilter("Code", code, null, null, "Equals", false, false, false, "string");
        }
        myService.getByFilters(filters).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var cards = myResponse.Result;
                if (cards != null && cards.length > 0) {
                    var card = cards[0];
                    _this.CompanyId = card.Id;
                }
                _this.IsReady = true;
                _this.RunTicketListByShipment();
            }
        });
    };
    TicketsComponent.prototype.RefreshButtonClicked = function () {
        this.LoadAllScreenData();
    };
    TicketsComponent.prototype.LoadAllScreenData = function () {
        this.LoadFilteredQueries();
    };
    TicketsComponent.prototype.LoadFilteredQueries = function () {
        //this.OwnerId = this.selectedUserId;
        //this.EmployeeGroupId = this.selectedEmployeeGroupId;
        this.SetQueriesVisibility();
        this.ReloadUsersQuery();
        this.LoadQueriesCounts();
        this.LoadRecentTickets();
        this.BuildTopTicketsFilters();
        this.LoadTopTickets();
        this.LoadOpenTicketsDueTimeData();
    };
    TicketsComponent.prototype.SetQueriesVisibility = function () {
        this.OpenQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllOpenTickets") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.UnassignedTickets") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SLAFailureTickets") ? true : false;
        this.UnassignedTicketQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.UnassignedTickets") ? true : false;
        this.RecentlyUpdatedVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.RecentlyUpdatedTickets") ? true : false;
        this.SLAFailureVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SLAFailureTickets") ? true : false;
        this.AllOpenQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllOpenTickets") ? true : false;
        this.SolvedQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedSLAFailureTickets") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedTickets") ? true : false;
        this.SolvedSLAFailureQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedSLAFailureTickets") ? true : false;
        this.SolvedTicketsQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.SolvedTickets") ? true : false;
        this.OthersQueriesVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllCancelledTickets") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllTickets") ? true : false;
        this.AllCancelledQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllCancelledTickets") ? true : false;
        this.AllTicketsQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "Ticket.Q.AllTickets") ? true : false;
        this.MyViewsQueryVisibility = FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "BUILDQUERIES") ? true : false;
    };
    // Queries
    TicketsComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetTicketsCounts(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResult) {
            if (myResult != null) {
                _this.AllOpenCount = myResult.MyOpenDataCount > 1000 ? "1000+" : myResult.MyOpenDataCount.toString();
                _this.SLAFailureCount = myResult.SLA_Failures > 1000 ? "1000+" : myResult.SLA_Failures.toString();
                _this.AllUnassignedTicketCount = myResult.Unassigned_Tickets > 1000 ? "1000+" : myResult.Unassigned_Tickets.toString();
                _this.RecentlyUpdatedCount = myResult.RecentlyUpdated_Tickets > 1000 ? "1000+" : myResult.RecentlyUpdated_Tickets.toString();
            }
        });
    };
    TicketsComponent.prototype.ViewTicketQuery = function (queryCode) {
        var _this = this;
        if (queryCode != null) {
            var objectTableName = "Ticket";
            var displayTitle = "";
            var backButtonTitle = "Tickets";
            this.filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
                this.filterAgrs.addAdditionalFilter("OwnerId", this.OwnerId, null, null, "Equals", false, false, false, "string");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
                this.filterAgrs.addAdditionalFilter("EmployeeGroupId", this.EmployeeGroupId, null, null, "Equals", false, false, false, "string");
            }
            switch (queryCode) {
                case "Open:Unassigned":
                    {
                        displayTitle = "Unassigned Tickets";
                        queryCode = "Unassigned Tickets";
                        break;
                    }
                case "Open:SLAFailure":
                    {
                        displayTitle = "SLA Open Failures";
                        queryCode = "SLA Failures";
                        break;
                    }
                case "Open:All":
                    {
                        displayTitle = "All Open Tickets";
                        queryCode = "All Open Tickets";
                        break;
                    }
                case "Open:ReOpen":
                    {
                        break;
                    }
                case "Open:RecentlyUpdated":
                    {
                        displayTitle = "Recently Updated";
                        queryCode = "Recently Updated Tickets";
                        break;
                    }
                case "Others:AllTickets":
                    {
                        displayTitle = "All Tickets";
                        queryCode = "All Tickets";
                        break;
                    }
                case "Solved:SLAFailure":
                    {
                        displayTitle = "SLA Solved\\Closed Failures";
                        queryCode = "Solved with SLA Failures";
                        break;
                    }
                case "Solved:Solved":
                    {
                        displayTitle = "All Solved\\Closed Tickets";
                        queryCode = "Solved Tickets";
                        break;
                    }
                case "Others:AllCancelled":
                    {
                        displayTitle = "Cancelled Tickets";
                        queryCode = "All Cancelled Tickets";
                        break;
                    }
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = this.filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = "Ticket";
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
    TicketsComponent.prototype.OnBackFromList = function () {
        this.LoadRecentTickets();
    };
    TicketsComponent.prototype.LoadRecentTickets = function () {
        var _this = this;
        var crmService = new CRMDomainService_1.CRMDomainService();
        crmService.GetRecentTickets(null, null).subscribe(function (myResult) {
            if (myResult == null) {
                _this.RecentTicketsList = [];
                _this.RecentTicketsCount = 0;
            }
            else {
                _this.RecentTicketsList = myResult;
                _this.RecentTicketsCount = myResult.length;
            }
        });
    };
    //Edit Ticket 
    TicketsComponent.prototype.EditTicket = function (entity) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: 'Ticket', BackButtonLabel: 'Tickets' });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.OnBackFromEdit();
                _this.LoadAllScreenData();
            });
        });
    };
    TicketsComponent.prototype.OnBackFromEdit = function () {
        SessionLocator_1.SessionLocator.ClearExternalParams();
    };
    TicketsComponent.prototype.LoadTopTickets = function () {
        var _this = this;
        this.TopTicketsList = [];
        if (this.myTicketListService == null) {
            this.myTicketListService = new TicketListService_1.TicketListService();
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        var sortingCol;
        var sortingDir;
        var myOwnerId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            myOwnerId = this.OwnerId;
        }
        var myEmployeeGroupId = null;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            myEmployeeGroupId = this.EmployeeGroupId;
        }
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsCancelled", false, null, null, "Equals", false, false, false, "Boolean");
        if (!Tools_1.AppTool.IsNullOrEmpty(this.OwnerId)) {
            filters.addAdditionalFilter("OwnerId", myOwnerId, null, null, "Equals", false, false, false, "string");
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EmployeeGroupId)) {
            filters.addAdditionalFilter("EmployeeGroupId", myEmployeeGroupId, null, null, "Equals", false, false, false, "string");
        }
        this.SeverityVisibility = false;
        this.RankVisibility = false;
        this.ActivityWatchVisibility = false;
        this.UpdateDateVisibility = false;
        switch (this.topTicketsComboListSelectedItem.Code) {
            case "SEV":
                {
                    sortingCol = "SeverityPriority";
                    sortingDir = "Ascending";
                    this.SeverityVisibility = true;
                    break;
                }
            case "CRK":
                {
                    sortingCol = "RankCode";
                    sortingDir = "Descending";
                    this.RankVisibility = true;
                    break;
                }
            case "AWT":
                {
                    sortingCol = "ActivityWatch";
                    sortingDir = "Descending";
                    this.ActivityWatchVisibility = true;
                    break;
                }
            case "UPD":
                {
                    sortingCol = "UpdateDate";
                    sortingDir = "Descending";
                    this.UpdateDateVisibility = true;
                    break;
                }
        }
        filters.PageIndex = 0;
        filters.PageSize = 10;
        filters.SortBy = sortingCol;
        filters.SortDirection = sortingDir;
        //filters.GetCount = false;
        this.myTicketListService.getByFilters(filters).subscribe(function (myResult) {
            _this.TopTicketsList = [];
            _this.TopTicketsCount = 0;
            var baselist = myResult.Result;
            _this.TopTicketsCount = baselist.length;
            switch (_this.topTicketsComboListSelectedItem.Code) {
                case "SEV":
                    {
                        baselist.forEach(function (item) {
                            _this.TopTicketsList.push(new TopTicketClass(item, _this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }
                case "CRK":
                    {
                        baselist.sort(function (a, b) { return (a.RankCode === b.RankCode) ? 0 : (a.RankCode < b.RankCode) ? -1 : 1; }).forEach(function (item) {
                            _this.TopTicketsList.push(new TopTicketClass(item, _this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }
                case "AWT":
                    {
                        baselist.sort(function (a, b) { return (a.ActivityWatch === b.ActivityWatch) ? 0 : a.ActivityWatch ? -1 : 1; }).forEach(function (item) {
                            _this.TopTicketsList.push(new TopTicketClass(item, _this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }
                case "UPD":
                    {
                        var tempList = baselist.filter(function (d) { return d.UpdatedByUserId != SessionLocator_1.SessionLocator.LoggedUserId; });
                        tempList.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.UpdateDate) === Tools_1.DateTool.GetDateFromDate(b.UpdateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.UpdateDate) > Tools_1.DateTool.GetDateFromDate(b.UpdateDate)) ? -1 : 1; }).forEach(function (item) {
                            _this.TopTicketsList.push(new TopTicketClass(item, _this.topTicketsComboListSelectedItem));
                        });
                        break;
                    }
            }
        });
    };
    TicketsComponent.prototype.LoadOpenTicketsDueTimeData = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetOpenTicketsByDueTime(this.OwnerId, this.EmployeeGroupId).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                if (myResponse.Result.length == 0) {
                    _this.OpenTicketsDueTimeExistance = false;
                    try {
                        var elm = document.getElementById(_this.OpenTicketsDueTimeId);
                    }
                    catch (er) { }
                    elm.innerHTML = "";
                }
                else {
                    _this.OpenTicketsDueTimeExistance = true;
                    _this.FillOpenTicketsDueTimeList(myResponse.Result);
                }
            }
        });
    };
    TicketsComponent.prototype.FillOpenTicketsDueTimeList = function (List) {
        var _this = this;
        var index = 0;
        var NewCustomerXAxis = [];
        var NewCustomerYAxis = [];
        var StringArr = new Array();
        var j = 0;
        List = List.filter(function (element) { return element.DataTypeCode == "FR" || element.DataTypeCode == "RE"; });
        List.forEach(function (element) {
            if (!StringArr.includes(element.LabelProperty)) {
                StringArr.push(element.LabelProperty);
                NewCustomerYAxis[j] = { data: [], label: null, BindingElement: [], OwnerIds: [], LabelProperty: [], DateTime: [], EmployeeGroupId: [] };
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
                    if (element.DataTypeCode == "RE")
                        k = 1;
                    if (NewCustomerYAxis[i].data.length == 0)
                        NewCustomerYAxis[i].data = new Array(2);
                    NewCustomerYAxis[i].data[k] = element.IntegerProperty;
                    NewCustomerYAxis[i].label = element.Code;
                    NewCustomerYAxis[i].BindingElement[k] = element.DataTypeCode;
                    NewCustomerYAxis[i].EmployeeGroupId[k] = element.EmployeeGroupId;
                    NewCustomerYAxis[i].DateTime[k] = element.DateTimeProperty;
                    NewCustomerYAxis[i].OwnerIds[k] = element.OwnerId;
                    NewCustomerYAxis[i].LabelProperty[k] = element.LabelProperty;
                    if (!NewCustomerXAxis.includes(element.LabelProperty) && element.LabelProperty != null) {
                        if (NewCustomerXAxis[i] == null)
                            NewCustomerXAxis[i] = (element.LabelProperty);
                    }
                }
            }
        });
        var barChartColors = [
            {
                backgroundColor1: '#DA7B38',
                backgroundColor2: '#ecbd9b',
                borderWidth: 0,
            },
            {
                backgroundColor1: '#487E9F',
                backgroundColor2: '#c8d8e2',
                borderWidth: 0
            },
        ];
        this.NewOpenTicketsDueTime = [];
        var DataProvider = [];
        var objectArray = [];
        var maximum = 0;
        if (NewCustomerYAxis.length > 0)
            maximum = NewCustomerYAxis[0].data[0];
        if (maximum == null || maximum === undefined)
            maximum = 0;
        var k = 0;
        NewCustomerYAxis.forEach(function (element) {
            for (var i = 0; i < element.data.length; i++) {
                if (_this.NewOpenTicketsDueTime[i] == null) {
                    _this.NewOpenTicketsDueTime[i] = { data: [], label: null, BindingElement: [], OwnerIds: [], LabelProperty: [], DateTime: [], EmployeeGroupId: [] };
                }
                if (element.data[i] > maximum)
                    maximum = element.data[i];
                _this.NewOpenTicketsDueTime[i].data.push(element.data[i]);
                _this.NewOpenTicketsDueTime[i].BindingElement.push(element.BindingElement[i]);
                _this.NewOpenTicketsDueTime[i].OwnerIds.push(element.OwnerIds[i]);
                _this.NewOpenTicketsDueTime[i].EmployeeGroupId.push(element.EmployeeGroupId[i]);
                _this.NewOpenTicketsDueTime[i].DateTime.push(element.DateTime[i]);
                _this.NewOpenTicketsDueTime[i].label = element.label;
                _this.NewOpenTicketsDueTime[i].LabelProperty.push(element.LabelProperty[i]);
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
            k++;
        });
        var InProgressBookingDashboardFilterd = new Array();
        try {
            if (NewCustomerXAxis.length != 0) {
                maximum += 1;
                while (maximum % 5 != 0) {
                    maximum += 1;
                }
                makeAmBarChart(this.OpenTicketsDueTimeId, Graphs, DataProvider, maximum, null, null, 0, false);
            }
        }
        catch (e) {
        }
    };
    TicketsComponent.prototype.BuildTopTicketsFilters = function () {
        this.TopTicketsComboList = [];
        this.TopTicketsComboList.push(new CodeNameClass_1.CodeNameClass("SEV", "Severity"));
        this.TopTicketsComboList.push(new CodeNameClass_1.CodeNameClass("CRK", "Customer Rank"));
        this.TopTicketsComboList.push(new CodeNameClass_1.CodeNameClass("AWT", "Customer Watch"));
        this.TopTicketsComboList.push(new CodeNameClass_1.CodeNameClass("UPD", "Update Date"));
        var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue(this.filterControlNameSpace, this.filterName_TopTickets);
        if (Tools_1.AppTool.IsNullOrEmpty(defaultFilterCode)) {
            defaultFilterCode = "SEV";
        }
        this.topTicketsComboListSelectedItem = this.TopTicketsComboList.filter(function (d) { return d.Code == defaultFilterCode; })[0];
    };
    ;
    Object.defineProperty(TicketsComponent.prototype, "TopTicketsComboListSelectedItem", {
        get: function () { return this.topTicketsComboListSelectedItem; },
        set: function (value) {
            if (this.topTicketsComboListSelectedItem != value) {
                this.topTicketsComboListSelectedItem = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_TopTickets, (value == null ? null : value.Code));
                this.LoadTopTickets();
            }
        },
        enumerable: true,
        configurable: true
    });
    // New Button 
    TicketsComponent.prototype.RunNewTicketWizard = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(function (response) {
            var windowTitle = "New Ticket";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 700;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewTicketWindowClosed($event); });
            logWindow.Show('./CRMModules/CRMTickets/Components/NewEntity/NewTicketComponent');
        });
    };
    ;
    TicketsComponent.prototype.OnNewTicketWindowClosed = function (arg) {
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            SessionLocator_1.SessionLocator.ClearExternalParams();
        }
        if (arg == 'OK') {
            this.LoadAllScreenData();
        }
    };
    TicketsComponent.prototype.RunTicketListByShipment = function () {
        var _this = this;
        if (this.IsReady) {
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            if (!Tools_1.AppTool.IsNullOrEmpty(this.ShipmentId)) {
                filters.addAdditionalFilter("ShipmentId", this.ShipmentId, null, null, "Equals", false, false, false, "string");
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CompanyId)) {
                filters.addAdditionalFilter("CompanyId", this.CompanyId, null, null, "Equals", false, false, false, "string");
            }
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filters;
            listArgs.QueryCode = "All Tickets";
            listArgs.ObjectTableName = "Ticket";
            listArgs.DisplayTitle = "All Tickets By Shipment";
            listArgs.BackButtonTitle = "Tickets";
            this._entityResourceService.getEntityResourceByTableName("General", 0).subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(function (response) {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run(listArgs);
                        cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.QueryBackClicked(); });
                        _this.CurrentSession.AddMenuReference(cmpRef);
                        SessionLocator_1.SessionLocator.ClearExternalParams();
                    });
                });
            });
        }
    };
    TicketsComponent.prototype.QueryBackClicked = function () {
        SessionLocator_1.SessionLocator.ClearExternalParams();
        this.LoadAllScreenData();
    };
    TicketsComponent.prototype.BuildEmployeeGroupFilterList = function () {
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
    TicketsComponent.prototype.BuildUsersFilters = function (isUpdatingFilter) {
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
                            //this.OwnerId = null;
                            //this.listOfValuesUserId = null;
                            //this.selectedUserFilter = null;
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
                        this.UsersFilterList = [];
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
                                    loadedUsersList = myResponse.Result.sort(function (a, b) { return (a.EnglishName.toLowerCase() === b.EnglishName.toLowerCase()) ? 0 : (a.EnglishName.toLowerCase() < b.EnglishName.toLowerCase()) ? -1 : 1; });
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
                                    _this.GetSelectedOwnerId();
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
    TicketsComponent.prototype.GetSelectedEmployeeGroup = function () {
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
    TicketsComponent.prototype.GetSelectedOwnerId = function () {
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
    Object.defineProperty(TicketsComponent.prototype, "SelectedEmployeeGroupFilter", {
        get: function () {
            return this.selectedEmployeeGroupFilter;
        },
        set: function (newValue) {
            if (this.selectedEmployeeGroupFilter != newValue) {
                this.selectedEmployeeGroupFilter = newValue;
                this.GetSelectedEmployeeGroup();
                LastFilterClass_1.LastFilterClass.UpdateFilter(this.filterControlNameSpace, this.filterName_EmployeeGroup, (newValue == null ? null : newValue.Code));
                this.BuildUsersFilters(false);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketsComponent.prototype, "SelectedUserFilter", {
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
    Object.defineProperty(TicketsComponent.prototype, "ListOfValuesUserId", {
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
    Object.defineProperty(TicketsComponent.prototype, "IsUsersFilterEnabled", {
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
    Object.defineProperty(TicketsComponent.prototype, "IsListOfValuesVisible", {
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
    TicketsComponent.prototype.GetIdsString = function (ids) {
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
    // My Views
    TicketsComponent.prototype.ReloadUsersQuery = function () {
        this.ReloadUserQueries.emit();
    };
    TicketsComponent.prototype.onUserQueriesBackComplete = function (event) {
        this.LoadAllScreenData();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TicketsComponent.prototype, "ReloadUserQueries", void 0);
    TicketsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TicketsComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TicketsComponent);
    return TicketsComponent;
}(BaseComponent_1.BaseComponent));
exports.TicketsComponent = TicketsComponent;
var TopTicketClass = /** @class */ (function () {
    function TopTicketClass(entityList, filter) {
        this.SeverityBrush = "Black";
        this.severityVisibility = false;
        this.rankVisibility = false;
        this.activityWatchVisibility = false;
        this.updateDateVisibility = false;
        this.entityList = entityList;
        this.filter = filter;
        this.setVisibilityByCode();
        this.GetSeverityBrush();
    }
    Object.defineProperty(TopTicketClass.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "Subject", {
        get: function () { return this.entityList.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "CompanyName", {
        get: function () { return this.entityList.CompanyName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "ContactName", {
        get: function () { return this.entityList.ContactName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "SeverityName", {
        get: function () { return this.entityList.SeverityName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "SeverityCode", {
        get: function () { return this.entityList.SeverityCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "FilterLable", {
        get: function () { return this.filter.Name + ": "; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "CreateDate", {
        get: function () { return this.entityList.CreateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "UpdateDate", {
        get: function () { return this.entityList.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "OwnerName", {
        get: function () { return this.entityList.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "ActivityWatch", {
        get: function () { return this.entityList.ActivityWatch; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "RankCode", {
        get: function () { return this.entityList.RankCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "TicketNumber", {
        get: function () { return this.entityList.TicketNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "UpdatedByUserName", {
        get: function () { return this.entityList.UpdatedByUserName; },
        enumerable: true,
        configurable: true
    });
    TopTicketClass.prototype.GetSeverityBrush = function () {
        var result = "Black";
        switch (this.entityList.SeverityCode) {
            case "UI": {
                result = "Red";
                break;
            }
            case "HI": {
                result = "Orange";
                break;
            }
            case "MD": {
                result = "Gray";
                break;
            }
            case "LW": {
                result = "Blue";
                break;
            }
        }
        this.SeverityBrush = result;
    };
    Object.defineProperty(TopTicketClass.prototype, "SeverityVisibility", {
        get: function () { return this.severityVisibility; },
        set: function (value) {
            this.severityVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "RankVisibility", {
        get: function () { return this.rankVisibility; },
        set: function (value) {
            this.rankVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "ActivityWatchVisibility", {
        get: function () { return this.activityWatchVisibility; },
        set: function (value) {
            this.activityWatchVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "UpdateDateVisibility", {
        get: function () { return this.updateDateVisibility; },
        set: function (value) {
            this.updateDateVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "CreateDateVisibility", {
        get: function () {
            var myResult = true;
            if (this.UpdateDateVisibility == true) {
                myResult = false;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    TopTicketClass.prototype.setVisibilityByCode = function () {
        this.SeverityVisibility = false;
        this.RankVisibility = false;
        this.ActivityWatchVisibility = false;
        this.UpdateDateVisibility = false;
        switch (this.filter.Code) {
            case "SEV":
                {
                    this.SeverityVisibility = true;
                    break;
                }
            case "CRK":
                {
                    this.RankVisibility = true;
                    break;
                }
            case "AWT":
                {
                    if (this.ActivityWatch) {
                        this.ActivityWatchVisibility = true;
                    }
                    break;
                }
            case "UPD":
                {
                    this.UpdateDateVisibility = true;
                    break;
                }
        }
    };
    Object.defineProperty(TopTicketClass.prototype, "RankName", {
        get: function () {
            var myResult = null;
            if (this.entityList != null) {
                myResult = this.entityList.RankCode;
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "RankSource1", {
        get: function () {
            var myResult = null;
            // silver to lower
            if (this.entityList != null) {
                var RankCode = this.entityList.RankCode;
                switch (RankCode) {
                    case "1":
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "RankSource2", {
        get: function () {
            var myResult = null;
            if (this.entityList != null) {
                var RankCode = this.entityList.RankCode;
                switch (RankCode) {
                    case "2":
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TopTicketClass.prototype, "RankSource3", {
        get: function () {
            var myResult = null;
            if (this.entityList != null) {
                var RankCode = this.entityList.RankCode;
                switch (RankCode) {
                    case "3": {
                        myResult = "./Images/Icons/StarOrange.png";
                        break;
                    }
                    default: {
                        myResult = "./Images/Icons/StarGray.png";
                        break;
                    }
                }
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    return TopTicketClass;
}());
//# sourceMappingURL=TicketsComponent.js.map