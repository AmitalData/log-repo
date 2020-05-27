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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ShipmentAuditTabComponent = /** @class */ (function () {
    function ShipmentAuditTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.TabSelectedEvent = null;
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            this.EntityId = this.EntityPM.Id;
        }
        this.Listen();
    }
    ShipmentAuditTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent) {
            this.TabSelectedEvent = this.entityArgs.EditComponent.TabSelected.subscribe(function (tabCode) {
                if (tabCode == "SHAU") {
                    _this.CurrentSession.FireEvent("Shipment");
                }
            });
        }
    };
    ShipmentAuditTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.TabSelectedEvent);
    };
    ShipmentAuditTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentAuditTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ShipmentAuditTabComponent);
    return ShipmentAuditTabComponent;
}());
exports.ShipmentAuditTabComponent = ShipmentAuditTabComponent;
//# sourceMappingURL=ShipmentAuditTabComponent.js.map