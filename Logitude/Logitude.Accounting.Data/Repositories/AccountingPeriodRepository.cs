 
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
   public partial class AccountingPeriodRepository:IRepository<AccountingPeriod>
   {
        
		public List<AccountingPeriod> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public List<AccountingPeriod> GetAccountingPeriodsByYear(int year, int tenant)
        {
            List<AccountingPeriod> accountingPeriods;
            accountingPeriods = (from a in context.AccountingPeriods
                   where a.Tenant == tenant && a.Year == year
                   select a).ToList();
            return accountingPeriods;
        }

        public List<AccountingPeriod> GetAccountingPeriodsByYearAndType(int year, string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods;
            accountingPeriods = (from a in context.AccountingPeriods
                                 where a.Tenant == tenant && a.PeriodTypeCode == periodTypeCode && a.Year == year 
                                 select a).ToList();
            return accountingPeriods;
        }

        public List<AccountingPeriod> GetAccountingPeriodsByTenantAndType(string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods;
            accountingPeriods = (from a in context.AccountingPeriods
                                 where a.Tenant == tenant && a.PeriodTypeCode == periodTypeCode
                                 select a).ToList();
            return accountingPeriods;
        }



        public List<AccountingPeriod> GetAccountingPeriodsByType(string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods;
            accountingPeriods = (from a in context.AccountingPeriods
                                 where a.Tenant == tenant && a.PeriodTypeCode == periodTypeCode 
                                 select a).ToList();
            return accountingPeriods;
        }

        public List<AccountingPeriod> GetAccountingPeriodsByYearAndMonth(int year,int month, int tenant)
        {
            List<AccountingPeriod> accountingPeriods;
            accountingPeriods = (from a in context.AccountingPeriods
                                 where a.Tenant == tenant && a.Year == year && a.OpenMonth == month
                                 select a).ToList();
            return accountingPeriods;
        }


    }

}
   