using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;

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
            mixPanelEvent = MixPanelFactory.Create(mixPanelActionsEvent.ProjectName, tenant);
            MixPanelEventTracker eventTracker = new MixPanelEventTracker(mixPanelEvent.ProjectToken, mixPanelActionsEvent.Email, tenant);
            eventTracker.TrackEvent(mixPanelEvent.BuildEvent(mixPanelActionsEvent));
        }
    }

}