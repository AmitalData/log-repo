
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs; 
using Logitude.Accounting.Data;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class PaymentChequeLineDataMapping: IMapping<PaymentChequeLinePM, PaymentChequeLine>
   {

        public void CustomPMToPOCO(PaymentChequeLinePM entityPM, PaymentChequeLine entityPOCO)
        {
            AddPOCOPropertyName(POCOPropertyNames.PaymentChequeId);
            AddPOCOPropertyName(POCOPropertyNames.Tenant);
            AddPOCOPropertyName(POCOPropertyNames.Line);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.PaymentChequeId = entityPM.PaymentChequeId;
                entityPOCO.Tenant = entityPM.Tenant;
                entityPOCO.Line = entityPM.Line;
            }
        }

        public void CustomPOCOToPM(PaymentChequeLinePM entityPM, PaymentChequeLine entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   