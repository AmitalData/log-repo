using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.BL.Utils
{
    public class ReconciliationAfterConversionBatch
    {
        private string _ResponseText;
        private HttpStatusCode _StatusCode;

        public ReconciliationAfterConversionBatch()
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
        public void RunReconciliationAfterConversion(int tenant)
        {
            IAccountingContext context = AccountingContext.GetContext(tenant);
            JournalLineListQueryService journalLineListQueryService = new JournalLineListQueryService(context);
       //     List<JournalLineList> journalLins = journalLineListQueryService.GetOpenRevaluationList(tenant);
        }
    }
}