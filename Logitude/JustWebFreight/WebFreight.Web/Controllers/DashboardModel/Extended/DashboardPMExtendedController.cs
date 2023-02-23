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

        public HttpResponseMessage GetDashboardsUserSettings(string userId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IDashboardContext myContext = DashboardContext.GetContext(authToken.Tenant);
                DashboardsUserSettingQueryService DashboardsUserSettingQueryService = new DashboardsUserSettingQueryService(myContext);
                DashboardsUserSettingPM DashboardsUserSettingsRecord = DashboardsUserSettingQueryService.GetDashboardsUserSettingsByUserId(userId, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, DashboardsUserSettingsRecord);
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
                IQueryable<Dashboard> predefinedDashboards = dashboardRepository.GetAll(0).Where(d => d.PinnedByDefault);
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
                //SecurityUtility.CheckContactFeature("DashboardsUserSetting", "NEW", authToken.Tenant);

                this.tenant = authToken.Tenant;
                loggedUserId = this.GetLoggedContactId(authToken.Email, tenant);

                IDashboardContext myContext = DashboardContext.GetContext(tenant);
                DashboardsUserSettingRepository DashboardsUserSettingRepository = new DashboardsUserSettingRepository(myContext);
                DashboardsUserSettingQueryService DashboardsUserSettingQueryService = new DashboardsUserSettingQueryService(myContext);
                DashboardsUserSetting DashboardsUserSettings = DashboardsUserSettingRepository.GetPinnedDashboardsByUserId(loggedUserId, tenant);

                if (DashboardsUserSettings == null)
                {
                    DashboardsUserSettings = this.AddPinnedDashboardRecord(pinnedDashboardTab);
                    DashboardsUserSettingRepository.Add(DashboardsUserSettings);
                }

                else
                {
                    this.UpdatePinnedDashboardRecord(DashboardsUserSettings, pinnedDashboardTab);
                    DashboardsUserSettingRepository.Update(DashboardsUserSettings);
                }

                DashboardsUserSettingRepository.SubmitChanges();

                DashboardsUserSettingPM DashboardsUserSettingsPM = DashboardsUserSettingQueryService.GetSingle(DashboardsUserSettings.Id, false, false);
                return Request.CreateResponse(HttpStatusCode.OK, DashboardsUserSettingsPM);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        private DashboardsUserSetting AddPinnedDashboardRecord(PinnedDashboard pinnedDashboardTab)
        {
            List<PinnedDashboard> pinnedDashboards = new List<PinnedDashboard>();
            pinnedDashboards.Add(new PinnedDashboard()
            {
                Id = pinnedDashboardTab.Id,
                Order = 0
            });

            return new DashboardsUserSetting()
            {
                Id = IdCounter.GetNumber("DashboardsUserSetting", tenant),
                Tenant = tenant,
                UserId = loggedUserId,
                PinnedDashboards = JsonConvert.SerializeObject(pinnedDashboards),
            };
        }
        private void UpdatePinnedDashboardRecord(DashboardsUserSetting DashboardsUserSettings, PinnedDashboard pinnedDashboardTab)
        {
            List<PinnedDashboard> pinnedDashs = new List<PinnedDashboard>();

            if (string.IsNullOrEmpty(DashboardsUserSettings.PinnedDashboards))
            {
                pinnedDashs.Add(new PinnedDashboard()
                {
                    Id = pinnedDashboardTab.Id,
                    Order = 0
                });
            }

            else
            {
                pinnedDashs = JsonConvert.DeserializeObject<List<PinnedDashboard>>(DashboardsUserSettings.PinnedDashboards);
                int maxOrder = !pinnedDashs.Any() ? 0 : pinnedDashs.Max(s => s.Order);

                pinnedDashs.Add(new PinnedDashboard()
                {
                    Id = pinnedDashboardTab.Id,
                    Order = maxOrder + 1
                });
            }

            DashboardsUserSettings.PinnedDashboards = JsonConvert.SerializeObject(pinnedDashs);
        }

        public HttpResponseMessage GetUnPinDashboard(string dashboardsUserSettingsId, string dashboardId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                this.tenant = authToken.Tenant;
                loggedUserId = this.GetLoggedContactId(authToken.Email, tenant);

                IDashboardContext myContext = DashboardContext.GetContext(tenant);
                DashboardsUserSettingRepository DashboardsUserSettingRepository = new DashboardsUserSettingRepository(myContext);
                DashboardsUserSettingQueryService DashboardsUserSettingQueryService = new DashboardsUserSettingQueryService(myContext);
                DashboardsUserSetting DashboardsUserSettings = DashboardsUserSettingRepository.GetSingle(dashboardsUserSettingsId, tenant);

                if (DashboardsUserSettings != null)
                {
                    List<PinnedDashboard> pinnedDashs = JsonConvert.DeserializeObject<List<PinnedDashboard>>(DashboardsUserSettings.PinnedDashboards);
                    pinnedDashs = pinnedDashs.Where(d => d.Id != dashboardId).ToList();
                    DashboardsUserSettings.PinnedDashboards = JsonConvert.SerializeObject(pinnedDashs);
                    DashboardsUserSettingRepository.Update(DashboardsUserSettings);
                    DashboardsUserSettingRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, "OK");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage PostPinPredefinedDashboards(string[] dashboardIds)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                var tenant = authToken.Tenant;
                var loggedUserId = this.GetLoggedContactId(authToken.Email, tenant);

                IDashboardContext myContext = DashboardContext.GetContext(tenant);
                DashboardsUserSettingRepository dashboardsUserSettingRepository = new DashboardsUserSettingRepository(myContext);
                DashboardsUserSettingQueryService dashboardsUserSettingQueryService = new DashboardsUserSettingQueryService(myContext);
                var existSettings = dashboardsUserSettingQueryService.GetDashboardsUserSettingsByUserId(loggedUserId, tenant);
                if (existSettings != null) return Request.CreateResponse(HttpStatusCode.OK, existSettings);

                List<PinnedDashboard> pinnedDashboards = new List<PinnedDashboard>();
                pinnedDashboards.AddRange(dashboardIds.Select((x, i) => new PinnedDashboard { Id = x, Order = i }));
                var dashboardsUserSetting = new DashboardsUserSetting()
                {
                    Id = IdCounter.GetNumber("DashboardsUserSetting", tenant),
                    Tenant = tenant,
                    UserId = loggedUserId,
                    PinnedDashboards = JsonConvert.SerializeObject(pinnedDashboards),
                };
                dashboardsUserSettingRepository.Add(dashboardsUserSetting);
                dashboardsUserSettingRepository.SubmitChanges();
                DashboardsUserSettingPM dashboardsUserSettingPM = dashboardsUserSettingQueryService.GetSingle(dashboardsUserSetting.Id, false, false);
                return Request.CreateResponse(HttpStatusCode.OK, dashboardsUserSettingPM);
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
    }
}