import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ContainerPM } from '../EntityPMs/ContainerPM';
import { DateTool } from '../../Infrastructure/Tools';

export class ContainerPMInitService {

    public static InitValues(entityPM: ContainerPM, isNew: boolean) {
    
    }

    public static ApplyUIPoperties(entityPM: ContainerPM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("MainCarriageCarrierId", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageVesselId", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageETA", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageETD", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageATA", "Container", false);
            entityPM.UIProperties.SetEnabled("MainCarriageATD", "Container", false);
            entityPM.UIProperties.SetEnabled("Master", "Container", false);
            entityPM.UIProperties.SetEnabled("TerminalAddress", "Container", false);
            entityPM.UIProperties.SetEnabled("TerminalPhone", "Container", false);
        }

    }
}
