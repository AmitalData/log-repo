using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL
{
    public interface IYearTransferService
    {
        JournalPM ProccessJournal(IAccountingContext accountingContext, int YYyear, int tenant);
        JournalPM CancelYear(IAccountingContext accountingContext, int YYyear, int tenant);
        JournalPM DoCancelYear(IAccountingContext accountingContext, JournalPM origPM, int tenant);
        JournalPM CheckCancelYear(IAccountingContext accountingContext, int YYyear, int tenant);
    }
}