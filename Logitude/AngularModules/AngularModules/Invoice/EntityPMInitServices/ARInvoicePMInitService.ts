import {DateTool} from '../../Infrastructure/Tools';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {ARInvoicePM} from '../EntityPMs/ARInvoicePM';
import {InvoiceTool} from '../Tools';
import {AppTool} from '../../Infrastructure/Tools'; 

export class ARInvoicePMInitService {

    public static InitValues(entityPM: ARInvoicePM, isNew: boolean) {
        if (isNew) {
            var todayDate = DateTool.GetCurrentDateAsUtc();

            entityPM.StatusCode = "DR";
            entityPM.StatusName = "Draft";
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.IssuedByUserId = SessionLocator.LoggedUserId;
            entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            entityPM.CreateDate = todayDate;
            entityPM.UpdateDate = todayDate;
            entityPM.InvoiceDate = todayDate;
            entityPM.BranchId = SessionLocator.LoggedUserPM.BranchId;
            entityPM.LocalCurrencyId = SessionLocator.LocalCurrencyId;
            entityPM.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
            entityPM.ProfitCurrencyId = SessionLocator.TenantPM.ProfitCurrencyId;
            entityPM.ProfitCurrencyCode = SessionLocator.TenantPM.ProfitCurrencyCode;
            entityPM.SATTransferStatusCode = "NT";
            entityPM.SATTransferStatusName = "Not Transfered";
            entityPM.NewConcurrencyGUID = AppTool.GetNewGuid();
        }
    }

    public static ApplyUIPoperties(entityPM: ARInvoicePM, isNew: boolean) {
        entityPM.UIProperties.SetEnabled("UpdateDate", "ARInvoice", false);
        entityPM.UIProperties.SetEnabled("UpdatedByUserId", "ARInvoice", false);        

        var isAllowedEdit = InvoiceTool.IsEditingARInvoiceEnabled(entityPM);
        entityPM.UIProperties.SetEnabled("Sent", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("HouseNumber", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("MasterNumber", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BranchId", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("CustomerRef", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("SATPaymentMethodCode", "ARInvoice", isAllowedEdit);
        //entityPM.UIProperties.SetEnabled("SalesmanUserId", "ARInvoice", isAllowedEdit);
        entityPM.UIProperties.SetEnabled("BankAccountLiteId", "ARInvoice", isAllowedEdit);

        if (FeatureLocator.HasFeaturePermession("ARInvoice", "Intercompany")) {
            entityPM.UIProperties.SetVisibility("Intercompany", "ARInvoice", true);
        }
        else {
            entityPM.UIProperties.SetVisibility("Intercompany", "ARInvoice", false);
      }


      if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
        if (AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
          entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", true);
        }

        if (AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
          entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", true);
        }
      }
    }
}
