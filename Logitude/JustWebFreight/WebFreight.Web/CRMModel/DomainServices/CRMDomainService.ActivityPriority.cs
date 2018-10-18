using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        public ActivityPriorityPM GetSinglePriorityPM(string code, int tenant)
        {
            crmContext = CRMContext.GetContext(tenant);
            priorityQuery = new ActivityPriorityQueryService(crmContext);
            ActivityPriorityPM Priority = priorityQuery.GetSingle(code, false, false);
            return Priority;
        }

        public ActivityPriorityList GetSinglePriorityList(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (crmContext == null)
            {
                crmContext = CRMContext.GetContext(tenant);
            }
            crmContext = CRMContext.GetContext(tenant);
            ActivityPriorityListQueryService listService = new ActivityPriorityListQueryService(crmContext);
            return listService.GetSingle(code);
        }

        public List<ActivityPriorityList> GetPriorityLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityPriorityListQueryService listService = new ActivityPriorityListQueryService(crmContext);

            List<ActivityPriorityList> result = listService.GetList(tenant);
            return result;
        }

        public List<ActivityPriorityList> GetActivityPriorityFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityPriorityListQueryService listService = new ActivityPriorityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);
        }

        public int GetActivityPriorityFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            crmContext = CRMContext.GetContext(tenant);
            ActivityPriorityListQueryService queryService = new ActivityPriorityListQueryService(crmContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations);
        }
    }
}