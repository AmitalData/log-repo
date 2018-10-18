
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class GLAccountTotalByMonthQueryService : EntityQueryService<GLAccountTotalByMonth, GLAccountTotalByMonthKeys, GLAccountTotalByMonthPM, object, GLAccountTotalByMonthKeys>
    {
        public decimal? GetBalanceLocalAmountByMonthAndCurrency(string accountId, int year, int month, int tenant, string currencyId)
        {
            return this.repository.GLAccountTotalLocalAmountBalanceByCurrency(accountId, year, month, tenant, currencyId);
        }

        public List<GLAccountTotalByMonthPM> GetMonthTotals(int year, int month, int tenant)
        {
            var pocos=this.repository.GetMounthTotals(year, month, tenant);
            if (pocos == null)
            {
                return new List<GLAccountTotalByMonthPM>();
            }
            var pms=pocos.Select(poco => GetEntityPM(poco)).ToList();
            return pms;
        }


        public List<GLAccountTotalByMonthPM> GetCustomerMonthTotals(int year, int month, string typeCode, int tenant)
        {
            var pocos = this.repository.GetMounthTotals(year, month, tenant);
            if (pocos == null)
            {
                return new List<GLAccountTotalByMonthPM>();
            }
            var result = pocos.Select(poco => GetEntityPM(poco)).ToList();

            var onlyCards = (from a in result
                             where a.CardId != null && a.AccountTypeCode == typeCode
                             select a).ToList();

            return onlyCards;
        }



        public List<CurrencySum> GetSumByMonth(string accountId, int year, int month, int tenant)
        {
            return this.repository.GLAccountTotalByMonth(accountId, year, month, tenant);
        }

        public List<CurrencySum> GetCurrencySumUntillNotInclude(IQueryable<string> accountIdList, int year, int month, int tenant)
        {
            return this.repository.GetAllCurrencySumUntillNotInclude(accountIdList, year, month, tenant);
        }

        public DateTime GLAccountMonthTotalsUpToDate(string accountId, DateTime toDate, int tenant)
        {
            return this.repository.GLAccountMonthTotalsUpToDate(accountId, toDate, tenant);
        }

    }
}
