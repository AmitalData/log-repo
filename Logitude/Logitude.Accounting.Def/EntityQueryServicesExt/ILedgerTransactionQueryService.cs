using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityQueryServicesExt
{
    public interface ILedgerTransactionQueryService
    {
        List<LedgerTransactionPM> GetByJournalId(string journalId, int tenant);

        List<LedgerTransactionPM> GetLedgerTransactionPMsByIdList(List<string> idList, int tenant);
    }
}
