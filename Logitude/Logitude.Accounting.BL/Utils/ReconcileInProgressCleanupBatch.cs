using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Net;

namespace Logitude.Accounting.BL.Utils
{
    public class ReconcileInProgressCleanupBatch
    {

        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public ReconcileInProgressCleanupBatch()
        {
            _ResponseText = "";
            _StatusCode = HttpStatusCode.Accepted;
        }

        public string ResponseText()
        {
            return _ResponseText;
        }

        public HttpStatusCode StatusCode()
        {
            return _StatusCode;
        }

        public void ResetInProgressTransactions(List<int> tenantsAccountingActivated)
        {

            IAccountingContext MyContext = AccountingContext.GetContext(0);
            LedgerTransactionQueryService queryService = new LedgerTransactionQueryService(MyContext);
            List<LedgerTransactionPM> ledgerTransactionInProgress = queryService.GetLedgerTransactionPMInProgress(tenantsAccountingActivated);

            foreach (LedgerTransactionPM item in ledgerTransactionInProgress)
            {        item.InProgressExternalReconcile = false;
                    item.InReconcileProgress = false;
                    item.ChangeSetOp = ChangeSetOperation.Update;
             }
            LedgerTransactionUpdateService service = new LedgerTransactionUpdateService(MyContext, new Dictionary<string, IContext>(), 0);

            foreach (LedgerTransactionPM item in ledgerTransactionInProgress)
            {
                service.Update(item, true);
            }
            
        }

        

    }
}

