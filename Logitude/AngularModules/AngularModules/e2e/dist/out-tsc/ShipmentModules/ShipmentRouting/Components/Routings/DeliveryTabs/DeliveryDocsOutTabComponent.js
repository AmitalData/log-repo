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
var DeliveryDocsOutTabComponent = /** @class */ (function () {
    function DeliveryDocsOutTabComponent() {
        this.EntityPM = null;
        this.ObjectTableName = "ShipmentPickUpDelivery";
        this.DataContext = this;
    }
    DeliveryDocsOutTabComponent.prototype.InitTab = function (myEntityPM, myShipmentPM) {
        this.EntityPM = myEntityPM;
        this.ShipmentPM = myShipmentPM;
        var deliveryobjecttable = window.ObjectTables.filter(function (d) { return d.Name == "ShipmentPickUpDelivery"; })[0];
        var shipmentobjecttable = window.ObjectTables.filter(function (d) { return d.Name == "Shipment"; })[0];
        this.EntityId = this.ShipmentPM.Id;
        this.ChildEntityId = this.EntityPM.Id;
        this.ChildEntityReference = this.EntityPM.PickUpDeliveryNumber;
        this.ObjectTableId = shipmentobjecttable.Id;
        this.ChildObjectTableId = deliveryobjecttable.Id;
        this.EntityReference = this.ShipmentPM.ShipmentNumber;
    };
    DeliveryDocsOutTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeliveryDocsOutTabComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DeliveryDocsOutTabComponent);
    return DeliveryDocsOutTabComponent;
}());
exports.DeliveryDocsOutTabComponent = DeliveryDocsOutTabComponent;
//# sourceMappingURL=DeliveryDocsOutTabComponent.js.map