import {DateTool} from '../../Infrastructure/Tools';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {APInvoicePM} from '../EntityPMs/APInvoicePM';
import {InvoiceTool} from '../Tools';

export class APInvoicePMInitService {

    public static InitValues(entityPM: APInvoicePM, isNew: boolean) {
        if (isNew) {
            var todayDateTime = DateTool.GetCurrentDateAsUtc();

            entityPM.Tenant = SessionLocator.TenantPM.Id;
            entityPM.StatusCode = "WA";
            entityPM.StatusName = "Waiting for Approval";
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;
            entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
            entityPM.LocalCurrencyId = SessionLocator.LocalCurrencyId;
            entityPM.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
            entityPM.PaymentTermId = SessionLocator.TenantPM.PaymentTermId;
            entityPM.SubTotalInLocalCurrency = 0;
            entityPM.SubTotalInInvoiceCurrency = 0;
        }
    }

    public static ApplyUIPoperties(entityPM: APInvoicePM, isNew: boolean) {
        //entityPM.UIProperties = new UIProperties;
        entityPM.UIProperties.SetVisibility("EnableConsolidationInvoices", "APInvoice", FeatureLocator.HasFeaturePermession("ARInvoice", "Consolidation.Constituent"));


        entityPM.UIProperties.SetEnabled("UpdateDate", "APInvoice", false);
        entityPM.UIProperties.SetEnabled("UpdatedByUserId", "APInvoice",  false);

        var isAllowedEdit = InvoiceTool.IsEditingAPInvoiceEnabled(entityPM);
        entityPM.UIProperties.SetEnabled("HouseNumber", "APInvoice",  isAllowedEdit);
        entityPM.UIProperties.SetEnabled("MasterNumber", "APInvoice",  isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BranchId", "APInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("GlobalTaxCalculation", "APInvoice", isAllowedEdit);



        if (SessionLocator.TenantPM.AccountingActivated) {
            entityPM.UIProperties.SetEnabled("AccountingDate", "APInvoice", isAllowedEdit);
        }

        if (entityPM.IsMultipleEntities) {
            entityPM.UIProperties.SetVisibility("HouseNumber", "APInvoice", false);
            entityPM.UIProperties.SetVisibility("MasterNumber", "APInvoice", false);
        }

        if (!(SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG")) {
            entityPM.UIProperties.SetVisibility("GlobalTaxCalculation", "APInvoice", false);
        } 
    }

}
