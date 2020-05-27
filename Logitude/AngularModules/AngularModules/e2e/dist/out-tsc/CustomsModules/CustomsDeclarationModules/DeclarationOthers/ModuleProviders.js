"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SendDeclarationComponent_1 = require("./Components/SendDeclaration/SendDeclarationComponent");
var SendManifestComponent_1 = require("./Components/SendDeclaration/SendManifestComponent");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "SendDeclarationService": {
                myResult = new SendDeclarationComponent_1.SendDeclarationService();
                break;
            }
            case "SendManifestService": {
                myResult = new SendManifestComponent_1.SendManifestService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map