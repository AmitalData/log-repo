"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomsDocumentsComponent_1 = require("./Components/CustomsDocumentsComponent");
var AddEditCustomsDocumentComponent_1 = require("./Components/AddEditCustomsDocumentComponent");
var DocumentsFilingsQueryComponent_1 = require("./Components/DocumentsFilingsQueryComponent");
exports.Components = [
    CustomsDocumentsComponent_1.CustomsDocumentsComponent,
    DocumentsFilingsQueryComponent_1.DocumentsFilingsQueryComponent,
    AddEditCustomsDocumentComponent_1.AddEditCustomsDocumentComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "CustomsDocumentsComponent": {
                myResult = CustomsDocumentsComponent_1.CustomsDocumentsComponent;
                break;
            }
            case "AddEditCustomsDocumentComponent": {
                myResult = AddEditCustomsDocumentComponent_1.AddEditCustomsDocumentComponent;
                break;
            }
            case "DocumentsFilingsQueryComponent": {
                myResult = DocumentsFilingsQueryComponent_1.DocumentsFilingsQueryComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map