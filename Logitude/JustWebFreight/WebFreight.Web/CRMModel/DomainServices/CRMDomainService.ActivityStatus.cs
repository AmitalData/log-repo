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
        public ActivityStatusPM GetSingleActivityStatusPM(string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            activityStatusQuery = new ActivityStatusQueryService(crmContext);
            ActivityStatusPM Status = activityStatusQuery.GetSingle(code, false, false);
            return Status;
        }

        public ActivityStatusList GetSingleActivityStatusList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            crmContext = CRMContext.GetContext(tenant);
            ActivityStatusListQueryService listService = new ActivityStatusListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<ActivityStatusList> GetActivityStatusLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityStatusListQueryService listService = new ActivityStatusListQueryService(crmContext);
            return listService.GetList(tenant);
        }

        public List<ActivityStatusList> GetActivityStatusFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityStatusListQueryService listService = new ActivityStatusListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetActivityStatusFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityStatusListQueryService queryService = new ActivityStatusListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}