"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AddEditFCLChargeComponent_1 = require("./Components/AddEditFCLChargeComponent");
var AddEditLCLChargeComponent_1 = require("./Components/AddEditLCLChargeComponent");
var AddEditPriceStepComponent_1 = require("./Components/AddEditPriceStepComponent");
var ChargesTabComponent_1 = require("./Components/ChargesTabComponent");
var FCLChargesComponent_1 = require("./Components/FCLChargesComponent");
var LCLChargesComponent_1 = require("./Components/LCLChargesComponent");
var QuoteVATDetailsComponent_1 = require("./Components/QuoteVATDetailsComponent");
exports.Components = [
    AddEditFCLChargeComponent_1.AddEditFCLChargeComponent,
    AddEditLCLChargeComponent_1.AddEditLCLChargeComponent,
    AddEditPriceStepComponent_1.AddEditPriceStepComponent,
    ChargesTabComponent_1.ChargesTabComponent,
    FCLChargesComponent_1.FCLChargesComponent,
    LCLChargesComponent_1.LCLChargesComponent,
    QuoteVATDetailsComponent_1.QuoteVATDetailsComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "AddEditFCLChargeComponent": {
                myResult = AddEditFCLChargeComponent_1.AddEditFCLChargeComponent;
                break;
            }
            case "AddEditLCLChargeComponent": {
                myResult = AddEditLCLChargeComponent_1.AddEditLCLChargeComponent;
                break;
            }
            case "AddEditPriceStepComponent": {
                myResult = AddEditPriceStepComponent_1.AddEditPriceStepComponent;
                break;
            }
            case "ChargesTabComponent": {
                myResult = ChargesTabComponent_1.ChargesTabComponent;
                break;
            }
            case "FCLChargesComponent": {
                myResult = FCLChargesComponent_1.FCLChargesComponent;
                break;
            }
            case "LCLChargesComponent": {
                myResult = LCLChargesComponent_1.LCLChargesComponent;
                break;
            }
            case "QuoteVATDetailsComponent": {
                myResult = QuoteVATDetailsComponent_1.QuoteVATDetailsComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map