"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var APInvoicePMCustomCode = /** @class */ (function () {
    function APInvoicePMCustomCode() {
    }
    APInvoicePMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        if (SessionLocator_1.SessionLocator.TenantPM.AccountingActivated == true) {
            entityPM.UIProperties.SetVisibility("AccountingDate", "APInvoice", true);
        }
        else {
            entityPM.UIProperties.SetVisibility("AccountingDate", "APInvoice", false);
        }
    };
    return APInvoicePMCustomCode;
}());
exports.APInvoicePMCustomCode = APInvoicePMCustomCode;
//# sourceMappingURL=APInvoicePMCustomCode.js.map