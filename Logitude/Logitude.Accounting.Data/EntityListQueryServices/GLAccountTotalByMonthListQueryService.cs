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

    public partial class GLAccountTotalByMonthListQueryService
    {
	    private IQueryable<GLAccountTotalByMonthList> GetIqueryableList(IQueryable<GLAccountTotalByMonth> iQueryable)
        {
            IQueryable<GLAccountTotalByMonthList> query = (from a in iQueryable
                                                           join md in context.GLAccountMoreDatas on a.AccountId equals md.AccountId
                                                           select new GLAccountTotalByMonthList()
                                                            {
                                                                AccountId = a.AccountId,
                                                                Tenant = a.Tenant,
                                                                LocalAmountDebit = a.LocalAmountDebit,
                                                                LocalAmountCredit = a.LocalAmountCredit,
                                                                Year = a.Year,
                                                                Month = a.Month,
                                                                ForeignAmountDebit = a.ForeignAmountDebit,
                                                                ForeignAmountCredit = a.ForeignAmountCredit,
                                                                CurrencyId = a.CurrencyId,
                                                                CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                                CurrencySign = a.Currency != null ? a.Currency.Sign : null,
                                                                AccountName = a.GLAccount != null ? a.GLAccount.LocalName : null,
                                                                LocalBalance =
                                                                //a.GLAccount != null ? a.GLAccount.BalanceInLocalCurrency : 0,
                                                                md.BalanceInLocalCurrency != null ? md.BalanceInLocalCurrency : 0,
                                                               
                                                           });
            return query;
        }

		private IQueryable<GLAccountTotalByMonth> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<GLAccountTotalByMonth> iQueryable,int tenant)
        {
            return iQueryable;
		}

        public List<GLAccountTotalByMonthList> GetList(int tenant, int year, string accountid)
        {
            IQueryable<GLAccountTotalByMonth> GLAccountTotalByMonthQuery = (from a in context.GLAccountTotalByMonths.Where(tot => tot.DateTypeCode =="1")
                                                                            where a.AccountId == accountid && a.Year == year
                                                                            select a);


            IQueryable<GLAccountTotalByMonthList> GLAccountTotalByMonthListQuery = GetIqueryableList(GLAccountTotalByMonthQuery);
            List<GLAccountTotalByMonthList> GLAccountTotalByMonthList = GLAccountTotalByMonthListQuery.ToList();
            return GLAccountTotalByMonthList;
        }

     

		private IQueryable<GLAccountTotalByMonth> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<GLAccountTotalByMonth> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	