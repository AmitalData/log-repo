using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;

namespace WebFreight.Web.Helpers.MixPanel
{
    public class AuthenticationMixPanelService
    {
        private const string ProjectToken = "99de9de5af6505a670b915020e51380e";

        public static void CreateLoginEventForMixPanel(LoginParameters parameters, int tenant)
        {
            if (!CanCreateLoginEvent(parameters))
                return;

            MixPanelEvent LoginEvent = BuildMixPanelLoginEvent(parameters, tenant);

            MixPanelEventTracker eventTracker = new MixPanelEventTracker(ProjectToken, parameters.Email, tenant);
            eventTracker.TrackEvent(LoginEvent);
        }

        private static bool CanCreateLoginEvent(LoginParameters parameters)
        {
            return LogitudeSettings.LogitudeURL != "http://localhost:9996" && (LogitudeSettingConfigration.IsLogBoxEnvironment() || parameters.IsCargoTracking);
        }

        private static MixPanelEvent BuildMixPanelLoginEvent(LoginParameters parameters, int tenant)
        {
            string workEnvironment = GetWorkEnvironment(parameters, tenant);

            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = "login";
            mixPanelEvent.AddProperty("email", parameters.Email);
            mixPanelEvent.AddProperty("is_public", "False");
            mixPanelEvent.AddProperty("environment", workEnvironment);
            return mixPanelEvent;
        }

        private static string GetWorkEnvironment(LoginParameters parameters, int tenant)
        {
            if (parameters.IsCargoTracking)
            {
                return "cargoTracking";
            }

            var workEnvironment = LogitudeSettingConfigration.GetWorkEnvironment();

            if (workEnvironment != "privatelabel")
            {
                return workEnvironment;
            }
                       
            TenantManagmentPrivateLabelsQuery tenantManagmentPrivateLabelsQuery = new TenantManagmentPrivateLabelsQuery(tenant);
            TenantPM tenantPM = TenantQuery.GetSingleTenantPM(tenant, false);

            return tenantManagmentPrivateLabelsQuery.GetSinglePM(tenantPM.PrivateLabelId)?.PrivateLabelName ?? "";

        }
    }
}