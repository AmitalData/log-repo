"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../Infrastructure/Tools");
var QuotePMInitService = /** @class */ (function () {
    function QuotePMInitService() {
    }
    QuotePMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.IsClosed = false;
            entityPM.OpenDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.DepartmentId = SessionLocator_1.SessionLocator.LoggedUserPM.DepartmentId;
            entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
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
            entityPM.QuoteTypeCode = "A";
            entityPM.ExpirationDays = 30;
            entityPM.ExpirationDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 30);
            entityPM.SaleCurrencyId = SessionLocator_1.SessionLocator.TenantPM.QuoteSaleCurrencyId;
            entityPM.DimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
            entityPM.VolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
            entityPM.GrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode;
            entityPM.BusinessUnitId = SessionLocator_1.SessionLocator.LoggedUserPM.BusinessUnitId;
            entityPM.RatingCode = "N";
            entityPM.ValueOfGoodsCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
        }
    };
    QuotePMInitService.ApplyUIPoperties = function (entityPM, isNew) {
    };
    return QuotePMInitService;
}());
exports.QuotePMInitService = QuotePMInitService;
//# sourceMappingURL=QuotePMInitService.js.map