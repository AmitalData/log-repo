import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {VesselPM} from '../EntityPMs/VesselPM';

export class VesselPMInitService {

    public static InitValues(entityPM: VesselPM, isNew: boolean) {
        if (isNew) {
            entityPM.AddedManually = true;
        }
    }

    public static ApplyUIPoperties(entityPM: VesselPM, isNew: boolean) { 
        
    }
}