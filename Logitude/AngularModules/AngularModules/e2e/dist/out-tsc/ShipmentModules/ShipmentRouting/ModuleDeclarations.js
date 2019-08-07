"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RoutingsTabComponent_1 = require("./Components/Routings/RoutingsTabComponent");
var AddEditPreCarriageComponent_1 = require("./Components/Routings/AddEditPreCarriageComponent");
var AddEditOnCarriageComponent_1 = require("./Components/Routings/AddEditOnCarriageComponent");
var AddEditHouseRoutingComponent_1 = require("./Components/Routings/AddEditHouseRoutingComponent");
var AddEditMainCarriageComponent_1 = require("./Components/Routings/AddEditMainCarriageComponent");
var AddEditPickupComponent_1 = require("./Components/Routings/AddEditPickupComponent");
var AddEditWarehouseLegComponent_1 = require("./Components/Routings/AddEditWarehouseLegComponent");
var PickupMainTabComponent_1 = require("./Components/Routings/PickupTabs/PickupMainTabComponent");
var PickupPackagesTabComponent_1 = require("./Components/Routings/PickupTabs/PickupPackagesTabComponent");
var PickupDocsOutTabComponent_1 = require("./Components/Routings/PickupTabs/PickupDocsOutTabComponent");
var PickupDocsInTabComponent_1 = require("./Components/Routings/PickupTabs/PickupDocsInTabComponent");
var PickupPackagesAddEditComponent_1 = require("./Components/Routings/PickupTabs/PickupPackagesAddEditComponent");
var PickupPackagesChooseComponent_1 = require("./Components/Routings/PickupTabs/PickupPackagesChooseComponent");
var AddEditDeliveryComponent_1 = require("./Components/Routings/AddEditDeliveryComponent");
var DeliveryMainTabComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryMainTabComponent");
var DeliveryPackagesTabComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryPackagesTabComponent");
var DeliveryDocsOutTabComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryDocsOutTabComponent");
var DeliveryDocsInTabComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryDocsInTabComponent");
var DeliveryPackagesAddEditComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryPackagesAddEditComponent");
var DeliveryPackagesChooseComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryPackagesChooseComponent");
var DeliveryPackagesConnectComponent_1 = require("./Components/Routings/DeliveryTabs/DeliveryPackagesConnectComponent");
var OnCarriageDateComponent_1 = require("./Components/Routings/OnCarriageDateComponent");
var AddEditPackageHarmonizeComponent_1 = require("./Components/Routings/AddEditPackageHarmonizeComponent");
exports.Components = [
    RoutingsTabComponent_1.RoutingsTabComponent,
    AddEditPreCarriageComponent_1.AddEditPreCarriageComponent,
    AddEditOnCarriageComponent_1.AddEditOnCarriageComponent,
    AddEditHouseRoutingComponent_1.AddEditHouseRoutingComponent,
    AddEditMainCarriageComponent_1.AddEditMainCarriageComponent,
    AddEditPickupComponent_1.AddEditPickupComponent,
    AddEditWarehouseLegComponent_1.AddEditWarehouseLegComponent,
    PickupMainTabComponent_1.PickupMainTabComponent,
    PickupPackagesTabComponent_1.PickupPackagesTabComponent,
    PickupDocsOutTabComponent_1.PickupDocsOutTabComponent,
    PickupDocsInTabComponent_1.PickupDocsInTabComponent,
    PickupPackagesAddEditComponent_1.PickupPackagesAddEditComponent,
    PickupPackagesChooseComponent_1.PickupPackagesChooseComponent,
    AddEditDeliveryComponent_1.AddEditDeliveryComponent,
    DeliveryMainTabComponent_1.DeliveryMainTabComponent,
    DeliveryPackagesTabComponent_1.DeliveryPackagesTabComponent,
    DeliveryDocsOutTabComponent_1.DeliveryDocsOutTabComponent,
    DeliveryDocsInTabComponent_1.DeliveryDocsInTabComponent,
    DeliveryPackagesAddEditComponent_1.DeliveryPackagesAddEditComponent,
    DeliveryPackagesChooseComponent_1.DeliveryPackagesChooseComponent,
    DeliveryPackagesConnectComponent_1.DeliveryPackagesConnectComponent,
    AddEditPackageHarmonizeComponent_1.AddEditPackageHarmonizeComponent,
];
exports.ControlsComponents = [
    OnCarriageDateComponent_1.OnCarriageDateComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "RoutingsTabComponent": {
                myResult = RoutingsTabComponent_1.RoutingsTabComponent;
                break;
            }
            case "AddEditPreCarriageComponent": {
                myResult = AddEditPreCarriageComponent_1.AddEditPreCarriageComponent;
                break;
            }
            case "AddEditOnCarriageComponent": {
                myResult = AddEditOnCarriageComponent_1.AddEditOnCarriageComponent;
                break;
            }
            case "AddEditHouseRoutingComponent": {
                myResult = AddEditHouseRoutingComponent_1.AddEditHouseRoutingComponent;
                break;
            }
            case "AddEditMainCarriageComponent": {
                myResult = AddEditMainCarriageComponent_1.AddEditMainCarriageComponent;
                break;
            }
            case "AddEditPickupComponent": {
                myResult = AddEditPickupComponent_1.AddEditPickupComponent;
                break;
            }
            case "AddEditWarehouseLegComponent": {
                myResult = AddEditWarehouseLegComponent_1.AddEditWarehouseLegComponent;
                break;
            }
            case "PickupMainTabComponent": {
                myResult = PickupMainTabComponent_1.PickupMainTabComponent;
                break;
            }
            case "PickupPackagesTabComponent": {
                myResult = PickupPackagesTabComponent_1.PickupPackagesTabComponent;
                break;
            }
            case "PickupDocsOutTabComponent": {
                myResult = PickupDocsOutTabComponent_1.PickupDocsOutTabComponent;
                break;
            }
            case "PickupDocsInTabComponent": {
                myResult = PickupDocsInTabComponent_1.PickupDocsInTabComponent;
                break;
            }
            case "PickupPackagesAddEditComponent": {
                myResult = PickupPackagesAddEditComponent_1.PickupPackagesAddEditComponent;
                break;
            }
            case "PickupPackagesChooseComponent": {
                myResult = PickupPackagesChooseComponent_1.PickupPackagesChooseComponent;
                break;
            }
            case "AddEditDeliveryComponent": {
                myResult = AddEditDeliveryComponent_1.AddEditDeliveryComponent;
                break;
            }
            case "DeliveryMainTabComponent": {
                myResult = DeliveryMainTabComponent_1.DeliveryMainTabComponent;
                break;
            }
            case "DeliveryPackagesTabComponent": {
                myResult = DeliveryPackagesTabComponent_1.DeliveryPackagesTabComponent;
                break;
            }
            case "DeliveryDocsOutTabComponent": {
                myResult = DeliveryDocsOutTabComponent_1.DeliveryDocsOutTabComponent;
                break;
            }
            case "DeliveryDocsInTabComponent": {
                myResult = DeliveryDocsInTabComponent_1.DeliveryDocsInTabComponent;
                break;
            }
            case "DeliveryPackagesAddEditComponent": {
                myResult = DeliveryPackagesAddEditComponent_1.DeliveryPackagesAddEditComponent;
                break;
            }
            case "DeliveryPackagesChooseComponent": {
                myResult = DeliveryPackagesChooseComponent_1.DeliveryPackagesChooseComponent;
                break;
            }
            case "DeliveryPackagesConnectComponent": {
                myResult = DeliveryPackagesConnectComponent_1.DeliveryPackagesConnectComponent;
                break;
            }
            case "AddEditPackageHarmonizeComponent": {
                myResult = AddEditPackageHarmonizeComponent_1.AddEditPackageHarmonizeComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map