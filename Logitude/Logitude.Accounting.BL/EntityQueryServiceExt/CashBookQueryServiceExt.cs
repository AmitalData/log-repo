using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServiceExt
{
    public class CashBookQueryServiceExt : ICashBookQueryServiceExt
    {
        public CashBookQueryServiceExt()
        {

        }

        public CashBookPM  GetByPaymentAndCurrency(string currency, string paymentMethod, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.GetByPaymentAndCurrency(currency, paymentMethod, tenant);
        }

        public CashBookPM GetByPaymentAndCurrencyAndBranch(string currency, string paymentMethod,string branch, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.GetByPaymentAndCurrencyAndBranch(currency, paymentMethod, branch, tenant);
        }

        public CashBookPM GetSingleById(string Id, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.GetSingle(Id, true, false);

        }

        public List<CashBookPM> GetListByPaymentAndCurrencyAndBranch(string code, string currencyId, string branch, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.GetListByPaymentAndCurrencyAndBranch(code,currencyId,branch,tenant);
        }

        public bool CheckIfCashbookLineCreatedForPaymentCheque(string cashBookId, string paymentChequeId, int tenant)
        {
            CashBookQueryService query = new CashBookQueryService(tenant);
            return query.CheckIfCashbookLineCreatedForPaymentCheque(cashBookId, paymentChequeId, tenant);

        }

    }
}
