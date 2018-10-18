using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class PaymentOrderLineUpdateService
    {
        protected override void OnCreating(PaymentOrderLinePM entityPM, PaymentOrderPM entityParentPM)
        {
            //entityPM.PaymentOrderId = entityParentPM.Id; // itzik  try to solve 
            //mirit changing
            if (entityParentPM == null)
            {
                //throw new Exception("PaymentOrderLine Modification Update Service ,must be apart of Domain Model ");
                return;
            }
            entityPM.PaymentOrderId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;

            base.OnCreating(entityPM, entityParentPM);
        }
    }
}
