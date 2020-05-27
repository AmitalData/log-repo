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
var SharedManifestHeaderComponent = /** @class */ (function () {
    function SharedManifestHeaderComponent() {
    }
    SharedManifestHeaderComponent.prototype.Run = function (currentEntity, entityList) {
        this.SharedManifestHeaderData = new SharedManifestHeader(currentEntity, entityList);
    };
    SharedManifestHeaderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'SharedManifestHeaderComponent',
            templateUrl: './SharedManifestHeaderComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], SharedManifestHeaderComponent);
    return SharedManifestHeaderComponent;
}());
exports.SharedManifestHeaderComponent = SharedManifestHeaderComponent;
var SharedManifestHeader = /** @class */ (function () {
    function SharedManifestHeader(entityPM, entityList) {
        this.DirectionId = "";
        this.Route = "";
        this.LongMaster = "";
        this.TransportModeId = "";
        this.AgentName = "";
        this.ShipmentLevelName = "";
        this.AgentReference = "";
        this.CarrierName = "";
        this.FreightPrepaidCollectId = "";
        this.OtherPrepaidCollectId = "";
        this.ShipmentLevelCode = "";
        if (entityPM) {
            this.CreateDate = entityPM.CreateDate;
            this.AgentReference = entityPM.AgentReference;
            this.ManifestSL = entityPM.ManifestSL;
        }
        if (entityList) {
            this.Route = entityList.Routing;
            this.ShipmentLevelName = entityList.ShipmentLevelName;
        }
        if (this.ManifestSL) {
            this.LongMaster = this.ManifestSL.LongMaster;
            this.AgentName = this.ManifestSL.AgentName;
            this.ShipperName = this.ManifestSL.ShipperName;
            this.CarrierName = this.ManifestSL.CarrierName;
            this.FreightPrepaidCollectId = this.ManifestSL.FreightPrepaidCollectId;
            this.OtherPrepaidCollectId = this.ManifestSL.OtherPrepaidCollectId;
            this.TransportModeId = this.ManifestSL.TransportModeId;
            this.DirectionId = this.ManifestSL.DirectionId;
            this.MainCarriageATD = this.ManifestSL.MainCarriageATD;
            this.MainCarriageETA = this.ManifestSL.MainCarriageETA;
            this.MAWBOBLDate = this.ManifestSL.MAWBOBLDate;
            this.ShipmentLevelCode = this.ManifestSL.ShipmentLevelCode;
        }
    }
    return SharedManifestHeader;
}());
exports.SharedManifestHeader = SharedManifestHeader;
//# sourceMappingURL=SharedManifestHeaderComponent.js.map