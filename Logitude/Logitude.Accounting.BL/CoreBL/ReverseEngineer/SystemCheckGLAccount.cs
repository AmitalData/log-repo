using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL
{
    public class SystemCheckGLAccount
    {
        public List<JournalLineLedgerDTO> StartCheck(string glAccountId, int tenant)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            var LedgerTransactionQS = new LedgerTransactionQueryService(accountingContext);
            var res = LedgerTransactionQS.GetReportCompareToJournalLine(DateTime.MinValue, DateTime.MaxValue, tenant, glAccountId: glAccountId);
            if (res != null && res.Count > 0)
            {
                return res;
            }
            throw new Exception("LedgerTransaction to  JournalLine ITS OK,TODO TOTAL TO LADGER !!!!");
        }

        private IQueryable<JournalLineLedgerDTO> GetLedgerTrans(IAccountingContext accountingContext, string glAccountId, int tenant)
        {
            throw new NotImplementedException();
            //var LedgerTransactionQS = new LedgerTransactionQueryService(accountingContext);
            //var JournalLineLedgerDTOList = LedgerTransactionQS.GetReportCompareToJournalLine(DateTime.MinValue, DateTime.MaxValue, tenant, glAccountId: glAccountId);
            //return JournalLineLedgerDTOList;
        }

       

        
    }
}
