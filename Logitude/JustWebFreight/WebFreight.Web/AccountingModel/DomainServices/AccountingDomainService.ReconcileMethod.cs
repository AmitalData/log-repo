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
        public ReconcileMethodPM GetSingleReconcileMethodPM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            reconcileMethodQuery = new ReconcileMethodQueryService(accountingContext);
            ReconcileMethodPM reconcileMethodPM = reconcileMethodQuery.GetSingle(code, true, false);
            return reconcileMethodPM;
        }

        public ReconcileMethodList GetSingleReconcileMethodList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileMethod", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileMethodListQueryService listService = new ReconcileMethodListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<ReconcileMethodList> GetReconcileMethodLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileMethodListQueryService listService = new ReconcileMethodListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ReconcileMethodList> GetReconcileMethodFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileMethodListQueryService listService = new ReconcileMethodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetReconcileMethodFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.ReconcileMethod", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileMethodListQueryService queryService = new ReconcileMethodListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateReconcileMethodList(ReconcileMethodList entity)
        {

        }


    }
}