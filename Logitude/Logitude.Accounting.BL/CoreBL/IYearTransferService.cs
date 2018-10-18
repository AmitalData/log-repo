using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL
{
    public interface IYearTransferService
    {
        JournalPM ProccessJournal(IAccountingContext accountingContext, int YYyear, int tenant);
    }
}