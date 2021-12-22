import {ShipmentPM} from '../EntityPMs/ShipmentPM';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

export class ShipmentPMInitService {
    public static InitValues(entityPM: ShipmentPM, isNew: boolean) {
        if (isNew) {

            var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();

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
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.AWBCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
            entityPM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            entityPM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            entityPM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            //entityPM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
            entityPM.DepartmentId = SessionLocator.LoggedUserPM.DepartmentId;
            entityPM.NewConcurrencyGUID = AppTool.GetNewGuid();
            entityPM.ValueOfGoodsCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
            entityPM.OnCarriageAdditionalTransportModeCode = "BYTR";
            entityPM.OnForwardingAdditionalTransportModeCode = "BYTR";
        }
    }

    public static ApplyUIPoperties(entityPM: ShipmentPM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("IsPODReceived", "Shipment", false);
            entityPM.UIProperties.SetEnabled("PODReceivedDate", "Shipment", false);
        }
    }

}
