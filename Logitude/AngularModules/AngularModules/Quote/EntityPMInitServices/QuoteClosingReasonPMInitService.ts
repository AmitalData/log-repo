import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { QuoteClosingReasonPM } from '../EntityPMs/QuoteClosingReasonPM';
import { DateTool } from '../../Infrastructure/Tools';

export class QuoteClosingReasonPMInitService {

    public static InitValues(entityPM: QuoteClosingReasonPM, isNew: boolean) {
        if (isNew) {
            entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }
    }

    public static ApplyUIPoperties(entityPM: QuoteClosingReasonPM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("Inactive", "QuoteClosingReason", false);
        }

        else {
            entityPM.UIProperties.SetEnabled("Code", "QuoteClosingReason", false);
        }
    }
}
