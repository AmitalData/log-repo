import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {DateTool} from '../../Infrastructure/Tools';
import {ActivityPM} from '../EntityPMs/ActivityPM';

export class ActivityPMInitService {

    public static InitValues(entityPM: ActivityPM, isNew: boolean) {
        if (isNew) {

            var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.IsOpen = true;
            entityPM.ActivityStatusCode = "N";
            entityPM.PriorityCode = "02";
            entityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
            entityPM.OwnerId = SessionLocator.LoggedUserId;
            entityPM.BusinessUnitId = SessionLocator.LoggedUserPM.BusinessUnitId;
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
    }

    public static ApplyUIPoperties(entityPM: ActivityPM, isNew: boolean) {

    }

}