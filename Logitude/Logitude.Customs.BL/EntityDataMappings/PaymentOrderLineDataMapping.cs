
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
   
   public partial class PaymentOrderLineDataMapping: IMapping<PaymentOrderLinePM, PaymentOrderLine>
   {

        public void CustomPMToPOCO(PaymentOrderLinePM entityPM, PaymentOrderLine entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.PaymentOrderId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ParagraphTypeCode);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
             
                entityPOCO.PaymentOrderId = entityPM.PaymentOrderId;
                entityPOCO.Tenant = entityPM.Tenant;              
                entityPOCO.ParagraphTypeCode = entityPM.ParagraphTypeCode;
            }

        }

        public void CustomPOCOToPM(PaymentOrderLinePM entityPM, PaymentOrderLine entityPOCO)
        {

            this.CustomMappedPMProperties.Add(PMPropertyNames.ParagraphTypeName);


            if (entityPOCO.ParagraphTypeCode != null)
            {
                ParagraphTypeQueryService paragraphTypeQueryService = new ParagraphTypeQueryService(entityPOCO.Tenant);
                ParagraphTypePM paragraphType = paragraphTypeQueryService.GetSingle(entityPOCO.ParagraphTypeCode, false, true);
                entityPM.ParagraphTypeName = paragraphType.LocalName;

            }
        }
   }


}
   