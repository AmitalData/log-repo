using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Queries;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Web.DataContracts;
using System;

namespace AmitalCloud.Infrastructure.Web.Helpers
{
    public class AuthenticationMixPanelService
    {
        private const string cargoTrackingProjectToken = "99de9de5af6505a670b915020e51380e";
        private const string logBoxProjectToken = "6da04c25721c3a0269bab184d35fbc1a";

        public static void CreateLoginEventForMixPanel(LoginParameters parameters, int tenant)
        {
            if (!CanCreateLoginEvent(parameters))
                return;

            MixPanelEvent LoginEvent = BuildMixPanelLoginEvent(parameters, tenant);


            string projectToken = parameters.IsCargoTracking ? cargoTrackingProjectToken : logBoxProjectToken;
            MixPanelEventTracker eventTracker = new MixPanelEventTracker(projectToken, parameters.Email, tenant);
            eventTracker.TrackEvent(LoginEvent);
        }

        private static bool CanCreateLoginEvent(LoginParameters parameters)
        {
            return AmitalCloudSettings.LogitudeURL != "http://localhost:9996" && (AmitalCloudSettingConfigration.IsLogBoxEnvironment() || parameters.IsCargoTracking);
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

            var workEnvironment = AmitalCloudSettingConfigration.GetWorkEnvironment();

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