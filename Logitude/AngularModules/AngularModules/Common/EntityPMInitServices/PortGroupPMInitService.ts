import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { PortGroupPM } from '../EntityPMs/PortGroupPM';

export class PortGroupPMInitService {

    public static InitValues(entityPM: PortGroupPM, isNew: boolean) {
        
    }

    public static ApplyUIPoperties(entityPM: PortGroupPM, isNew: boolean) {
        entityPM.UIProperties.SetVisibility("Inactive", "PortGroup", !isNew);
    }
}
