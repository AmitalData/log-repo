using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityListQueryServices;
using Logitude.DashboardModule.Data.EntityLists;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
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

namespace WebFreight.Web.Controllers.DashboardModel.Extended
{
    public class DashboardListExtendedController : ApiController
    {
        public HttpResponseMessage GetDashboardsForDropDown()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "READ", authToken.Tenant);

                IDashboardContext dashboardContext = DashboardContext.GetContext(authToken.Tenant);
                DashboardRepository dashboardRepository = new DashboardRepository(dashboardContext);
                DashboardListQueryService dashboardListQuery = new DashboardListQueryService(dashboardContext);

                IQueryable<Dashboard> zeroDashboards = dashboardRepository.GetAll(0);
                IQueryable<DashboardList> zeroDashboardLists = dashboardListQuery.GetIqueryableList(zeroDashboards);

                IQueryable<DashboardList> dashboardLists = null;

                if (authToken.Tenant != 0)
                {
                    IQueryable<Dashboard> dashboards = dashboardRepository.GetAll(authToken.Tenant);
                    dashboards = dashboardListQuery.ApplyCustomFilters(new QueryOperations(), dashboards, authToken.Tenant);
                    IQueryable<DashboardList> myDashboardLists = dashboardListQuery.GetIqueryableList(dashboards);
                    dashboardLists = myDashboardLists.Concat(zeroDashboardLists);
                }

                else
                {
                    dashboardLists = zeroDashboardLists;
                }

                return Request.CreateResponse(HttpStatusCode.OK, dashboardLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}