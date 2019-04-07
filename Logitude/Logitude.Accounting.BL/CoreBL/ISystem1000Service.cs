using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL
{
    public interface ISystem1000Service
    {
        string GetSystem1000FlatFile(IAccountingContext accountingContext, int tenant);
    }
}
