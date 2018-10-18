using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountCurrencyQueryService : EntityQueryService<GLAccountCurrency, GLAccountCurrencyKeys, GLAccountCurrencyPM, GLAccountPM, GLAccountKeys>
    {
        public List<GLAccountCurrencyPM> GetRelatedCurrenciesAccount(int tenant, string GLAccountId)
        {
            var pocos= this.repository.GetRelatedCurrenciesAccountByCustomerGLAccount(tenant, GLAccountId);
            return pocos.Select(r => this.GetEntityPM(r)).ToList();

        }
    }
}
