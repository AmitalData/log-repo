using System.Collections.Generic;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public CallTypePM GetSingleCallTypePM(string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            callTypeQuery = new CallTypeQueryService(crmContext);
            CallTypePM CallType = callTypeQuery.GetSingle(code, false, false);
            return CallType;
        }

        public CallTypeList GetSingleCallTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            crmContext = CRMContext.GetContext(tenant);
            CallTypeListQueryService listService = new CallTypeListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<CallTypeList> GetCallTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            CallTypeListQueryService listService = new CallTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<CallTypeList> GetCallTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            CallTypeListQueryService listService = new CallTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetCallTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            CallTypeListQueryService queryService = new CallTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}