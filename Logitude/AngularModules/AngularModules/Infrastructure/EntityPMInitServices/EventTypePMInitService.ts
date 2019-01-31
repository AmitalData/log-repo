import {EventTypePM} from '../EntityPMs/EventTypePM';

export class EventTypePMInitService {

    public static InitValues(entityPM: EventTypePM, isNew: boolean) {
        if (isNew) {
            
        }
    }

    public static ApplyUIPoperties(entityPM: EventTypePM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("IsFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("ManualActivatedFollowUp", "EventType", entityPM.AddedManually);
        }
    }
}