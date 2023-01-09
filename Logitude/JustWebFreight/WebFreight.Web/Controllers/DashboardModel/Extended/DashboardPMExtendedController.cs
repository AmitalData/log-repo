using Logitude.DashboardModule.BL.EntityPMs;
using Logitude.DashboardModule.BL.EntityQueryServices;
using Logitude.DashboardModule.BL.EntityUpdateServices;
using Logitude.DashboardModule.Data;
using Logitude.DashboardModule.Data.EntityListQueryServices;
using Logitude.DashboardModule.Data.EntityLists;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.Server.Tools.Counters;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
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
    public class DashboardPMExtendedController : ApiController
    {
        public HttpResponseMessage GetDefaultDashboardId()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "READ", authToken.Tenant);

                string loggedContactId = this.GetLoggedContactId(authToken.Email, authToken.Tenant);

                DashboardQueryService dashboardQueryService = new DashboardQueryService(authToken.Tenant);
                string dashboardId = dashboardQueryService.GetDefaultDashboardId(authToken.Tenant, loggedContactId);

                return Request.CreateResponse(HttpStatusCode.OK, dashboardId);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private string GetLoggedContactId(string email, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            string loggedContactId = contactRepository.GetConactIdByemail(email, tenant);

            if (string.IsNullOrEmpty(loggedContactId))
                loggedContactId = contactRepository.GetConactIdByemail(email, 0);

            return loggedContactId;
        }

        public HttpResponseMessage Delete(string dashboardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "UPDATE", authToken.Tenant);

                IDashboardContext MyContext = DashboardContext.GetContext(authToken.Tenant);
                DashboardUpdateService service = new DashboardUpdateService(MyContext, new Dictionary<string, IContext>(), authToken.Tenant);
                service.Delete(dashboardId, authToken.Tenant);

                return Request.CreateResponse(HttpStatusCode.OK, "ok");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUsersDashboardsCount()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "READ", authToken.Tenant);

                IDashboardContext myContext = DashboardContext.GetContext(authToken.Tenant);
                int count = new DashboardListQueryService(myContext).GetUsersDashboardsCount(authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetDashboardsFromIds(string dashboardsIds)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Dashboard", "READ", authToken.Tenant);

                string loggedContactId = this.GetLoggedContactId(authToken.Email, authToken.Tenant);

                IDashboardContext myContext = DashboardContext.GetContext(authToken.Tenant);
                DashboardRepository dashboardRepository = new DashboardRepository(myContext);
                IQueryable<Dashboard> dashboards = dashboardRepository.GetAllByIds(dashboardsIds, authToken.Tenant);

                DashboardListQueryService dashboardQueryService = new DashboardListQueryService(myContext);
                IQueryable<DashboardList> dashboardLists = dashboardQueryService.GetDashboardsByIds(dashboards);

                return Request.CreateResponse(HttpStatusCode.OK, dashboardLists);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetUserHasPinnedDashboards(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IDashboardContext myContext = DashboardContext.GetContext(authToken.Tenant);
                UserPinnedDashboardQueryService userPinnedDashboardQueryService = new UserPinnedDashboardQueryService(myContext);
                UserPinnedDashboardPM userPinnedDashboardsRecord = userPinnedDashboardQueryService.GetPinnedDashboardsByUserId(userId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, userPinnedDashboardsRecord);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetPredefinedDashboardsFromTenantZero()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IDashboardContext myContext = DashboardContext.GetContext(authToken.Tenant);
                DashboardRepository dashboardRepository = new DashboardRepository(myContext);
                DashboardListQueryService dashboardListQueryService = new DashboardListQueryService(myContext);
                IQueryable<Dashboard> predefinedDashboards = dashboardRepository.GetAll(0).Where(d => d.LoadedAutomatically);
                IQueryable<DashboardList> dashboards = dashboardListQueryService.GetIqueryableList(predefinedDashboards);
                return Request.CreateResponse(HttpStatusCode.OK, dashboards);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }


        private string loggedUserId;
        private int tenant;
        public HttpResponseMessage PostPinDashboard(PinnedDashboard pinnedDashboardTab)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                //SecurityUtility.CheckContactFeature("UserPinnedDashboard", "NEW", authToken.Tenant);

                this.tenant = authToken.Tenant;
                loggedUserId = this.GetLoggedContactId(authToken.Email, tenant);

                IDashboardContext myContext = DashboardContext.GetContext(tenant);
                UserPinnedDashboardRepository userPinnedDashboardRepository = new UserPinnedDashboardRepository(myContext);
                UserPinnedDashboardQueryService userPinnedDashboardQueryService = new UserPinnedDashboardQueryService(myContext);
                UserPinnedDashboard userPinnedDashboards = userPinnedDashboardRepository.GetPinnedDashboardsByUserId(loggedUserId, tenant);

                if (userPinnedDashboards == null)
                {
                    userPinnedDashboards = this.AddPinnedDashboardRecord(pinnedDashboardTab);
                    userPinnedDashboardRepository.Add(userPinnedDashboards);
                }

                else
                {
                    this.UpdatePinnedDashboardRecord(userPinnedDashboards, pinnedDashboardTab);
                    userPinnedDashboardRepository.Update(userPinnedDashboards);
                }

                userPinnedDashboardRepository.SubmitChanges();

                UserPinnedDashboardPM userPinnedDashboardsPM = userPinnedDashboardQueryService.GetSingle(userPinnedDashboards.Id, false, false);
                return Request.CreateResponse(HttpStatusCode.OK, userPinnedDashboardsPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private UserPinnedDashboard AddPinnedDashboardRecord(PinnedDashboard pinnedDashboardTab)
        {
            List<PinnedDashboard> pinnedDashboards = new List<PinnedDashboard>();
            pinnedDashboards.Add(new PinnedDashboard()
            {
                Id = pinnedDashboardTab.Id,
                Order = 0,
                IsPredefined = pinnedDashboardTab.IsPredefined,
            });

            return new UserPinnedDashboard()
            {
                Id = IdCounter.GetNumber("UserPinnedDashboard", tenant),
                Tenant = tenant,
                UserId = loggedUserId,
                Dashboards = JsonConvert.SerializeObject(pinnedDashboards),
            };
        }
        private void UpdatePinnedDashboardRecord(UserPinnedDashboard userPinnedDashboards, PinnedDashboard pinnedDashboardTab)
        {
            List<PinnedDashboard> pinnedDashs = new List<PinnedDashboard>();

            if (string.IsNullOrEmpty(userPinnedDashboards.Dashboards))
            {
                pinnedDashs.Add(new PinnedDashboard()
                {
                    Id = pinnedDashboardTab.Id,
                    Order = 0,
                    IsPredefined = pinnedDashboardTab.IsPredefined,
                });
            }

            else
            {
                pinnedDashs = JsonConvert.DeserializeObject<List<PinnedDashboard>>(userPinnedDashboards.Dashboards);
                int maxOrder = !pinnedDashs.Any() ? 0 : pinnedDashs.Max(s => s.Order);

                pinnedDashs.Add(new PinnedDashboard()
                {
                    Id = pinnedDashboardTab.Id,
                    Order = maxOrder + 1,
                    IsPredefined = pinnedDashboardTab.IsPredefined,
                });
            }

            userPinnedDashboards.Dashboards = JsonConvert.SerializeObject(pinnedDashs);
        }

        public HttpResponseMessage GetUnPinDashboard(string userPinnedDashboardsId, string dashboardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                this.tenant = authToken.Tenant;
                loggedUserId = this.GetLoggedContactId(authToken.Email, tenant);

                IDashboardContext myContext = DashboardContext.GetContext(tenant);
                UserPinnedDashboardRepository userPinnedDashboardRepository = new UserPinnedDashboardRepository(myContext);
                UserPinnedDashboardQueryService userPinnedDashboardQueryService = new UserPinnedDashboardQueryService(myContext);
                UserPinnedDashboard userPinnedDashboards = userPinnedDashboardRepository.GetSingle(userPinnedDashboardsId, tenant);

                if (userPinnedDashboards != null)
                {
                    List<PinnedDashboard> pinnedDashs = JsonConvert.DeserializeObject<List<PinnedDashboard>>(userPinnedDashboards.Dashboards);
                    pinnedDashs = pinnedDashs.Where(d => d.Id != dashboardId).ToList();
                    userPinnedDashboards.Dashboards = JsonConvert.SerializeObject(pinnedDashs);
                    userPinnedDashboardRepository.Update(userPinnedDashboards);
                    userPinnedDashboardRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }

    public class PinnedDashboard
    {
        public string Id { get; set; }
        public int Order { get; set; }
        public bool IsPredefined { get; set; }
    }
}