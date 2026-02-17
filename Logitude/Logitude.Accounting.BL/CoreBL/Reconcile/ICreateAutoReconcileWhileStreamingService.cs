using System.Collections.Generic;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;

namespace Logitude.Accounting.BL.CoreBL
{
    public interface ICreateAutoReconcileWhileStreamingService
    {
        List<ReconciliationPM> ReconciliationList { get; }
        void MustInit(
            IAccountingContext accountingContext,
            JournalPM JournalPM,
            List<LedgerTransactionPM> myNewLedgerTransactionsWithCounters
            );
        void CreateAutoReconcileWhileStreaming();
    }
}