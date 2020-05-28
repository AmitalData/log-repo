"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var ARInvoicePMCustomCode = /** @class */ (function () {
    function ARInvoicePMCustomCode() {
    }
    ARInvoicePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", true);
            }
            else
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", false);
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", true);
            }
            else
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", false);
        }
    };
    return ARInvoicePMCustomCode;
}());
exports.ARInvoicePMCustomCode = ARInvoicePMCustomCode;
//# sourceMappingURL=ARInvoicePMCustomCode.js.map