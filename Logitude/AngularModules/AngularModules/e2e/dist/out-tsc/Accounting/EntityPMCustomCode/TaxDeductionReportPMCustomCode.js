"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var TaxDeductionReportPMCustomCode = /** @class */ (function () {
    function TaxDeductionReportPMCustomCode() {
    }
    TaxDeductionReportPMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        entityPM.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);
        entityPM.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);
    };
    return TaxDeductionReportPMCustomCode;
}());
exports.TaxDeductionReportPMCustomCode = TaxDeductionReportPMCustomCode;
//# sourceMappingURL=TaxDeductionReportPMCustomCode.js.map