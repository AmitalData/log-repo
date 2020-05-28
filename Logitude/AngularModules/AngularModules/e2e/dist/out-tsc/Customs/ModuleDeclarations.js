"use strict";
// Edit Tabs
Object.defineProperty(exports, "__esModule", { value: true });
var FieldTemplateComponent_1 = require("./Components/Templates/FieldTemplateComponent");
var DocumentsFilingTemplateComponent_1 = require("./Components/Templates/DocumentsFilingTemplateComponent");
var EndDateComponent_1 = require("./Components/ListTemplates/EndDateComponent");
//Short Titles
var DeclarationShortTitleComponent_1 = require("./Components/ShortTitles/DeclarationShortTitleComponent");
//PhysicalCheck
var CustomsSpotlightComponent_1 = require("./Components/Spotlight/CustomsSpotlightComponent");
exports.CustomsControlsComponents = [];
exports.Components = [
    FieldTemplateComponent_1.FieldTemplateComponent,
    DocumentsFilingTemplateComponent_1.DocumentsFilingTemplateComponent,
    EndDateComponent_1.EndDateComponent,
    //Controls
    //short titles
    DeclarationShortTitleComponent_1.DeclarationShortTitleComponent,
    //PhysicalCheck
    CustomsSpotlightComponent_1.CustomsSpotlightComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            // New Entity
            case "FieldTemplateComponent": {
                myResult = FieldTemplateComponent_1.FieldTemplateComponent;
                break;
            }
            case "DocumentsFilingTemplateComponent": {
                myResult = DocumentsFilingTemplateComponent_1.DocumentsFilingTemplateComponent;
                break;
            }
            case "EndDateComponent": {
                myResult = EndDateComponent_1.EndDateComponent;
                break;
            }
            //short titles
            case "DeclarationShortTitleComponent": {
                myResult = DeclarationShortTitleComponent_1.DeclarationShortTitleComponent;
                break;
            }
            case "CustomsSpotlightComponent": {
                myResult = CustomsSpotlightComponent_1.CustomsSpotlightComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map