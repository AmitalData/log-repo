using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityUpdateServicesExt
{
    public interface IAPPaymentInvoicesTransactionFetcherExt
    {
        Tuple<LedgerTransactionPM, List<LedgerTransactionPM>> GetInvoicesLedgerTransactions(string appaymentId, string accountId, int tenant);
    }
}
