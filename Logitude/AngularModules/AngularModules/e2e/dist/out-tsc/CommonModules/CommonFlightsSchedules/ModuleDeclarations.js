"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var FVASimulatorComponent_1 = require("./Components/FlightsSchedules/FVASimulatorComponent");
var FlightsSchedulesComponent_1 = require("./Components/FlightsSchedules/FlightsSchedulesComponent");
var XMLFlightsSimulatorComponent_1 = require("./Components/FlightsSchedules/XMLFlightsSimulatorComponent");
exports.Components = [
    FVASimulatorComponent_1.FVASimulatorComponent,
    FlightsSchedulesComponent_1.FlightsSchedulesComponent,
    XMLFlightsSimulatorComponent_1.XMLFlightsSimulatorComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "FVASimulatorComponent": {
                myResult = FVASimulatorComponent_1.FVASimulatorComponent;
                break;
            }
            case "FlightsSchedulesComponent": {
                myResult = FlightsSchedulesComponent_1.FlightsSchedulesComponent;
                break;
            }
            case "XMLFlightsSimulatorComponent": {
                myResult = XMLFlightsSimulatorComponent_1.XMLFlightsSimulatorComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map