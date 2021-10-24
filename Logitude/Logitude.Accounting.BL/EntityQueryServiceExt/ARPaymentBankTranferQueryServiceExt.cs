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
    public class ARPaymentBankTranferQueryServiceExt : IARPaymentBankTranferQueryServiceExt
    {
        public ARPaymentBankTranferQueryServiceExt()
        {

        }

        public ARPaymentBankTranferPM GetSingleARPaymentBankTranferByPaymentId(string id, int tenant)
        {
            ARPaymentBankTranferQueryService query = new ARPaymentBankTranferQueryService(tenant);
            return query.GetSingleARPaymentBankTranferByPaymentId(id,tenant);
        }
    }
}
