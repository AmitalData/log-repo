"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DashboardComponent_1 = require("../Dashboard/Components/Workspace/DashboardComponent");
var ActivityStatusDetailsComponent_1 = require("../Dashboard/Components/Workspace/ActivityStatusDetailsComponent");
var AirLineDashboardComponent_1 = require("../Dashboard/Components/Workspace/AirLineDashboardComponent");
exports.Components = [
    DashboardComponent_1.DashboardComponent,
    ActivityStatusDetailsComponent_1.ActivityStatusDetailsComponent,
    AirLineDashboardComponent_1.AirLineDashboardComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "DashboardComponent": {
                myResult = DashboardComponent_1.DashboardComponent;
                break;
            }
            case "ActivityStatusDetailsComponent": {
                myResult = ActivityStatusDetailsComponent_1.ActivityStatusDetailsComponent;
                break;
            }
            case "AirLineDashboardComponent": {
                myResult = AirLineDashboardComponent_1.AirLineDashboardComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map