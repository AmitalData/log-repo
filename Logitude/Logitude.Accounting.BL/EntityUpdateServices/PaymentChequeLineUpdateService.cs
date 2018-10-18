using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
  public partial  class PaymentChequeLineUpdateService
    {
        protected override void OnCreating(PaymentChequeLinePM entityPM, PaymentChequePM entityParentPM)
        {
            entityPM.PaymentChequeId = entityParentPM.Id;
            entityParentPM.PaymentChequeLineLastLine += 1;
            entityPM.Line = entityParentPM.PaymentChequeLineLastLine;
        }

    }
}
