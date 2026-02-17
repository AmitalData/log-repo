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
            entityPM.ExpirationDate = DateTool.AddDays(DateTool.GetCurrentDateAsUtc(), 30);
            entityPM.SaleCurrencyId = SessionLocator.TenantPM.QuoteSaleCurrencyId;
            entityPM.DimensionsUnitCode = SessionLocator.TenantPM.DimensionsUnitCode;
            entityPM.VolumeUnitCode = SessionLocator.TenantPM.VolumeUnitCode;
            entityPM.GrossWeightUnitCode = SessionLocator.TenantPM.GrossWeightUnitCode;
            entityPM.ChargeableWeightUnitCode = SessionLocator.TenantPM.ChargeableWeightUnitCode;
            entityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
            entityPM.RatingCode = "N";
            entityPM.ValueOfGoodsCurrencyId = SessionLocator.TenantPM.FreightCurrencyId;
        }
    }

    public static ApplyUIPoperties(entityPM: QuotePM, isNew: boolean) {
        
    }

}