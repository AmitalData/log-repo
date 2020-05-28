"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AddEditComputingPartnerComponent_1 = require("./Components/AddEditComputingPartnerComponent");
var NewComputingPartnerConmponent_1 = require("./Components/NewComputingPartnerConmponent");
var ComputingPartnerGeneralTabComponent_1 = require("./Components/ComputingPartnerGeneralTabComponent");
var ComputingPartnerTranslateComponent_1 = require("./Components/ComputingPartnerTranslateComponent");
var TranslationDetailsComponent_1 = require("./Components/TranslationDetailsComponent");
var EditTranslationComputingPartners_1 = require("./Components/EditTranslationComputingPartners");
var btnComponentComputingPartner_1 = require("./Components/QueryColumnsComponents/btnComponentComputingPartner");
var btnComponentComputingPartnerEdit_1 = require("./Components/QueryColumnsComponents/btnComponentComputingPartnerEdit");
exports.Components = [
    AddEditComputingPartnerComponent_1.AddEditComputingPartnerComponent,
    NewComputingPartnerConmponent_1.NewComputingPartnerConmponent,
    ComputingPartnerGeneralTabComponent_1.ComputingPartnerGeneralTabComponent,
    ComputingPartnerTranslateComponent_1.ComputingPartnerTranslateComponent,
    TranslationDetailsComponent_1.TranslationDetailsComponent,
    EditTranslationComputingPartners_1.EditTranslationComputingPartners,
    btnComponentComputingPartner_1.btnComponentComputingPartner,
    btnComponentComputingPartnerEdit_1.btnComponentComputingPartnerEdit,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "AddEditComputingPartnerComponent": {
                myResult = AddEditComputingPartnerComponent_1.AddEditComputingPartnerComponent;
                break;
            }
            case "NewComputingPartnerConmponent": {
                myResult = NewComputingPartnerConmponent_1.NewComputingPartnerConmponent;
                break;
            }
            case "ComputingPartnerGeneralTabComponent": {
                myResult = ComputingPartnerGeneralTabComponent_1.ComputingPartnerGeneralTabComponent;
                break;
            }
            case "ComputingPartnerTranslateComponent": {
                myResult = ComputingPartnerTranslateComponent_1.ComputingPartnerTranslateComponent;
                break;
            }
            case "TranslationDetailsComponent": {
                myResult = TranslationDetailsComponent_1.TranslationDetailsComponent;
                break;
            }
            case "EditTranslationComputingPartners": {
                myResult = EditTranslationComputingPartners_1.EditTranslationComputingPartners;
                break;
            }
            case "btnComponentComputingPartner": {
                myResult = btnComponentComputingPartner_1.btnComponentComputingPartner;
                break;
            }
            case "btnComponentComputingPartnerEdit": {
                myResult = btnComponentComputingPartnerEdit_1.btnComponentComputingPartnerEdit;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map