import {DateTool} from '../../Infrastructure/Tools';
import {FeatureLocator} from '../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {UIProperties, UIProperty} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ARPaymentPM} from '../EntityPMs/ARPaymentPM';
import {InvoiceTool} from '../Tools';
import {AppTool} from '../../Infrastructure/Tools';

export class ARPaymentPMInitService {

  public static InitValues(entityPM: ARPaymentPM, isNew: boolean) {
    if (isNew) {
      entityPM.SATTransferStatusCode = "NT";
      entityPM.SATTransferStatusName = "Not Transfered";


    }
  }

  public static ApplyUIPoperties(entityPM: ARPaymentPM, isNew: boolean) {
    entityPM.UIProperties.SetEnabled("UpdateDate", "ARPayment", false);
    entityPM.UIProperties.SetEnabled("UpdatedByUserId", "ARPayment", false);

    if (SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF" || SessionLocator.SATInterfaceSettings.SATInterfaceCode == "PROF33") {
      if (AppTool.IsNullOrEmpty(entityPM.SATPaymentMethodCode)) {
        entityPM.UIProperties.SetRequired("SATPaymentMethodCode", "ARPayment", true);
      }

      if (AppTool.IsNullOrEmpty(entityPM.MetodoPagoCode)) {
        entityPM.UIProperties.SetRequired("MetodoPagoCode", "ARPayment", true);
      }
    }
  }

}
