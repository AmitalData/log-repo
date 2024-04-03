import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {DateTool} from '../../Infrastructure/Tools';
import {QuotePM} from '../EntityPMs/QuotePM';

export class QuotePMInitService {

    public static InitValues(entityPM: QuotePM, isNew: boolean) {
        if (isNew) {
            var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();

            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.IsClosed = false;
            entityPM.OpenDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.DepartmentId = SessionLocator.LoggedUserPM.DepartmentId;
            entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
            entityPM.QuoteTypeCode = "A";
            entityPM.StartDate = DateTool.GetCurrentDateAsUtc();
            entityPM.SaleCurrencyId = SessionLocator.TenantPM.QuoteSaleCurrencyId;
            entityPM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            entityPM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            entityPM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
            entityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
            entityPM.RatingCode = "N";
            entityPM.ValueOfGoodsCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
            entityPM.ValidByTypeCode = 'EAD'; 
        }
    }

    public static ApplyUIPoperties(entityPM: QuotePM, isNew: boolean) {
        
    }
}
