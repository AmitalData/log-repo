"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CommunicationLogPMViewModel = /** @class */ (function () {
    function CommunicationLogPMViewModel(communicationLogPM) {
        this.Id = communicationLogPM.Id;
        this.Tenant = communicationLogPM.Tenant;
        this.To = communicationLogPM.To;
        this.CC = communicationLogPM.CC;
        this.Subject = communicationLogPM.Subject;
        this.CommunicationStatusTypeName = communicationLogPM.CommunicationStatusTypeName;
        this.CreateDate = communicationLogPM.CreateDate;
        this.DoneDate = communicationLogPM.DoneDate;
        this.CommunicationStatusTypeCode = communicationLogPM.CommunicationStatusTypeCode;
        this.EmailDeliveryError = communicationLogPM.EmailDeliveryError;
        this.CurrentEntityPm = communicationLogPM;
    }
    return CommunicationLogPMViewModel;
}());
exports.CommunicationLogPMViewModel = CommunicationLogPMViewModel;
//# sourceMappingURL=CommunicationLogPMViewModel.js.map