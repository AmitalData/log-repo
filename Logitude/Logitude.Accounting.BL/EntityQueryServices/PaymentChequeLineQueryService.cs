using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
  public partial  class PaymentChequeLineQueryService
    {


        public int GetMaxLineNumber(string paymentChequeId, int tenant)
        {
            return repository.GetMaxLineNumber(paymentChequeId, tenant);
        }
    }
}
