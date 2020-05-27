"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var OpenFormatReportPMCustomCode = /** @class */ (function () {
    function OpenFormatReportPMCustomCode() {
    }
    OpenFormatReportPMCustomCode.ApplyEntityChanged = function (propertyName, entityPM) {
        entityPM.UIProperties.SetEnabled("FromDate", "OpenFormatReport", false);
        entityPM.UIProperties.SetEnabled("ToDate", "OpenFormatReport", false);
    };
    return OpenFormatReportPMCustomCode;
}());
exports.OpenFormatReportPMCustomCode = OpenFormatReportPMCustomCode;
//# sourceMappingURL=OpenFormatReportPMCustomCode.js.map