"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WarehouseEntryListService_1 = require("./Services/StandardLists/WarehouseEntryListService");
var WarehouseEntryPackagesReleaseListService_1 = require("./Services/StandardLists/WarehouseEntryPackagesReleaseListService");
var WarehouseEntryStatusListService_1 = require("./Services/StandardLists/WarehouseEntryStatusListService");
var WarehouseReleaseListService_1 = require("./Services/StandardLists/WarehouseReleaseListService");
var WarehouseReleaseStatusListService_1 = require("./Services/StandardLists/WarehouseReleaseStatusListService");
var WarehouseEntryPackagesReleasePMService_1 = require("./Services/StandardPMs/WarehouseEntryPackagesReleasePMService");
var WarehouseEntryPMService_1 = require("./Services/StandardPMs/WarehouseEntryPMService");
var WarehouseReleasePMService_1 = require("./Services/StandardPMs/WarehouseReleasePMService");
// Menu Buttons 
var WarehouseReleaseMenuButtonsHandler_1 = require("./Components/MenuButtons/WarehouseReleaseMenuButtonsHandler");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "WarehouseEntryPackagesReleasePMService": {
                myResult = new WarehouseEntryPackagesReleasePMService_1.WarehouseEntryPackagesReleasePMService();
                break;
            }
            case "WarehouseEntryPMService": {
                myResult = new WarehouseEntryPMService_1.WarehouseEntryPMService();
                break;
            }
            case "WarehouseReleasePMService": {
                myResult = new WarehouseReleasePMService_1.WarehouseReleasePMService();
                break;
            }
            case "WarehouseEntryListService": {
                myResult = new WarehouseEntryListService_1.WarehouseEntryListService();
                break;
            }
            case "WarehouseEntryPackagesReleaseListService": {
                myResult = new WarehouseEntryPackagesReleaseListService_1.WarehouseEntryPackagesReleaseListService();
                break;
            }
            case "WarehouseEntryStatusListService": {
                myResult = new WarehouseEntryStatusListService_1.WarehouseEntryStatusListService();
                break;
            }
            case "WarehouseReleaseListService": {
                myResult = new WarehouseReleaseListService_1.WarehouseReleaseListService();
                break;
            }
            case "WarehouseReleaseStatusListService": {
                myResult = new WarehouseReleaseStatusListService_1.WarehouseReleaseStatusListService();
                break;
            }
            case "WarehouseReleaseMenuButtonsHandler": {
                myResult = new WarehouseReleaseMenuButtonsHandler_1.WarehouseReleaseMenuButtonsHandler();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map