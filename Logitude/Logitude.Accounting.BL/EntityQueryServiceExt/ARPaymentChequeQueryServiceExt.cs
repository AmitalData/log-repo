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
    public class ARPaymentChequeQueryServiceExt: IARPaymentChequeQueryServiceExt
    {
        public ARPaymentChequeQueryServiceExt()
        {

        }

        public List<ARPaymentChequePM> GetListByPaymentId(string paymentId, int tenant)
        {
            ARPaymentChequeQueryService query = new ARPaymentChequeQueryService(tenant);
            return query.GetListByPaymentId(paymentId, tenant);
        }
    }
}
