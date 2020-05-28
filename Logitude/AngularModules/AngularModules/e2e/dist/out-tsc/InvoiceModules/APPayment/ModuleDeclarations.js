"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var APPaymentDetailsTabComponent_1 = require("./Components/EditTabs/APPaymentDetailsTabComponent");
var APPaymentDocsInTabComponent_1 = require("./Components/EditTabs/APPaymentDocsInTabComponent");
var APPaymentDocsOutTabComponent_1 = require("./Components/EditTabs/APPaymentDocsOutTabComponent");
var APPaymentTransferTabComponent_1 = require("./Components/EditTabs/APPaymentTransferTabComponent");
var APPaymentTransferTemplate_1 = require("./Components/NewEntity/APPaymentTransferTemplate");
var APEditMultiCurrency_1 = require("./Components/EditTabs/APEditMultiCurrency");
exports.Components = [
    APPaymentTransferTabComponent_1.APPaymentTransferTabComponent,
    APPaymentTransferTemplate_1.APPaymentTransferTemplate,
    APPaymentDetailsTabComponent_1.APPaymentDetailsTabComponent,
    APPaymentDocsInTabComponent_1.APPaymentDocsInTabComponent,
    APPaymentDocsOutTabComponent_1.APPaymentDocsOutTabComponent,
    APEditMultiCurrency_1.APEditMultiCurrency,
];
var ModuleDeclarations = /** @class */ (function () {
    function ModuleDeclarations() {
    }
    ModuleDeclarations.Get = function (name) {
        var myResult = null;
        switch (name) {
            case "APPaymentDetailsTabComponent": {
                myResult = APPaymentDetailsTabComponent_1.APPaymentDetailsTabComponent;
                break;
            }
            case "APPaymentDocsInTabComponent": {
                myResult = APPaymentDocsInTabComponent_1.APPaymentDocsInTabComponent;
                break;
            }
            case "APPaymentDocsOutTabComponent": {
                myResult = APPaymentDocsOutTabComponent_1.APPaymentDocsOutTabComponent;
                break;
            }
            case "APPaymentTransferTabComponent": {
                myResult = APPaymentTransferTabComponent_1.APPaymentTransferTabComponent;
                break;
            }
            case "APPaymentTransferTemplate": {
                myResult = APPaymentTransferTemplate_1.APPaymentTransferTemplate;
                break;
            }
            case "APEditMultiCurrency": {
                myResult = APEditMultiCurrency_1.APEditMultiCurrency;
                break;
            }
        }
        return myResult;
    };
    return ModuleDeclarations;
}());
exports.ModuleDeclarations = ModuleDeclarations;
//# sourceMappingURL=ModuleDeclarations.js.map