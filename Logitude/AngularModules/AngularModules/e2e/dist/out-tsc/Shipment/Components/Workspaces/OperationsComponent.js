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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var OperationsComponent = /** @class */ (function () {
    function OperationsComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.IsMenuVisible = false;
        this.IsBookingItemVisible = false;
        this.IsSharedManifestItemVisible = false;
        this.IsContainersFUItemVisible = false;
        this.IsResourcesReady = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_SHMA = null;
        this.Page_BOOK = null;
        this.Page_SHIP = null;
        this.Page_CNFU = null;
    }
    OperationsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("Shipment", 0).subscribe(function (res1) {
            _this._entityResourceService.getEntityResourceByTableName("Master", 0).subscribe(function (res2) {
                _this.IsResourcesReady = true;
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Booking", "Booking.Menu")) {
                    _this.IsBookingItemVisible = true;
                    _this.IsMenuVisible = true;
                }
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "AgentSharedManifest")) {
                    _this.IsSharedManifestItemVisible = true;
                    _this.IsMenuVisible = true;
                }
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Area.ContainersFU")) {
                    _this.IsContainersFUItemVisible = true;
                    _this.IsMenuVisible = true;
                }
                _this.RunComponent();
            });
        });
    };
    OperationsComponent.prototype.RunComponent = function () {
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
    OperationsComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    OperationsComponent.prototype.SetSelectedItem = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Booking", "Booking.Menu")) {
            this.SelectedItem = "BOOK";
        }
        else {
            this.SelectedItem = "SHIP";
        }
    };
    Object.defineProperty(OperationsComponent.prototype, "SelectedItem", {
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
    OperationsComponent.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "BOOK": {
                            if (this.Page_BOOK == null) {
                                this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Booking/Components/Workspaces/BookingsComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_BOOK = cmpRef.instance;
                                        _this.Page_BOOK.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                        case "SHIP": {
                            if (this.Page_SHIP == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/ShipmentsComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_SHIP = cmpRef.instance;
                                    _this.Page_SHIP.InitComponent();
                                });
                            }
                            break;
                        }
                        case "SHMA": {
                            if (this.Page_SHMA == null) {
                                this._entityResourceService.getEntityResourceByTableName("AgentSharedManifest", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./ShipmentModules/ShipmentSharedManifest/Components/SharedManifestsWorkSpaces', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_SHMA = cmpRef.instance;
                                        _this.Page_SHMA.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                        case "CNFU": {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Shipment/Components/Workspaces/ContainersFUsComponent', myLocation_1.viewContainerRef)
                                .then(function (cmpRef) {
                                _this.Page_CNFU = cmpRef.instance;
                            });
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
    ], OperationsComponent.prototype, "AllLocations", void 0);
    OperationsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'OperationsComponent',
            templateUrl: './OperationsComponent.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], OperationsComponent);
    return OperationsComponent;
}());
exports.OperationsComponent = OperationsComponent;
//# sourceMappingURL=OperationsComponent.js.map