"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var ActivityPMInitService = /** @class */ (function () {
    function ActivityPMInitService() {
    }
    ActivityPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.IsOpen = true;
            entityPM.ActivityStatusCode = "N";
            entityPM.PriorityCode = "02";
            entityPM.CreateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdateDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
            entityPM.OwnerId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
            //entityPM.Field1 = new CustomFieldClass { TableName = "Quote", FieldName = "Field1" };
            //entityPM.Field2 = new CustomFieldClass { TableName = "Quote", FieldName = "Field2" };
            //entityPM.Field3 = new CustomFieldClass { TableName = "Quote", FieldName = "Field3" };
            //entityPM.Field4 = new CustomFieldClass { TableName = "Quote", FieldName = "Field4" };
            //entityPM.Field5 = new CustomFieldClass { TableName = "Quote", FieldName = "Field5" };
            //entityPM.Field6 = new CustomFieldClass { TableName = "Quote", FieldName = "Field6" };
            //entityPM.Field7 = new CustomFieldClass { TableName = "Quote", FieldName = "Field7" };
            //entityPM.Field8 = new CustomFieldClass { TableName = "Quote", FieldName = "Field8" };
            //entityPM.Field9 = new CustomFieldClass { TableName = "Quote", FieldName = "Field9" };
            //entityPM.Field10 = new CustomFieldClass { TableName = "Quote", FieldName = "Field10" };
        }
    };
    ActivityPMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return ActivityPMInitService;
}());
exports.ActivityPMInitService = ActivityPMInitService;
//# sourceMappingURL=ActivityPMInitService.js.map