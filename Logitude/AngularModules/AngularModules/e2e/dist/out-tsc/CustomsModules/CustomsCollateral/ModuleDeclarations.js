"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsCollateralComponent_1 = require("./Components/CustomsCollateralComponent");
var CustomsCollateralAnswerComponent_1 = require("./Components/CustomsCollateralAnswerComponent");
exports.Components = [
    CustomsCollateralComponent_1.CustomsCollateralComponent,
    CustomsCollateralAnswerComponent_1.CustomsCollateralAnswerComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsCollateralComponent": {
                myResult = CustomsCollateralComponent_1.CustomsCollateralComponent;
                break;
            }
            case "CustomsCollateralAnswerComponent": {
                myResult = CustomsCollateralAnswerComponent_1.CustomsCollateralAnswerComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map