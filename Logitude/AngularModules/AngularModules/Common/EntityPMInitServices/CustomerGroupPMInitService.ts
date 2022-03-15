import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { CustomerGroupPM } from '../EntityPMs/CustomerGroupPM';
import { DateTool } from '../../Infrastructure/Tools';

export class CustomerGroupPMInitService {

    public static InitValues(entityPM: CustomerGroupPM, isNew: boolean) {
        if (isNew) {
            entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }
    }

    public static ApplyUIPoperties(entityPM: CustomerGroupPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("InActive", "CustomerGroup", false);
        }  
    }
}
