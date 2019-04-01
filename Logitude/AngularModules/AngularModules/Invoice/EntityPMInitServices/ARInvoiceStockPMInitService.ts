import { ARInvoiceStockPM } from '../EntityPMs/ARInvoiceStockPM';
import { DateTool } from '../../Infrastructure/Tools';
import { SessionLocator } from '../../Infrastructure/Utilities/SessionLocator';

export class ARInvoiceStockPMInitService {
    public static InitValues(entityPM: ARInvoiceStockPM, isNew: boolean) {
        if (isNew) {
            var todayDateTime = DateTool.GetCurrentDateAsUtc();

            entityPM.Tenant = SessionLocator.TenantPM.Id;
            entityPM.StatusCode = "N";
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;           
        }
    }

    public static ApplyUIPoperties(entityPM: ARInvoiceStockPM, isNew: boolean) {

    }
}
