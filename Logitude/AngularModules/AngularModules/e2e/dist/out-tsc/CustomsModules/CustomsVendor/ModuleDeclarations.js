"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var VendorGeneralTabComponent_1 = require("./Components/EditTabs/General/VendorGeneralTabComponent");
var VendorEditComponent_1 = require("./Components/EditTabs/VendorEditComponent");
var AddVendorCommunicationComponent_1 = require("./Components/EditTabs/General/AddVendorCommunicationComponent");
var NewVendorComponent_1 = require("./Components/NewEntity/NewVendorComponent");
exports.Components = [
    VendorGeneralTabComponent_1.VendorGeneralTabComponent,
    VendorEditComponent_1.VendorEditComponent,
    AddVendorCommunicationComponent_1.AddVendorCommunicationComponent,
    NewVendorComponent_1.NewVendorComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "VendorGeneralTabComponent": {
                myResult = VendorGeneralTabComponent_1.VendorGeneralTabComponent;
                break;
            }
            case "VendorEditComponent": {
                myResult = VendorEditComponent_1.VendorEditComponent;
                break;
            }
            case "AddVendorCommunicationComponent": {
                myResult = AddVendorCommunicationComponent_1.AddVendorCommunicationComponent;
                break;
            }
            case "NewVendorComponent": {
                myResult = NewVendorComponent_1.NewVendorComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map