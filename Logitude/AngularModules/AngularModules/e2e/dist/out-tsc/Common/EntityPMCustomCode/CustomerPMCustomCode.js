"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var CustomerPMCustomCode = /** @class */ (function () {
    function CustomerPMCustomCode() {
    }
    CustomerPMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        if (!SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
            entityPM.UIProperties.SetVisibility("CreditLimitAmount", "Customer", false);
        }
    };
    return CustomerPMCustomCode;
}());
exports.CustomerPMCustomCode = CustomerPMCustomCode;
//# sourceMappingURL=CustomerPMCustomCode.js.map