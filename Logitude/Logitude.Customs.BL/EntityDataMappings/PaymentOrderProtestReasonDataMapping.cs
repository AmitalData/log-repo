
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class PaymentOrderProtestReasonDataMapping: IMapping<PaymentOrderProtestReasonPM, PaymentOrderProtestReason>
   {

        public void CustomPMToPOCO(PaymentOrderProtestReasonPM entityPM, PaymentOrderProtestReason entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.PaymentOrderId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Line);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                
                entityPOCO.PaymentOrderId = entityPM.PaymentOrderId;             
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(PaymentOrderProtestReasonPM entityPM, PaymentOrderProtestReason entityPOCO)
        {
            CustomMappedPMProperties.Add(PMPropertyNames.ProtestTypeName);
            if (entityPOCO.ProtestTypeCode != null)
            {
                PaymentProtestTypeQueryService paymentProtestTypeQueryService=new PaymentProtestTypeQueryService(entityPOCO.Tenant);
                PaymentProtestTypePM paymentProtestType = paymentProtestTypeQueryService.GetSingle(entityPOCO.ProtestTypeCode, false, true);
                entityPM.ProtestTypeName = paymentProtestType.LocalName;
            }
        }
   }


}
   