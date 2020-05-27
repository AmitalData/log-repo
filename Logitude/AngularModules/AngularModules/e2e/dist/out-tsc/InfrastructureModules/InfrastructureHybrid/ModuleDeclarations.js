"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewHybridPartnerComponent_1 = require("./Components/HypridPartner/NewHybridPartnerComponent");
var HybridPartnerTabComponent_1 = require("./Components/HypridPartner/HybridPartnerTabComponent");
var HybridPartnerUploadLogoComponent_1 = require("./Components/HypridPartner/HybridPartnerUploadLogoComponent");
var HybridTenantStateComponent_1 = require("./Components/HybridTenantState/HybridTenantStateComponent");
var PermissionsHybridPartnerTabComponent_1 = require("./Components/HypridPartner/PermissionsHybridPartnerTabComponent");
exports.Components = [
    NewHybridPartnerComponent_1.NewHybridPartnerComponent,
    HybridPartnerTabComponent_1.HybridPartnerTabComponent,
    HybridPartnerUploadLogoComponent_1.HybridPartnerUploadLogoComponent,
    HybridTenantStateComponent_1.HybridTenantStateComponent,
    PermissionsHybridPartnerTabComponent_1.PermissionsHybridPartnerTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewHybridPartnerComponent": {
                myResult = NewHybridPartnerComponent_1.NewHybridPartnerComponent;
                break;
            }
            case "HybridPartnerTabComponent": {
                myResult = HybridPartnerTabComponent_1.HybridPartnerTabComponent;
                break;
            }
            case "HybridPartnerUploadLogoComponent": {
                myResult = HybridPartnerUploadLogoComponent_1.HybridPartnerUploadLogoComponent;
                break;
            }
            case "HybridTenantStateComponent": {
                myResult = HybridTenantStateComponent_1.HybridTenantStateComponent;
                break;
            }
            case "PermissionsHybridPartnerTabComponent": {
                myResult = PermissionsHybridPartnerTabComponent_1.PermissionsHybridPartnerTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map