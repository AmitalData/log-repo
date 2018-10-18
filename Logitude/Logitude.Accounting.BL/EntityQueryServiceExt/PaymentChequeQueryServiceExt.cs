using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class PaymentChequeQueryServiceExt : IPaymentChequeQueryServiceExt
    {
        public PaymentChequeQueryServiceExt()
        {

        }

        List<PaymentChequePM> IPaymentChequeQueryServiceExt.GetPaymentChequesByPaymentId(string paymentId, int tenant)
        {
            PaymentChequeQueryService query = new PaymentChequeQueryService(tenant);
            return query.GetPaymentChequesByPaymentId(paymentId, tenant);
        }
           
    }
}
