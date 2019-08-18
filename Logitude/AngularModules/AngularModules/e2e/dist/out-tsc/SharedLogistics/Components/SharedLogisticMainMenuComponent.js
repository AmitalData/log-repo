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
var FeatureLocator_1 = require("../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var SharedLogisticMainMenuComponent = /** @class */ (function () {
    function SharedLogisticMainMenuComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.CustomerTenantAccessVisibility = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_BOOK = null;
        this.Page_SHIP = null;
        this.RunComponent();
    }
    SharedLogisticMainMenuComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    SharedLogisticMainMenuComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    SharedLogisticMainMenuComponent.prototype.SetSelectedItem = function () {
        this.CustomerTenantAccessVisibility = false;
        this.SelectedItem = "SHLO";
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMERTENANTACCESSES")) {
            this.CustomerTenantAccessVisibility = true;
        }
    };
    Object.defineProperty(SharedLogisticMainMenuComponent.prototype, "SelectedItem", {
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
    SharedLogisticMainMenuComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation != null) {
                    switch (this.SelectedItem) {
                        //SharedLogistics
                        case "SHLO": {
                            if (this.Page_SHIP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/SharedLogisticsMainComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_SHIP = cmpRef.instance;
                                    //this.Page_SHIP.InitComponent();
                                });
                            }
                            break;
                        }
                        //LogBox
                        case "LOBO": {
                            if (this.Page_BOOK == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./SharedLogistics/Components/CutsomerTenantAccessManagementComponent', myLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_SHIP = cmpRef.instance;
                                    //this.Page_SHIP.InitComponent();
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], SharedLogisticMainMenuComponent.prototype, "AllLocations", void 0);
    SharedLogisticMainMenuComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedLogisticMainMenuComponent',
            templateUrl: './SharedLogisticMainMenuComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], SharedLogisticMainMenuComponent);
    return SharedLogisticMainMenuComponent;
}());
exports.SharedLogisticMainMenuComponent = SharedLogisticMainMenuComponent;
//# sourceMappingURL=SharedLogisticMainMenuComponent.js.map