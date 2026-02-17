 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class CashBookRepository : IRepository<CashBook>
    {

        public List<CashBook> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public List<CashBook> GetByCurrencyAndTypeAndBranch(string currencyId, string typeCode, string branchId, int tenant)
        {
            if (string.IsNullOrWhiteSpace(currencyId) || string.IsNullOrWhiteSpace(typeCode) || string.IsNullOrWhiteSpace(branchId))
            {
                return null;
            }

            List<CashBook> cashBook = (from a in context.CashBooks
                                       where a.CurrencyId == currencyId && a.CashBookTypeCode == typeCode && a.BranchId == branchId
                                            && a.Tenant == tenant && !(a.Inactive.HasValue && a.Inactive.Value == true)
                                       select a).ToList();
            return cashBook;
        }


        public CashBook GetByPaymentAndCurrency(string currencyId, string methodCode, int tenant)
        {
            if (string.IsNullOrWhiteSpace(currencyId) || string.IsNullOrWhiteSpace(methodCode))
            {
                return null;
            }
            return (from a in context.CashBooks
                    where a.CurrencyId == currencyId && a.CashBookTypeCode == methodCode
                    && a.Tenant == tenant && a.Inactive != true
                    select a).FirstOrDefault();
        }


        public CashBook GetByPaymentAndCurrencyAndBranch(string currencyId, string methodCode,string branch, int tenant)
        {
            if (string.IsNullOrWhiteSpace(currencyId) || string.IsNullOrWhiteSpace(methodCode) || string.IsNullOrWhiteSpace(branch))
            {
                return null;
            }
            return (from a in context.CashBooks
                    where a.CurrencyId == currencyId && a.CashBookTypeCode == methodCode && a.BranchId == branch
                    && a.Tenant == tenant && a.Inactive != true
                    select a).FirstOrDefault();
        }

        public List<CashBook> GetListByPaymentAndCurrencyAndBranch(string code, string currencyId, string branch, int tenant)
        {
            List<CashBook> cashbook = (from a in context.CashBooks
                            where a.CurrencyId == currencyId && a.CashBookTypeCode == code && a.BranchId == branch
                            && a.Tenant == tenant && a.Inactive != true
                            select a).ToList();
            if (cashbook == null)
            {
                cashbook = (from a in context.CashBooks
                            where a.CurrencyId == currencyId && a.CashBookTypeCode == code
                            && a.Tenant == tenant && a.Inactive != true
                            select a).ToList();
            }

            return cashbook;
        }
    }

}
   