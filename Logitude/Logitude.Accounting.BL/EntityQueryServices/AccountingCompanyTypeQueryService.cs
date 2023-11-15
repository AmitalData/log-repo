using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class AccountingCompanyTypeQueryService
    {

        public AccountingCompanyTypePM GetByCode(string code, int tenant, bool getFromCache = false)
        {
            AccountingCompanyType poco = this.repository.GetSingleAccountingCompanyTypeByCode(code, tenant, getFromCache);
            if (poco != null)
            {
                return GetEntityPM(poco);
            }
            else
            {
                return null;
            }
        }

    }
}
