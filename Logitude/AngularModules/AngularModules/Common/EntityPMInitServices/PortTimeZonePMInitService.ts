import { PortTimeZonePM } from '../EntityPMs/PortTimeZonePM';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class PortTimeZonePMInitService {

    public static InitValues(entityPM: PortTimeZonePM, isNew: boolean) {
        if (isNew) {
        }
    }

    public static ApplyUIPoperties(entityPM: PortTimeZonePM, isNew: boolean) {
        if (SessionLocator.TenantPM.Id != 0) {
            entityPM.UIProperties.SetEnabled("Code", "PortTimeZone", false);
            entityPM.UIProperties.SetEnabled("Name", "PortTimeZone", false);
            entityPM.UIProperties.SetEnabled("Inactive", "PortTimeZone", false);
            entityPM.UIProperties.SetEnabled("Notes", "PortTimeZone", false);
            entityPM.UIProperties.SetEnabled("UTCOffset", "PortTimeZone", false);
            entityPM.UIProperties.SetEnabled("UTCDSTOffset", "PortTimeZone", false);
        } 
    }
}
