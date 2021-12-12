using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;
using WebFreight.Web.Helpers.MixPanel.events;

namespace WebFreight.Web.Helpers.MixPanel
{
    public class MixPanelActionsService
    {
        private IMixPanelActionsService mixPanelEvent;
        private readonly int tenant;

        public MixPanelActionsService(int tenant)
        {
            this.tenant = tenant;
        }

        public void Build(MixPanelActionsEvent mixPanelActionsEvent)
        {
            MixPanelFactory(mixPanelActionsEvent.ProjectName);
            MixPanelEventTracker eventTracker = new MixPanelEventTracker(mixPanelEvent.ProjectToken, mixPanelActionsEvent.Email, tenant);
            eventTracker.TrackEvent(mixPanelEvent.BuildMixPanelEvent(mixPanelActionsEvent));
        }

        private void MixPanelFactory(string projectName)
        {
            if (projectName == "LogBox")
            {
                mixPanelEvent = new LogBoxActionsEvent(tenant);
            }
        }
    }


}