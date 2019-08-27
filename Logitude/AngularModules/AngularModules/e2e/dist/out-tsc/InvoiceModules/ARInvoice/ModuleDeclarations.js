"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewARInvoiceComponent_1 = require("./Components/NewEntity/NewARInvoiceComponent");
var NewConsolidationComponent_1 = require("./Components/NewEntity/NewConsolidationComponent");
var NewGeneralARInvoiceComponent_1 = require("./Components/NewEntity/NewGeneralARInvoiceComponent");
var ARInvoiceDetailsTabComponent_1 = require("./Components/EditTabs/ARInvoiceDetailsTabComponent");
var ARInvoiceDetailsTabNormal_1 = require("./Components/EditTabs/ARInvoiceDetailsTabNormal");
var ARInvoiceDetailsTabConsolidation_1 = require("./Components/EditTabs/ARInvoiceDetailsTabConsolidation");
var ARInvoiceDocsInTabComponent_1 = require("./Components/EditTabs/ARInvoiceDocsInTabComponent");
var ARInvoiceDocsOutTabComponent_1 = require("./Components/EditTabs/ARInvoiceDocsOutTabComponent");
var ARInvoicePaymentsTabComponent_1 = require("./Components/EditTabs/ARInvoicePaymentsTabComponent");
var ARInvoiceTransferTabComponent_1 = require("./Components/EditTabs/ARInvoiceTransferTabComponent");
var AddEditARInvoiceLineComponent_1 = require("./Components/EditTabs/AddEditARInvoiceLineComponent");
var ARInvoiceDetailsTabGeneral_1 = require("./Components/EditTabs/ARInvoiceDetailsTabGeneral");
var AddEditARGeneralInvoiceLineComponent_1 = require("./Components/EditTabs/AddEditARGeneralInvoiceLineComponent");
var ARInvoiceTransferTemplate_1 = require("./Components/NewEntity/ARInvoiceTransferTemplate");
var ARInvoiceGeneralTabComponent_1 = require("./Components/EditTabs/ARInvoiceGeneralTabComponent");
exports.Components = [
    NewARInvoiceComponent_1.NewARInvoiceComponent,
    NewConsolidationComponent_1.NewConsolidationComponent,
    NewGeneralARInvoiceComponent_1.NewGeneralARInvoiceComponent,
    ARInvoiceDetailsTabComponent_1.ARInvoiceDetailsTabComponent,
    ARInvoiceDetailsTabNormal_1.ARInvoiceDetailsTabNormal,
    ARInvoiceDetailsTabConsolidation_1.ARInvoiceDetailsTabConsolidation,
    ARInvoiceDocsInTabComponent_1.ARInvoiceDocsInTabComponent,
    ARInvoiceDocsOutTabComponent_1.ARInvoiceDocsOutTabComponent,
    ARInvoicePaymentsTabComponent_1.ARInvoicePaymentsTabComponent,
    ARInvoiceTransferTabComponent_1.ARInvoiceTransferTabComponent,
    AddEditARInvoiceLineComponent_1.AddEditARInvoiceLineComponent,
    ARInvoiceDetailsTabGeneral_1.ARInvoiceDetailsTabGeneral,
    AddEditARGeneralInvoiceLineComponent_1.AddEditARGeneralInvoiceLineComponent,
    ARInvoiceTransferTemplate_1.ARInvoiceTransferTemplate,
    ARInvoiceGeneralTabComponent_1.ARInvoiceGeneralTabComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewARInvoiceComponent": {
                myResult = NewARInvoiceComponent_1.NewARInvoiceComponent;
                break;
            }
            case "NewConsolidationComponent": {
                myResult = NewConsolidationComponent_1.NewConsolidationComponent;
                break;
            }
            case "NewGeneralARInvoiceComponent": {
                myResult = NewGeneralARInvoiceComponent_1.NewGeneralARInvoiceComponent;
                break;
            }
            case "ARInvoiceDetailsTabComponent": {
                myResult = ARInvoiceDetailsTabComponent_1.ARInvoiceDetailsTabComponent;
                break;
            }
            case "ARInvoiceDetailsTabNormal": {
                myResult = ARInvoiceDetailsTabNormal_1.ARInvoiceDetailsTabNormal;
                break;
            }
            case "ARInvoiceDetailsTabConsolidation": {
                myResult = ARInvoiceDetailsTabConsolidation_1.ARInvoiceDetailsTabConsolidation;
                break;
            }
            case "ARInvoiceDocsInTabComponent": {
                myResult = ARInvoiceDocsInTabComponent_1.ARInvoiceDocsInTabComponent;
                break;
            }
            case "ARInvoiceDocsOutTabComponent": {
                myResult = ARInvoiceDocsOutTabComponent_1.ARInvoiceDocsOutTabComponent;
                break;
            }
            case "ARInvoicePaymentsTabComponent": {
                myResult = ARInvoicePaymentsTabComponent_1.ARInvoicePaymentsTabComponent;
                break;
            }
            case "ARInvoiceTransferTabComponent": {
                myResult = ARInvoiceTransferTabComponent_1.ARInvoiceTransferTabComponent;
                break;
            }
            case "AddEditARInvoiceLineComponent": {
                myResult = AddEditARInvoiceLineComponent_1.AddEditARInvoiceLineComponent;
                break;
            }
            case "ARInvoiceDetailsTabGeneral": {
                myResult = ARInvoiceDetailsTabGeneral_1.ARInvoiceDetailsTabGeneral;
                break;
            }
            case "AddEditARGeneralInvoiceLineComponent": {
                myResult = AddEditARGeneralInvoiceLineComponent_1.AddEditARGeneralInvoiceLineComponent;
                break;
            }
            case "ARInvoiceTransferTemplate": {
                myResult = ARInvoiceTransferTemplate_1.ARInvoiceTransferTemplate;
                break;
            }
            case "ARInvoiceGeneralTabComponent": {
                myResult = ARInvoiceGeneralTabComponent_1.ARInvoiceGeneralTabComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map