"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var QuotationComponent_1 = require("./Components/Quotation/QuotationComponent");
var QuoteSettingsComponent_1 = require("./Components/Maintenance/QuoteSettingsComponent");
exports.Components = [
    QuotationComponent_1.QuotationComponent,
    QuoteSettingsComponent_1.QuoteSettingsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "QuotationComponent": {
                myResult = QuotationComponent_1.QuotationComponent;
                break;
            }
            case "QuoteSettingsComponent": {
                myResult = QuoteSettingsComponent_1.QuoteSettingsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map