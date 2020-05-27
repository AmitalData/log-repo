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
var MasterTabComponent = /** @class */ (function () {
    function MasterTabComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.InfoMessage = null;
        this.IsButtonEnabled = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
    }
    MasterTabComponent.prototype.ngOnInit = function () {
        if (this.EntityPM != null) {
            if (this.EntityPM.ShipmentLevelCode == "D") {
                this.InfoMessage = "This shipment is direct";
            }
            else if (this.EntityPM.ShipmentLevelCode == "H") {
                if (this.EntityPM.MasterShipmentDataId == null) {
                    this.InfoMessage = "This house is not connected to master";
                }
                else {
                    this.IsButtonEnabled = true;
                }
            }
        }
    };
    MasterTabComponent.prototype.ButtonClicked = function () {
        var _this = this;
        var myBackButtonLabel = "Shipment: " + this.EntityPM.ShipmentNumber;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.EntityPM.MasterShipmentDataId, ObjectTableName: 'Shipment', BackButtonLabel: myBackButtonLabel });
        });
    };
    MasterTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MasterTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], MasterTabComponent);
    return MasterTabComponent;
}());
exports.MasterTabComponent = MasterTabComponent;
//# sourceMappingURL=MasterTabComponent.js.map