import { FeatureTogglePM } from '../EntityPMs/FeatureTogglePM';
import { DateTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class FeatureTogglePMInitService {

    public static InitValues(entityPM: FeatureTogglePM, isNew: boolean) {
        if (isNew) {
            var todayDate: Date = DateTool.GetCurrentDateTimeAsUtc();

            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }
    }

    public static ApplyUIPoperties(entityPM: FeatureTogglePM, isNew: boolean) {

    }
}
