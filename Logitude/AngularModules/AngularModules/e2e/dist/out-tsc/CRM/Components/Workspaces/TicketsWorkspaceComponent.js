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
var TicketsWorkspaceComponent = /** @class */ (function () {
    function TicketsWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.IsMenuVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsTicketDashboardVisible = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_TW = null;
        this.Page_DW = null;
        this.mySelectedActivityFilter = "All";
        this.RunComponent();
    }
    TicketsWorkspaceComponent.prototype.ngOnInit = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Ticket", "TicketDashboard.Menu")) {
            this.IsTicketDashboardVisible = true;
            this.IsMenuVisible = true;
        }
    };
    TicketsWorkspaceComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SelectedItem = "TIW";
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    TicketsWorkspaceComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    Object.defineProperty(TicketsWorkspaceComponent.prototype, "SelectedItem", {
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
    TicketsWorkspaceComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "TIW": {
                            if (this.Page_TW == null) {
                                this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(function (response) {
                                    _this._entityResourceService.getEntityResourceByTableName("Opportunity", 0).subscribe(function (response2) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketsComponent', myLocation_1.viewContainerRef)
                                            .then(function (cmpRef) {
                                            _this.Page_TW = cmpRef.instance;
                                        });
                                    });
                                });
                            }
                            else {
                                this.Page_TW.LoadAllScreenData();
                            }
                            break;
                        }
                        case "DBW": {
                            if (this.Page_DW == null) {
                                this._entityResourceService.getEntityResourceByTableName("Ticket", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketDashboardComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_DW = cmpRef.instance;
                                    });
                                });
                            }
                            break;
                        }
                    }
                    this.CurrentSession.ChangeSessionHeader({ Text: TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Ticket") + "\\" + this.GetPageName() });
                }
            }
        }
    };
    TicketsWorkspaceComponent.prototype.GetPageName = function () {
        var myResult = "";
        switch (this.SelectedItem) {
            case "TIW": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.Ticket");
                break;
            }
            case "DBW": {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.AirlineDashboard");
                break;
            }
        }
        return myResult;
    };
    Object.defineProperty(TicketsWorkspaceComponent.prototype, "SelectedActivityFilter", {
        get: function () { return this.mySelectedActivityFilter; },
        set: function (value) {
            if (this.mySelectedActivityFilter != value) {
                this.mySelectedActivityFilter = value;
                this.Page_TW.SelectedActivityFilter = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], TicketsWorkspaceComponent.prototype, "AllLocations", void 0);
    TicketsWorkspaceComponent = __decorate([
        core_1.Component({
            selector: 'CRMComponent',
            moduleId: module.id,
            templateUrl: './TicketsWorkspaceComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], TicketsWorkspaceComponent);
    return TicketsWorkspaceComponent;
}());
exports.TicketsWorkspaceComponent = TicketsWorkspaceComponent;
//# sourceMappingURL=TicketsWorkspaceComponent.js.map