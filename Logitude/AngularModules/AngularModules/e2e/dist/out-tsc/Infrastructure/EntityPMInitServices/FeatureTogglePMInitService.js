"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var FeatureTogglePMInitService = /** @class */ (function () {
    function FeatureTogglePMInitService() {
    }
    FeatureTogglePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
    };
    FeatureTogglePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return FeatureTogglePMInitService;
}());
exports.FeatureTogglePMInitService = FeatureTogglePMInitService;
//# sourceMappingURL=FeatureTogglePMInitService.js.map