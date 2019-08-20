"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TMEmployeeTimeListService_1 = require("./Services/StandardLists/TMEmployeeTimeListService");
var TMProjectListService_1 = require("./Services/StandardLists/TMProjectListService");
var TMLocationListService_1 = require("./Services/StandardLists/TMLocationListService");
var TMOfficeHourListService_1 = require("./Services/StandardLists/TMOfficeHourListService");
var TMBudgetListService_1 = require("./Services/StandardLists/TMBudgetListService");
var TMProjectCategoryListService_1 = require("./Services/StandardLists/TMProjectCategoryListService");
var TMEmployeeTimePMService_1 = require("./Services/StandardPMs/TMEmployeeTimePMService");
var TMOfficeHourPMService_1 = require("./Services/StandardPMs/TMOfficeHourPMService");
var TMProjectPMService_1 = require("./Services/StandardPMs/TMProjectPMService");
var TMBudgetPMService_1 = require("./Services/StandardPMs/TMBudgetPMService");
var TMProjectCategoryPMService_1 = require("./Services/StandardPMs/TMProjectCategoryPMService");
var SprintListService_1 = require("./Services/StandardLists/SprintListService");
var SprintPMService_1 = require("./Services/StandardPMs/SprintPMService");
var TMDayOffTypeListService_1 = require("./Services/StandardLists/TMDayOffTypeListService");
var ModuleProviders = /** @class */ (function () {
    function ModuleProviders() {
    }
    ModuleProviders.GetInstance = function (name) {
        var myResult = null;
        switch (name) {
            case "TMEmployeeTimeListService": {
                myResult = new TMEmployeeTimeListService_1.TMEmployeeTimeListService();
                break;
            }
            case "TMProjectListService": {
                myResult = new TMProjectListService_1.TMProjectListService();
                break;
            }
            case "TMLocationListService": {
                myResult = new TMLocationListService_1.TMLocationListService();
                break;
            }
            case "TMOfficeHourListService": {
                myResult = new TMOfficeHourListService_1.TMOfficeHourListService();
                break;
            }
            case "TMBudgetListService": {
                myResult = new TMBudgetListService_1.TMBudgetListService();
                break;
            }
            case "TMProjectCategoryListService": {
                myResult = new TMProjectCategoryListService_1.TMProjectCategoryListService();
                break;
            }
            case "TMEmployeeTimePMService": {
                myResult = new TMEmployeeTimePMService_1.TMEmployeeTimePMService();
                break;
            }
            case "TMOfficeHourPMService": {
                myResult = new TMOfficeHourPMService_1.TMOfficeHourPMService();
                break;
            }
            case "TMProjectPMService": {
                myResult = new TMProjectPMService_1.TMProjectPMService();
                break;
            }
            case "TMBudgetPMService": {
                myResult = new TMBudgetPMService_1.TMBudgetPMService();
                break;
            }
            case "TMProjectCategoryPMService": {
                myResult = new TMProjectCategoryPMService_1.TMProjectCategoryPMService();
                break;
            }
            case "SprintListService": {
                myResult = new SprintListService_1.SprintListService();
                break;
            }
            case "SprintPMService": {
                myResult = new SprintPMService_1.SprintPMService();
                break;
            }
            case "TMDayOffTypeListService": {
                myResult = new TMDayOffTypeListService_1.TMDayOffTypeListService();
                break;
            }
        }
        return myResult;
    };
    return ModuleProviders;
}());
exports.ModuleProviders = ModuleProviders;
//# sourceMappingURL=ModuleProviders.js.map