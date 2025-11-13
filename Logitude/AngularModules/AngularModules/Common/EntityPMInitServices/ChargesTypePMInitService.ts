import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ChargesTypePM} from '../EntityPMs/ChargesTypePM';

export class ChargesTypePMInitService {

    public static InitValues(entityPM: ChargesTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.IsAir = true;
            entityPM.IsInland = true;
            entityPM.IsOcean = true;
            entityPM.AWBPrintDescription = true;
            entityPM.ViewOrder = 100;
        }
    }

    public static InitValuesForAccounting(entityPM: ChargesTypePM, isNew: boolean) {
        this.InitValues(entityPM, isNew);
        if (isNew) {
            entityPM.ChargesGroupCode = "OCH";
            entityPM.ChargesGroupId = "OCH";
            entityPM.MeasurementCode = "FIXD"
            entityPM.MeasurementId = "FIXD"
            entityPM.IsReceivable = true;
            entityPM.IsPayable = true;
        }
    }

    public static ApplyUIPoperties(entityPM: ChargesTypePM, isNew: boolean) {
    }

}