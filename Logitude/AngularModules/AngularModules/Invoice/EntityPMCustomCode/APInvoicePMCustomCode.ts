import {APInvoicePM} from '../EntityPMs/APInvoicePM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../Infrastructure/Tools';

export class APInvoicePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: APInvoicePM) {
        if (SessionLocator.TenantPM.AccountingActivated == true) {
            entityPM.UIProperties.SetVisibility("AccountingDate", "APInvoice", true);
        }
        else {
            entityPM.UIProperties.SetVisibility("AccountingDate", "APInvoice", false);
        }
    }
}