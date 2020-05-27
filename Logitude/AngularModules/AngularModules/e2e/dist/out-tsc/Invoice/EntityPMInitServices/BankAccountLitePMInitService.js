"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var BankAccountLitePMInitService = /** @class */ (function () {
    function BankAccountLitePMInitService() {
    }
    BankAccountLitePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
        }
    };
    BankAccountLitePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (!isNew) {
            if (!SessionLocator_1.SessionLocator.TenantPM.AccountingActivated) {
                entityPM.UIProperties.SetRequired("BranchNumber", "BankAccountLite", false);
            }
        }
    };
    return BankAccountLitePMInitService;
}());
exports.BankAccountLitePMInitService = BankAccountLitePMInitService;
//# sourceMappingURL=BankAccountLitePMInitService.js.map