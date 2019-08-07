"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var ARPaymentPMInitService = /** @class */ (function () {
    function ARPaymentPMInitService() {
    }
    ARPaymentPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            entityPM.SATTransferStatusCode = "NT";
            entityPM.SATTransferStatusName = "Not Transfered";
        }
    };
    ARPaymentPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        entityPM.UIProperties.SetEnabled("UpdateDate", "ARPayment", false);
        entityPM.UIProperties.SetEnabled("UpdatedByUserId", "ARPayment", false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", true);
            }
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", true);
            }
        }
    };
    return ARPaymentPMInitService;
}());
exports.ARPaymentPMInitService = ARPaymentPMInitService;
//# sourceMappingURL=ARPaymentPMInitService.js.map