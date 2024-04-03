using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class APPaymentInvoicesTransactionFetcherExt : IAPPaymentInvoicesTransactionFetcherExt
    {
        
        public APPaymentInvoicesTransactionFetcherExt()
        {

        }
        
        public Tuple<LedgerTransactionPM, List<LedgerTransactionPM>> GetInvoicesLedgerTransactions(string appaymentId, string accountId, int tenant)
        {
            APPaymentInvoicesTransactionFetcher invoiceTransactionsFetcher = new APPaymentInvoicesTransactionFetcher(appaymentId, accountId, tenant, true);
            
            return Tuple.Create(invoiceTransactionsFetcher.paymentTransactionPM, invoiceTransactionsFetcher.FetchSorted());
        }
    }
}
