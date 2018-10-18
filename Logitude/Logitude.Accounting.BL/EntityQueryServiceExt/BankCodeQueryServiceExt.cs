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
    public class BankCodeQueryServiceExt : IBankCodeQueryServiceExt
    {
        public BankCodeQueryServiceExt()
        {

        }

        public BankCodePM GetByFirstOrDefault(int tenant)
        {
            BankCodeQueryService query = new BankCodeQueryService(tenant);
            return query.GetSingleByTenant(tenant);
        }
    }

}
