using Logitude.BL.CommonDataModel.Tools.MixPanelTracker;
using Simplog.Data.CommonDataModel.Repositories;

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
            if (!mixPanelEvent.IsValid) return;
            MixPanelEventTracker eventTracker = new MixPanelEventTracker(mixPanelEvent.ProjectToken, GetDistinctId(mixPanelActionsEvent), tenant);

            eventTracker.TrackEvent(mixPanelEvent.BuildEvent(mixPanelActionsEvent));
        }

        private string GetDistinctId(MixPanelActionsEvent mixPanelActionsEvent)
        {
            if (mixPanelActionsEvent.ProjectName == "Dashboard") return GetLoggedContactId(mixPanelActionsEvent.Email, tenant);
            return mixPanelActionsEvent.Email;
        }

        private string GetLoggedContactId(string email, int tenant)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            string loggedContactId = contactRepository.GetConactIdByemail(email, tenant);
            if (string.IsNullOrEmpty(loggedContactId)) loggedContactId = contactRepository.GetConactIdByemail(email, 0);
            return loggedContactId;
        }
    }

}