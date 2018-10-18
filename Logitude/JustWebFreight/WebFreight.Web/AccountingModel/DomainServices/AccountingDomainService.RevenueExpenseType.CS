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
        public RevenueExpenseTypePM GetSingleRevenueExpenseTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            revenueExpenseTypeQuery = new RevenueExpenseTypeQueryService(accountingContext);
            RevenueExpenseTypePM revenueExpenseTypePM = revenueExpenseTypeQuery.GetSingle(code, true, false);
            return revenueExpenseTypePM;
        }

        public RevenueExpenseTypeList GetSingleRevenueExpenseTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            RevenueExpenseTypeListQueryService listService = new RevenueExpenseTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<RevenueExpenseTypeList> GetRevenueExpenseTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            RevenueExpenseTypeListQueryService listService = new RevenueExpenseTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<RevenueExpenseTypeList> GetRevenueExpenseTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            RevenueExpenseTypeListQueryService listService = new RevenueExpenseTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetRevenueExpenseTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            RevenueExpenseTypeListQueryService queryService = new RevenueExpenseTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateRevenueExpenseTypeList(RevenueExpenseTypeList entity)
        {

        }


    }
}