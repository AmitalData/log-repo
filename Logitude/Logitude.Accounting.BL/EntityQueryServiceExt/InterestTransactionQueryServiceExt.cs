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
    public class InterestTransactionQueryServiceExt : IInterestTransactionQueryServiceExt
    {
        public InterestTransactionQueryServiceExt()
        {

        }
        public InterestTransactionPM GetInterestTransactionPMByEntityId(string entityId,int tenant)
        {
            InterestTransactionQueryService query = new InterestTransactionQueryService(tenant);
            return query.GetInterestTransactionPMByEntityId(entityId,tenant);
        }
    }
}
