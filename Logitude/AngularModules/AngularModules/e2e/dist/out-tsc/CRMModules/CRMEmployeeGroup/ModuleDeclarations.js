"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var EmployeeGroupGeneralTabComponent_1 = require("./Components/EditTabs/EmployeeGroupGeneralTabComponent");
var NewEmployeeComponent_1 = require("./Components/NewEntity/NewEmployeeComponent");
exports.Components = [
    NewEmployeeComponent_1.NewEmployeeComponent,
    EmployeeGroupGeneralTabComponent_1.EmployeeGroupGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewEmployeeComponent": {
                myResult = NewEmployeeComponent_1.NewEmployeeComponent;
                break;
            }
            case "EmployeeGroupGeneralTabComponent": {
                myResult = EmployeeGroupGeneralTabComponent_1.EmployeeGroupGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map