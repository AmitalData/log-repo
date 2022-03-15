import { FeatureLocator } from '../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';
import { UIProperties, UIProperty } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import { PaymentTermPM } from '../EntityPMs/PaymentTermPM';

export class PaymentTermPMInitService {

    public static InitValues(entityPM: PaymentTermPM, isNew: boolean) {
    }

    public static ApplyUIPoperties(entityPM: PaymentTermPM, isNew: boolean) {

        entityPM.UIProperties.SetEnabled("NumberOfMonths", "PaymentTerm", entityPM.EndOfMonth);

    }
}
