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
var WarehouseEntryPMExtendedService_1 = require("../../../Services/ExtendedPMs/WarehouseEntryPMExtendedService");
var WarehouseConnectionsTabComponent = /** @class */ (function () {
    function WarehouseConnectionsTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsShowMessageNoConnectedEntity = false;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.warehouseEntryPMExtendedService = new WarehouseEntryPMExtendedService_1.WarehouseEntryPMExtendedService();
        if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.ShipmentId)) {
            this.LoadData();
        }
        else
            this.IsShowMessageNoConnectedEntity = true;
    }
    WarehouseConnectionsTabComponent.prototype.ngOnInit = function () {
    };
    WarehouseConnectionsTabComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.ItemsSource = [];
        this.warehouseEntryPMExtendedService.GetWarehouseConnectedEntitiesByEntityId(this.EntityPM.ShipmentId).subscribe(function (myResponse) {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    _this.ItemsSource = myResponse.Result;
                    if (!_this.ItemsSource || _this.ItemsSource.length == 0) {
                        _this.IsShowMessageNoConnectedEntity = true;
                    }
                    else
                        _this.IsShowMessageNoConnectedEntity = false;
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    WarehouseConnectionsTabComponent.prototype.ViewEntitytClicked = function () {
        var _this = this;
        var backLabel = this.ObjectTableName == "WarehouseEntry" ? "Entry " + this.EntityPM.EntryNumber : "Release " + this.EntityPM.ReleaseNumber;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.EntityPM.ShipmentId, ObjectTableName: "Shipment", BackButtonLabel: backLabel });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    WarehouseConnectionsTabComponent = __decorate([
        core_1.Component({
            selector: 'WarehouseConnectionsTabComponent',
            moduleId: module.id,
            templateUrl: './WarehouseConnectionsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], WarehouseConnectionsTabComponent);
    return WarehouseConnectionsTabComponent;
}());
exports.WarehouseConnectionsTabComponent = WarehouseConnectionsTabComponent;
//# sourceMappingURL=WarehouseConnectionsTabComponent.js.map