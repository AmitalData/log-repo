"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var BusinessRolePMInitService = /** @class */ (function () {
    function BusinessRolePMInitService() {
    }
    BusinessRolePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        }
    };
    BusinessRolePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return BusinessRolePMInitService;
}());
exports.BusinessRolePMInitService = BusinessRolePMInitService;
//# sourceMappingURL=BusinessRolePMInitService.js.map