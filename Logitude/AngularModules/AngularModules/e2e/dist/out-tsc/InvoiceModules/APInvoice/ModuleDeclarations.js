"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewAPInvoiceComponent_1 = require("./Components/NewEntity/NewAPInvoiceComponent");
var APInvoiceDetailsTabComponent_1 = require("./Components/EditTabs/APInvoiceDetailsTabComponent");
var APInvoiceDocsInTabComponent_1 = require("./Components/EditTabs/APInvoiceDocsInTabComponent");
var APInvoiceDocsOutTabComponent_1 = require("./Components/EditTabs/APInvoiceDocsOutTabComponent");
var APInvoicePaymentsTabComponent_1 = require("./Components/EditTabs/APInvoicePaymentsTabComponent");
var APInvoiceTransferTabComponent_1 = require("./Components/EditTabs/APInvoiceTransferTabComponent");
var APInvoiceDetailsTabNormal_1 = require("./Components/EditTabs/APInvoiceDetailsTabNormal");
var AddEditAPInvoiceLineComponent_1 = require("./Components/EditTabs/AddEditAPInvoiceLineComponent");
var APInvoiceMultipleDetailsTabComponent_1 = require("./Components/EditTabs/APInvoiceMultipleDetailsTabComponent");
var EditMultipleShipmentComponent_1 = require("./Components/EditTabs/EditMultipleShipmentComponent");
var AddEditMultipleAPInvoiceLineComponent_1 = require("./Components/EditTabs/AddEditMultipleAPInvoiceLineComponent");
var APInvoiceTransferTemplate_1 = require("./Components/NewEntity/APInvoiceTransferTemplate");
var NewGeneralAPInvoiceComponent_1 = require("./Components/NewEntity/NewGeneralAPInvoiceComponent");
var APInvoiceDetailsTabGeneral_1 = require("./Components/EditTabs/APInvoiceDetailsTabGeneral");
var AddEditAPGeneralInvoiceLineComponent_1 = require("./Components/EditTabs/AddEditAPGeneralInvoiceLineComponent");
exports.Components = [
    NewAPInvoiceComponent_1.NewAPInvoiceComponent,
    APInvoiceDetailsTabComponent_1.APInvoiceDetailsTabComponent,
    APInvoiceMultipleDetailsTabComponent_1.APInvoiceMultipleDetailsTabComponent,
    EditMultipleShipmentComponent_1.EditMultipleShipmentComponent,
    AddEditMultipleAPInvoiceLineComponent_1.AddEditMultipleAPInvoiceLineComponent,
    APInvoiceDocsInTabComponent_1.APInvoiceDocsInTabComponent,
    APInvoiceDocsOutTabComponent_1.APInvoiceDocsOutTabComponent,
    APInvoicePaymentsTabComponent_1.APInvoicePaymentsTabComponent,
    APInvoiceTransferTabComponent_1.APInvoiceTransferTabComponent,
    APInvoiceDetailsTabNormal_1.APInvoiceDetailsTabNormal,
    AddEditAPInvoiceLineComponent_1.AddEditAPInvoiceLineComponent,
    APInvoiceTransferTemplate_1.APInvoiceTransferTemplate,
    NewGeneralAPInvoiceComponent_1.NewGeneralAPInvoiceComponent,
    APInvoiceDetailsTabGeneral_1.APInvoiceDetailsTabGeneral,
    AddEditAPGeneralInvoiceLineComponent_1.AddEditAPGeneralInvoiceLineComponent,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewAPInvoiceComponent": {
                myResult = NewAPInvoiceComponent_1.NewAPInvoiceComponent;
                break;
            }
            case "APInvoiceDetailsTabComponent": {
                myResult = APInvoiceDetailsTabComponent_1.APInvoiceDetailsTabComponent;
                break;
            }
            case "APInvoiceMultipleDetailsTabComponent": {
                myResult = APInvoiceMultipleDetailsTabComponent_1.APInvoiceMultipleDetailsTabComponent;
                break;
            }
            case "EditMultipleShipmentComponent": {
                myResult = EditMultipleShipmentComponent_1.EditMultipleShipmentComponent;
                break;
            }
            case "AddEditMultipleAPInvoiceLineComponent": {
                myResult = AddEditMultipleAPInvoiceLineComponent_1.AddEditMultipleAPInvoiceLineComponent;
                break;
            }
            case "APInvoiceDocsInTabComponent": {
                myResult = APInvoiceDocsInTabComponent_1.APInvoiceDocsInTabComponent;
                break;
            }
            case "APInvoiceDocsOutTabComponent": {
                myResult = APInvoiceDocsOutTabComponent_1.APInvoiceDocsOutTabComponent;
                break;
            }
            case "APInvoicePaymentsTabComponent": {
                myResult = APInvoicePaymentsTabComponent_1.APInvoicePaymentsTabComponent;
                break;
            }
            case "APInvoiceTransferTabComponent": {
                myResult = APInvoiceTransferTabComponent_1.APInvoiceTransferTabComponent;
                break;
            }
            case "APInvoiceDetailsTabNormal": {
                myResult = APInvoiceDetailsTabNormal_1.APInvoiceDetailsTabNormal;
                break;
            }
            case "AddEditAPInvoiceLineComponent": {
                myResult = AddEditAPInvoiceLineComponent_1.AddEditAPInvoiceLineComponent;
                break;
            }
            case "APInvoiceTransferTemplate": {
                myResult = APInvoiceTransferTemplate_1.APInvoiceTransferTemplate;
                break;
            }
            case "NewGeneralAPInvoiceComponent": {
                myResult = NewGeneralAPInvoiceComponent_1.NewGeneralAPInvoiceComponent;
                break;
            }
            case "APInvoiceDetailsTabGeneral": {
                myResult = APInvoiceDetailsTabGeneral_1.APInvoiceDetailsTabGeneral;
                break;
            }
            case "AddEditAPGeneralInvoiceLineComponent": {
                myResult = AddEditAPGeneralInvoiceLineComponent_1.AddEditAPGeneralInvoiceLineComponent;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map