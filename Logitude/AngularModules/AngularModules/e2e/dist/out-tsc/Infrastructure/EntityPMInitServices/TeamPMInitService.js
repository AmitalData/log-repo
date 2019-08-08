"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var TeamPMInitService = /** @class */ (function () {
    function TeamPMInitService() {
    }
    TeamPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
    };
    TeamPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return TeamPMInitService;
}());
exports.TeamPMInitService = TeamPMInitService;
//# sourceMappingURL=TeamPMInitService.js.map