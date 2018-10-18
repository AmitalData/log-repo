using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.Security;

namespace WebFreight.Web.AccountingModel.DomainServices
{
    public partial class AccountingDomainService
    {
        public ChartOfAccountsTypePM GetSingleChartOfAccountsTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            chartOfAccountsTypeQuery = new ChartOfAccountsTypeQueryService(accountingContext);
            ChartOfAccountsTypePM chartOfAccountPM = chartOfAccountsTypeQuery.GetSingle(code, true, false);
            return chartOfAccountPM;
        }

        public ChartOfAccountsTypeList GetSingleChartOfAccountsTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountsTypeListQueryService listService = new ChartOfAccountsTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public void UpdateChartOfAccountsTypeList(ChartOfAccountsTypeList entity)
        {

        }

        public List<ChartOfAccountsTypeList> GetChartOfAccountsTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountsTypeListQueryService listService = new ChartOfAccountsTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ChartOfAccountsTypeList> GetChartOfAccountsTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountsTypeListQueryService listService = new ChartOfAccountsTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetChartOfAccountsTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ChartOfAccountsTypeListQueryService queryService = new ChartOfAccountsTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);// tenant);

        }


    }
}