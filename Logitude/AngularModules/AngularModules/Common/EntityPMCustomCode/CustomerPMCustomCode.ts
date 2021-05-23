import { CustomerPM } from '../EntityPMs/CustomerPM';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class CustomerPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: CustomerPM) {

        if (!SessionLocator.TenantPM.AccountingActivated) {
            entityPM.UIProperties.SetVisibility("CreditLimitAmount", "Customer", false);
            entityPM.UIProperties.SetVisibility("InsuredcreditLimit", "Customer", false);

        }
    }
}
