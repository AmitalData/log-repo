import { PortPM } from '../EntityPMs/PortPM';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class PortPMInitService {

    public static InitValues(entityPM: PortPM, isNew: boolean) {
        if (isNew) {
        }
    }

    public static ApplyUIPoperties(entityPM: PortPM, isNew: boolean) {
        if (SessionLocator.TenantPM.Id != 0) {
            entityPM.UIProperties.SetEnabled("PortTimeZoneCode", "Port", false);
            entityPM.UIProperties.SetVisibility("PortGroupId", "Port", false);
        }
    }
}
