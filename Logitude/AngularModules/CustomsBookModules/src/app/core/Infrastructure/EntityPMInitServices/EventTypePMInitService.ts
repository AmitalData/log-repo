import {EventTypePM} from '../EntityPMs/EventTypePM';

export class EventTypePMInitService {

    public static InitValues(entityPM: EventTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.AddedManually = true;
            entityPM.IsManualEntry = true;
        }
    }

    public static ApplyUIPoperties(entityPM: EventTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("InActive", "EventType", false);
        }

        else {
            entityPM.UIProperties.SetEnabled("IsFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("ManualActivatedFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("EntityStatusId", "EventType", entityPM.AddedManually);
        }
    }
}
