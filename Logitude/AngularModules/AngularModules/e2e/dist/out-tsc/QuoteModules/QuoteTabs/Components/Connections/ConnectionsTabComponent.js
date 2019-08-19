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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var QuoteDomainService_1 = require("../../../../Quote/Services/QuoteDomainService");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ConnectionsTabComponent = /** @class */ (function () {
    function ConnectionsTabComponent(entityArgs, entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.TabSelectedEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsShipmentGridVisible = false;
        this.IsTicketsGridVisible = false;
        this.IsOpportunitiesGridVisible = false;
        this.ShipmentsGridHeight = 90;
        this.TicketsGridHeight = 90;
        this.OpportunitiesGridHeight = 90;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.myDomainService = new QuoteDomainService_1.QuoteDomainService();
        this.Listen();
        this.LoadData();
    }
    ConnectionsTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
        }
    };
    ConnectionsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "LoadConnectedShipments") {
                    _this.LoadData();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.LoadData();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.LoadData();
                }
            });
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "QTCE") {
                    _this.LoadData();
                }
            });
        }
    };
    ConnectionsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ConnectionsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.myDomainService.GetQuoteConnectedEntities(this.EntityPM.Id, this.EntityPM.OpportunityId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list = myResponse.Result;
                    _this.FillItemSources(list);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ConnectionsTabComponent.prototype.FillItemSources = function (list) {
        var _this = this;
        this.ItemsSource = [];
        this.ShipmentsItemsSource = [];
        this.TicketsItemsSource = [];
        list.forEach(function (item) {
            _this.ItemsSource.push(new QuoteConnectedEntityItem(item, _this));
        });
        this.ShipmentsItemsSource = this.ItemsSource.filter(function (d) { return d.ObjectTable == "Shipment"; });
        this.TicketsItemsSource = this.ItemsSource.filter(function (d) { return d.ObjectTable == "Ticket"; });
        this.OpportunitiesItemsSource = this.ItemsSource.filter(function (d) { return d.ObjectTable == "Opportunity"; });
        this.IsShipmentGridVisible = this.ShipmentsItemsSource.length == 0 ? false : true;
        this.IsTicketsGridVisible = this.TicketsItemsSource.length == 0 ? false : true;
        this.IsOpportunitiesGridVisible = this.OpportunitiesItemsSource.length == 0 ? false : true;
        this.ShipmentsGridHeight = this.ComputeGridHeight(this.ShipmentsItemsSource);
        this.TicketsGridHeight = this.ComputeGridHeight(this.TicketsItemsSource);
        this.OpportunitiesGridHeight = this.ComputeGridHeight(this.OpportunitiesItemsSource);
    };
    ConnectionsTabComponent.prototype.ComputeGridHeight = function (list) {
        var height = 90;
        if (list.length == 0 || list.length == 1) {
            height = 90;
        }
        else if (list.length == 2) {
            height = 110;
        }
        else if (list.length == 3) {
            height = 130;
        }
        else {
            height = 250;
        }
        return height;
    };
    ConnectionsTabComponent.prototype.ViewOpportunityClicked = function (entity) {
        if (entity != null) {
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: entity.EntityId, ObjectTableName: "Opportunity", BackButtonLabel: " Quotes" });
                cmpRef.instance.BackCompleted.subscribe(function ($event) { });
            });
        }
    };
    ConnectionsTabComponent = __decorate([
        core_1.Component({
            selector: 'ConnectionsTabComponent',
            moduleId: module.id,
            templateUrl: './ConnectionsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ConnectionsTabComponent);
    return ConnectionsTabComponent;
}());
exports.ConnectionsTabComponent = ConnectionsTabComponent;
var QuoteConnectedEntityItem = /** @class */ (function () {
    function QuoteConnectedEntityItem(entity, fatherComponent) {
        this.fatherComponent = fatherComponent;
        this.myEntity = new QuoteDomainService_1.QuoteConnectedEntity();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myEntity = entity;
    }
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "EntityId", {
        get: function () { return this.myEntity.EntityId; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "ShipmentType", {
        get: function () { return this.myEntity.ShipmentType; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "EntityNumber", {
        get: function () { return this.myEntity.EntityNumber; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "ObjectTable", {
        get: function () { return this.myEntity.ObjectTable; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "EntityStatus", {
        get: function () { return this.myEntity.EntityStatus; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "OpenDate", {
        get: function () { return this.myEntity.OpenDate; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "Master", {
        get: function () { return this.myEntity.Master; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "House", {
        get: function () { return this.myEntity.House; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "Customer", {
        get: function () { return this.myEntity.Customer; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "From", {
        get: function () { return this.myEntity.From; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "To", {
        get: function () { return this.myEntity.To; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "GrossWeight", {
        get: function () { return this.myEntity.GrossWeight; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "VolumeInKG", {
        get: function () { return this.myEntity.VolumeInKG; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "EntityOwner", {
        get: function () { return this.myEntity.EntityOwner; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QuoteConnectedEntityItem.prototype, "EntityClosingDate", {
        get: function () { return this.myEntity.EntityClosingDate; },
        enumerable: true,
        configurable: true
    });
    QuoteConnectedEntityItem.prototype.ViewEntitytClicked = function () {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectTable)) {
            var backLabel = "Quote: " + this.fatherComponent.EntityPM.QuoteNumber;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: _this.EntityId, ObjectTableName: _this.ObjectTable, BackButtonLabel: backLabel });
                var isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    if (isEditComponentSaved) {
                        _this.fatherComponent.entityArgs.EditComponent.ReloadEntityPM();
                    }
                });
                cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
        }
    };
    return QuoteConnectedEntityItem;
}());
//# sourceMappingURL=ConnectionsTabComponent.js.map