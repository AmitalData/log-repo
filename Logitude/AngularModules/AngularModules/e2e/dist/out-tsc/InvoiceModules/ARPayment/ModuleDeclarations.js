"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var NewARPaymentComponent_1 = require("./Components/NewEntity/NewARPaymentComponent");
var ARPaymentGeneralTabComponent_1 = require("./Components/EditTabs/ARPaymentGeneralTabComponent");
var ARPaymentDetailsTabComponent_1 = require("./Components/EditTabs/ARPaymentDetailsTabComponent");
var ARPaymentDocsInTabComponent_1 = require("./Components/EditTabs/ARPaymentDocsInTabComponent");
var ARPaymentDocsOutTabComponent_1 = require("./Components/EditTabs/ARPaymentDocsOutTabComponent");
var ARPaymentTransferTabComponent_1 = require("./Components/EditTabs/ARPaymentTransferTabComponent");
var ARPaymentTransferTemplate_1 = require("./Components/NewEntity/ARPaymentTransferTemplate");
var EditMultiCurrency_1 = require("./Components/EditTabs/EditMultiCurrency");
var ARPaymentDetailsFullAccountingTab_1 = require("./Components/EditTabs/ARPaymentDetailsFullAccountingTab");
exports.Components = [
    NewARPaymentComponent_1.NewARPaymentComponent,
    ARPaymentDetailsTabComponent_1.ARPaymentDetailsTabComponent,
    ARPaymentDocsInTabComponent_1.ARPaymentDocsInTabComponent,
    ARPaymentDocsOutTabComponent_1.ARPaymentDocsOutTabComponent,
    ARPaymentTransferTabComponent_1.ARPaymentTransferTabComponent,
    ARPaymentTransferTemplate_1.ARPaymentTransferTemplate,
    EditMultiCurrency_1.EditMultiCurrency,
    ARPaymentGeneralTabComponent_1.ARPaymentGeneralTabComponent,
    ARPaymentDetailsFullAccountingTab_1.ARPaymentDetailsFullAccountingTab,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "NewARPaymentComponent": {
                myResult = NewARPaymentComponent_1.NewARPaymentComponent;
                break;
            }
            case "ARPaymentDetailsTabComponent": {
                myResult = ARPaymentDetailsTabComponent_1.ARPaymentDetailsTabComponent;
                break;
            }
            case "ARPaymentDocsInTabComponent": {
                myResult = ARPaymentDocsInTabComponent_1.ARPaymentDocsInTabComponent;
                break;
            }
            case "ARPaymentDocsOutTabComponent": {
                myResult = ARPaymentDocsOutTabComponent_1.ARPaymentDocsOutTabComponent;
                break;
            }
            case "ARPaymentTransferTabComponent": {
                myResult = ARPaymentTransferTabComponent_1.ARPaymentTransferTabComponent;
                break;
            }
            case "ARPaymentTransferTemplate": {
                myResult = ARPaymentTransferTemplate_1.ARPaymentTransferTemplate;
                break;
            }
            case "EditMultiCurrency": {
                myResult = EditMultiCurrency_1.EditMultiCurrency;
                break;
            }
            case "ARPaymentGeneralTabComponent": {
                myResult = ARPaymentGeneralTabComponent_1.ARPaymentGeneralTabComponent;
                break;
            }
            case "ARPaymentDetailsFullAccountingTab": {
                myResult = ARPaymentDetailsFullAccountingTab_1.ARPaymentDetailsFullAccountingTab;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map