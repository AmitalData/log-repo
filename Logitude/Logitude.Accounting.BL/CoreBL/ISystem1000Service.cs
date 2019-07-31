using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using System.Collections.Generic;

namespace Logitude.Accounting.BL.CoreBL
{
    public interface ISystem1000Service
    {
        List<string> GetSystem1000FlatFile(IAccountingContext accountingContext, int tenant);
        string EmailIt(string Email, List<string> flatFiles, int tenant);
    }
}
