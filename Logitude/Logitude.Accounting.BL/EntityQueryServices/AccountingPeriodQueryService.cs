using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityDataMappings;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class AccountingPeriodQueryService : EntityQueryService<AccountingPeriod, AccountingPeriodKeys, AccountingPeriodPM, object, AccountingPeriodKeys>
    {
  
        public List<AccountingPeriodList> GetAccountingPeriodListByYear(int year, int tenant)
        {
            List<AccountingPeriod> accountingPeriods = null;
            accountingPeriods = repository.GetAccountingPeriodsByYear(year, tenant);
            List<AccountingPeriodPM> pms = accountingPeriods.Select(poco => this.GetEntityPM(poco)).ToList();
            List<AccountingPeriodList> lists = null;
            if (pms != null)
            {
                lists = new List<AccountingPeriodList>();
                foreach (AccountingPeriodPM pm in pms)
                {
                    lists.Add(new AccountingPeriodList
                    {
                        Id = pm.Id,
                        Tenant = pm.Tenant,
                        Year = pm.Year,
                        ClosedMonth = pm.ClosedMonth,
                        OpenMonth = pm.OpenMonth,
                        PeriodTypeCode = pm.PeriodTypeCode,
                        PeriodTypeName = pm.PeriodTypeName,
                    });
                }
            }
            else
            {
                lists = null;
            }
            return lists;
        }


        public List<AccountingPeriodList> GetAccountingPeriodListByYearAndType(int year, string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods = null;
            accountingPeriods = repository.GetAccountingPeriodsByYearAndType(year, periodTypeCode, tenant);
            List<AccountingPeriodPM> pms = accountingPeriods.Select(poco => this.GetEntityPM(poco)).ToList();
            List<AccountingPeriodList> lists = null;
            if (pms != null)
            {
                lists = new List<AccountingPeriodList>();
                foreach (AccountingPeriodPM pm in pms)
                {
                    lists.Add(new AccountingPeriodList
                    {
                        Id = pm.Id,
                        Tenant = pm.Tenant,
                        Year = pm.Year,
                        ClosedMonth = pm.ClosedMonth,
                        OpenMonth = pm.OpenMonth,
                        PeriodTypeCode = pm.PeriodTypeCode,
                        PeriodTypeName = pm.PeriodTypeName,
                    });
                }
            }
            else
            {
                lists = null;
            }
            return lists;
        }
        public List<AccountingPeriodPM> GetAccountingPeriodsByTenantAndType(string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods = null;
            accountingPeriods = repository.GetAccountingPeriodsByTenantAndType(periodTypeCode, tenant);
            List<AccountingPeriodPM> pms = accountingPeriods.Select(poco => GetEntityPM(poco)).ToList();
            return pms;
        }
        public List<AccountingPeriodPM> GetAccountingPeriodByType(string periodTypeCode, int tenant)
        {
            List<AccountingPeriod> accountingPeriods = null;
            accountingPeriods = repository.GetAccountingPeriodsByType(periodTypeCode, tenant);
            List<AccountingPeriodPM> pms = accountingPeriods.Select(poco => this.GetEntityPM(poco)).ToList();
            return pms;
        }

    }
}
