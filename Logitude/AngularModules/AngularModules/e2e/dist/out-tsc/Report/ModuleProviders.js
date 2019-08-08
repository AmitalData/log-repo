"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReportsDomainService_1 = require("./Services/ReportsDomainService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "ReportsDomainService": {
                myResult = new ReportsDomainService_1.ReportsDomainService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map