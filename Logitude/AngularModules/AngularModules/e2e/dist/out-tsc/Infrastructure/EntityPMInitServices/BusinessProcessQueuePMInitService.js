"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var BusinessProcessQueuePMInitService = /** @class */ (function () {
    function BusinessProcessQueuePMInitService() {
    }
    BusinessProcessQueuePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
    };
    BusinessProcessQueuePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return BusinessProcessQueuePMInitService;
}());
exports.BusinessProcessQueuePMInitService = BusinessProcessQueuePMInitService;
//# sourceMappingURL=BusinessProcessQueuePMInitService.js.map