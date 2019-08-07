"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var VehicleEditComponent_1 = require("./Components/EditTabs/VehicleEditComponent");
var VehicleGeneralComponent_1 = require("./Components/EditTabs/VehicleGeneralComponent");
var VehicleMoreDetailsTabComponent_1 = require("./Components/EditTabs/VehicleMoreDetailsTabComponent");
var VehiclesOwnersAndSafetyTabComponent_1 = require("./Components/EditTabs/VehiclesOwnersAndSafetyTabComponent");
var VehiclesSelectionComponent_1 = require("./Components/EditTabs/VehiclesSelectionComponent");
var SendVehicleComponent_1 = require("./Components/SendVehicle/SendVehicleComponent");
var DeleteVehicleComponent_1 = require("./Components/SendVehicle/DeleteVehicleComponent");
exports.Components = [
    VehicleEditComponent_1.VehicleEditComponent,
    VehicleGeneralComponent_1.VehicleGeneralComponent,
    VehicleMoreDetailsTabComponent_1.VehicleMoreDetailsTabComponent,
    VehiclesOwnersAndSafetyTabComponent_1.VehiclesOwnersAndSafetyTabComponent,
    VehiclesSelectionComponent_1.VehiclesSelectionComponent,
    SendVehicleComponent_1.SendVehicleComponent,
    DeleteVehicleComponent_1.DeleteVehicleComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "VehicleEditComponent": {
                myResult = VehicleEditComponent_1.VehicleEditComponent;
                break;
            }
            case "VehicleGeneralComponent": {
                myResult = VehicleGeneralComponent_1.VehicleGeneralComponent;
                break;
            }
            case "VehicleMoreDetailsTabComponent": {
                myResult = VehicleMoreDetailsTabComponent_1.VehicleMoreDetailsTabComponent;
                break;
            }
            case "VehiclesOwnersAndSafetyTabComponent": {
                myResult = VehiclesOwnersAndSafetyTabComponent_1.VehiclesOwnersAndSafetyTabComponent;
                break;
            }
            case "VehiclesSelectionComponent": {
                myResult = VehiclesSelectionComponent_1.VehiclesSelectionComponent;
                break;
            }
            case "SendVehicleComponent": {
                myResult = SendVehicleComponent_1.SendVehicleComponent;
                break;
            }
            case "DeleteVehicleComponent": {
                myResult = DeleteVehicleComponent_1.DeleteVehicleComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map