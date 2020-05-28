"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TariffTypeListService_1 = require("./Services/StandardLists/TariffTypeListService");
var TariffListService_1 = require("./Services/StandardLists/TariffListService");
var TariffPMService_1 = require("./Services/StandardPMs/TariffPMService");
var TariffDomainService_1 = require("./Services/TariffDomainService");
var TariffMenuButtonsHandler_1 = require("./Components/MenuButtons/TariffMenuButtonsHandler");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            // List
            case "TariffListService": {
                myResult = new TariffListService_1.TariffListService();
                break;
            }
            case "TariffTypeListService": {
                myResult = new TariffTypeListService_1.TariffTypeListService();
                break;
            }
            // PM
            case "TariffPMService": {
                myResult = new TariffPMService_1.TariffPMService();
                break;
            }
            // DomainService
            case "TariffDomainService": {
                myResult = new TariffDomainService_1.TariffDomainService();
                break;
            }
            // MenuButtons
            case "TariffMenuButtonsHandler": {
                myResult = new TariffMenuButtonsHandler_1.TariffMenuButtonsHandler();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map