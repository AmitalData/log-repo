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
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var CRMWorkspaceComponent = /** @class */ (function () {
    function CRMWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.IsOccasionVisible = false;
        this.IsContactsVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_OVE = null;
        this.Page_CUS = null;
        this.Page_QUT = null;
        this.Page_ACT = null;
        this.Page_OPP = null;
        this.Page_CON = null;
        this.Page_DAS = null;
        this.Page_OCC = null;
        this.mySelectedActivityFilter = "All";
        // Upcoming
        this.upcomingCount = 0;
        this.upcomingCountVisibility = false;
        this.RunComponent();
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Occasion", "Module")) {
            this.IsOccasionVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CONTACTS")) {
            this.IsContactsVisible = true;
        }
    }
    CRMWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SelectedItem = "OVE";
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    CRMWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(CRMWorkspaceComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    CRMWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "OVE": {
                            if (this.Page_OVE == null) {
                                this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(function (response2) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OverviewWorkspaceComponent', myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.Page_OVE = cmpRef.instance;
                                            _this.Page_OVE.InitComponent(_this);
                                        });
                                    });
                                });
                            }
                            else {
                                this.Page_OVE.LoadAllScreenData();
                            }
                            break;
                        }
                        case "CUS": {
                            if (this.Page_CUS == null) {
                                this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/CustomerWorkspaceComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_CUS = cmpRef.instance;
                                    });
                                });
                            }
                            break;
                        }
                        case "QUT": {
                            if (this.Page_QUT == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Quote/Components/Workspaces/QuotesComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_QUT = cmpRef.instance;
                                });
                            }
                            break;
                        }
                        case "ACT": {
                            if (this.Page_ACT == null) {
                                this._entityResourceService.getEntityResourceByTableName("Activity", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/ActivityWorkspaceComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_ACT = cmpRef.instance;
                                    });
                                });
                            }
                            else {
                                this.Page_ACT.LoadUpcomingEntities();
                            }
                            break;
                        }
                        case "OPP": {
                            if (this.Page_OPP == null) {
                                this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OpportunityWorkspaceComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_OPP = cmpRef.instance;
                                    });
                                });
                            }
                            break;
                        }
                        case "CON": {
                            if (this.Page_CON == null) {
                                this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/ContactWorkspaceComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_CON = cmpRef.instance;
                                        _this.Page_CON.InitComponent(_this);
                                    });
                                });
                            }
                            break;
                        }
                        case "DAS": {
                            if (this.Page_DAS == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/DashboardWorkspaceComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_DAS = cmpRef.instance;
                                    //this.Page_DAS.InitComponent();
                                });
                            }
                            break;
                        }
                        case "OCC": {
                            if (this.Page_OCC == null) {
                                this._entityResourceService.getEntityResourceByTableName("Occasion", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/OccasionWorkspaceComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_OCC = cmpRef.instance;
                                    });
                                });
                            }
                            break;
                        }
                    }
                    this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.CRM") + "\\" + this.GetPageName() });
                }
            }
        }
    };
    CRMWorkspaceComponent.prototype.GetPageName = function () {
        var myResult = "";
        switch (this.SelectedItem) {
            case "OVE": {
                myResult = "Overview";
                break;
            }
            case "CUS": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Customers");
                ;
                break;
            }
            case "QUT": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Quotes");
                ;
                break;
            }
            case "ACT": {
                myResult = "Activities";
                break;
            }
            case "OPP": {
                myResult = "Opportunities";
                break;
            }
            case "CON": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Contacts");
                break;
            }
            case "DAS": {
                myResult = "Dashboard";
                break;
            }
            case "OCC": {
                myResult = "Occasion";
                break;
            }
        }
        return myResult;
    };
    Object.defineProperty(CRMWorkspaceComponent.prototype, "SelectedActivityFilter", {
        get: function () { return this.mySelectedActivityFilter; },
        set: function (value) {
            if (this.mySelectedActivityFilter != value) {
                this.mySelectedActivityFilter = value;
                this.Page_ACT.SelectedActivityFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CRMWorkspaceComponent.prototype, "UpcomingCount", {
        get: function () { return this.upcomingCount; },
        set: function (value) {
            this.upcomingCount = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CRMWorkspaceComponent.prototype, "UpcomingCountVisibility", {
        get: function () { return this.upcomingCountVisibility; },
        set: function (value) {
            this.upcomingCountVisibility = value;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], CRMWorkspaceComponent.prototype, "AllLocations", void 0);
    CRMWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'CRMComponent',
            moduleId: module.id,
            templateUrl: './CRMWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], CRMWorkspaceComponent);
    return CRMWorkspaceComponent;
}());
exports.CRMWorkspaceComponent = CRMWorkspaceComponent;
//# sourceMappingURL=CRMWorkspaceComponent.js.map