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
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var OpportunityListService_1 = require("../../../../CRM/Services/StandardLists/OpportunityListService");
var OpportunityPM_1 = require("../../../../CRM/EntityPMs/OpportunityPM");
var ActivityListService_1 = require("../../../../CRM/Services/StandardLists/ActivityListService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var OpportunityPMInitService_1 = require("../../../../CRM/EntityPMInitServices/OpportunityPMInitService");
var Args_1 = require("../../../../CRM/Args");
var Args_2 = require("../../../../Infrastructure/Args");
var QuoteListService_1 = require("../../../../Quote/Services/StandardLists/QuoteListService");
var Args_3 = require("../../../../Quote/Args");
var TicketListService_1 = require("../../../../CRM/Services/StandardLists/TicketListService");
var Args_4 = require("../../../../CRM/Args");
var Tools_2 = require("../../../../CRM/Tools");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var Tools_3 = require("../../../../Infrastructure/Tools");
var CustomerSalesTabComponent = /** @class */ (function () {
    function CustomerSalesTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ObjectTableName = "Customer";
        this.OpportunityObsList = [];
        this.ActivityObsList = [];
        this.QuoteObsList = [];
        this.TicketsList = [];
        this.IsTicketTabDim = false;
        this.RegardingEntity = "";
        this.EntityId = "";
        this.EntityDescription = "";
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EnabledNewQuote = false;
        this.EnableNewOpportunity = false;
        this.EnableNewActivity = false;
        this.SessionEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsVisible = false;
        this.noOppData = true;
        this.noActData = true;
        this.noQutData = true;
        this.noTicketData = true;
        // Watch
        this.WatchToolTip = "";
        // Commands 
        this.refresh = "";
        this.EntityPM = entityArgs.EntityPM;
        this.InitializeServices();
        this.EntityId = this.EntityPM.Id;
        this.EntityDescription = this.EntityPM.EnglishName;
        this.RegardingEntity = "Regarding Customer : " + this.EntityPM.Code + " " + this.EntityPM.EnglishName;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            this.IsTicketTabDim = true;
        }
        this.BuildScreenData();
        this.getToolTip();
        this.Listen();
        this.SetUIProperties();
    }
    CustomerSalesTabComponent.prototype.SetUIProperties = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "NEWQUOTE")) {
            this.EnabledNewQuote = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Opportunity", "NEW")) {
            this.EnableNewOpportunity = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Activity", "NEW")) {
            this.EnableNewActivity = true;
        }
    };
    CustomerSalesTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "LoadActivity") {
                    _this.GetActivities();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                }
            });
        }
    };
    CustomerSalesTabComponent.prototype.ngOnDestroy = function () {
        Tools_3.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_3.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_3.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    CustomerSalesTabComponent.prototype.InitializeServices = function () {
        this.OpportunityListService = new OpportunityListService_1.OpportunityListService();
        this.ActivityListService = new ActivityListService_1.ActivityListService();
        this.QuoteListService = new QuoteListService_1.QuoteListService();
        this.TicketListService = new TicketListService_1.TicketListService();
    };
    CustomerSalesTabComponent.prototype.BuildScreenData = function () {
        this.GetOpportunities();
        //this.GetActivities();
        //this.GetQuotes();
        //this.GetTickets();
    };
    //Opportunity
    CustomerSalesTabComponent.prototype.GetOpportunities = function () {
        var _this = this;
        if (this.OpportunityObsList != null) {
            this.OpportunityObsList = [];
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.OpportunityListService.getByFilters(filters).subscribe(function (response) {
            if (!response.HasError) {
                if (response.Result == null) {
                    _this.OpportunityObsList = [];
                }
                else {
                    var tempList = [];
                    var tempList_Open = [];
                    var tempList_Clos = [];
                    var baselist = response.Result;
                    var openList = baselist.filter(function (d) { return !d.IsClosed; });
                    var closList = baselist.filter(function (d) { return d.IsClosed; });
                    baselist.filter(function (d) { return !d.IsClosed; }).forEach(function (item) {
                        tempList_Open.push(new OpportunityData(item, _this));
                    });
                    baselist.filter(function (d) { return d.IsClosed; }).forEach(function (item) {
                        tempList_Clos.push(new OpportunityData(item, _this));
                    });
                    tempList_Open = tempList_Open.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.DisplayDate) === Tools_1.DateTool.GetDateFromDate(b.DisplayDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.DisplayDate) > Tools_1.DateTool.GetDateFromDate(b.DisplayDate)) ? -1 : 1; });
                    tempList_Clos = tempList_Clos.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.DisplayDate) === Tools_1.DateTool.GetDateFromDate(b.DisplayDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.DisplayDate) > Tools_1.DateTool.GetDateFromDate(b.DisplayDate)) ? -1 : 1; });
                    tempList = tempList_Open.concat(tempList_Clos);
                    tempList.forEach(function (item) {
                        if (_this.OpportunityObsList.length < 5) {
                            _this.OpportunityObsList.push(item);
                        }
                    });
                    if (_this.OpportunityObsList.length > 0) {
                        _this.NoOppData = false;
                    }
                }
                _this.GetActivities();
            }
        });
    };
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoOppData", {
        get: function () { return this.noOppData; },
        set: function (value) { this.noOppData = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoOppDataIsEnabled", {
        get: function () { return !this.NoOppData; },
        enumerable: true,
        configurable: true
    });
    //Activity
    CustomerSalesTabComponent.prototype.GetActivities = function () {
        var _this = this;
        if (this.ActivityObsList != null) {
            this.ActivityObsList = [];
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.ActivityListService.getByFilters(filters).subscribe(function (response) {
            if (!response.HasError) {
                if (response.Result == null) {
                    _this.ActivityObsList = [];
                }
                else {
                    var dataResult = response.Result;
                    if (dataResult != null && dataResult.length > 0) {
                        var myTempList = [];
                        dataResult.filter(function (d) { return d.IsOpen == true && d.DueDate != null; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.DueDate) === Tools_1.DateTool.GetDateFromDate(b.DueDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.DueDate) < Tools_1.DateTool.GetDateFromDate(b.DueDate)) ? 1 : -1; }).forEach(function (item) {
                            myTempList.push(item);
                        });
                        dataResult.filter(function (d) { return d.IsOpen == true && d.DueDate == null; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.StartDateTime) === Tools_1.DateTool.GetDateFromDate(b.StartDateTime)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.StartDateTime) < Tools_1.DateTool.GetDateFromDate(b.StartDateTime)) ? 1 : -1; }).forEach(function (item) {
                            myTempList.push(item);
                        });
                        dataResult.filter(function (d) { return d.IsOpen == false; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CompleteDate) === Tools_1.DateTool.GetDateFromDate(b.CompleteDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CompleteDate) < Tools_1.DateTool.GetDateFromDate(b.CompleteDate)) ? 1 : -1; }).forEach(function (item) {
                            myTempList.push(item);
                        });
                        myTempList.forEach(function (item) {
                            if (_this.ActivityObsList.length < 5) {
                                _this.ActivityObsList.push(new ActivityData(item, _this));
                            }
                        });
                        if (_this.ActivityObsList.length > 0) {
                            _this.NoActData = false;
                        }
                    }
                }
                _this.GetQuotes();
            }
        });
    };
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoActData", {
        get: function () { return this.noActData; },
        set: function (value) { this.noActData = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoActDataIsEnabled", {
        get: function () { return !this.NoActData; },
        enumerable: true,
        configurable: true
    });
    // Quote
    CustomerSalesTabComponent.prototype.GetQuotes = function () {
        var _this = this;
        if (this.QuoteObsList != null) {
            this.QuoteObsList = [];
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.SortBy = "OpenDate";
        filters.SortDirection = "Descending";
        filters.addAdditionalFilter("CustomerId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.QuoteListService.getByFilters(filters).subscribe(function (response) {
            if (!response.HasError) {
                if (response.Result == null) {
                    _this.QuoteObsList = [];
                }
                else {
                    var baselist = response.Result;
                    baselist.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.OpenDate) === Tools_1.DateTool.GetDateFromDate(b.OpenDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.OpenDate) > Tools_1.DateTool.GetDateFromDate(b.OpenDate)) ? -1 : 1; }).forEach(function (item) {
                        if (_this.QuoteObsList.length < 5) {
                            _this.QuoteObsList.push(new QuoteData(item, _this));
                        }
                    });
                    if (_this.QuoteObsList.length > 0) {
                        _this.NoQutData = false;
                    }
                }
                _this.GetTickets();
            }
        });
    };
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoQutData", {
        get: function () { return this.noQutData; },
        set: function (value) { this.noQutData = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoQutDataIsEnabled", {
        get: function () { return !this.NoQutData; },
        enumerable: true,
        configurable: true
    });
    // Ticket
    CustomerSalesTabComponent.prototype.GetTickets = function () {
        var _this = this;
        if (this.TicketsList != null) {
            this.TicketsList = [];
        }
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.addAdditionalFilter("CompanyId", this.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        this.TicketListService.getByFilters(filters).subscribe(function (response) {
            if (!response.HasError) {
                if (response.Result == null) {
                    _this.TicketsList = [];
                }
                else {
                    var baselist = response.Result;
                    if (baselist != null && baselist.length > 0) {
                        baselist.sort(function (a, b) { return (Tools_1.DateTool.GetDateFromDate(a.CreateDate) === Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? 0 : (Tools_1.DateTool.GetDateFromDate(a.CreateDate) > Tools_1.DateTool.GetDateFromDate(b.CreateDate)) ? -1 : 1; }).forEach(function (item) {
                            if (_this.TicketsList.length < 5) {
                                _this.TicketsList.push(new TicketData(item, _this));
                            }
                        });
                        if (_this.TicketsList.length > 0) {
                            _this.NoTicketData = false;
                        }
                    }
                    _this.IsVisible = true;
                }
            }
        });
    };
    Object.defineProperty(CustomerSalesTabComponent.prototype, "NoTicketData", {
        get: function () { return this.noTicketData; },
        set: function (value) {
            this.noTicketData = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomerSalesTabComponent.prototype, "IsTicketDataEnabled", {
        get: function () { return !this.NoTicketData; },
        enumerable: true,
        configurable: true
    });
    CustomerSalesTabComponent.prototype.getToolTip = function () {
        if (this.ActivityWatch) {
            this.WatchToolTip = "Disable Activity Watch";
        }
        else {
            this.WatchToolTip = "Enable Activity Watch";
        }
    };
    Object.defineProperty(CustomerSalesTabComponent.prototype, "ActivityWatch", {
        get: function () {
            var d = this.entityArgs.EditComponent.ComponentId;
            return this.EntityPM.ActivityWatch;
        },
        set: function (value) {
            this.EntityPM.ActivityWatch = value;
            this.getToolTip();
        },
        enumerable: true,
        configurable: true
    });
    CustomerSalesTabComponent.prototype.SetActivity = function (value) {
        this.ActivityWatch = value;
    };
    CustomerSalesTabComponent.prototype.Add = function (m) {
        var _this = this;
        var title = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        var objectTableName = "";
        var args = null;
        switch (m) {
            case "OPP":
                {
                    path = "./CRMModules/CRMOpportunity/Components/NewEntity/NewOpportunityComponent";
                    title = "New Opportunity";
                    this.refresh = "opp";
                    var opp = new OpportunityPM_1.OpportunityPM();
                    OpportunityPMInitService_1.OpportunityPMInitService.InitValues(opp, true);
                    opp.CustomerId = this.EntityPM.Id;
                    args = new Args_1.OpportunityArgs();
                    args.Entity = opp;
                    args.IsNew = true;
                    args.IsAddCustomerVisible = false;
                    logWindow.WindowArgs = args;
                    objectTableName = "Opportunity";
                    logWindow.Width = 960;
                    logWindow.Height = 570;
                    break;
                }
            case "QUT":
                {
                    path = "./Quote/Components/NewEntity/NewQuoteComponent";
                    title = "New Quote";
                    this.refresh = "qut";
                    objectTableName = "Quote";
                    //var quotePM: QuotePM = new QuotePM();
                    //QuotePMInitService.InitValues(quotePM, true);
                    //quotePM.CustomerId = this.EntityPM.Id;
                    //quotePM.CustomerName = this.EntityPM.EnglishName;
                    //quotePM.CustomerNote = this.EntityPM.Notes;
                    //quotePM.CustomerRankName = this.EntityPM.RankName;
                    //quotePM.IsCustomerSet = true;
                    //args.Entity = quotePM;
                    args = new Args_3.NewQuoteComponentArgs();
                    args.DefaultCustomerId = this.EntityPM.Id;
                    logWindow.WindowArgs = args;
                    logWindow.Width = 960;
                    logWindow.Height = 570;
                    break;
                }
            case "CAS":
                {
                    break;
                }
            case "CMT":
                {
                    break;
                }
            case "ACT":
                {
                    break;
                }
            case "TKT":
                {
                    path = "./CRMModules/CRMTickets/Components/NewEntity/NewTicketComponent";
                    title = "New Ticket";
                    this.refresh = "tkt";
                    objectTableName = "Ticket";
                    args = new Args_4.NewTicketArgs();
                    args.CompanyId = this.EntityPM.Id;
                    logWindow.WindowArgs = args;
                    logWindow.Width = 850;
                    logWindow.Height = 700;
                    break;
                }
        }
        this._entityResourceService.getEntityResourceByTableName(objectTableName, 0).subscribe(function (response) {
            logWindow.Title = title;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.Refresh();
                }
            });
        });
    };
    CustomerSalesTabComponent.prototype.Refresh = function () {
        if (this.refresh == "act") {
            this.GetActivities();
        }
        else if (this.refresh == "opp") {
            this.GetOpportunities();
        }
        else if (this.refresh == "qut") {
            this.GetQuotes();
        }
        else if (this.refresh == "tkt") {
            this.GetTickets();
        }
    };
    CustomerSalesTabComponent.prototype.ViewEntity = function (tableName, entityId) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: entityId, ObjectTableName: tableName, BackButtonLabel: "Customer" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.BuildScreenData();
            });
        });
    };
    CustomerSalesTabComponent.prototype.ViewAllData = function (m) {
        var _this = this;
        var objectTableName = "";
        var queryCode = "";
        var filterName = "CustomerId";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        switch (m) {
            case "OPP":
                {
                    objectTableName = "Opportunity";
                    queryCode = "All Opportunities";
                    break;
                }
            case "ACT":
                {
                    objectTableName = "Activity";
                    queryCode = "All Activities";
                    break;
                }
            case "QUT":
                {
                    objectTableName = "Quote";
                    queryCode = "All Quotes";
                    break;
                }
            case "TKT":
                {
                    objectTableName = "Ticket";
                    queryCode = "All Tickets";
                    filterName = "CompanyId";
                    break;
                }
        }
        filterAgrs.addAdditionalFilter(filterName, this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_2.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.DisplayTitle = listArgs.QueryCode;
        listArgs.BackButtonTitle = "Back";
        this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, SessionLocator_1.SessionLocator.Tenant).subscribe(function (response) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
            });
        });
    };
    CustomerSalesTabComponent.prototype.NewEntity = function (code) {
        var _this = this;
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        switch (code) {
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
        this.refresh = "act";
        var windowArgs = new Args_1.ActivityInputArgs();
        windowArgs.TypeCode = code;
        windowArgs.IsAddCustomerAllowed = false;
        windowArgs.CustomerId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.Refresh();
                }
            });
        });
    };
    CustomerSalesTabComponent.prototype.AddEmailActivity = function () {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Customer", "CUST", this.EntityPM.Id, "", "", "", "", "", null, "LoadActivity", this.EntityPM, true, "CEMO");
            this.EmailSender.ShowFullSendControll();
        }
    };
    CustomerSalesTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './CustomerSalesTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], CustomerSalesTabComponent);
    return CustomerSalesTabComponent;
}());
exports.CustomerSalesTabComponent = CustomerSalesTabComponent;
var OpportunityData = /** @class */ (function () {
    function OpportunityData(item, father) {
        this.father = father;
        this.entityPM = item;
        this.GetListBoxBackground();
        this.ComputeDisplayDate();
    }
    Object.defineProperty(OpportunityData.prototype, "Id", {
        get: function () { return this.entityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "RatingCode", {
        get: function () { return this.entityPM.RatingCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "RatingName", {
        get: function () { return this.entityPM.RatingName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "Topic", {
        get: function () { return this.entityPM.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "StageName", {
        get: function () { return this.entityPM.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "NumberOfShipments", {
        get: function () { return this.entityPM.NumberOfShipments; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "IsClosed", {
        get: function () { return this.entityPM.IsClosed; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityData.prototype, "UpdateDate", {
        get: function () { return this.entityPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    OpportunityData.prototype.ComputeDisplayDate = function () {
        var date = this.entityPM.LastStageDate;
        if (this.entityPM.IsClosed) {
            date = this.entityPM.ActualClosingDate;
        }
        this.DisplayDate = date;
    };
    OpportunityData.prototype.GetListBoxBackground = function () {
        var result = "rgb(255, 255, 255)";
        if (this.IsClosed) {
            result = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    };
    return OpportunityData;
}());
exports.OpportunityData = OpportunityData;
var ActivityData = /** @class */ (function () {
    function ActivityData(item, father) {
        this.father = father;
        this.entityPM = item;
        this.GetDueDateForeground();
        this.GetListBoxBackground();
        this.GetModifiedOrCompleted();
        this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(this.entityPM.ActivityTypePathCode);
    }
    Object.defineProperty(ActivityData.prototype, "Id", {
        get: function () { return this.entityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "Subject", {
        get: function () { return this.entityPM.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "DueDate", {
        get: function () { return this.entityPM.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "ActivityTypePathCode", {
        get: function () { return this.entityPM.ActivityTypePathCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "ActivityTypeName", {
        get: function () { return this.entityPM.ActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "IsOpen", {
        get: function () { return this.entityPM.IsOpen; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "UpdatedByUserName", {
        get: function () { return this.entityPM.UpdatedByUserName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "OwnerName", {
        get: function () { return this.entityPM.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityData.prototype, "UpdateDate", {
        get: function () { return this.entityPM.UpdateDate; },
        enumerable: true,
        configurable: true
    });
    ActivityData.prototype.GetDueDateForeground = function () {
        var result = "rgb(40, 46, 48)";
        var now = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (this.DueDate != null && (this.DueDate.valueOf() < now.valueOf())) {
            result = "red";
        }
        this.DueDateForeground = result;
    };
    ActivityData.prototype.GetListBoxBackground = function () {
        var result = "rgb(255,255,255)";
        if (!this.IsOpen) {
            result = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    };
    ActivityData.prototype.GetModifiedOrCompleted = function () {
        var myResult = "Modified by";
        if (this.entityPM.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }
        else if (this.entityPM.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }
        else {
            switch (this.entityPM.ActivityStatusCode) {
                case "C":
                    {
                        myResult = "Completed by";
                        break;
                    }
                case "X":
                    {
                        myResult = "Closed by";
                        break;
                    }
                default:
                    {
                        myResult = "Modified by";
                        break;
                    }
            }
        }
        this.ModifiedOrCompleted = myResult;
    };
    return ActivityData;
}());
exports.ActivityData = ActivityData;
var QuoteData = /** @class */ (function () {
    function QuoteData(item, father) {
        this.father = father;
        this.IsClosed = false;
        this.entityPM = item;
        this.GetListBoxBackground();
        this.GetIsClosed();
    }
    Object.defineProperty(QuoteData.prototype, "Id", {
        get: function () { return this.entityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "RatingCode", {
        get: function () { return this.entityPM.RatingCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "RatingName", {
        get: function () { return this.entityPM.RatingName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "Subject", {
        get: function () { return this.entityPM.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "FromCountryCode", {
        get: function () { return this.entityPM.FromCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "FromPortCountry", {
        get: function () { return this.entityPM.FromPortCountry; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "ToCountryCode", {
        get: function () { return this.entityPM.ToCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "ToPortCountry", {
        get: function () { return this.entityPM.ToPortCountry; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "OpenDate", {
        get: function () { return this.entityPM.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "DirectionId", {
        get: function () { return this.entityPM.DirectionId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "DirectionName", {
        get: function () { return this.entityPM.DirectionName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "TransportModeId", {
        get: function () { return this.entityPM.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "TransportModeName", {
        get: function () { return this.entityPM.TransportModeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteData.prototype, "StageName", {
        get: function () { return this.entityPM.StageName; },
        enumerable: true,
        configurable: true
    });
    QuoteData.prototype.GetIsClosed = function () {
        var isClosed = false;
        if (this.entityPM.StageName == "Declined") {
            isClosed = true;
        }
        else if (this.entityPM != null && this.entityPM.ExpirationDate != null && (this.entityPM.ExpirationDate.valueOf() < Tools_1.DateTool.GetCurrentDateAsUtc().valueOf())) {
            if (this.entityPM.StageName != "Accepted") {
                isClosed = true;
            }
        }
        this.IsClosed = isClosed;
    };
    QuoteData.prototype.GetListBoxBackground = function () {
        var result = "rgb(255,255,255)";
        if (this.IsClosed) {
            result = "rgba(0,0,0,0.1)";
        }
        this.ListBoxBackground = result;
    };
    return QuoteData;
}());
exports.QuoteData = QuoteData;
var TicketData = /** @class */ (function () {
    function TicketData(item, father) {
        this.father = father;
        this.entityPM = item;
    }
    Object.defineProperty(TicketData.prototype, "Id", {
        get: function () { return this.entityPM.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "IsClosed", {
        get: function () { return this.entityPM.IsClosed; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "SeverityName", {
        get: function () { return this.entityPM.SeverityName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "SeverityCode", {
        get: function () { return this.entityPM.SeverityCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "Subject", {
        get: function () { return this.entityPM.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "TypeName", {
        get: function () { return this.entityPM.TypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "CompanyName", {
        get: function () { return this.entityPM.CompanyName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "StageName", {
        get: function () { return this.entityPM.StageName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "ContactName", {
        get: function () { return this.entityPM.ContactName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "OwnerName", {
        get: function () { return this.entityPM.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TicketData.prototype, "TicketNumber", {
        get: function () { return this.entityPM.TicketNumber; },
        enumerable: true,
        configurable: true
    });
    return TicketData;
}());
exports.TicketData = TicketData;
//# sourceMappingURL=CustomerSalesTabComponent.js.map