import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { ShippingLinePM } from '../EntityPMs/ShippingLinePM';

export class ShippingLinePMInitService {
    public static InitValues(entityPM: ShippingLinePM, isNew: boolean) {
        
    }

    public static ApplyUIPoperties(entityPM: ShippingLinePM, isNew: boolean) {
        if (SessionLocator.Tenant != 0) {
            entityPM.UIProperties.SetVisibility("IsSendingByContainer", "ShippingLine", false);
            entityPM.UIProperties.SetVisibility("IsSendingByBillOfLading", "ShippingLine", false);
        }
    }
}
