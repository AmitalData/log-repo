using Logitude.Accounting.BL.EntityPMs;
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
        public ReconcileCurrencyTypePM GetSingleReconcileCurrencyTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            reconcileCurrencyTypeQuery = new ReconcileCurrencyTypeQueryService(accountingContext);
            ReconcileCurrencyTypePM reconcileCurrencyTypePM = reconcileCurrencyTypeQuery.GetSingle(code, true, false);
            return reconcileCurrencyTypePM;
        }

        public ReconcileCurrencyTypeList GetSingleReconcileCurrencyTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileCurrencyType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileCurrencyTypeListQueryService listService = new ReconcileCurrencyTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<ReconcileCurrencyTypeList> GetReconcileCurrencyTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileCurrencyType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileCurrencyTypeListQueryService listService = new ReconcileCurrencyTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ReconcileCurrencyTypeList> GetReconcileCurrencyTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileCurrencyType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileCurrencyTypeListQueryService listService = new ReconcileCurrencyTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetReconcileCurrencyTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileCurrencyType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileCurrencyTypeListQueryService queryService = new ReconcileCurrencyTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }



    }
}