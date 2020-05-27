"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewClientComponent_1 = require("./Components/NewClient/NewClientComponent");
var ClientEditComponent_1 = require("./Components/EditTabs/ClientEditComponent");
var ClientGeneralTabComponent_1 = require("./Components/EditTabs/General/ClientGeneralTabComponent");
var ClientAddressesTabComponent_1 = require("./Components/EditTabs/Addresses/ClientAddressesTabComponent");
var AddEditAddressComponent_1 = require("./Components/EditTabs/Addresses/AddEditAddressComponent");
var ClientDrivingLicenseTabComponent_1 = require("./Components/EditTabs/License/ClientDrivingLicenseTabComponent");
exports.Components = [
    NewClientComponent_1.NewClientComponent,
    ClientEditComponent_1.ClientEditComponent,
    ClientGeneralTabComponent_1.ClientGeneralTabComponent,
    ClientAddressesTabComponent_1.ClientAddressesTabComponent,
    AddEditAddressComponent_1.AddEditAddressComponent,
    ClientDrivingLicenseTabComponent_1.ClientDrivingLicenseTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewClientComponent": {
                myResult = NewClientComponent_1.NewClientComponent;
                break;
            }
            case "ClientEditComponent": {
                myResult = ClientEditComponent_1.ClientEditComponent;
                break;
            }
            case "ClientGeneralTabComponent": {
                myResult = ClientGeneralTabComponent_1.ClientGeneralTabComponent;
                break;
            }
            case "ClientAddressesTabComponent": {
                myResult = ClientAddressesTabComponent_1.ClientAddressesTabComponent;
                break;
            }
            case "AddEditAddressComponent": {
                myResult = AddEditAddressComponent_1.AddEditAddressComponent;
                break;
            }
            case "ClientDrivingLicenseTabComponent": {
                myResult = ClientDrivingLicenseTabComponent_1.ClientDrivingLicenseTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map