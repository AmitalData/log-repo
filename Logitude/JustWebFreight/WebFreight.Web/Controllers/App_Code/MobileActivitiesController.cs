using Logitude.CRM.Data;
using Logitude.CRM.Data.EntityListQueryServices;
using Logitude.CRM.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.App_Code
{
    public class MobileActivitiesController : ApiController
    {
        public List<ActivityList> PostFilteredActivities(int tenant, ActivityFilters filters)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(tenant);

            ObjectTableRepository rep = new ObjectTableRepository(0);
            ObjectTable table = rep.GetObjectTableByName("Customer", 0, false);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.SetFilter("EntityId", filters.EntityId, false, "Equals", null, false);
            queryOperations.SetFilter("IsOpen", filters.IsOpen, false, "Equals", null, false);
            queryOperations.SetFilter("ObjectTableId", table.Id, false, "Equals", null, false);
            
            ICRMContext crmContext = CRMContext.GetContext(tenant);
            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetList(queryOperations, tenant);
        }

        public List<ActivityList> PostActivities(int activitiesTenant, ActivityFilters filters)
        {
            string token = HttpContext.Current.Request.Headers["Token"];
            AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
            SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
            SecurityUtility.AuthenticationOnTenant(activitiesTenant);

            QueryOperations queryOperations = new QueryOperations();
            queryOperations.PageIndex = 0;
            queryOperations.PageSize = 25;
            queryOperations.SetFilter("SearchFields", filters.SearchField, false, "Contains", null, true);

            if (filters.IsMyActivities)
            {
                queryOperations.SetFilter("MyOpenActivities", true, true, "Contains", null, true);
            }

            ICRMContext crmContext = CRMContext.GetContext(activitiesTenant);
            ActivityListQueryService listService = new ActivityListQueryService(crmContext);
            return listService.GetList(queryOperations, activitiesTenant);
        }
    }
}