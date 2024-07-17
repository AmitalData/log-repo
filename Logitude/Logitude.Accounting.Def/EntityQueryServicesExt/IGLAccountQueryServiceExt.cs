using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityQueryServicesExt
{
    public interface IGLAccountQueryServiceExt
    {
        GLAccountPM GetSingleGLAccountPM(string id, int tenant);

        string GetDisplayNumberByGLAccountId(string id, int tenant);

        GLAccountPM GetGLAccountByDisplayNumber(string id, int tenant);
        GLAccountPM GetGLAccountByInternalNumber(string id, int tenant);
        GLAccountPM GetSplittedByCurrencyGLAccount(string accountId, int tenant, string currency);
        GLAccountPM GetSplittedGLAccount(string accountId, int tenant, string currency);
        string GetGLAccountDisplayNoAndLocalName(string glAccountId, int tenant);
        GLAccountPM GetSingleGLAccountWithComposition(string glAccountId, int tenant);
        IQueryable<GLAccountPM> GetSplittedByCurrencyGLAccounts(string accountId, int tenant);
        bool CheckInactiveGLAccounts(List<string> glaccountIds, int tenant);
        List<string> GetChildrenByCurrencyGLAccountIds(string accountId, int tenant);
    }
}
