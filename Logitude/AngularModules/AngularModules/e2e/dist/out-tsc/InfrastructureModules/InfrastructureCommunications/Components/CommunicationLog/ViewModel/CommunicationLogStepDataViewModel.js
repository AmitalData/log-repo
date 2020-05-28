"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../../../../Infrastructure/Tools");
var CommunicationLogStepDataViewModel = /** @class */ (function () {
    function CommunicationLogStepDataViewModel(stepList) {
        this.StepList = stepList;
        this.StepNumber = stepList.StepNumber;
        this.Name = stepList.Name;
        this.CommunicationLogId = stepList.CommunicationLogId;
        this.Tenant = stepList.Tenant;
        this.Retries = stepList.Retries;
        if (Tools_1.AppTool.IsNullOrEmpty(stepList.StatusName)) {
            this.Status = this.GetCommunicationStatusTypesStatusName(stepList);
        }
        this.Log = stepList.Log;
        this.StartDate = stepList.StartDate;
        this.EndDate = stepList.EndDate;
        var t = new Date(stepList.EndDate).getTime() - new Date(stepList.StartDate).getTime();
        t = t / 1000;
        var n = parseFloat(t.toString());
        n = Math.round(n * 100) / 100;
        this.Duration = //t.toPrecision(2);
            parseFloat(n.toString()).toFixed(2);
        this.DocumentId = stepList.DocumentId;
        this.StatusName = stepList.StatusName;
    }
    CommunicationLogStepDataViewModel.prototype.GetCommunicationStatusTypesStatusName = function (step) {
        switch (step.Status) {
            case "W":
                return "Waiting";
            case "D":
                return "Done";
            case "F":
                return "Fail";
            case "P":
                return "I.Progress";
            default:
                break;
        }
        return "";
    };
    return CommunicationLogStepDataViewModel;
}());
exports.CommunicationLogStepDataViewModel = CommunicationLogStepDataViewModel;
//# sourceMappingURL=CommunicationLogStepDataViewModel.js.map