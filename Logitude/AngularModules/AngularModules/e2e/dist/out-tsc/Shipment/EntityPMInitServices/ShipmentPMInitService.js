"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Tools_1 = require("../../Infrastructure/Tools");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var ShipmentPMInitService = /** @class */ (function () {
    function ShipmentPMInitService() {
    }
    ShipmentPMInitService.InitValues = function (entityPM, isNew) {
        if (isNew) {
            var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            entityPM.FHLStatusCode = "NSEN";
            entityPM.FWBStatusCode = "NSEN";
            entityPM.FHLStatusName = "Not Sent";
            entityPM.FWBStatusName = "Not Sent";
            entityPM.ManifestStatusCode = "NSEN";
            entityPM.LocalCustomsTransmissionsStatusCode = "NSEN";
            entityPM.IsOperationalClosed = false;
            entityPM.CreateDateTime = todayDate;
            entityPM.LastUpdateDate = todayDate;
            entityPM.StatusDate = todayDate;
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
            entityPM.AWBCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
            entityPM.ProfitCurrencyId = SessionLocator_1.SessionLocator.TenantPM.ProfitCurrencyId;
            entityPM.VolumeUnitCode = SessionLocator_1.SessionLocator.TenantPM.VolumeUnitCode;
            entityPM.DimensionsUnitCode = SessionLocator_1.SessionLocator.TenantPM.DimensionsUnitCode;
            entityPM.GrossWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = SessionLocator_1.SessionLocator.TenantPM.ChargeableWeightUnitCode;
            entityPM.CreatedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            entityPM.BranchId = SessionLocator_1.SessionLocator.LoggedUserPM.BranchId;
            entityPM.DepartmentId = SessionLocator_1.SessionLocator.LoggedUserPM.DepartmentId;
            entityPM.NewConcurrencyGUID = Tools_1.AppTool.GetNewGuid();
            entityPM.ValueOfGoodsCurrencyId = SessionLocator_1.SessionLocator.TenantPM.FreightCurrencyId;
            entityPM.OnCarriageAdditionalTransportModeCode = "BYTR";
        }
    };
    return ShipmentPMInitService;
}());
exports.ShipmentPMInitService = ShipmentPMInitService;
//# sourceMappingURL=ShipmentPMInitService.js.map