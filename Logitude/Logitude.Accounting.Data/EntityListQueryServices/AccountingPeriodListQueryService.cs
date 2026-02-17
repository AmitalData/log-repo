	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class AccountingPeriodListQueryService
    {
	    private IQueryable<AccountingPeriodList> GetIqueryableList(IQueryable<AccountingPeriod> iQueryable)
        {
            IQueryable<AccountingPeriodList> query = (from a in iQueryable
                                                      select new AccountingPeriodList()
                                               {
                                                   Id = a.Id,
                                                   Tenant = a.Tenant,
                                                   Year = a.Year,
                                                   OpenMonth = a.OpenMonth,
                                                   ClosedMonth = a.ClosedMonth,
                                                   PeriodTypeCode = a.PeriodTypeCode,
                                                   PeriodTypeName = a.PeriodType != null ? a.PeriodType.EnglishName : null,
                                               });
            return query;
        }

        public AccountingPeriodList GetByYear(int year , string typeCode, int tenant)
        {
            IQueryable<AccountingPeriod> AccountingPeriodQuery = (from a in context.AccountingPeriods
                                                                  where a.Year == year && a.PeriodTypeCode == typeCode && a.Tenant == tenant
                                                                  select a);


            IQueryable<AccountingPeriodList> AccountingPeriodListQuery = GetIqueryableList(AccountingPeriodQuery);
            AccountingPeriodList AccountingPeriodList = AccountingPeriodListQuery.FirstOrDefault();
            return AccountingPeriodList;

        }

		private IQueryable<AccountingPeriod> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AccountingPeriod> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<AccountingPeriod> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AccountingPeriod> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	