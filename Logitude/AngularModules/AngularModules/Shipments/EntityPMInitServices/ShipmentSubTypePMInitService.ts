import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import { ShipmentSubTypePM } from '../EntityPMs/ShipmentSubTypePM';
import { DateTool } from '../../Infrastructure/Tools';

export class ShipmentSubTypePMInitService {

    public static InitValues(entityPM: ShipmentSubTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        }
    }

    public static ApplyUIPoperties(entityPM: ShipmentSubTypePM, isNew: boolean) {
        if (isNew) {
            entityPM.UIProperties.SetVisibility("Inactive", "ShipmentSubType", false);
        }

        else {
            entityPM.UIProperties.SetEnabled("Code", "ShipmentSubType", false);
        }

        if (!entityPM.IsManuallyAdded) {
            entityPM.UIProperties.SetEnabled("Inactive", "ShipmentSubType", false);
        }
    }
}
