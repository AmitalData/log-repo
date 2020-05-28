"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DashboardDomainService_1 = require("./Services/DashboardDomainService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "DashboardDomainService": {
                myResult = new DashboardDomainService_1.DashboardDomainService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map