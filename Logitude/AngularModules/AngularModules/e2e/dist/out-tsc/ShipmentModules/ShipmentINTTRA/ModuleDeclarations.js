"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var WizardComponent_1 = require("./Components/Wizard/WizardComponent");
var SimulatorComponent_1 = require("./Components/Wizard/SimulatorComponent");
var INTTRASettingsComponent_1 = require("./Components/Maintenance/INTTRASettingsComponent");
var INTTRACommunicationSettingsComponent_1 = require("./Components/Maintenance/INTTRACommunicationSettingsComponent");
var SimulatorBookingComponent_1 = require("./Components/Wizard/SimulatorBookingComponent");
exports.Components = [
    WizardComponent_1.WizardComponent,
    SimulatorComponent_1.SimulatorComponent,
    INTTRASettingsComponent_1.INTTRASettingsComponent,
    INTTRACommunicationSettingsComponent_1.INTTRACommunicationSettingsComponent,
    SimulatorBookingComponent_1.SimulatorBookingComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "WizardComponent": {
                myResult = WizardComponent_1.WizardComponent;
                break;
            }
            case "SimulatorComponent": {
                myResult = SimulatorComponent_1.SimulatorComponent;
                break;
            }
            case "INTTRASettingsComponent": {
                myResult = INTTRASettingsComponent_1.INTTRASettingsComponent;
                break;
            }
            case "INTTRACommunicationSettingsComponent": {
                myResult = INTTRACommunicationSettingsComponent_1.INTTRACommunicationSettingsComponent;
                break;
            }
            case "SimulatorBookingComponent": {
                myResult = SimulatorBookingComponent_1.SimulatorBookingComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map