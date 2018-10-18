import {BankAccountLitePM} from '../EntityPMs/BankAccountLitePM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class BankAccountLitePMInitService {

    public static InitValues(entityPM: BankAccountLitePM, isNew: boolean) {
        if (isNew) {

        }
    }

    public static ApplyUIPoperties(entityPM: BankAccountLitePM, isNew: boolean) {
        if (!isNew) {
            if (!SessionLocator.TenantPM.AccountingActivated) {
                entityPM.UIProperties.SetRequired("BranchNumber", "BankAccountLite", false);
            }
        }
    }
}