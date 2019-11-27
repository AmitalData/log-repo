 
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
using Simplog.Data.Helpers;

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

        public int GetCashChequesTotalsForCashbook(string cashbookId, int tenant)
        {
            DateTime todayDate = GetTodayDate(tenant);

            List<CashBookLine> query = (from cbLine in context.CashBookLines
                         join arpch in context.ARPaymentCheques on cbLine.ARPChequeId equals arpch.Id
                         where
                             cbLine.CashBookId == cashbookId
                             && arpch.ValueDate <= todayDate
                             && cbLine.Tenant == tenant
                         select cbLine).ToList();

            return query.Count();
        }
        public int GetPostdatedChequesTotalsForCashbook(string cashbookId, int tenant)
        {
            DateTime todayDate = GetTodayDate(tenant);

            List<CashBookLine> query = (from cb in context.CashBooks
                                        join cbLine in context.CashBookLines on cb.Id equals cbLine.CashBookId
                                        join arpch in context.ARPaymentCheques on cbLine.ARPChequeId equals arpch.Id
                                        where
                                            cb.Id == cashbookId
                                            && arpch.ValueDate > todayDate
                                            && cb.Tenant == tenant
                                        select cbLine).ToList();
            return query.Count();
        }
        public int GetUndepositedChequesCount(string cashbookId, int tenant)
        {
            List<CashBookLine> query = (from cbLine in context.CashBookLines
                                        where
                                            cbLine.CashBookId == cashbookId
                                            && cbLine.IsDeposited == false
                                            && cbLine.Tenant == tenant
                                        select cbLine).ToList();
            return query.Count();
        }


        private static DateTime GetTodayDate(int tenant)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            DateTime todayDateEndDate = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day, 23, 59, 59);
            return todayDateEndDate;
        }
    }

}
   