"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsWizardComponent_1 = require("./Components/CustomsWizard/CustomsWizardComponent");
var ArtemusWizardComponent_1 = require("./Components/ArtemusWizard/ArtemusWizardComponent");
var SentToCustomComponent_1 = require("./Components/SentToCustomComponent/SentToCustomComponent");
var SentToCustomLinkComponent_1 = require("./Components/SentToCustomComponent/SentToCustomLinkComponent");
exports.Components = [
    CustomsWizardComponent_1.CustomsWizardComponent,
    SentToCustomComponent_1.SentToCustomComponent,
    SentToCustomLinkComponent_1.SentToCustomLinkComponent,
    ArtemusWizardComponent_1.ArtemusWizardComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsWizardComponent": {
                myResult = CustomsWizardComponent_1.CustomsWizardComponent;
                break;
            }
            case "SentToCustomComponent": {
                myResult = SentToCustomComponent_1.SentToCustomComponent;
                break;
            }
            case "SentToCustomLinkComponent": {
                myResult = SentToCustomLinkComponent_1.SentToCustomLinkComponent;
                break;
            }
            case "ArtemusWizardComponent": {
                myResult = ArtemusWizardComponent_1.ArtemusWizardComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map