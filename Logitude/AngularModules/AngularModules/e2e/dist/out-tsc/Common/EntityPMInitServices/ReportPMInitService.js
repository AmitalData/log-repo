"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ReportPMInitService = /** @class */ (function () {
    function ReportPMInitService() {
    }
    ReportPMInitService.InitValues = function (entityPM, isNew) {
    };
    ReportPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("Code", "Report", false);
        }
    };
    return ReportPMInitService;
}());
exports.ReportPMInitService = ReportPMInitService;
//# sourceMappingURL=ReportPMInitService.js.map