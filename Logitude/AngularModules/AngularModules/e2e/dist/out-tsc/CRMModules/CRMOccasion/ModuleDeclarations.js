"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewOccasionComponent_1 = require("./Components/NewEntity/NewOccasionComponent");
var OccasionGeneralTabComponent_1 = require("./Components/EditTabs/OccasionGeneralTabComponent");
exports.Components = [
    NewOccasionComponent_1.NewOccasionComponent,
    OccasionGeneralTabComponent_1.OccasionGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewOccasionComponent": {
                myResult = NewOccasionComponent_1.NewOccasionComponent;
                break;
            }
            case "OccasionGeneralTabComponent": {
                myResult = OccasionGeneralTabComponent_1.OccasionGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map