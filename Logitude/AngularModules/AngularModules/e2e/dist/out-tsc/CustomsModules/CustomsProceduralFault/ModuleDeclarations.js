"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ProceduralFaultsGeneralTabComponent_1 = require("./Components/EditTabs/General/ProceduralFaultsGeneralTabComponent");
exports.Components = [
    ProceduralFaultsGeneralTabComponent_1.ProceduralFaultsGeneralTabComponent
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "ProceduralFaultsGeneralTabComponent": {
                myResult = ProceduralFaultsGeneralTabComponent_1.ProceduralFaultsGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map