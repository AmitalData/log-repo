using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class ReconciliationLineQueryService : EntityQueryService<ReconciliationLine, ReconciliationLineKeys, ReconciliationLinePM, ReconciliationPM, ReconciliationKeys>
    {
        public bool IsReconciledBy(int tenant, List<string> transactionIdList)
        {
            return this.repository.IsReconciledBy(tenant, transactionIdList);
        }

        public ReconciliationLine GetLineByTransactionId(string transId, int tenant)
        {
            ReconciliationLine recoLine = (from a in context.ReconciliationLines
                                           where a.TransactionId == transId && a.Tenant == tenant
                                           select a).FirstOrDefault();
            //var pm = GetEntityPM(recoLine);
            return recoLine;
        }
        


    }
}
