using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.Helpers;
using Logitude.CRM.BL.EntityPMs;
using Logitude.CRM.BL.EntityQueryServices;
using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.CRMModel.DomainServices
{
    public partial class CRMDomainService
    {
        public ActivityTypePM GetSingleActivityTypePM(string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            activityTypeQuery = new ActivityTypeQueryService(crmContext);
            ActivityTypePM ActivityType = activityTypeQuery.GetSingle(code, false, false);
            return ActivityType;
        }

        public ActivityTypeList GetSingleActivityTypeList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            crmContext = CRMContext.GetContext(tenant);
            ActivityTypeListQueryService listService = new ActivityTypeListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<ActivityTypeList> GetActivityTypeLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTypeListQueryService listService = new ActivityTypeListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<ActivityTypeList> GetActivityTypeFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTypeListQueryService listService = new ActivityTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetActivityTypeFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityTypeListQueryService queryService = new ActivityTypeListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}