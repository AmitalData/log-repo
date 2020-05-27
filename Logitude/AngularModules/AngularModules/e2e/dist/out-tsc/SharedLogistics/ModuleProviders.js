"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SharedLogisticsService_1 = require("./Services/Others/SharedLogisticsService");
var SharedLogisticContactService_1 = require("./Services/ExtendedPMs/SharedLogisticContactService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "SharedLogisticsService": {
                myResult = new SharedLogisticsService_1.SharedLogisticsService();
                break;
            }
            case "SharedLogisticContactService": {
                myResult = new SharedLogisticContactService_1.SharedLogisticContactService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map