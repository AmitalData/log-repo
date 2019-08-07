"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var StageGeneralTabComponent_1 = require("./Components/StageGeneralTabComponent");
exports.Components = [
    StageGeneralTabComponent_1.StageGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "StageGeneralTabComponent": {
                myResult = StageGeneralTabComponent_1.StageGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map