using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;
using Logitude.BL.GlobalModel.EntityQueries;
using Simplog.Data.Helpers;

namespace WebFreight.Web.Helpers.MixPanel.events
{
    public class LogBoxActionsEvent : IMixPanelActionsService
    {
        private readonly int tenant;

        public LogBoxActionsEvent(int tenant)
        {
            this.tenant = tenant;
        }

        public string ProjectToken { get { return "6da04c25721c3a0269bab184d35fbc1a"; } }

        public MixPanelEvent BuildMixPanelEvent(MixPanelActionsEvent mixPanelActionsEvent)
        {
            string workEnvironment = GetWorkEnvironment(tenant);

            MixPanelEvent mixPanelEvent = new MixPanelEvent();
            mixPanelEvent.Name = mixPanelActionsEvent.ActionName;
            mixPanelEvent.AddProperty("email", mixPanelActionsEvent.Email);
            mixPanelEvent.AddProperty("is_public", "False");
            mixPanelEvent.AddProperty("environment", workEnvironment);
            return mixPanelEvent;
        }

        private string GetWorkEnvironment(int tenant)
        {
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