"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var ARPaymentPMCustomCode = /** @class */ (function () {
    function ARPaymentPMCustomCode() {
    }
    ARPaymentPMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", false);
        if (SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator_1.SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", true);
            }
            else
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", false);
            if (Tools_1.AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", true);
            }
            else
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", false);
            if (!Tools_1.AppTool.IsNullOrEmpty(entityPM.TipoCadenaPago) && entityPM.TipoCadenaPago == "01" && entityPM.SATPaymentMethodCode == "03") {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.CertPago))
                    entityPM.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("CertPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.CadPago))
                    entityPM.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("CadPago", "ARPayment", false);
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.SelloPago))
                    entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                entityPM.UIProperties.SetRequired("CertPago", "ARPayment", false);
                entityPM.UIProperties.SetRequired("CadPago", "ARPayment", false);
                entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    };
    return ARPaymentPMCustomCode;
}());
exports.ARPaymentPMCustomCode = ARPaymentPMCustomCode;
//# sourceMappingURL=ARPaymentPMCustomCode.js.map