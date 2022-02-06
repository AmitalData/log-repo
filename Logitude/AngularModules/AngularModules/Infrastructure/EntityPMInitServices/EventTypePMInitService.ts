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
            entityPM.UIProperties.SetVisibility("EventTrigger", "EventType", this.IsTenantZero(entityPM.Tenant));
        }
        else {
            entityPM.UIProperties.SetEnabled("IsFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("ManualActivatedFollowUp", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("EntityStatusId", "EventType", entityPM.AddedManually);
            entityPM.UIProperties.SetEnabled("EventTrigger", "EventType", this.IsTenantZero(entityPM.Tenant));
        }
    }

    private static IsTenantZero(tenant: number) {
        if (tenant == 0) {
            return true;
        }
        return false;
    }
}
