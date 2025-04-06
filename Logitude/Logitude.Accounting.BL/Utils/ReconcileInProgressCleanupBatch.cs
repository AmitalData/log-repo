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

        public void ResetInProgressTransactions()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(0);
           JournalQueryService queryService = new JournalQueryService(MyContext);
           queryService.FixFailedReconcileJournals();
            _ResponseText = "ResetInProgressTransactions:Success";
        }

        

    }
}

