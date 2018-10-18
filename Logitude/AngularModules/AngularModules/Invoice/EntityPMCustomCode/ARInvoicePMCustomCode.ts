import {ARInvoicePM} from '../EntityPMs/ARInvoicePM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../Infrastructure/Tools'; 

export class ARInvoicePMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: ARInvoicePM) {
       
        entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", false);
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
            if (AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", true);
            }
            else
              entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARInvoice", false);

          if (AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
            entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", true);
          }
          else
            entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARInvoice", false);
        }
    }
}
