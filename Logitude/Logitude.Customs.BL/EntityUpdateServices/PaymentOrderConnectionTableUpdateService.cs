using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PaymentOrderConnectionTableUpdateService : EntityUpdateService<PaymentOrderConnectionTable, PaymentOrderConnectionTablePM, PaymentOrderPM>
    {
        protected override void OnCreating(PaymentOrderConnectionTablePM entityPM, PaymentOrderPM entityParentPM)
        {
            if (entityParentPM == null)
            {
                //throw new Exception("PaymentOrderLine Modification Update Service ,must be apart of Domain Model ");
                return;
            }
            entityPM.PaymentOrderId = entityParentPM.Id;
            entityPM.Tenant = entityPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);
        }
        
    }
}
