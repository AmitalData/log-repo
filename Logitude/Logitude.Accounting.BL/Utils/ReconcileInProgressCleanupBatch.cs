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

        private string responseText;
        private HttpStatusCode statusCode;

        public ReconcileInProgressCleanupBatch()
        {
            responseText = "";
            statusCode = HttpStatusCode.Accepted;
        }

        public string GetResponseText()
        {
            return responseText;
        }

        public HttpStatusCode GetStatusCode()
        {
            return statusCode;
        }

        public void ResetInProgressTransactions()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(0);
           JournalQueryService queryService = new JournalQueryService(MyContext);
           queryService.FixFailedReconcileJournals();
            responseText = "ResetInProgressTransactions:Success";
        }

        

    }
}

