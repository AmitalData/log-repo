"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var PhysicalCheckGeneralTabComponent_1 = require("./Components/EditTabs/General/PhysicalCheckGeneralTabComponent");
exports.Components = [
    PhysicalCheckGeneralTabComponent_1.PhysicalCheckGeneralTabComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "PhysicalCheckGeneralTabComponent": {
                myResult = PhysicalCheckGeneralTabComponent_1.PhysicalCheckGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map