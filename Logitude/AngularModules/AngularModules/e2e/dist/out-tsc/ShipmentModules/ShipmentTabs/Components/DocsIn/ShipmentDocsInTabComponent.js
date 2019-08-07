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
var ShipmentDocsInTabComponent = /** @class */ (function () {
    function ShipmentDocsInTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
    }
    ShipmentDocsInTabComponent.prototype.ngOnInit = function () {
        this.EntityPM = this.entityArgs.EntityPM;
        if (this.EntityPM != null) {
            var table = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
            if (table)
                this.ObjectTableId = table.Id;
            this.EntityId = this.EntityPM.Id;
            this.TransportModeId = this.EntityPM.TransportModeId;
            this.ShipmentlevelCode = this.EntityPM.ShipmentLevelCode;
        }
    };
    ShipmentDocsInTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ShipmentDocsInTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], ShipmentDocsInTabComponent);
    return ShipmentDocsInTabComponent;
}());
exports.ShipmentDocsInTabComponent = ShipmentDocsInTabComponent;
//# sourceMappingURL=ShipmentDocsInTabComponent.js.map