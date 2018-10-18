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
        public ActivityTimeTypePM GetSingleActivityTimeTypePM(string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            activityTimeTypeQuery = new ActivityTimeTypeQueryService(crmContext);
            ActivityTimeTypePM ActivityTimeType = activityTimeTypeQuery.GetSingle(code, false, false);
            return ActivityTimeType;
        }

        public ActivityTimeTypeList GetSingleActivityTimeTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            crmContext = CRMContext.GetContext(tenant);
            ActivityTimeTypeListQueryService listService = new ActivityTimeTypeListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<ActivityTimeTypeList> GetActivityTimeTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTimeTypeListQueryService listService = new ActivityTimeTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<ActivityTimeTypeList> GetActivityTimeTypesFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTimeTypeListQueryService listService = new ActivityTimeTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetActivityTimeTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTimeTypeListQueryService queryService = new ActivityTimeTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
	}
}