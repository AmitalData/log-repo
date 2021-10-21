using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Def.EntityQueryServicesExt
{
    public interface ICashBookQueryServiceExt
    {
        CashBookPM GetByPaymentAndCurrency(string currency, string paymentMethod, int tenant);
        CashBookPM GetByPaymentAndCurrencyAndBranch(string currency, string paymentMethod, string branch, int tenant);
        CashBookPM GetSingleById(string Id, int tenant);
        List<CashBookPM> GetListByPaymentAndCurrencyAndBranch(string code, string currencyId, string branch, int tenant);
        bool CheckIfCashbookLineCreatedForPaymentCheque(string cashbookId, string paymentChequeId, int tenant);
    }
}
