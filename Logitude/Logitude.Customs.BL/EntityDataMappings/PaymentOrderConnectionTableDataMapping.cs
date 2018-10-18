
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class PaymentOrderConnectionTableDataMapping: IMapping<PaymentOrderConnectionTablePM, PaymentOrderConnectionTable>
   {

        public void CustomPMToPOCO(PaymentOrderConnectionTablePM entityPM, PaymentOrderConnectionTable entityPOCO)
        {
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.PaymentOrderId);
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ConnectedEntityId);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                entityPOCO.PaymentOrderId = entityPM.PaymentOrderId;
                entityPOCO.ConnectedEntityId = entityPM.ConnectedEntityId;
            }
        }

        public void CustomPOCOToPM(PaymentOrderConnectionTablePM entityPM, PaymentOrderConnectionTable entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   