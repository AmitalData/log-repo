import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import { DateTool } from '../../Infrastructure/Tools';
import { ContainerTrackingProviderPM } from 'Shipment/EntityPMs/ContainerTrackingProviderPM';

export class ContainerTrackingProviderPMInitService {

    public static InitValues(entityPM: ContainerTrackingProviderPM, isNew: boolean) {
        if (isNew) {

        }
    }

    public static ApplyUIPoperties(entityPM: ContainerTrackingProviderPM, isNew: boolean) {
        if (isNew) {
        }
        else {
            entityPM.UIProperties.SetEnabled("SourceCode", "ContainerTrackingProvider", false);
        }
    }
}
