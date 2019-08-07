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
var Tools_1 = require("../../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var Args_1 = require("../../../../Quote/Args");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var AdditionalServiceListService_1 = require("../../../../Common/Services/StandardLists/AdditionalServiceListService");
var OpportunityAdditionalServicePM_1 = require("../../../../CRM/EntityPMs/OpportunityAdditionalServicePM");
var CompetitorListService_1 = require("../../../../Common/Services/StandardLists/CompetitorListService");
var OpportunityCompetitorPM_1 = require("../../../../CRM/EntityPMs/OpportunityCompetitorPM");
var Cloner_1 = require("../../../../Infrastructure/Utilities/Cloner");
var DateTimePipe_1 = require("../../../../Controls/Pipes/DateTimePipe");
var CRMDomainService_1 = require("../../../../CRM/Services/CRMDomainService");
var GeneralEmailSender_1 = require("../../../../Infrastructure/Helpers/GeneralEmailSender");
var Tools_2 = require("../../../../CRM/Tools");
var Args_2 = require("../../../../CRM/Args");
var StageListService_1 = require("../../../../CRM/Services/StandardLists/StageListService");
var RatingListService_1 = require("../../../../CRM/Services/StandardLists/RatingListService");
var Args_3 = require("../../../../Infrastructure/Args");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var OpportunityOverviewTabComponent = /** @class */ (function (_super) {
    __extends(OpportunityOverviewTabComponent, _super);
    function OpportunityOverviewTabComponent(entityArgs, _entityResourceService) {
        var _this = _super.call(this) || this;
        _this._entityResourceService = _entityResourceService;
        _this.ObjectTableName = "Opportunity";
        _this.DataContext = _this;
        _this.QuotesObslist = [];
        _this.RegardingEntity = "";
        _this.EntityId = "";
        _this.EntityDescription = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsEditingEnabled = true;
        _this.IsEditingEnabledWithoutCancel = true;
        _this.SearchTextAdditionalServiceModeDropButtonId = "SearchTextAdditionalServiceModeDropButtonId";
        _this.SearchTextAdditionalServiceId = "SearchTextAdditionalServiceId";
        _this.SearchTextCompetitorsDropButtonId = "SearchTextCompetitorsDropButtonId";
        _this.SearchTextCompetitorsId = "SearchTextCompetitorsId";
        _this.CompetitorToggleButtonList = [];
        _this.AllCompetitors = [];
        _this.Competitors = [];
        _this.searchTextCompetitor = null;
        _this.ProductsToggleButtonList = [];
        _this.ToggleButtonListService = [];
        _this.ToggleButtonList = [];
        _this.searchTextAdditionalService = null;
        _this.SavingMethodCode = "";
        _this.noServicesVisibility = false;
        _this.servicesVisibility = false;
        _this.noCompetitorVisibility = false;
        _this.Services = [];
        _this.ActivitiesContent = "";
        _this.ActivitiesList = [];
        _this.IsAddActivityEnabled = true;
        _this.EntityPM = entityArgs.EntityPM;
        _this.EntityId = _this.EntityPM.Id;
        _this.EntityDescription = _this.EntityPM.Subject;
        _this.RegardingEntity = "Regarding Opportunity : " + _this.EntityPM.Subject;
        _this.Listen();
        _this.SetUIProperties();
        _this.LoadActivities();
        return _this;
    }
    OpportunityOverviewTabComponent.prototype.SetUIProperties = function () {
        var fieldsIsEnabled = true;
        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldsIsEnabled = false;
        }
        this.IsEditingEnabled = fieldsIsEnabled;
        this.IsEditingEnabledWithoutCancel = true;
        this.UIProperties.SetEnabled("Subject", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("CustomerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ContactId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("EstimatedClosingDate", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("StageId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Probability", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("RatingCode", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("OwnerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("CurrencyId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Description", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("StageDueDate", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadUserId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadSourceId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("AgentId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadPartnerId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("LeadDescription", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ForeignClientId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("ClosingReasonId", "Opportunity", fieldsIsEnabled);
        this.UIProperties.SetEnabled("Notes", "Opportunity", true);
        this.SetUIProperties_TotalsOfProducts();
        this.SearchTextCompetitorsDropButtonId += "2" + this.CurrentSession.GetNewId("SearchTextCompetitorsDropButtonId_2");
        this.SearchTextCompetitorsId += "2" + this.CurrentSession.GetNewId("SearchTextCompetitorsId_2");
        this.SearchTextAdditionalServiceModeDropButtonId += "2" + this.CurrentSession.GetNewId("SearchTextAdditionalServiceModeDropButtonId_2");
        this.SearchTextAdditionalServiceId += "2" + this.CurrentSession.GetNewId("SearchTextAdditionalServiceId_2");
    };
    OpportunityOverviewTabComponent.prototype.SetUIProperties_TotalsOfProducts = function () {
        var fieldsIsEnabled = true;
        if (this.EntityPM.IsClosed || this.EntityPM.IsCancelled) {
            fieldsIsEnabled = false;
        }
        if (fieldsIsEnabled) {
            if (this.EntityPM.OpportunityProducts.length > 0) {
                fieldsIsEnabled = false;
            }
        }
        this.UIProperties.SetEnabled("NumberOfShipments", "Opportunity", fieldsIsEnabled);
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "SearchTextCompetitor", {
        get: function () { return this.searchTextCompetitor; },
        set: function (newValue) {
            this.searchTextCompetitor = newValue;
            this.BuildCompetitorToggleButtonList();
        },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.BuildCompetitorToggleButtonList = function () {
        var _this = this;
        this.CompetitorToggleButtonList = [];
        var data = this.AllCompetitors;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchTextCompetitor))
            data = this.AllCompetitors.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchTextCompetitor.toLowerCase()) > -1; });
        data.forEach(function (item) {
            _this.CompetitorToggleButtonList.push(new CompetitorItemClass(item, _this.EntityPM, _this));
        });
    };
    OpportunityOverviewTabComponent.prototype.BuildCompetitorsObsList = function () {
        var _this = this;
        this.Competitors = [];
        this.EntityPM.OpportunityCompetitors.forEach(function (item) {
            _this.Competitors.push(new CompetitorViewModelData(item, _this));
        });
        if (this.Competitors.length == 0)
            this.NoCompetitorVisibility = true;
        else
            this.NoCompetitorVisibility = false;
    };
    OpportunityOverviewTabComponent.prototype.DeleteItemCompetitorList = function (Item) {
        if (this.EntityPM.OpportunityCompetitors.filter(function (p) { return p.CompetitorId == Item.CompetitorId; })[0] != null)
            this.EntityPM.RemoveOpportunityCompetitor(this.EntityPM.OpportunityCompetitors.filter(function (d) { return d.CompetitorId == Item.CompetitorId; })[0]);
        this.BuildCompetitorToggleButtonList();
        this.BuildCompetitorsObsList();
    };
    OpportunityOverviewTabComponent.prototype.Clone = function (EntityPM) {
        this.myCloner = new Cloner_1.Cloner(EntityPM);
        this.myCloner.AddField('Notes');
        this.myCloner.AddEntity(EntityPM);
        this.myCloner.AddEntity(this.EntityPM);
    };
    OpportunityOverviewTabComponent.prototype.RejectChanges = function () {
        this.myCloner.RejectChanges();
    };
    OpportunityOverviewTabComponent.prototype.EditCompetitor = function (Item) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: Item.CompetitorId, ObjectTableName: 'Competitor', BackButtonLabel: "CRM Details" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.getCompetitorList();
            });
        });
    };
    OpportunityOverviewTabComponent.prototype.EditItemServiceObsList = function (item) {
        var _this = this;
        var editWindow = new LogitudeWindow_1.LogitudeWindow();
        editWindow.Title = "Edit " + item.AdditionalServiceName + " Additional Service";
        editWindow.Width = 500;
        editWindow.Height = 350;
        editWindow.WindowArgs = item;
        editWindow.ComponentLoaded.subscribe(function (p) {
            p.ShowRadioButtons = false;
        });
        this.Clone(item);
        editWindow.WindowClosed.subscribe(function (result) {
            if (result == "Cancel") {
                _this.RejectChanges();
            }
            else {
            }
        });
        var entityResource = new EntityResourceService_1.EntityResourceService();
        entityResource.getEntityResourceByTableName("CustomerAdditionalService", 0).subscribe(function (p) {
            editWindow.Show('./CommonModules/CommonCustomer/Components/EditTabs/EditCustomerAdditionalServiceComponent');
        });
    };
    OpportunityOverviewTabComponent.prototype.ClearPlaceHolderCompetitor = function () {
        var temp = document.getElementById(this.SearchTextCompetitorsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    OpportunityOverviewTabComponent.prototype.OnDeleteValueCompetitor = function () {
        var temp = document.getElementById(this.SearchTextCompetitorsId);
        temp.value = null;
        this.SearchTextCompetitor = null;
        temp.focus();
    };
    OpportunityOverviewTabComponent.prototype.OnDeleteValueAddtionalService = function () {
        var temp = document.getElementById(this.SearchTextAdditionalServiceId);
        temp.value = null;
        this.SearchTextAdditionalService = null;
        temp.focus();
    };
    OpportunityOverviewTabComponent.prototype.FillPlaceHoldeCompetitor = function () {
        if (!this.SearchTextCompetitor) {
            var temp = document.getElementById(this.SearchTextCompetitorsId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextCompetitorsDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    OpportunityOverviewTabComponent.prototype.setToggleButtonMenuAdditionalServicesTemp = function () {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    OpportunityOverviewTabComponent.prototype.setToggleButtonMenuAdditionalServices = function () {
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    OpportunityOverviewTabComponent.prototype.LoadQuotesList = function () {
        var _this = this;
        this.QuotesObslist = [];
        var quoteDomainService = new QuoteDomainService_1.QuoteDomainService();
        quoteDomainService.GetQuotesByOpportunityId(this.EntityPM.Id).subscribe(function (result) {
            var quoteList = result.Result;
            quoteList.sort(function (a, b) {
                return (Tools_1.DateTool.GetDateParts(a.OpenDate).DateObject === Tools_1.DateTool.GetDateParts(b.OpenDate).DateObject) ? 0 : (Tools_1.DateTool.GetDateParts(a.OpenDate).DateObject < Tools_1.DateTool.GetDateParts(b.OpenDate).DateObject) ? -1 : 1;
            });
            quoteList.reverse();
            quoteList.forEach(function (item) {
                _this.QuotesObslist.push(new QuoteObslistItemClass(item));
            });
        });
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "NoQuotesVisibility", {
        get: function () { if (this.QuotesObslist.length > 0)
            return true; return false; },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    // Your work after you get PM
                    if (_this.SavingMethodCode == "NewQuote")
                        _this.OpenNewQuote();
                    _this.SetUIProperties();
                    _this.SavingMethodCode = "";
                    _this.CurrentSession.FireEvent("SocialPostsRefresh");
                }
            });
            this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.SetUIProperties();
                    // Your work after you get PM
                }
            });
            this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "LoadActivity") {
                    _this.LoadActivities();
                }
            });
        }
    };
    OpportunityOverviewTabComponent.prototype.getAdditionalSerivceList = function () {
        var _this = this;
        var AddtionalService = new AdditionalServiceListService_1.AdditionalServiceListService();
        AddtionalService.getAllFromCache().subscribe(function (result) {
            _this.ToggleButtonListService = [];
            _this.ToggleButtonListService = result.Result.filter(function (s) { return !s.InActive; });
            _this.ToggleButtonListService.sort(function (a, b) { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1; });
            _this.BuildToggleButtonList();
            _this.BuildObsList();
        });
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "SearchTextAdditionalService", {
        get: function () { return this.searchTextAdditionalService; },
        set: function (newValue) {
            this.searchTextAdditionalService = newValue;
            this.BuildToggleButtonList();
        },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.BuildToggleButtonList = function () {
        var _this = this;
        this.ToggleButtonList = [];
        var data = this.ToggleButtonListService;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchTextAdditionalService))
            data = this.ToggleButtonListService.filter(function (f) { return f.Name.toLowerCase().indexOf(_this.SearchTextAdditionalService.toLowerCase()) > -1; });
        data.forEach(function (item) {
            _this.ToggleButtonList.push(new ServiceItemClass(item, _this.EntityPM, _this));
        });
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "IsAddRemoveEnabled", {
        get: function () {
            return true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "RatingCode", {
        get: function () { return this.EntityPM.RatingCode; },
        set: function (value) {
            var _this = this;
            if (this.EntityPM.RatingCode != value) {
                this.EntityPM.RatingCode = value;
                var ratingListService = new RatingListService_1.RatingListService();
                ratingListService.getAllFromCache().subscribe(function (result) {
                    var ratingList = result.Result.filter(function (p) { return p.Code == value; })[0];
                    if (ratingList != null)
                        _this.EntityPM.RatingName = ratingList.Name;
                    else
                        _this.EntityPM.RatingName = null;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "StageId", {
        get: function () {
            return this.EntityPM.StageId;
        },
        set: function (value) {
            if (this.EntityPM.StageId != value) {
                this.EntityPM.StageId = value;
                this.OnStageChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.NewQuote = function () {
        this.SavingMethodCode = "NewQuote";
        this.CurrentSession.CurrentEditComponent.SaveChanges();
    };
    OpportunityOverviewTabComponent.prototype.EditQuote = function (item) {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: item.entityId, ObjectTableName: 'Quote', BackButtonLabel: "Quotes" });
            cmpRef.instance.BackCompleted.subscribe(function ($event) {
                _this.LoadQuotesList();
            });
        });
    };
    OpportunityOverviewTabComponent.prototype.ClearPlaceHolderAdditionalService = function () {
        var temp = document.getElementById(this.SearchTextAdditionalServiceId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    OpportunityOverviewTabComponent.prototype.FillPlaceHolderAdditionalService = function () {
        if (!this.SearchTextAdditionalService) {
            var temp = document.getElementById(this.SearchTextAdditionalServiceId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        var ToggleBTN = document.getElementById(this.SearchTextAdditionalServiceModeDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    OpportunityOverviewTabComponent.prototype.OpenNewQuote = function () {
        var _this = this;
        var args = new Args_1.NewQuoteComponentArgs();
        args.DefaultCustomerId = this.EntityPM.CustomerId;
        args.OpportunityId = this.EntityPM.Id;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = args;
        logWindow.Title = "Create New Quote";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(function (response) {
            logWindow.Show('./Quote/Components/NewEntity/NewQuoteComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadQuotesList();
                }
            });
        });
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "Notes", {
        get: function () { return this.EntityPM.Notes; },
        set: function (value) { if (this.EntityPM.Notes != value)
            this.EntityPM.Notes = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "StageDueDate", {
        get: function () { return this.EntityPM.StageDueDate; },
        set: function (value) {
            if (this.EntityPM.StageDueDate != value) {
                this.EntityPM.StageDueDate = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "Probability", {
        get: function () {
            return this.EntityPM.Probability;
        },
        set: function (value) {
            if (this.EntityPM.Probability != value) {
                this.EntityPM.Probability = value;
                this.ComputeValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "NumberOfShipments", {
        get: function () {
            return this.EntityPM.NumberOfShipments;
        },
        set: function (value) {
            if (this.EntityPM.NumberOfShipments != value) {
                this.EntityPM.NumberOfShipments = value;
                this.ComputeValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "IsClosed", {
        get: function () { return this.EntityPM.IsClosed; },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.ConnectQuotesMethod = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 960;
        logWindow.Height = 570;
        logWindow.WindowArgs = this.EntityPM;
        logWindow.Title = "Choose Quotes";
        this._entityResourceService.getEntityResourceByTableName("Quote", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMOpportunity/Components/EditTabs/QuotesWindowComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s == "ok") {
                    _this.LoadQuotesList();
                }
            });
        });
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "ClosingBackground", {
        get: function () {
            var myResult = "#FFFFFFFF";
            if (this.EntityPM.IsClosed) {
                myResult = "#AA009161";
            }
            if (this.EntityPM.IsClosedLost) {
                myResult = "#AAFFB23C";
            }
            return myResult;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "ClosingDescription", {
        get: function () { return this.EntityPM.ClosingDescription; },
        set: function (value) { if (this.EntityPM.ClosingDescription != value)
            this.EntityPM.ClosingDescription = value; },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.ComputeValue = function () {
        var field1 = this.Probability == null ? 0 : parseFloat(this.Probability + "");
        var field2 = this.NumberOfShipments == null ? 0 : parseFloat(this.NumberOfShipments + "");
        var myValue = field1 * field2 / 100;
        this.ValueField = myValue;
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "ValueField", {
        get: function () { return this.EntityPM == null ? null : this.EntityPM.ValueField; },
        set: function (value) {
            if (this.EntityPM.ValueField != value) {
                this.EntityPM.ValueField = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.OnStageChanged = function () {
        var _this = this;
        var myStageName = null;
        var myProbability = null;
        var myStageDueDate = Tools_1.DateTool.GetDateParts(this.EntityPM.LastStageDate).DateObject;
        var myStageAge = Tools_1.DateTool.GetDateParts(this.EntityPM.LastStageDate).DateObject;
        var stageService = new StageListService_1.StageListService();
        stageService.getAllFromCache().subscribe(function (result) {
            var myStage = result.Result.filter(function (d) { return d.Id == _this.StageId && d.Tenant == SessionLocator_1.SessionLocator.Tenant; })[0];
            if (myStage != null) {
                var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
                myStageName = myStage.Name;
                myProbability = myStage.Probability;
                var fieldsIsEnabled = true;
                if (_this.EntityPM.IsClosed || _this.EntityPM.IsCancelled) {
                    fieldsIsEnabled = false;
                }
                if (fieldsIsEnabled) {
                    if (myStage.MaxDays != null) {
                        myStageDueDate.setDate(todayDateTime.getDate() + myStage.MaxDays);
                    }
                }
                myStageAge = todayDateTime;
            }
            _this.EntityPM.StageName = myStageName;
            _this.EntityPM.StageDueDate = myStageDueDate;
            _this.EntityPM.Probability = myProbability;
            _this.ComputeValue();
        });
    };
    OpportunityOverviewTabComponent.prototype.ngOnInit = function () {
        this.LoadQuotesList();
        this.getAdditionalSerivceList();
        this.getCompetitorList();
    };
    OpportunityOverviewTabComponent.prototype.getCompetitorList = function () {
        var _this = this;
        var competitorListService = new CompetitorListService_1.CompetitorListService();
        competitorListService.getAll().subscribe(function (result) {
            _this.AllCompetitors = result.Result;
            _this.BuildCompetitorToggleButtonList();
            _this.BuildCompetitorsObsList();
        });
    };
    OpportunityOverviewTabComponent.prototype.ExistingItemNotes = function (Item) {
        return Tools_1.AppTool.IsNullOrEmpty(Item.Notes);
    };
    OpportunityOverviewTabComponent.prototype.DeleteItemServiceObsList = function (Item) {
        if (this.EntityPM.OpportunityAdditionalServices.filter(function (p) { return p.AdditionalServiceId == Item.Id; })[0] != null)
            this.EntityPM.RemoveOpportunityAdditionalService(this.EntityPM.OpportunityAdditionalServices.filter(function (d) { return d.AdditionalServiceId == Item.Id; })[0]);
        this.BuildToggleButtonList();
        this.BuildObsList();
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "NoServicesVisibility", {
        get: function () { return this.noServicesVisibility; },
        set: function (value) { this.noServicesVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "ServicesVisibility", {
        get: function () { return this.servicesVisibility; },
        set: function (value) { this.servicesVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "NoCompetitorVisibility", {
        get: function () { return this.noCompetitorVisibility; },
        set: function (value) { this.noCompetitorVisibility = value; },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.BuildObsList = function () {
        var _this = this;
        this.Services = [];
        this.EntityPM.OpportunityAdditionalServices.forEach(function (item) {
            _this.Services.push(new ServiceViewModelData(item, _this));
        });
        this.NoServicesVisibility = this.Services.length == 0 ? true : false;
        this.ServicesVisibility = this.Services.length == 0 ? false : true;
        // this.ActivityWatch = this.Services.Count == 0 ? false : true;         
    };
    Object.defineProperty(OpportunityOverviewTabComponent.prototype, "ViewAllActivitiesIsEnabled", {
        get: function () { return this.viewAllActivitiesIsEnabled; },
        set: function (value) {
            this.viewAllActivitiesIsEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    OpportunityOverviewTabComponent.prototype.GetActivitiesContent = function () {
        var count = this.ActivitiesList.length;
        this.ViewAllActivitiesIsEnabled = count > 0 ? true : false;
        this.ActivitiesContent = "Activities (" + count + ")";
    };
    OpportunityOverviewTabComponent.prototype.AddActivity = function (code) {
        var _this = this;
        var windowTitle = "";
        var windowTitleIcon = "";
        var path = "";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
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
        if (code.toUpperCase() == "CL") {
            windowArgs.CallWithId = this.EntityPM.ContactId;
            windowArgs.IsOpen = false;
            windowArgs.IsMarkedCompleted = true;
        }
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.OpportunityId = this.EntityPM.Id;
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadActivities();
                }
            });
        });
    };
    OpportunityOverviewTabComponent.prototype.AddEmail = function () {
        if (!this.EmailSender || (this.EmailSender && !this.EmailSender.LoadingSendingComponent)) {
            this.EmailSender = new GeneralEmailSender_1.GeneralEmailSender("Opportunity", "OPPO", this.EntityPM.Id, "", "", "", "", "", null, "LoadActivity", this.EntityPM, true, "OEMO");
            this.EmailSender.ShowFullSendControll();
        }
    };
    OpportunityOverviewTabComponent.prototype.LoadActivities = function () {
        var _this = this;
        this.ActivitiesList = [];
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetActivitiesByOpportunityId(this.EntityPM.Id).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var dataResult = myResponse.Result;
                if (dataResult.length > 0) {
                    dataResult.filter(function (d) { return d.IsOpen; }).sort(function (a, b) {
                        return (Tools_1.DateTool.GetDateParts(a.SortingDate).DateObject === Tools_1.DateTool.GetDateParts(b.SortingDate).DateObject) ? 0 : (Tools_1.DateTool.GetDateParts(a.SortingDate).DateObject > Tools_1.DateTool.GetDateParts(b.SortingDate).DateObject) ? -1 : 1;
                    }).forEach(function (item) {
                        _this.ActivitiesList.push(new ActivityItemClass(item, _this));
                    });
                    dataResult.filter(function (d) { return !d.IsOpen; }).sort(function (a, b) { return (Tools_1.DateTool.GetDateParts(a.SortingDate).DateObject === Tools_1.DateTool.GetDateParts(b.SortingDate).DateObject) ? 0 : (Tools_1.DateTool.GetDateParts(a.SortingDate).DateObject > Tools_1.DateTool.GetDateParts(b.SortingDate).DateObject) ? -1 : 1; }).forEach(function (item) {
                        _this.ActivitiesList.push(new ActivityItemClass(item, _this));
                    });
                }
                _this.GetActivitiesContent();
            }
        });
    };
    OpportunityOverviewTabComponent.prototype.ViewEntity = function (entity) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: "Activity", BackButtonLabel: "Opportunities" });
                    cmpRef.instance.BackCompleted.subscribe(function ($event) {
                        _this.LoadActivities();
                    });
                });
            });
        }
    };
    OpportunityOverviewTabComponent.prototype.Updated = function (arg) {
        if (arg) {
            this.LoadActivities();
        }
    };
    OpportunityOverviewTabComponent.prototype.AddTaskClicked = function () {
        var _this = this;
        var windowTitle = "New Task";
        var windowTitleIcon = "./Images/Activities/TS.png";
        var windowArgs = new Args_2.ActivityInputArgs();
        windowArgs.TypeCode = "TS";
        windowArgs.IsAddCustomerAllowed = true;
        windowArgs.CustomerId = this.EntityPM.CustomerId;
        windowArgs.OpportunityId = this.EntityPM.Id;
        windowArgs.Subject = "due date reminder";
        windowArgs.DueDate = this.StageDueDate;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.TitleIcon = windowTitleIcon;
        logWindow.WindowArgs = windowArgs;
        this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
            logWindow.Show('./CRMModules/CRMActivity/Components/NewEntity/NewActivity/NewActivityComponent');
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadActivities();
                }
            });
        });
    };
    OpportunityOverviewTabComponent.prototype.ViewAllData = function () {
        var _this = this;
        var objectTableName = "Activity";
        var queryCode = "All Activities";
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        filterAgrs.addAdditionalFilter("OpportunityId", this.EntityPM.Id, null, null, "Equals", false, false, false, "String");
        var listArgs = new Args_3.ListComponentArgs();
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
    OpportunityOverviewTabComponent = __decorate([
        core_1.Component({
            selector: 'OpportunityOverviewTabComponent',
            moduleId: module.id,
            templateUrl: './OpportunityOverviewTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], OpportunityOverviewTabComponent);
    return OpportunityOverviewTabComponent;
}(BaseComponent_1.BaseComponent));
exports.OpportunityOverviewTabComponent = OpportunityOverviewTabComponent;
var ServiceViewModelData = /** @class */ (function () {
    function ServiceViewModelData(item, trigger) {
        this.entityPM = item;
        this.trigger = trigger;
    }
    Object.defineProperty(ServiceViewModelData.prototype, "AdditionalServiceName", {
        get: function () { return this.entityPM.EnglishName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "Notes", {
        get: function () {
            return this.entityPM.Notes;
        },
        set: function (value) {
            if (this.entityPM.Notes != value) {
                this.entityPM.Notes = value;
                ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Customer", "Notes update");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "BrushedNotesIconVisibility", {
        get: function () {
            if (this.entityPM != null && !(this.entityPM.Notes == null || this.entityPM.Notes == ""))
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "DefaultNotesIconVisibility", {
        get: function () {
            if (this.entityPM != null && (this.entityPM.Notes == null || this.entityPM.Notes == ""))
                return true;
            return false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceViewModelData.prototype, "Id", {
        get: function () { return this.entityPM.AdditionalServiceId; },
        enumerable: true,
        configurable: true
    });
    return ServiceViewModelData;
}());
exports.ServiceViewModelData = ServiceViewModelData;
var QuoteObslistItemClass = /** @class */ (function () {
    function QuoteObslistItemClass(item) {
        this.entityList = item;
        this.entityId = item.Id;
    }
    Object.defineProperty(QuoteObslistItemClass.prototype, "RatingCode", {
        get: function () { return this.entityList.RatingCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "RatingName", {
        get: function () { return this.entityList.RatingName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "Subject", {
        get: function () { return this.entityList.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "FromCountryCode", {
        get: function () { return this.entityList.FromCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "ToCountryCode", {
        get: function () { return this.entityList.ToCountryCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "OpenDate", {
        get: function () { return this.entityList.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "QuoteNumber", {
        get: function () { return this.entityList.QuoteNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "DirectionId", {
        get: function () { return this.entityList.DirectionId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "DirectionName", {
        get: function () { return this.entityList.DirectionName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "TransportModeId", {
        get: function () { return this.entityList.TransportModeId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteObslistItemClass.prototype, "TransportModeName", {
        get: function () { return this.entityList.TransportModeName; },
        enumerable: true,
        configurable: true
    });
    return QuoteObslistItemClass;
}());
exports.QuoteObslistItemClass = QuoteObslistItemClass;
var ServiceItemClass = /** @class */ (function () {
    function ServiceItemClass(itemList, itemPM, Parent) {
        var _this = this;
        this.Parent = Parent;
        this.TenantPM = SessionLocator_1.SessionLocator.TenantPM;
        this.entityPM = itemPM;
        this.entityList = itemList;
        this.isChecked = this.entityPM.OpportunityAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.entityList.Id; })[0] != null;
    }
    Object.defineProperty(ServiceItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "Foreground", {
        get: function () { return this.IsChecked ? "#FF6E7172" : "#FF282E30"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "Id", {
        get: function () { return this.entityList.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ServiceItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var notesRightToLeft = false;
                    if (SessionLocator_1.SessionLocator.TenantPM.IsNotesRightToLeftEnabled == true) {
                        notesRightToLeft = true;
                    }
                    var newItem = new OpportunityAdditionalServicePM_1.OpportunityAdditionalServicePM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.OpportunityId = this.entityPM.Id;
                    newItem.AdditionalServiceId = this.Id;
                    newItem.NotesRightToLeft = notesRightToLeft;
                    var type = null;
                    var addtionalService = new AdditionalServiceListService_1.AdditionalServiceListService();
                    addtionalService.getSingleFromCache(this.Id).subscribe(function (result) {
                        var typeList = result.Result;
                        if (typeList != null) {
                            type = typeList.Name;
                        }
                        newItem.EnglishName = type;
                        if (!_this.entityPM.OpportunityAdditionalServices.includes(newItem)) {
                            _this.entityPM.AddOpportunityAdditionalService(newItem);
                        }
                    });
                }
                else {
                    var item = this.entityPM.OpportunityAdditionalServices.filter(function (d) { return d.AdditionalServiceId == _this.Id; })[0];
                    if (item != null) {
                        if (this.entityPM.OpportunityAdditionalServices.includes(item)) {
                            this.entityPM.RemoveOpportunityAdditionalService(item);
                        }
                    }
                }
                this.Parent.BuildObsList();
                this.Parent.BuildToggleButtonList();
            }
        },
        enumerable: true,
        configurable: true
    });
    return ServiceItemClass;
}());
exports.ServiceItemClass = ServiceItemClass;
var CompetitorViewModelData = /** @class */ (function () {
    function CompetitorViewModelData(item, trigger) {
        this.entityPM = item;
        this.trigger = trigger;
    }
    Object.defineProperty(CompetitorViewModelData.prototype, "CompetitorId", {
        get: function () { return this.entityPM.CompetitorId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorViewModelData.prototype, "Name", {
        get: function () {
            var _this = this;
            var result = "";
            var list = this.trigger.AllCompetitors.filter(function (d) { return d.Id == _this.entityPM.CompetitorId; })[0];
            if (list != null) {
                result = list.Name;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    return CompetitorViewModelData;
}());
exports.CompetitorViewModelData = CompetitorViewModelData;
var CompetitorItemClass = /** @class */ (function () {
    function CompetitorItemClass(item, entityPM, trigger) {
        var _this = this;
        this.entityList = item;
        this.entityPM = entityPM;
        this.trigger = trigger;
        this.isChecked = entityPM.OpportunityCompetitors.filter(function (d) { return d.CompetitorId == _this.entityList.Id; })[0] != null;
    }
    Object.defineProperty(CompetitorItemClass.prototype, "Name", {
        get: function () { return this.entityList.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CompetitorItemClass.prototype, "IsChecked", {
        get: function () { return this.isChecked; },
        set: function (value) {
            var _this = this;
            if (this.isChecked != value) {
                this.isChecked = value;
                if (value) {
                    var newItem = new OpportunityCompetitorPM_1.OpportunityCompetitorPM(null);
                    newItem.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    newItem.OpportunityId = this.entityPM.Id;
                    newItem.CompetitorId = this.entityList.Id;
                    newItem.EnglishName = this.entityList.Name;
                    if (!this.entityPM.OpportunityCompetitors.includes(newItem)) {
                        this.entityPM.AddOpportunityCompetitor(newItem);
                    }
                }
                else {
                    var item = this.entityPM.OpportunityCompetitors.filter(function (d) { return d.CompetitorId == _this.entityList.Id; })[0];
                    if (item != null) {
                        if (this.entityPM.OpportunityCompetitors.includes(item)) {
                            this.entityPM.RemoveOpportunityCompetitor(item);
                        }
                    }
                }
                this.trigger.BuildCompetitorsObsList();
                this.trigger.BuildToggleButtonList();
            }
        },
        enumerable: true,
        configurable: true
    });
    return CompetitorItemClass;
}());
var ProductTypeList = /** @class */ (function () {
    function ProductTypeList() {
    }
    Object.defineProperty(ProductTypeList.prototype, "Id", {
        get: function () { return this.id; },
        set: function (value) { this.id = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "Code", {
        get: function () { return this.code; },
        set: function (value) { this.code = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "Name", {
        get: function () { return this.name; },
        set: function (value) { this.name = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "InActive", {
        get: function () { return this.inActive; },
        set: function (value) { this.inActive = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ProductTypeList.prototype, "SearchFields", {
        get: function () { return this.searchFields; },
        set: function (value) { this.searchFields = value; },
        enumerable: true,
        configurable: true
    });
    return ProductTypeList;
}());
var ActivityItemClass = /** @class */ (function (_super) {
    __extends(ActivityItemClass, _super);
    function ActivityItemClass(item, father) {
        var _this = _super.call(this) || this;
        _this.father = father;
        // Properties
        _this.background = "rgb(255,255,255)";
        _this.DateLable = "";
        _this.DateValue = "";
        _this.DueDateForeground = "";
        // Action Fields
        _this.Action = "";
        _this.ActionBy = "";
        _this.completeVisi = false;
        _this.reopenVisi = false;
        _this.completetogVisi = false;
        _this.entity = item;
        _this.EntityId = item.Id;
        _this.ImageSrc = Tools_2.CRMTool.GetActivityImageSrc(_this.entity.ActivityTypePathCode);
        _this.GetDueDateForeground();
        _this.GetDateValue();
        _this.GetDateLable();
        _this.GetAction();
        _this.GetActionBy();
        _this.getActionDate();
        return _this;
    }
    Object.defineProperty(ActivityItemClass.prototype, "Background", {
        get: function () {
            if (!this.entity.IsOpen) {
                this.background = "rgba(0,0,0,0.1)";
            }
            return this.background;
        },
        set: function (value) {
            this.background = value;
        },
        enumerable: true,
        configurable: true
    });
    ActivityItemClass.prototype.ControlIsEnabled = function () { return this.entity.IsOpen; };
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypePathCode", {
        get: function () { return this.entity.ActivityTypePathCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Id", {
        get: function () { return this.entity.Id; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CallWithId", {
        get: function () { return this.entity.CallWithId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypeName", {
        get: function () { return this.entity.ActivityTypeName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ActivityTypeCode", {
        get: function () { return this.entity.ActivityTypeCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Subject", {
        get: function () { return this.entity.Subject; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "Owner", {
        get: function () { return this.entity.OwnerName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CustomerId", {
        get: function () { return this.entity.CustomerId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "SortingBy", {
        get: function () { return this.entity.SortingBy; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "DueDate", {
        get: function () { return this.entity.DueDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "StartDate", {
        get: function () { return this.entity.StartDateTime; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "SortingDate", {
        get: function () { return this.entity.SortingDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "MeetingSummary", {
        get: function () { return this.entity.MeetingSummary; },
        set: function (value) {
            if (this.entity.MeetingSummary != value) {
                this.entity.MeetingSummary = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "PostToFollowers", {
        get: function () { return this.entity.PostToFollowers; },
        set: function (value) {
            if (this.entity.PostToFollowers != value) {
                this.entity.PostToFollowers = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    ActivityItemClass.prototype.GetDateLable = function () {
        var myResult = "Due Date";
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = "Due Date";
                    }
                    else {
                        myResult = "Start Date";
                    }
                    break;
                }
            case "AP":
                {
                    myResult = "Start Date";
                    break;
                }
            case "EO":
                {
                    myResult = "To";
                    break;
                }
            case "EI":
                {
                    myResult = "From";
                    break;
                }
        }
        this.DateLable = myResult;
    };
    ActivityItemClass.prototype.GetDateValue = function () {
        var DatePipe = new DateTimePipe_1.DateTimePipe();
        var myResult = "";
        if (this.DueDate != null) {
            myResult = DatePipe.transform(this.DueDate, "SD");
        }
        switch (this.ActivityTypeCode) {
            case "TS":
                {
                    if (this.DueDate != null) {
                        myResult = DatePipe.transform(this.DueDate, "SD");
                    }
                    else if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }
                    break;
                }
            case "AP":
                {
                    if (this.StartDate != null) {
                        myResult = DatePipe.transform(this.StartDate, "SD");
                    }
                    break;
                }
            case "EO":
                {
                    myResult = this.entity.RecipientsEmails;
                    break;
                }
            case "EI":
                {
                    break;
                }
        }
        this.DateValue = myResult;
    };
    ActivityItemClass.prototype.GetDueDateForeground = function () {
        var result = "rgb(40,46,48)";
        if (this.DueDate != null && Tools_1.DateTool.GetDateParts(this.DueDate) < Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateTimeAsUtc())) {
            result = "Red";
        }
        this.DueDateForeground = result;
    };
    ActivityItemClass.prototype.GetAction = function () {
        var myResult = "Modified by";
        if (this.entity.ActivityTypeCode == "EI") {
            myResult = "Recorded by";
        }
        else if (this.entity.ActivityTypeCode == "EO") {
            myResult = "Sent by";
        }
        else {
            switch (this.entity.ActivityStatusCode) {
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
        this.Action = myResult;
    };
    ActivityItemClass.prototype.GetActionBy = function () {
        var myResult = this.entity.UpdatedByUserName;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityTypeCode == "EO") {
                myResult = this.entity.CreatedByUserName;
            }
        }
        this.ActionBy = myResult;
    };
    ActivityItemClass.prototype.getActionDate = function () {
        var myResult = this.entity.UpdateDate;
        if (!this.entity.IsOpen) {
            if (this.entity.ActivityStatusCode == "C") {
                myResult = this.entity.CompleteDate;
            }
        }
        this.ActionDate = myResult;
    };
    Object.defineProperty(ActivityItemClass.prototype, "CompleteButtonVisibility", {
        get: function () {
            if (this.entity.IsOpen && this.entity.ActivityTypeCode != "AP") {
                this.completeVisi = true;
            }
            return this.completeVisi;
        },
        set: function (value) { this.completeVisi = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "ReopenButtonVisibility", {
        get: function () {
            if (!this.entity.IsOpen) {
                if (this.entity.ActivityTypeCode != "EI" && this.entity.ActivityTypeCode != "EO") {
                    this.reopenVisi = true;
                }
            }
            return this.reopenVisi;
        },
        set: function (value) { this.reopenVisi = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ActivityItemClass.prototype, "CompleteToggleButtonVisibility", {
        get: function () {
            if (this.entity.IsOpen && this.entity.ActivityTypeCode == "AP") {
                this.completetogVisi = true;
            }
            return this.completetogVisi;
        },
        set: function (value) { this.completetogVisi = value; },
        enumerable: true,
        configurable: true
    });
    // Commands
    ActivityItemClass.prototype.CompleteClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetCompleteActivity(this.EntityId, this.PostToFollowers, this.MeetingSummary).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entity = resp.Result;
                _this.father.LoadActivities();
                _this.ReopenButtonVisibility = true;
                _this.CompleteButtonVisibility = false;
                _this.CompleteToggleButtonVisibility = false;
                _this.Background = "rgba(0,0,0,0.1)";
            }
        });
    };
    ActivityItemClass.prototype.ReopenClicked = function () {
        var _this = this;
        var myService = new CRMDomainService_1.CRMDomainService();
        myService.GetReopenActivity(this.EntityId).subscribe(function (resp) {
            if (!resp.HasError) {
                _this.entity = resp.Result;
                _this.father.LoadActivities();
                _this.ReopenButtonVisibility = false;
                if (_this.entity.ActivityTypeCode == "AP") {
                    _this.CompleteToggleButtonVisibility = true;
                }
                else {
                    _this.CompleteButtonVisibility = true;
                }
                _this.Background = "rgb(255,255,255)";
            }
        });
    };
    return ActivityItemClass;
}(BaseComponent_1.BaseComponent));
exports.ActivityItemClass = ActivityItemClass;
//# sourceMappingURL=OpportunityOverviewTabComponent.js.map