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
var EntityPMService_1 = require("../../../../Infrastructure/Services/EntityPMService");
var SendVehicleComponent_1 = require("./SendVehicleComponent");
var DeleteVehicleComponent = /** @class */ (function () {
    function DeleteVehicleComponent(entityPMService) {
        this.entityPMService = entityPMService;
        //------------------------------------------------------//
        this.ObjectTableName = "Customs.Vehicle";
    }
    DeleteVehicleComponent.prototype.Run = function (args) {
        this.EntityPM = args.EntityPM;
        this.ObjectTable = args.ObjectTable;
    };
    DeleteVehicleComponent.prototype.OnCustomSendOptionsButtonClick = function (customSendOptionsArgs) {
        this.RequestVIA = customSendOptionsArgs.RequestVIA;
        this.Option = customSendOptionsArgs.Option;
        this.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
        SendVehicleComponent_1.SendVehicleComponent.SaveEntityChanges(customSendOptionsArgs, this.EntityPM, true);
    };
    DeleteVehicleComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DeleteVehicleComponent',
            templateUrl: "DeleteVehicleComponent.html",
        }),
        __metadata("design:paramtypes", [EntityPMService_1.EntityPMService])
    ], DeleteVehicleComponent);
    return DeleteVehicleComponent;
}());
exports.DeleteVehicleComponent = DeleteVehicleComponent;
//# sourceMappingURL=DeleteVehicleComponent.js.map