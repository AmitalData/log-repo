import { ARPaymentPM } from '../EntityPMs/ARPaymentPM';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../Infrastructure/Tools'; 

export class ARPaymentPMCustomCode {
    public static ApplyEntityChanged(propertyName: string, entityPM: ARPaymentPM) {

        entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", false);
        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF40") {
            if (AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", true);
            }
            else
                entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", false);

            if (AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", true);
            }
            else
                entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", false);


            if (!AppTool.IsNullOrEmpty(entityPM.TipoCadenaPago) && entityPM.TipoCadenaPago == "01" && entityPM.SATPaymentMethodCode == "03") {
                if (AppTool.IsNullOrEmpty(entityPM.CertPago))
                    entityPM.UIProperties.SetRequired("CertPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("CertPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(entityPM.CadPago))
                    entityPM.UIProperties.SetRequired("CadPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("CadPago", "ARPayment", false);

                if (AppTool.IsNullOrEmpty(entityPM.SelloPago))
                    entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", true);
                else
                    entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
            else {
                entityPM.UIProperties.SetRequired("CertPago", "ARPayment", false);
                entityPM.UIProperties.SetRequired("CadPago", "ARPayment", false);
                entityPM.UIProperties.SetRequired("SelloPago", "ARPayment", false);
            }
        }
    }
}
