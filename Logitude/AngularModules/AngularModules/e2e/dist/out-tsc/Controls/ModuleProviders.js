"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CRMControlsService_1 = require("./Services/CRMControlsService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "CRMControlsService": {
                myResult = new CRMControlsService_1.CRMControlsService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map