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
    public class LedgerTransactionQueryServiceExt : ILedgerTransactionQueryService
    {
        public LedgerTransactionQueryServiceExt()
        {

        }

        public List<LedgerTransactionPM> GetByJournalId(string journalId, int tenant)
        {
            LedgerTransactionQueryService query = new LedgerTransactionQueryService(tenant);
            return query.GetByJournalId(journalId, tenant);
        }
    }
}
