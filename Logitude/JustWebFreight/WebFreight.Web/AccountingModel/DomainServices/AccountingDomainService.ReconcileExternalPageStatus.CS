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
        public ReconcileExternalPageStatusPM GetSingleReconcileExternalPageStatusPM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageStatusQuery = new ReconcileExternalPageStatusQueryService(accountingContext);
            ReconcileExternalPageStatusPM ReconcileExternalPageStatusPM = ReconcileExternalPageStatusQuery.GetSingle(code, true, false);
            return ReconcileExternalPageStatusPM;
        }

        public ReconcileExternalPageStatusList GetSingleReconcileExternalPageStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<ReconcileExternalPageStatusList> GetReconcileExternalPageStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<ReconcileExternalPageStatusList> GetReconcileExternalPageStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageStatusListQueryService listService = new ReconcileExternalPageStatusListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetReconcileExternalPageStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.JournalStatusType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            ReconcileExternalPageStatusListQueryService queryService = new ReconcileExternalPageStatusListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdateReconcileExternalPageStatusList(ReconcileExternalPageStatusList entity)
        {

        }


    }
}