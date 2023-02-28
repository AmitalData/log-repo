using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Accounting.BL.EntityUpdateServiceExt
{
    public class APPaymentInvoicesTransactionFetcherExt : IAPPaymentInvoicesTransactionFetcherExt
    {
        
        public APPaymentInvoicesTransactionFetcherExt()
        {

        }

        public string GetInvoiceRecociliationsNumbers(string appaymentId, string accountId, int tenant, string invoiceNumber)
        {
            APPaymentInvoicesTransactionFetcher invoiceTransactionsFetcher = new APPaymentInvoicesTransactionFetcher(appaymentId, accountId, tenant, true);
            var transactions = invoiceTransactionsFetcher.FetchSorted();
            if (transactions.Count > 0) {
                var invoiceTransaction = transactions.Where(x => x.Reference1 == invoiceNumber).FirstOrDefault();
                if (invoiceTransaction != null) {
                    return invoiceTransaction.Reference3;
                }
            }

            return null;
        }
    }
}
