import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { OccasionTypePM } from '../EntityPMs/OccasionTypePM';

export class OccasionTypePMInitService {

    public static InitValues(entityPM: OccasionTypePM, isNew: boolean) {
        if (isNew) {

        }
    }

    public static ApplyUIPoperties(entityPM: OccasionTypePM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("Code", "OccasionType", false);
        }
    }
}
