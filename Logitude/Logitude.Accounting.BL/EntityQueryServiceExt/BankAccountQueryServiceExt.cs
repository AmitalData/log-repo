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
    public class BankAccountQueryServiceExt: IBankAccountQueryServiceExt
    {
        public BankAccountQueryServiceExt()
        {

        }

        public BankAccountPM GetByFirstOrDefault(string id, int tenant)
        {
            BankAccountQueryService query = new BankAccountQueryService(tenant);
            return query.GetByFirstOrDefault(id, tenant);
        }
        public BankAccountPM GetBankAccountByNumber(string number, int tenant)
        {
            BankAccountQueryService query = new BankAccountQueryService(tenant);
            return query.GetByAccountNumber(number, tenant);
        }
    }

}
