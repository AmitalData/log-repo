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
        public PeriodTypePM GetSinglePeriodTypePM(string code, int tenant)
        {
            accountingContext = AccountingContext.GetContext(tenant);
            periodTypeQuery = new PeriodTypeQueryService(accountingContext);
            PeriodTypePM periodTypePM = periodTypeQuery.GetSingle(code, true, false);
            return periodTypePM;
        }

        public PeriodTypeList GetSinglePeriodTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.PeriodType", "READ", tenant);

            if (accountingContext == null)
            {
                accountingContext = AccountingContext.GetContext(tenant);
            }
            accountingContext = AccountingContext.GetContext(tenant);
            PeriodTypeListQueryService listService = new PeriodTypeListQueryService(accountingContext);
            return listService.GetSingle(code);
        }

        public List<PeriodTypeList> GetPeriodTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.PeriodType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            PeriodTypeListQueryService listService = new PeriodTypeListQueryService(accountingContext);
            return listService.GetList(tenant);
        }


        public List<PeriodTypeList> GetPeriodTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.PeriodType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            PeriodTypeListQueryService listService = new PeriodTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public int GetPeriodTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Accounting.PeriodType", "READ", tenant);
            accountingContext = AccountingContext.GetContext(tenant);
            PeriodTypeListQueryService queryService = new PeriodTypeListQueryService(accountingContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);//, tenant);

        }

        public void UpdatePeriodTypeList(PeriodTypeList entity)
        {

        }


    }
}