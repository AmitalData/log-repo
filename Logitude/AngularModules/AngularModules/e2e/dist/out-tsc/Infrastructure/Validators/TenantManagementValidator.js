"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../Tools");
var TextCodeTranslator_1 = require("../Utilities/TextCodeTranslator");
var TenantManagementValidator = /** @class */ (function () {
    function TenantManagementValidator() {
    }
    TenantManagementValidator.prototype.Validate = function (entityPM) {
        var msg = TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.FieldIsRequired");
        var errors = [];
        var objectTableName;
        var isInlandDomestic = false;
        if (entityPM != null) {
            if (entityPM.IsMultiPackage) {
                if (entityPM.TenantManagementLicenses.length == 0 && entityPM.AddOns.length == 0) {
                    errors.push("Multi Package tenant must have 1 package added at least");
                }
            }
            if (entityPM.IsTrial) {
                if (entityPM.TrialStartDate == null) {
                    errors.push("Trial Start Date is Required");
                }
                if (entityPM.TrialEndDate == null) {
                    errors.push("Trial End Date is Required");
                }
            }
            if (entityPM.IsRecurring) {
                if (entityPM.RecurringPeriodCode == null) {
                    errors.push("Recurring Period is Required");
                }
            }
            if (entityPM.TemporalPackageCode != null) {
                if (entityPM.TemporalStartDate == null) {
                    errors.push("Temporal Start Date is Required");
                }
                if (entityPM.TemporalEndDate == null) {
                    errors.push("Temporal End Date is Required");
                }
            }
            if (entityPM.AWBMessagesCCSTypeCode == "CHAMP") {
                //if (AppTool.IsNullOrEmpty(tenantMngmnt.TTY))
                //{
                //    errors.push("TTY field is Required");
                //}
            }
            else if (entityPM.AWBMessagesCCSTypeCode == "GLSHK") {
                if (Tools_1.AppTool.IsNullOrEmpty(entityPM.PIMA)) {
                    errors.push("PIMA field is Required");
                }
            }
            if (entityPM.TenantTypeCode == "AIR") {
            }
        }
        return errors;
    };
    return TenantManagementValidator;
}());
exports.TenantManagementValidator = TenantManagementValidator;
//# sourceMappingURL=TenantManagementValidator.js.map