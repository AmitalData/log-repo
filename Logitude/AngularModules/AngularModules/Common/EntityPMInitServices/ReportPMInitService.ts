import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ReportPM} from '../EntityPMs/ReportPM';

export class ReportPMInitService {

    public static InitValues(entityPM: ReportPM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: ReportPM, isNew: boolean) {
        if (!isNew) {
            entityPM.UIProperties.SetEnabled("Code", "Report", false);
        }
    }

}
